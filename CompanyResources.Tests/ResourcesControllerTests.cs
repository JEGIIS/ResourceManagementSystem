using CompanyResources.API.Controllers;
using CompanyResources.API.Data;
using CompanyResources.API.Hubs;
using CompanyResources.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CompanyResources.Tests
{
    public class ResourcesControllerTests
    {
        // Ta metoda tworzy "fałszywą" bazę danych w pamięci RAM na potrzeby jednego testu
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unikalna nazwa dla izolacji testów
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetAll_ReturnsAllResources_WhenDatabaseIsNotEmpty()
        {
            // ARRANGE (Przygotowanie)
            // 1. Tworzymy bazę i dodajemy dane testowe
            var context = GetInMemoryDbContext();
            context.Resources.Add(new Resource { Name = "Test Laptop", Type = "Sprzęt" });
            context.Resources.Add(new Resource { Name = "Test Sala", Type = "Sala" });
            await context.SaveChangesAsync();

            // 2. Mockujemy Hub SignalR (udajemy, że istnieje, żeby kontroler się nie wywalił)
            var mockHub = new Mock<IHubContext<ResourceHub>>();

            // 3. Tworzymy kontroler
            var controller = new ResourcesController(context, mockHub.Object);

            // ACT (Działanie)
            var result = await controller.GetAll();

            // ASSERT (Sprawdzenie)
            var actionResult = Assert.IsType<ActionResult<List<Resource>>>(result);
            var returnedList = Assert.IsType<List<Resource>>(actionResult.Value);

            Assert.Equal(2, returnedList.Count); // Czy mamy 2 elementy?
            Assert.Equal("Test Laptop", returnedList[0].Name); // Czy nazwa się zgadza?
        }

        [Fact]
        public async Task Create_AddsResourceAndReturnsIt()
        {
            // ARRANGE
            var context = GetInMemoryDbContext();

            // Setup mocka dla SignalR (trochę magii, żeby Mock obsłużył wywołanie Clients.All)
            var mockHub = new Mock<IHubContext<ResourceHub>>();
            var mockClients = new Mock<IClientProxy>();
            mockHub.Setup(h => h.Clients.All).Returns(mockClients.Object);

            var controller = new ResourcesController(context, mockHub.Object);
            var newResource = new Resource { Name = "Nowy Rzutnik", Type = "Sprzęt" };

            // ACT
            var result = await controller.Create(newResource);

            // ASSERT
            // Sprawdź czy kontroler zwrócił wynik Created (lub wynik z obiektem)
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdResource = Assert.IsType<Resource>(actionResult.Value);

            // Sprawdź czy faktycznie zapisało się w bazie
            var dbEntry = await context.Resources.FirstOrDefaultAsync(r => r.Name == "Nowy Rzutnik");
            Assert.NotNull(dbEntry);
            Assert.Equal("Nowy Rzutnik", createdResource.Name);
        }
    }
}
