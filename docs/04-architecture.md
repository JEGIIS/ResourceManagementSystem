# Architektura Systemu

System został zaprojektowany w architekturze rozproszonej, gdzie warstwa prezentacji jest całkowicie oddzielona od warstwy danych.

## Diagram przepływu danych

1.  **Klient (Blazor)** wysyła żądanie HTTP (np. `POST /api/resources`) do API.
2.  **API** weryfikuje token JWT i zapisuje zmiany w Bazie Danych (In-Memory).
3.  **API** (poprzez SignalR Hub) wysyła asynchroniczne powiadomienie do wszystkich podłączonych klientów.
4.  **Inni Klienci** odbierają sygnał i automatycznie pobierają zaktualizowaną listę danych (GET).

## Kluczowe wzorce projektowe
* **Repository Pattern (uproszczony):** Wykorzystanie `DbContext` jako warstwy dostępu do danych.
* **Dependency Injection:** Wstrzykiwanie serwisów (np. `HttpClient`, `AppDbContext`) do kontrolerów i komponentów.
* **Observer Pattern:** Realizowany przez SignalR (subskrypcja zmian).
