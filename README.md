# BackendService

BackendService — микросервис с REST API для работы с постами, тегами и картинками. Поддерживает интеграцию с NewsApi, авторизацию через Keycloak и асинхронное взаимодействие через RabbitMQ с сервисом модерации.

> **Связанные репозитории:**
> - [ModerationService](https://github.com/ElizavetaLevina/ModerationService) — сервис проверки контента
> - [Shared](https://github.com/ElizavetaLevina/Shared) — общие DTO и Enum для RabbitMQ

## Документация

### Быстрый старт
Все компоненты (PostgreSQL, Keycloak, RabbitMQ) поднимаются через Docker. Swagger доступен после запуска проекта.

### Настройка окружения

Создайте файл `.env` в корне проекта:

```env
API_KEY=ваш_ключ_от_NewsApi
BASE_API_URL=https://newsapi.org/v2
```

### Доступ к сервисам

| Сервис | Адрес |
|--------|-------|
| Swagger API | `https://localhost:{ваш_порт}/swagger` |
| Keycloak Admin | `http://localhost:8090` |
| RabbitMQ Management | `http://localhost:15672` |

## Основные возможности

- **Посты** — создание, редактирование, удаление, просмотр
- **Теги** — привязка к постам и поиск по ним
- **Картинки** — хранение в базе данных как массив байтов
- **Новости** — интеграция с внешним NewsApi
- **Авторизация** — Keycloak (просмотр без входа, создание и редактирование — только автор)
- **Модерация** — отправка постов в RabbitMQ для проверки вторым сервисом

## Используемые технологии

C# / ASP.NET Core / Entity Framework Core / PostgreSQL / Keycloak / RabbitMQ (MassTransit) / Docker / Serilog / NewsApi