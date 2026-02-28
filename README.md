# 🔗 URL Shortener API

A modern, high-performance, and production-ready URL Shortener service engineered utilizing Clean Architecture principles.

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-4169E1?logo=postgresql)
![Redis](https://img.shields.io/badge/Redis-7.0-DC382D?logo=redis)
![Docker](https://img.shields.io/badge/Docker-Enabled-2496ED?logo=docker)

---

## 🌟 Overview

This project is a scalable RESTful Web API that takes long, cumbersome URLs and condenses them into short, shareable links (similar to Bit.ly or TinyURL). It features a highly optimal cache-aside pattern to ensure blazing fast redirects, robust data persistence, and click-tracking analytics.

## 🏛 Architecture

The solution is heavily inspired by **Clean Architecture**, promoting loose coupling and high testability across four distinct layers:

1. **`Domain`**: The core of the system containing business entities (e.g., `UrlMapping`).
2. **`Application`**: Contains business use cases, CQRS-style Data Transfer Objects (DTOs), and interfaces.
3. **`Infrastructure`**: Handles external concerns. Implements the `DbContext` (EF Core/PostgreSQL), Cache Services (StackExchange.Redis), and the base-62 short-code generation algorithm.
4. **`Api`**: The Presentation layer—exposing Controller endpoints and configuring Dependency Injection.

## ✨ Key Features

- **Blazing Fast Redirects**: Sub-millisecond read times using Redis caching. Database fallbacks automatically warm the cache.
- **Custom Aliases**: Users can supply their own custom vanity codes, or let the system generate a collision-resistant 7-character base-62 hash.
- **Click Analytics**: Tracks every redirect, incrementing global hit counters and securely logging the last-accessed timestamp.
- **Containerized Environment**: Seamless local development experience orchestrated via Docker Compose.

---

## 🚀 Getting Started

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop) installed and running.

### Installation & Run

1. Clone or navigate to the project root directory.
2. Open your terminal and run Docker Compose:

   ```bash
   docker-compose up -d --build
   ```

3. Docker will automatically pull the necessary database images, compile the .NET 9 API, apply PostgreSQL Entity Framework migrations, and bind to `http://localhost:5000`.

---

## 📡 API Endpoints

### 1. Create a Short URL (`POST /api/url`)

Converts a long URL into a short code.

**Request Body:**
```json
{
  "longUrl": "https://www.example.com/very/long/path/to/resource",
  "customAlias": "mybrand" 
}
```
*(Note: `customAlias` is optional)*

**Success Response (201 Created):**
```json
{
  "shortUrl": "http://localhost:5000/mybrand"
}
```

### 2. Redirect to Original URL (`GET /{shortCode}`)

Seamlessly redirects the user's browser to the destination. Highly optimized via Redis.

**Request:** `GET http://localhost:5000/mybrand`
**Response:** `302 Found` (Redirects to `longUrl`)

### 3. Fetch URL Analytics (`GET /api/url/{shortCode}/stats`)

Retrieves tracking data regarding how many times a link was utilized.

**Request:** `GET http://localhost:5000/api/url/mybrand/stats`
**Response (200 OK):**
```json
{
  "longUrl": "https://www.example.com/very/long/path/to/resource",
  "shortCode": "mybrand",
  "clickCount": 142,
  "createdAt": "2026-02-28T16:00:00.000Z",
  "lastAccessedAt": "2026-03-01T09:12:44.000Z"
}
```

---

## 🛠 Tech Stack Details

- **Framework:** .NET 9 ASP.NET Core Web API
- **ORM:** Entity Framework Core (Npgsql)
- **Primary Database:** PostgreSQL 15 (Relational persistence)
- **Cache / In-Memory DB:** Redis 7 
- **Tooling:** Docker & Docker-Compose

## 🛡️ Future Enhancements
- Implementation of a distributed locking mechanism (e.g., Redlock) for extreme high-concurrency custom alias creations.
- Rate limiting middleware per IP address to prevent abuse.
- User authentication and scoped URL management.
