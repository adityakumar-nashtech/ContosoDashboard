# Data Model

## Entities

-### Document
- **Id**: integer (PK)
- **Title**: string (required, max 250)
- **Description**: string (optional)
- **Category**: string (required)
- **ProjectId**: integer (nullable) — FK to `Project` when document is associated with a project
- **UploaderId**: integer (required) — FK to `User`
- **UploadDateUtc**: DateTime (required)
- **FileSizeBytes**: long (required)
- **ContentType**: string (required)
- **StoragePath**: string (required) — generated unique path outside webroot
- **Checksum**: string (optional) — for dedup/validation
- **Version**: integer (default 1) — increments on replacements
- **IsDeleted**: bool (soft delete for audit)
 - **ScanStatus**: string (enum: Queued, Scanning, Available, Quarantined) — represents background malware scan state

Validation rules:
- `Title` required and non-empty
- `FileSizeBytes` <= 25 * 1024 * 1024 (25 MB)
- `ContentType` must be one of allowed list (PDF, application/msword, application/vnd.openxmlformats-officedocument.wordprocessingml.document, image/*, text/*)
- `StoragePath` must be unique

State transitions:
- `Uploaded` -> (scan) `Scanned` -> `Available` or `Quarantined` (if malware found)
- `Available` -> `Replaced` (when owner uploads new version)
- `Available` -> `Deleted` (soft delete)

Queue message schema (for Azure Queue Storage):
```
{ "documentId": 123, "storagePath": "documents/2026/09/15/uuid.bin", "contentType": "application/pdf", "uploaderId": 45 }
```
The queue message should be small and idempotent-friendly; the Function should validate the document's current `Version` or `ScanStatus` before processing.

### DocumentShare
- **Id**: integer (PK)
- **DocumentId**: integer (FK to Document)
- **SharedWithUserId**: integer (FK to User) OR **SharedWithTeamId**: integer (nullable)
- **SharedByUserId**: integer
- **SharedAtUtc**: DateTime
- **Permissions**: enum (View, Download, Edit)

### DocumentAudit
- **Id**: integer (PK)
- **DocumentId**: integer
- **Action**: string (Upload, Download, Delete, Share, Replace)
- **PerformedByUserId**: integer
- **PerformedAtUtc**: DateTime
- **Details**: string (optional)

Notes:
- Documents must be stored outside the web-accessible `wwwroot` and served via controller endpoints that validate authorization.
- Use EF Core migrations to add tables, and consider an indexed `Document` table on `UploaderId`, `ProjectId`, and `Title` for search performance.
