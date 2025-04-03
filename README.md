# Московский архив погоды
**ASP.NET Core MVC приложение для загрузки и отображения архивов погодных условий в городе Москве**

Выполненно в рамках испыательного задания в DynamicSun

## Запуск

- Создать базу данных в PostgreSQL
- Указать данные о подключении в appsettings.json:
```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Port=5432;Database=weather_archive;Username=postgres;Password=postgres"
    }
}
```
- Восстановить зависимости
```
dotnet restore
```
- Запустить миграции
```
dotnet ef database update
```
- Запустить приложение
```
dotnet run
```