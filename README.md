# API_ALUMNO_NEW

API RESTful construida con **ASP.NET Core 8** para la gestión de maestros y sus alumnos.

---

## 🚀 Tecnologías utilizadas

- ASP.NET Core Web API (.NET 8)
- C#
- Almacenamiento en archivo local `registros.json`
- CORS habilitado para frontend externo (HTML, JS, Postman)
- Swagger/OpenAPI para documentación automática

---

## 📂 Estructura del proyecto

```
├── Controllers/
│   └── MaestrosController.cs
├── Models/
│   └── Maestro.cs
├── registros.json         ← Base de datos local (JSON)
├── Program.cs
├── appsettings.json
├── index.html             ← Cliente de ejemplo en JS
└── README.md
```

---

## 📮 Endpoints disponibles

### 🔍 Obtener todos los maestros y alumnos

- **GET** `/api/maestros`
- **Descripción:** Retorna un array con todos los maestros y sus respectivos alumnos.

---

### ➕ Agregar un maestro con alumnos

- **POST** `/api/maestros`
- **Body (JSON):**
```json
{
  "nombre": "Juan Pérez",
  "alumnos": ["Carlos", "Ana"]
}
```

---

### 🧑‍🏫 Buscar maestro por nombre

- **GET** `/api/maestros/buscar/maestro/{nombre}`
- **Ejemplo:** `/api/maestros/buscar/maestro/juan`

---

### 🎓 Buscar alumno y su maestro

- **GET** `/api/maestros/buscar/alumno/{nombre}`
- **Ejemplo:** `/api/maestros/buscar/alumno/carlos`

---

### ❌ Eliminar un maestro completamente

- **DELETE** `/api/maestros/maestro`
- **Body (JSON):**
```json
"Juan Pérez"
```

---

### ❌ Eliminar un alumno específico

- **DELETE** `/api/maestros/alumno`
- **Body (JSON):**
```json
{
  "maestro": "Juan Pérez",
  "alumno": "Carlos"
}
```

---

## 🧪 Pruebas con Postman

1. Abrir Postman
2. Crear nueva colección
3. Agregar cada endpoint con su método y body
4. Probar CRUD completo: agregar, leer, buscar y eliminar

---

## 🌐 Acceso a documentación Swagger

- Inicia el servidor (`dotnet run`)
- Accede a: [http://localhost:5000/swagger](http://localhost:5000/swagger)

---

## 🛠 Requisitos para correr

- .NET 8 SDK
- Visual Studio Code o Visual Studio
- Git
- Navegador o cliente como Postman

---

## 👤 Autor

**Brian Chay**  
[GitHub: Chay14](https://github.com/Chay14)

---
