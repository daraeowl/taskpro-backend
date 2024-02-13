# Backend

## Visión General

Este proyecto backend está desarrollado utilizando C# y .NET Core 8. Proporciona una API REST segura que utiliza JSON Web Tokens (JWT) para la autenticación de usuarios.

## Empezando

Para empezar a trabajar en el proyecto backend, sigue estos pasos:

1. Clona este repositorio en tu máquina local.
2. Navega al directorio del proyecto backend.
3. Abre la solución en Visual Studio o cualquier otro IDE compatible con .NET Core.
4. Compila y ejecuta la solución. ( comando dotnet watch run )

## Uso

Para utilizar el backend en un entorno local:

1. Asegúrate de que el frontend esté en funcionamiento y sea capaz de realizar solicitudes a la API.
2. Utiliza las rutas de la API proporcionadas para realizar diferentes operaciones.

## Contribuyendo

Si deseas contribuir al proyecto backend, sigue estos pasos:

1. Haz un fork de este repositorio.
2. Crea una nueva rama con tus cambios: `git checkout -b feature/NombreCaracteristica`.
3. Realiza tus cambios y haz commits: `git commit -m "Descripción de los cambios"`.
4. Sube tus cambios a tu repositorio: `git push origin feature/NombreCaracteristica`.
5. Abre un pull request en este repositorio.


**Backend API Endpoints:**

**AspNetUsers:**

- **GET** `/api/user`
  - Obtiene todos los usuarios.

- **GET** `/api/user/{id}`
  - Obtiene un usuario específico por su ID.

- **PUT** `/api/user/{id}`
  - Actualiza la información de un usuario específico por su ID.

- **DELETE** `/api/user/{id}`
  - Elimina un usuario específico por su ID.

**Project:**

- **GET** `/api/project`
  - Obtiene todos los proyectos.

- **POST** `/api/project`
  - Crea un nuevo proyecto.

- **GET** `/api/project/{id}`
  - Obtiene un proyecto específico por su ID.

- **PUT** `/api/project/{id}`
  - Actualiza la información de un proyecto específico por su ID.

- **DELETE** `/api/project/{id}`
  - Elimina un proyecto específico por su ID.
  
**Task:**

- **GET** `/api/task`
  - Obtiene todas las tareas.

- **POST** `/api/task`
  - Crea una nueva tarea.

- **GET** `/api/task/{id}`
  - Obtiene una tarea específica por su ID.

- **PUT** `/api/task/{id}`
  - Actualiza la información de una tarea específica por su ID.

- **DELETE** `/api/task/{id}`
  - Elimina una tarea específica por su ID.
