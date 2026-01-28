# 🏢 Rozproszony System Zarządzania Zasobami (CompanySystem)

System klasy Enterprise służący do zarządzania zasobami firmowymi (sprzęt, sale, pojazdy) w czasie rzeczywistym. Aplikacja wykorzystuje architekturę rozproszoną (Klient-Serwer), zabezpieczenia JWT oraz synchronizację danych poprzez WebSocket (SignalR).

**Technologie:** .NET 8, Blazor WebAssembly, ASP.NET Core Web API, Entity Framework Core, SignalR, xUnit.

---

## 🚀 Funkcjonalności

* **Zarządzanie Zasobami (CRUD):** Dodawanie, edycja, usuwanie i podgląd zasobów.
* **Real-time Sync:** Zmiany wprowadzane przez jednego użytkownika są natychmiast widoczne u innych (bez odświeżania strony).
* **Bezpieczeństwo:** Logowanie i autoryzacja oparta na tokenach JWT (JSON Web Token).
* **UI:** Nowoczesny interfejs w technologii Blazor WebAssembly.
* **Dokumentacja API:** Automatycznie generowana przez Swagger/OpenAPI.

## ⚙️ Wymagania

* .NET 8.0 SDK (lub nowszy)
* Visual Studio 2022 (z workloadem "ASP.NET and web development")

## 🛠️ Instrukcja Instalacji i Uruchomienia

1.  **Klonowanie repozytorium:**
    ```bash
    git clone [https://github.com/JEGIIS/ResourceManagementSystem](https://github.com/JEGIIS/ResourceManagementSystem)
    cd ResourceManagementSystem
    ```

2.  **Otwarcie projektu:**
    Otwórz plik `ResourceManagementSystem.sln` w Visual Studio 2022.

3.  **Konfiguracja uruchamiania (Kluczowe!):**
    System składa się z dwóch niezależnych aplikacji. Należy uruchomić obie jednocześnie.
    * Kliknij prawym przyciskiem myszy na nazwę solucji w *Solution Explorer*.
    * Wybierz **Configure Startup Projects** (Konfiguruj projekty startowe).
    * Zaznacz opcję **Multiple startup projects** (Wiele projektów startowych).
    * Ustaw **Action: Start** dla:
        * `CompanyResource.API`
        * `CompanyResource.UI`

4.  **Uruchomienie:**
    Naciśnij `F5`. Otworzą się dwa okna przeglądarki:
    * Backend (Swagger): `https://localhost:7234/swagger` (port przykładowy)
    * Frontend (Aplikacja): `https://localhost:7015`

5.  **Logowanie (Konto Demo):**
    * **Email:** `admin@firma.pl`
    * **Hasło:** `admin123`

## 📚 Dokumentacja Modułów

Szczegółowy opis techniczny poszczególnych komponentów znajduje się w folderze `docs/`:

* [📂 Backend & API (Dokumentacja)](docs/01-backend-api.md)
* [💻 Frontend & UI (Dokumentacja)](docs/02-frontend-ui.md)
* [🔐 Baza Danych i Autoryzacja](docs/03-database-auth.md)
* [🏗️ Architektura i Przepływ Danych](docs/04-architecture.md)
