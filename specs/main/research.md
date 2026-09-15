# research.md

This document records decisions made to resolve NEEDS CLARIFICATION entries from the implementation plan for the Document Management feature.

## Decision: Development Storage - SQLite (EF Core)
- Decision: Use SQLite for local development (EF Core provider), with migrations compatible with SQL Server for production.
- Rationale: The repository is a training/offline-first application; SQLite requires no external server, keeps onboarding minimal, and EF Core migrations are compatible with SQL Server for future migration.
- Alternatives considered: SQL Server LocalDB (requires Windows dev setup), In-memory provider (not suitable for migration testing).

## Decision: Testing Frameworks
- Decision: Use xUnit for unit tests, `Microsoft.AspNetCore.Mvc.Testing` for integration tests, and `bUnit` for Blazor component tests.
- Rationale: These are standard .NET testing libraries with good support for ASP.NET Core and Blazor. They allow headless verification of UI components and end-to-end HTTP flows in CI.
- Alternatives considered: NUnit (similar capabilities), MSTest (less commonly used in newer templates).

## Decision: Performance Goals
- Decision: Target interactive responsiveness: p95 page/API responses < 500ms for typical document listing and search operations in the training environment.
- Rationale: Keeps user experience snappy for demos while being achievable without production-grade infra. Document uploads will be throttled by file size; client-side progress must be visible.
- Alternatives considered: Stricter 200ms target (overly optimistic for training infra), looser 1s target (acceptably but less snappy).

## Decision: Scale & Scope
- Decision: Design for small-scale training usage: expect up to 1k concurrent users in demonstration scenarios; ensure correctness and isolation over horizontal scaling.
- Rationale: Aligns with training/dev goals and the project's constitution; focus on security, isolation, and clear architecture rather than high-scale optimizations.
- Alternatives considered: Large-scale design (unnecessary for training, increases complexity).

## Action Items from Research
- Add EF Core SQLite dev configuration and connection string examples in `appsettings.Development.json`.
- Add test project scaffolding with xUnit and bUnit and a simple integration test verifying upload and listing flows.
- Document the migration path for switching to SQL Server in production in `quickstart.md`.

## Decision: Async Virus Scanning — Azure Functions + Queue Storage
- Decision: Use Azure Queue Storage to enqueue uploaded file messages and an Azure Function (Queue trigger) to process virus scanning asynchronously.
- Rationale: Offloading scanning to a serverless background job decouples uploads from long-running scans, keeps upload latency low for users, and provides scalable, retryable processing with built-in Azure durability. Using Queue Storage + Functions is a minimal operational surface and integrates well with Azure-hosted storage and .NET runtime.
- Implementation notes:
	- Upload flow: On successful file persistence to protected storage, create a `Document` record with `ScanStatus = "Queued"` and enqueue a message containing `{ documentId, storagePath, contentType, uploaderId }` to an Azure Queue.
	- Scanner Function: The Azure Function is triggered by Queue messages, downloads the file from storage (or accesses via shared credential), runs a virus scan (e.g., call to a third-party scanning API or local ClamAV service), updates the `Document` record `ScanStatus` to `Available` or `Quarantined`, writes an audit record, and sends an in-app notification to the uploader and any relevant project managers.
	- Error handling: The Function should use Poison Queue handling (move to dead-letter queue) for repeatedly failing messages and log scan failures; ensure idempotency by checking document `Version` or a processed flag before taking destructive actions.
	- Local development: Recommend using Azurite for Queue Storage emulation and either a local ClamAV container or a mocked scanner implementation. Mark Azure Functions integration as optional for purely local training runs; provide an emulation path in `quickstart.md`.

## Action Items (background scanning)
- Add queue message schema and enqueue logic to upload endpoint.
- Add `ScanStatus` field to `Document` entity and include status in API responses.
- Add an Azure Function project skeleton (or local emulator instructions) and document configuration in `quickstart.md`.


Decision author: speckit.plan agent
Date: 2026-09-15
