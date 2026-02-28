# Production-Grade URL Shortener Service Instructions

## Objective

Design and implement a production-ready, scalable URL shortener similar
to Bitly or TinyURL using .NET Core Web API, PostgreSQL/SQL Server, and
Redis.

------------------------------------------------------------------------

## Functional Requirements

### Create Short URL

POST /api/url

Request: { "longUrl": "https://example.com/some/very/long/url",
"customAlias": "optional" }

Response: { "shortUrl": "https://short.ly/abc123" }

Requirements: - Generate unique short code (6--8 characters) - Support
custom aliases - Prevent duplicates - Store in database - Store in Redis
cache

------------------------------------------------------------------------

### Redirect

GET /{shortCode}

Requirements: - Lookup Redis first - Fallback to database - Redirect to
original URL - Increment click count - Update last accessed timestamp

------------------------------------------------------------------------

### Analytics

GET /api/url/{shortCode}/stats

Response: { "longUrl": "...", "shortCode": "...", "clickCount": 0,
"createdAt": "...", "lastAccessedAt": "..." }

------------------------------------------------------------------------

## Non-Functional Requirements

-   High scalability
-   Low latency
-   High availability
-   Fault tolerant
-   Secure
-   Production ready

------------------------------------------------------------------------

## Technical Stack

Backend: - .NET Core Web API - C# - Entity Framework Core - PostgreSQL
or SQL Server - Redis

Architecture: - Clean Architecture - Repository Pattern - Service
Layer - Dependency Injection

Infrastructure: - Docker support

------------------------------------------------------------------------

## Architecture Layers

Domain Layer: - Entities

Application Layer: - Services - Interfaces

Infrastructure Layer: - Repositories - Redis integration

API Layer: - Controllers

------------------------------------------------------------------------

## Database Schema

Table: UrlMappings

Columns: - Id (GUID) - LongUrl (string) - ShortCode (string, unique) -
CreatedAt (DateTime) - LastAccessedAt (DateTime) - ClickCount (int) -
ExpirationDate (optional)

------------------------------------------------------------------------

## Short Code Generator

Requirements: - Base62 encoding or equivalent - Length 6--8 characters -
Unique - Collision handling

Characters allowed: - a-z - A-Z - 0-9

------------------------------------------------------------------------

## Redis Requirements

Key: shortCode

Value: longUrl

Flow: 1. Check Redis 2. Check database if missing 3. Update Redis

TTL: 24 hours

------------------------------------------------------------------------

## API Endpoints

POST /api/url

GET /{shortCode}

GET /api/url/{shortCode}/stats

------------------------------------------------------------------------

## Validation

Validate: - URL format - Duplicate aliases - Invalid input

Return HTTP status codes: - 200 - 201 - 400 - 404 - 500

------------------------------------------------------------------------

## Performance

-   Redis caching
-   Async database calls
-   Efficient indexing

------------------------------------------------------------------------

## Security

-   Input validation
-   Rate limiting

Optional: - Authentication

------------------------------------------------------------------------

## Project Structure

UrlShortener/ ├── Api/ ├── Application/ ├── Domain/ ├── Infrastructure/
├── Dockerfile ├── docker-compose.yml └── instructions.md

------------------------------------------------------------------------

## Deliverables

-   Controllers
-   Services
-   Repositories
-   Redis integration
-   Database context
-   Short code generator
-   Docker support

------------------------------------------------------------------------

## Expected Outcome

System must: - Create short URLs - Redirect efficiently - Scale well -
Track analytics - Use Redis caching
