# 📦 Nous Backend Upload API

Aplikasi **Backend Upload API** dibangun menggunakan ASP.NET Core Web API (.NET 8). Backend ini digunakan untuk menerima file dari frontend, menyimpannya, dan mengelola informasi terkait file.

---

## 🚀 Teknologi yang Digunakan

- ASP.NET Core Web API (.NET 8)
- PostgreSQL / SQL Server (tergantung konfigurasi)
- JWT Authentication (Google OAuth support)
- CORS & Middleware Pipeline
- Swagger (OpenAPI)

---

## 📂 Struktur Folder Utama

```bash
nous-backend-upload/
│
├── Controllers/          # Endpoint API
├── CORE/                 # Service CORE (DataBase etc)
├── DTO/                  # Data Transfer Object
├── Models/               # Struktur Model
├── Services/             # Logic bisnis
├── Middlewares/           # Middleware kustom (jika ada)
├── appsettings.json      # Konfigurasi dasar
├── Program.cs            # Entry point aplikasi
└── README.md             # Dokumentasi ini
