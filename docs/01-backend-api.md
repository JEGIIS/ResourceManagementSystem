# Moduł Backend (CompanyResoures.API)

Backend aplikacji został zrealizowany jako RESTful API w technologii ASP.NET Core. Odpowiada za logikę biznesową, komunikację z bazą danych oraz rozgłaszanie zmian w czasie rzeczywistym.

## Główne Komponenty

### 1. Kontrolery (Controllers)
* **`AuthController`**: Obsługuje logowanie (`/api/auth/login`) oraz rejestrację. Generuje tokeny JWT podpisane algorytmem HMAC-SHA512.
* **`ResourcesController`**: Zapewnia operacje CRUD (Create, Read, Update, Delete) dla zasobów. Każda metoda modyfikująca dane (POST, PUT, DELETE) wysyła powiadomienie do SignalR.

### 2. SignalR Hub (`ResourceHub`)
Centralny punkt komunikacji WebSocket.
* **Metoda:** `ReceiveResourceUpdate`
* **Działanie:** W momencie zmiany danych w bazie, Hub wysyła sygnał do wszystkich podłączonych klientów, wymuszając odświeżenie listy zasobów.

### 3. Swagger / OpenAPI
Dokumentacja endpointów jest generowana automatycznie przy użyciu biblioteki `Swashbuckle`.
* Adres: `/swagger`
* Funkcje: Umożliwia testowanie API i autoryzację tokenem Bearer bezpośrednio z przeglądarki.

### 4. Konfiguracja (Program.cs)
* Skonfigurowano politykę **CORS** (`AllowAll`), aby umożliwić połączenia z aplikacji Blazor (Frontend).
* Wdrożono middleware autoryzacji JWT.
