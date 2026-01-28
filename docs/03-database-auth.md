# Baza Danych i Autoryzacja

## Model Danych (Shared)
Modele są współdzielone między API a UI dzięki bibliotece `CompanyResources.Shared`.
* **`Resource`**:
    * `Id` (int)
    * `Name` (string) - np. "Laptop Dell"
    * `Type` (string) - np. "Sprzęt"
    * `IsAvailable` (bool) - Status dostępności
* **`User`**: Przechowuje dane logowania (Email, PasswordHash, Role).

## Baza Danych (In-Memory)
Projekt wykorzystuje `Microsoft.EntityFrameworkCore.InMemory`.
* Jest to baza danych przechowywana w pamięci RAM serwera.
* **Zaleta:** Szybkość działania i brak konieczności instalowania SQL Servera do testów.
* **Seedowanie:** Przy starcie aplikacji (`Program.cs`) automatycznie tworzone jest konto administratora (`admin@firma.pl`).

## Autoryzacja (JWT Security)
System wykorzystuje standard **JSON Web Token**.
1.  **Logowanie:** Użytkownik wysyła dane POST-em.
2.  **Generowanie:** API weryfikuje dane i tworzy token podpisany kluczem symetrycznym (HMAC-SHA512).
3.  **Przesyłanie:** UI dołącza token do każdego żądania w nagłówku HTTP: `Authorization: Bearer <token>`.
4.  **Weryfikacja:** Serwer sprawdza ważność i podpis tokena przed wykonaniem operacji.
