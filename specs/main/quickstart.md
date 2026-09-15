# Quickstart: Document Management (validation guide)

## Prerequisites
- .NET 8 SDK installed
- Local repository checked out
- Optional: `dotnet-ef` tool installed for migrations (`dotnet tool install --global dotnet-ef`)

## Dev configuration (recommended)
1. Add SQLite dev connection to `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=contoso_documents_dev.db"
  }
}
```

2. Ensure EF Core uses SQLite in `Program.cs` when `ASPNETCORE_ENVIRONMENT=Development`.

## Run the app
From repository root run:

```bash
dotnet run --project ContosoDashboard\ContosoDashboard.csproj
```

Open the app in a browser at the configured URL (usually `https://localhost:5001` or the URL printed by the runtime).

## Validation scenarios

### 1) Upload a document (end-to-end)
- Steps:
  1. Sign in as a test user (use the app's mock or local auth flow).
  2. Navigate to the Document Upload page.
  3. Select a supported file < 25 MB and provide title, category, and optional project.
  4. Submit and observe success message.
- Expected outcome: The document appears in the user's document list with metadata and correct project association.

### 2) Upload a file exceeding limit
- Steps: Attempt to upload a file > 25 MB.
- Expected outcome: The app rejects the file with a clear error (413 or client-side validation).

### 3) Permission check
- Steps: Upload a document to a project as a different user; attempt to view it as an unauthorized user.
- Expected outcome: Unauthorized user cannot view or download the document.

### 4) Download/preview
- Steps: From the document list, click download/preview for a PDF or image.
- Expected outcome: File is streamed with correct `Content-Type` and authorization validated.

### 5) Share a document
- Steps: Share a document with another test user and verify they receive an in-app notification and can access the document.
- Expected outcome: Recipient can access the document according to the granted permissions.

## Migration notes
- To switch to SQL Server for staging/production, update `DefaultConnection` and ensure migrations are applied with `dotnet ef database update`.

## Quick checks
- Verify audit events are recorded for upload/download/delete actions.
- Verify files are stored outside `wwwroot` and cannot be accessed directly by guessing URLs.



