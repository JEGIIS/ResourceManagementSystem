# Modu³ Backend (CompanyResources.API)

Backend aplikacji zosta³ zrealizowany jako RESTful API w technologii ASP.NET Core. Odpowiada za logikê biznesow¹, komunikacjê z baz¹ danych oraz rozg³aszanie zmian w czasie rzeczywistym.

## G³ówne Komponenty

### 1. Kontrolery (Controllers)
* **`AuthController`**: Obs³uguje logowanie (`/api/auth/login`) oraz rejestracjê. Generuje tokeny JWT podpisane algorytmem HMAC-SHA512.
* **`ResourcesController`**: Zapewnia operacje CRUD (Create, Read, Update, Delete) dla zasobów. Ka¿da metoda modyfikuj¹ca dane (POST, PUT, DELETE) wysy³a powiadomienie do SignalR.

### 2. SignalR Hub (`ResourceHub`)
Centralny punkt komunikacji WebSocket.
* **Metoda:** `ReceiveResourceUpdate`
* **Dzia³anie:** W momencie zmiany danych w bazie, Hub wysy³a sygna³ do wszystkich pod³¹czonych klientów, wymuszaj¹c odœwie¿enie listy zasobów.

### 3. Swagger / OpenAPI
Dokumentacja endpointów jest generowana automatycznie przy u¿yciu biblioteki `Swashbuckle`.
* Adres: `/swagger`
* Funkcje: Umo¿liwia testowanie API i autoryzacjê tokenem Bearer bezpoœrednio z przegl¹darki.

### 4. Konfiguracja (Program.cs)
* Skonfigurowano politykê **CORS** (`AllowAll`), aby umo¿liwiæ po³¹czenia z aplikacji Blazor (Frontend).
* Wdro¿ono middleware autoryzacji JWT.