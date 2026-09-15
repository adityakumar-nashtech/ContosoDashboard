# Document API Contract

All endpoints require authentication. Authorization rules: users can act on documents they own or documents associated with projects they have access to; project managers have elevated rights for project documents.

## POST /api/documents
- Description: Upload a new document with metadata and file content.
- Request: multipart/form-data
  - `file`: binary file
  - `title`: string
  - `description`: string (optional)
  - `category`: string
  - `projectId`: integer (optional)
- Response 201 Created
  - Body: `{ "id": 123, "title": "...", "uploadDateUtc": "...", "status": "Scanning" }`
- Errors: 400 (validation), 413 (file too large), 415 (unsupported media type), 401/403 (auth)
  - Notes: The upload endpoint MUST enqueue a queue message for background virus scanning after persisting the file and `Document` record. The initial response may report `ScanStatus` as `Queued` or `Scanning`. Clients should poll or rely on in-app notifications to learn when the document becomes `Available`.

## GET /api/documents
- Description: List documents visible to the current user. Supports filtering and paging.
- Query params: `projectId`, `query` (title/tags), `page`, `pageSize`, `sort`
- Response 200 OK
  - Body: `{ "items": [ {"id":1,"title":"...","uploaderId":...}], "total": 123 }`

## GET /api/documents/{id}
- Description: Fetch metadata for a single document (does not stream file content).
- Response 200 OK: Document metadata.

## GET /api/documents/{id}/download
- Description: Download the document file blob. Controller MUST validate authorization before streaming.
- Response 200 OK: Content-Type matches `ContentType`, `Content-Disposition: attachment; filename="title.ext"`
 - Notes: The controller should reject downloads for documents with `ScanStatus = "Quarantined"` until resolved. Downloads for `Queued`/`Scanning` items may be blocked or allowed per policy; default behavior in this design: block downloads until `Available`.

## PUT /api/documents/{id}
- Description: Update document metadata (title, description, category) or replace file via a new upload.
- Request: JSON for metadata update, multipart/form-data for replacement.
- Response 200 OK: updated metadata

## POST /api/documents/{id}/share
- Description: Share a document with a user or team.
- Request: `{ "userId": 12 }` or `{ "teamId": 5, "permissions": "View" }`
- Response 200 OK

## DELETE /api/documents/{id}
- Description: Soft-delete a document (mark `IsDeleted`). Admins or owners can permanently remove with a different endpoint.
- Response 204 No Content

Security notes:
- Endpoints must validate project membership and per-document sharing rules.
- File streaming endpoints must never serve files directly from `wwwroot`—use protected streaming with Authorization checks.
- Background scanning integration notes:
  - The Azure Function or scanner service must authenticate when accessing the storage blob and when updating the document record (via secure API or direct DB access using managed identity in prod).
  - When scan completes, the scanner SHOULD update the document `ScanStatus`, create a `DocumentAudit` entry, and optionally send an in-app notification to relevant users.

Examples and sample request/response bodies should be implemented in a `contracts/openapi.yml` if desired, but the above covers the required surface area for the feature.
