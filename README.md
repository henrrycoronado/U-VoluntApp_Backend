# U-Voluntapp Backend

API REST para la gestión de voluntariado universitario. Digitaliza control de asistencia, asignación de horas sociales y auditoría de becas.

## Tech Stack

- .NET 10 — API REST con Clean Architecture
- PostgreSQL 16 — Base de datos relacional
- Entity Framework Core 9 — ORM
- ASP.NET Core Identity — Autenticación y gestión de roles
- Supabase — Storage de evidencias y fotos de perfil
- Docker — Entorno de desarrollo local

## Estructura del proyecto
```
U_VoluntApp_Backend/
├── Program.cs             ← Punto de entrada
├── Src/                   ← Código fuente principal
│   ├── Domain/            ← Entidades y lógica central
│   ├── Application/       ← DTOs e Interfaces de servicio
│   ├── Infrastructure/    ← Persistencia y servicios externos
│   └── Presentation/      ← Controladores API
```

## Requisitos previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- Credenciales de Supabase para Storage (URL y anon key)

## Configuración inicial

### 1. Clonar el repositorio
```bash
git clone [url-del-repo]
cd U-VoluntApp_Backend
dotnet tool restore
dotnet husky install
```

### 2. Configurar variables de entorno
```bash
cp .env.example .env
```

Editar el `.env` con los valores reales:

```env
# Base de datos
DB_CONNECTION_STRING=Host=localhost;Port=5432;Database=uvoluntapp;Username=uvoluntapp_testUser;Password=TU_PASSWORD

# JWT
JWT_SECRET=una_clave_secreta_minimo_32_caracteres
JWT_ISSUER=U_VoluntApp_Backend.Src
JWT_AUDIENCE=UVoluntapp.Client
JWT_EXPIRY_MINUTES=60

# Supabase (solo Storage)
SUPABASE_URL=https://tu-proyecto.supabase.co
SUPABASE_SERVICE_ROLE_KEY=tu_service_role_key

# Storage configurable (evita rutas quemadas)
# Base publica para construir URLs (si no se define, se deriva desde SUPABASE_URL)
STORAGE_PUBLIC_BASE_URL=https://tu-proyecto.supabase.co/storage/v1/object/public
# Bucket de archivos por defecto (NO se escribe aqui)
STORAGE_DEFAULTS_BUCKET=Defaults
# Bucket de carga de archivos nuevos (el backend solo sube aqui)
STORAGE_UPLOAD_BUCKET=CreatedFiles
# Carpetas internas dentro de STORAGE_UPLOAD_BUCKET
STORAGE_FOLDER_PROFILES=profiles
STORAGE_FOLDER_EVIDENCES=evidences

# SuperUsuario del sistema (mínimo 8 caracteres, al menos 1 dígito)
SUPERUSER_EMAIL=admin@tu-app.com
SUPERUSER_PASSWORD=Admin1234
```

### 3. Levantar la base de datos
```bash
cd docs
docker compose up -d
cd ..
```

### 4. Aplicar migraciones
```bash
dotnet ef database update
```

### 5. Ejecutar el proyecto
```bash
dotnet run
```

Swagger disponible en: `http://localhost:PUERTO_DESIGNADO/swagger`

### Generar migraciones (EF Core)

Instrucciones para crear y aplicar migraciones usando Entity Framework Core (versión 9). El `DbContext` principal en este proyecto se llama `AppDbContext` (archivo: [Src/Infrastructure/Persistence/AppDbContext.cs](Src/Infrastructure/Persistence/AppDbContext.cs#L1)).

- Asegúrate de tener instalada la herramienta `dotnet-ef`. Puedes instalarla globalmente si no la tienes:

```bash
dotnet tool install --global dotnet-ef --version 9.*
# o si prefieres usar una herramienta local definida en `dotnet-tools.json`:
dotnet tool restore
```

- Comando típico para crear una nueva migración desde la raíz del repositorio (ejemplo `InitialCreate`):

```bash
dotnet ef migrations add InitialCreate --context AppDbContext --output-dir Src/Infrastructure/Persistence/Migrations
```

- Si tu proyecto de arranque (`Program.cs`) está en un proyecto diferente al que contiene el `DbContext`, usa `--project` y `--startup-project`. Ejemplo:

```bash
dotnet ef migrations add InitialCreate --project Src/Infrastructure --startup-project . --context AppDbContext --output-dir Persistence/Migrations
```

- Para aplicar las migraciones a la base de datos (desde la raíz):

```bash
dotnet ef database update --context AppDbContext
```

- Requisitos y notas:
	- El paquete `Microsoft.EntityFrameworkCore.Design` ya está referenciado en el proyecto (ver [U-VoluntApp_Backend.csproj](U-VoluntApp_Backend.csproj#L1)).
	- Si recibes errores relacionados con el proveedor de base de datos, verifica la cadena de conexión en las variables de entorno o `appsettings` y que `Npgsql.EntityFrameworkCore.PostgreSQL` esté instalado.

---

## 6. CORS – Arquitectura Multi-plataforma

La API está configurada para servir 3 plataformas con CORS específico. La variable de entorno `CORS_ALLOWED_ORIGINS` controla qué dominios pueden acceder a la API.

---

## 7. Arquitectura Defensiva

La API implementa validación en **3 capas**:
1. **Entrada:** `RequestValidationMiddleware` valida formato y tamaño.
2. **Dominio:** Validaciones de negocio en entidades y servicios.
3. **Respuesta:** `ExceptionMiddleware` unifica el formato de error.

---

## 8. Flujo de Storage

- El backend usa bucket configurable para subidas (`STORAGE_UPLOAD_BUCKET`) y por seguridad bloquea configuraciones que apunten al bucket de defaults.
- `Defaults` queda reservado para assets base (por ejemplo avatares/banner por defecto) y no debe usarse para carga de fotos nuevas.
- Los defaults de dominio (foto de perfil, banner, etc.) ahora se construyen con `STORAGE_PUBLIC_BASE_URL` + `STORAGE_DEFAULTS_BUCKET`, evitando URLs hardcodeadas.

---

## 9. Gestion de States y Types (solo SuperUser)

Se agrego un catalogo administrativo para referencias:

- `GET /api/v1/reference-catalog/states/{stateGroup}`
- `PATCH /api/v1/reference-catalog/states/{stateGroup}/{stateCode}`
- `GET /api/v1/reference-catalog/types/{typeGroup}`
- `POST /api/v1/reference-catalog/types/{typeGroup}`
- `PATCH /api/v1/reference-catalog/types/{typeGroup}/{typeCode}`

Restriccion:

- Todos estos endpoints requieren rol `SuperUser`.

Grupos soportados:

- `stateGroup`: `activity`, `program`, `profile`, `enrollment`, `tracking`, `contract`, `role-request`.
- `typeGroup`: `activity`, `evidence`, `tracking`, `career`, `scholarship`.

---

## 10. Protocolo de reinicio limpio

Usar cuando se quiera borrar todos los datos y empezar desde cero:
```bash
cd docs
docker compose down -v
docker compose up -d
cd ..
dotnet ef database update
```

---

## 11 Branching strategy

GitFlow estricto: `main`, `develop`, `feature/[nombre]`.

---

## 12 Convención de commits

Este repositorio usa **Conventional Commits** (`tipo(módulo): descripción`).

---

## 13. Git Hooks

Se utilizan Git Hooks vía Husky.Net para validar formato, commits y build antes de subir al repositorio.
