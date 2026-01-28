# Moduł Frontend (CompanyResources.UI)

Interfejs użytkownika został zbudowany w technologii **Blazor WebAssembly** (Standalone). Aplikacja działa całkowicie po stronie przeglądarki klienta, komunikując się z API poprzez HTTP oraz WebSocket.

## Struktura i Komponenty

### 1. Strony (Pages)
* **`Login.razor`**: Formularz logowania. Po pomyślnej weryfikacji zapisuje token w pamięci aplikacji i przekierowuje do panelu głównego.
* **`Home.razor`**: Główny panel zarządzania. Zawiera:
    * Tabelę zasobów (aktualizowaną na żywo).
    * Formularz dodawania/edycji zasobu.
    * Mechanizm potwierdzania usuwania (`JSInterop`).

### 2. Usługi i Autoryzacja
* **`CustomAuthStateProvider`**: Niestandardowa implementacja dostawcy stanu autoryzacji. Odpowiada za:
    * Parsowanie tokena JWT.
    * Wyciąganie "Claims" (np. nazwa użytkownika, rola).
    * Powiadamianie komponentów Blazor o zmianie statusu (Zalogowany/Wylogowany).

### 3. Komponent `<AuthorizeView>`
Wykorzystywany do dynamicznego ukrywania treści.
* Użytkownik **niezalogowany** widzi tylko ekran powitalny.
* Użytkownik **zalogowany** otrzymuje dostęp do panelu CRUD i przycisku wylogowania.

### 4. Integracja z SignalR
Aplikacja wykorzystuje paczkę `Microsoft.AspNetCore.SignalR.Client`. Klient nasłuchuje zdarzenia `ReceiveResourceUpdate` i w reakcji na nie ponownie pobiera listę danych z API (`LoadData()`).
