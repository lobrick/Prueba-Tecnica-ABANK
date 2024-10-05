# Prueba-Tecnica-ABANK
Repositorio de prueba tecnica para banco ABANK

# API de Gestión de Usuarios

Este proyecto es una API desarrollada en .NET Core que permite la gestión de usuarios (crear, obtener, actualizar y eliminar). Además, se ha dockerizado para facilitar su despliegue.

## Características

- **Autenticación JWT**: Los usuarios pueden autenticarse con un token JWT que es generado al proporcionar un número de teléfono y contraseña válidos.
- **CRUD de Usuarios**: La API permite realizar operaciones de Crear, Leer, Actualizar y Eliminar (CRUD) sobre los usuarios.
- **Docker**: Esta API está lista para ser ejecutada en un contenedor Docker.

## Endpoints

### Autenticación

- `POST /api/login/login`: Permite autenticar a un usuario mediante su número de teléfono y contraseña. Devuelve un token JWT si las credenciales son correctas.

### Usuarios

- `POST /api/user/createuser`: Crea un nuevo usuario. El número de teléfono debe ser único.
- `GET /api/user/getallusers`: Obtiene el listado de todos los usuarios registrados.
- `GET /api/user/getuser/{id}`: Obtiene la información de un usuario por su ID.
- `PUT /api/user/updateuser`: Actualiza los datos de un usuario existente.
- `DELETE /api/user/deleteuser`: Elimina un usuario por su ID.

## Requisitos

- [.NET Core SDK](https://dotnet.microsoft.com/download) 6.0 o superior.
- [Docker](https://www.docker.com/products/docker-desktop) para ejecutar la imagen Docker.

## Configuración

### Base de Datos
La API utiliza una base de datos PostgreSQL. Debes configurar la conexión a la base de datos en el archivo `appsettings.json` o mediante variables de entorno.

### Configuración del JWT
El token JWT utiliza una clave secreta y un emisor/audiencia. Debes agregar las configuraciones en `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "tu_clave_secreta_aqui",
    "Issuer": "tu_issuer",
    "Audience": "tu_audience"
  }
}
Para iniciar el proyecto solamente hay que ejecutar:
docker compose up -d
 
E iniciará una instancia de Postgres 16, creará el esquema inicial y las tablas,  y compilará y ejecutará la API

Para detener el proceso usar:
docker compose down


