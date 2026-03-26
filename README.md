# TodoList API

Una aplicación completa de lista de tareas (Todo List) construida con arquitectura limpia, backend en .NET y frontend en Angular.

## Arquitectura

El proyecto sigue los principios de **Arquitectura Limpia (Clean Architecture)** con separación clara de responsabilidades:

- **Domain**: Entidades y lógica de negocio pura
- **Application**: Servicios de aplicación, DTOs y validaciones
- **Infrastructure**: Implementaciones de repositorios y configuración de base de datos
- **WebApi**: Controladores REST API y configuración de ASP.NET Core

## Tecnologías

### Backend
- **.NET 10.0**
- **ASP.NET Core Web API**
- **Entity Framework Core** (SQL Server)
- **FluentValidation** para validaciones
- **CORS** configurado para desarrollo local

### Frontend
- **Angular 21**
- **TypeScript**
- **Tailwind CSS** para estilos
- **Lucide Icons** para iconografía
- **Vitest** para testing

## Requisitos Previos

- **.NET SDK 10.0** o superior
- **Node.js 18+** y **npm**
- **SQL Server** (LocalDB, Express o instancia completa)
- **Angular CLI** (opcional, se instala globalmente)

## Instalación y Configuración

### 1. Clonar el repositorio
```bash
git clone <url-del-repositorio>
cd TodoListApi
```

### 2. Configurar la Base de Datos
Edita el archivo `src/TodoListApi.WebApi/appsettings.json` y configura tu cadena de conexión:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TodoListDb;Trusted_Connection=True;"
  }
}
```
ó

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost,1433;Database=TodoListDb;User Id=TU_USUARIO;Password=TU_PASSWORD;TrustServerCertificate=True"
    }
}
```

Para SQL Server Express:
```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=TodoListDb;Trusted_Connection=True;"
```

### 3. Ejecutar Migraciones de Base de Datos
```bash
cd src/TodoListApi.WebApi
dotnet ef database update
```

### 4. Instalar dependencias del Frontend
```bash
cd client/todo-list-app
npm install
```

## Cómo Ejecutar

### Backend (API)
```bash
cd src/TodoListApi.WebApi
dotnet run
```

La API estará disponible en: `https://localhost:5001` (HTTPS) y `http://localhost:5000` (HTTP)

Swagger UI: `https://localhost:5001/swagger`

### Frontend (Angular)
```bash
cd client/todo-list-app
npm start
# o
ng serve
```

La aplicación estará disponible en: `http://localhost:4200`

## API Endpoints

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/todos` | Obtener todas las tareas |
| GET | `/api/todos/{id}` | Obtener tarea por ID |
| POST | `/api/todos` | Crear nueva tarea |
| PUT | `/api/todos/{id}` | Actualizar tarea existente |
| DELETE | `/api/todos/{id}` | Eliminar tarea |

### Ejemplo de Request Body (POST/PUT)

```json
{
  "title": "Mi tarea importante",
  "description": "Descripción detallada de la tarea",
  "maxCompletionDate": "2024-12-31T23:59:59Z"
}
```

## Estructura del Proyecto

```
TodoListApi/
├── src/
│   ├── TodoListApi.Domain/          # Entidades e interfaces
│   │   ├── Entities/TodoItem.cs
│   │   └── Interfaces/ITodoRepository.cs
│   ├── TodoListApi.Application/     # Servicios y DTOs
│   │   ├── DTOs/
│   │   ├── Services/TodoService.cs
│   │   └── Validators/
│   ├── TodoListApi.Infrastructure/  # Repositorios y DB
│   │   ├── Persistence/AppDbContext.cs
│   │   └── Repositories/TodoRepository.cs
│   └── TodoListApi.WebApi/          # API REST
│       ├── Controllers/TodosController.cs
│       ├── Program.cs
│       └── appsettings.json
├── client/
│   └── todo-list-app/               # Aplicación Angular
│       ├── src/
│       └── package.json
└── TodoListApi.slnx                 # Archivo de solución
```

## Desarrollo

### Agregar nuevas dependencias
```bash
# Backend
cd src/TodoListApi.WebApi
dotnet add package NombreDelPaquete

# Frontend
cd client/todo-list-app
npm install nombre-del-paquete
```

### Crear migraciones de base de datos
```bash
cd src/TodoListApi.WebApi
dotnet ef migrations add NombreDeLaMigracion
dotnet ef database update
```



