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
U_VoluntApp_Core/
├── Program.cs             ← Punto de entrada
├── Src/                   ← Código fuente principal
│   ├── Domain/            ← Entidades y lógica central
│   ├── Application/       ← DTOs e Interfaces de servicio
│   ├── Infrastructure/    ← Persistencia y servicios externos
│   └── Presentation/      ← Controladores API
```

## Requisitos previos

- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- Credenciales de Supabase para Storage (URL y anon key)
- [.NET 10 SDK](https://dotnet.microsoft.com/download) solo si vas a ejecutar fuera de Docker o crear migraciones manualmente

## Configuración inicial

### 1. Clonar el repositorio
```bash
git clone [url-del-repo]
cd U-VoluntApp_Core
dotnet tool restore
dotnet husky install
```

### 2. Configurar variables de entorno
```bash
cp .env.example .env
```

Editar el `.env` con los valores reales. Si vas a usar Docker local, deja `DB_CONNECTION_STRING` apuntando a `postgres_db` y no a `localhost`:

```env
# Base de datos para Docker local
POSTGRES_USER=uvoluntapp
POSTGRES_PASSWORD=change_me_local
POSTGRES_DB=uvoluntapp
DB_CONNECTION_STRING=Host=postgres_db;Port=5432;Database=uvoluntapp;Username=uvoluntapp;Password=change_me_local

# JWT
JWT_SECRET=dev_secret_local_minimo_32_caracteres
JWT_ISSUER=U_VoluntApp_Core.Src
JWT_AUDIENCE=UVoluntapp.Client
JWT_EXPIRY_MINUTES=60

# Supabase (solo Storage)
SUPABASE_URL=https://tu-proyecto.supabase.co
SUPABASE_SERVICE_ROLE_KEY=tu_service_role_key

# SuperUsuario del sistema
SUPERUSER_EMAIL=admin@tu-app.com
SUPERUSER_PASSWORD=Admin1234

# Entorno local
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://+:8080
ENABLE_HTTPS_REDIRECTION=false

CORS_ALLOWED_ORIGINS=http://localhost:3000,http://localhost:5173,http://localhost:4200
```

### 3. Levantar el stack local con Docker
```bash
docker compose up -d --build
```

La API quedara disponible en `http://localhost:8080` y Swagger en `http://localhost:8080/swagger` mientras el entorno sea `Development`.
PostgreSQL queda accesible solo para la API dentro de la red de Docker; no se publica el puerto `5432` al host.

### 4. Plantilla para VPS
Si quieres desplegar en un VPS usando una imagen ya construida por GitHub Actions, usa `docker-compose.prod.yml`.

Este archivo no compila la aplicación; solo consume la imagen publicada en GHCR. Antes de levantarlo, define en tu `.env` estas variables adicionales:

```env
API_IMAGE=ghcr.io/henrrycoronado/u-voluntapp-backend:latest
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:8080
ENABLE_HTTPS_REDIRECTION=false
```

Luego puedes iniciar el stack con:

```bash
docker compose --env-file .env.prod -f docker-compose.prod.yml up -d
```

Si prefieres mantener un archivo separado, crea `.env.prod` a partir de tu `.env` local y sobrescribe `API_IMAGE`, `ASPNETCORE_ENVIRONMENT` y `ENABLE_HTTPS_REDIRECTION` para producción.

En este flujo, Nginx puede hacer el proxy inverso y manejar HTTP/HTTPS, mientras la API se mantiene en HTTP interno.

### 5. Reinicio limpio del stack
```bash
docker compose down -v
docker compose up -d --build
```

### 6. Ejecutar fuera de Docker, si lo necesitas
Si quieres correr la API con `dotnet run`, cambia `DB_CONNECTION_STRING` a `Host=localhost;...` en tu `.env` local o usa un perfil distinto para desarrollo sin contenedores.

### 7. GitHub Actions

- `CI` valida restauración, compilación y la configuración del compose en cada pull request y push a `develop` o `master`.
- `CD` construye la imagen Docker y la publica en GHCR con dos etiquetas: una por rama (`develop` o `latest`) y otra por commit SHA.
- El VPS no necesita compilar nada; solo debe hacer pull de la imagen y levantar el compose de producción.

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
	- El paquete `Microsoft.EntityFrameworkCore.Design` ya está referenciado en el proyecto (ver [U-VoluntApp_Core.csproj](U-VoluntApp_Core.csproj#L1)).
	- Si recibes errores relacionados con el proveedor de base de datos, verifica la cadena de conexión en las variables de entorno o `appsettings` y que `Npgsql.EntityFrameworkCore.PostgreSQL` esté instalado.

---

## 6. CORS – Arquitectura Multi-plataforma

La API está configurada para servir múltiples plataformas web (local o producción) con CORS específico. La variable `CORS_ALLOWED_ORIGINS` controla qué dominios pueden acceder a la API.

---

## 7. Arquitectura Defensiva y Seguridad

La API implementa validación en **3 capas**:
1. **Entrada:** `RequestValidationMiddleware` valida formato y tamaño, además de contar con `FluentValidation` para los DTOs.
2. **Dominio:** Validaciones de negocio robustas en los servicios de aplicación (Ej. Control de cupos en actividades).
3. **Respuesta:** `ExceptionMiddleware` unifica el formato de error usando el estándar HTTP Problem Details.

### Autorización Declarativa
El sistema de permisos se maneja de forma declarativa mediante atributos `[Authorize(Roles = "...")]` en los Controladores.
- **Roles base:** `SuperUser`, `Admin`, `Coordinator`, `Volunteer`.
- **Swagger:** La documentación OpenAPI detecta automáticamente estos atributos y agrega el candado `🔒 Roles Requeridos:` a cada endpoint en la interfaz de Swagger.

---

## 8. Flujo de Storage

- El backend usa un bucket configurable en Supabase para subidas (`STORAGE_UPLOAD_BUCKET`).
- `Defaults` queda reservado para assets base institucionales (por ejemplo avatares por defecto).
- Los defaults de dominio se construyen combinando `STORAGE_PUBLIC_BASE_URL` + `STORAGE_DEFAULTS_BUCKET`, evitando URLs hardcodeadas en la BD.

---

## 9. Vistas Materializadas y Reportes en Vivo

U-VoluntApp utiliza una arquitectura híbrida para la generación de reportes y analíticas:
- **Dashboards Globales (Vistas Materializadas):** Para vistas pesadas (historial completo, analítica de programas con miles de registros), PostgreSQL utiliza `MATERIALIZED VIEWS` para dar respuestas instantáneas a los administradores. Estas vistas se refrescan llamando al endpoint reservado `POST /api/v1/reports/refresh`.
- **Historial Individual (Live Query):** Cuando un voluntario solicita su propio avance (`GET /api/v1/reports/volunteers/me`), el backend ejecuta un cálculo en tiempo real (`LINQ` + `Entity Framework`) sobre sus horas registradas. Esto asegura que el voluntario vea sus horas validadas al instante sin depender del proceso de refresco global.

---

## 10. Gestión de Estados y Tipos (Solo SuperUser)

Catálogo administrativo centralizado para definir etapas (states) y categorías (types):
- `GET / PATCH` en `/api/v1/reference-catalog/states/{stateGroup}`
- `GET / POST / PATCH` en `/api/v1/reference-catalog/types/{typeGroup}`

**Grupos soportados:**
- `stateGroup`: `activity`, `program`, `profile`, `enrollment`, `tracking`, `contract`, `role-request`.
- `typeGroup`: `activity`, `evidence`, `tracking`, `career`, `scholarship`.

---

## 11. Flujo de CI/CD (GitHub Actions)

El repositorio incluye pipelines preconfigurados:
- **CI (Integración Continua):** Valida compilación, restore y formato de código (StyleCop) en PRs hacia `develop`.
- **CD (Despliegue Continuo):** Construye la imagen Docker y la sube a GHCR.
- **Secretos:** Revisa el archivo `.env.secrets.example` para saber qué secretos debes configurar en GitHub para que el Action corra correctamente (incluyendo opciones para BD remota o local).

---

## 12. Protocolo de reinicio limpio

Usar cuando se quiera borrar todos los datos (incluyendo volumen de BD local) y empezar desde cero:
```bash
docker compose down -v
docker compose up -d --build
```

---

## 13. Convenciones y Contribución

- **GitFlow Estricto:** Ramas principales `main`, `develop`, y feature branches `feature/[nombre]`.
- **Conventional Commits:** Este repositorio fuerza un formato (`tipo(módulo): descripción`).
- **Husky & Git Hooks:** Antes de cada commit, se validan las reglas de `dotnet-format` y el mensaje de commit.
