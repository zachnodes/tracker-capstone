# melee-tracker-capstone
## Architecture

Browser (React/Next.js)
    ↕ HTTP + JWT Bearer token
ASP.NET Core Web API
    ├── Auth (JWT issue/validate)
    ├── Dashboard endpoints (sets, tournaments, opponents)
    ├── SLP upload → S3
    └── RabbitMQ message publisher (tells parser "go process this file")
    ↕
Postgres