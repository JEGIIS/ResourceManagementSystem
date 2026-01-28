using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyResources.Shared
{
    public class Resource
    {
        public int Id { get; set; }

        // Nazwa zasobu, np. "Laptop Dell XPS 15"
        public string Name { get; set; } = string.Empty;

        // Typ zasobu, np. "Sprzęt", "Sala", "Samochód"
        public string Type { get; set; } = string.Empty;

        // Czy zasób jest wolny?
        public bool IsAvailable { get; set; } = true;
    }
}
