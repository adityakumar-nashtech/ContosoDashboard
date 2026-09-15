# Document Upload & Management - Specification

## Overview

Add document upload and management to ContosoDashboard. Centralized, secure document storage with metadata, search, project association, sharing, and basic auditing.

---

## User Stories

### US1 - Upload Documents (Priority: P1)
As an authenticated user, I can upload one or more documents with title, optional description, category, optional project association, and tags so that documents are stored securely and associated with my account.

Acceptance Criteria:
- Supported file types: PDF, Word, Excel, PowerPoint, TXT, JPEG, PNG
- Max file size 25 MB per file
- Required metadata: Document title, Category (predefined list)
- System captures upload date/time, uploader, file size, MIME type (255 chars)
- Files stored outside `wwwroot` using GUID-based filenames
- Upload flow shows progress and success/error messages
- Files scanned for malware before final storage

### US2 - View, Search, and Organize Documents (Priority: P1)
As a user, I can view my documents and project documents, search by title/description/tags/uploader/project, and filter/sort results so I can find documents quickly.

Acceptance Criteria:
- "My Documents" list shows title, category, upload date, file size, associated project
- Sort by title, upload date, category, file size
- Filter by category, project, date range
- Project document view shows all documents for that project to team members
- Search returns results within 2 seconds and only documents the user can access

### US3 - Download, Preview, Edit, Replace, Delete (Priority: P1)
As a user, I can download or preview documents I have access to, edit metadata for documents I uploaded, replace the file content, and delete my documents.

Acceptance Criteria:
- Download endpoint enforces authorization
- Preview available for PDFs and images in-browser
- Owners can edit title, description, category, tags
- Owners can replace file content; replacement follows same validation and storage rules
- Deletion requires confirmation and permanently removes file and metadata

### US4 - Share Documents & Notifications (Priority: P2)
As a document owner, I can share documents with specific users or teams; recipients receive in-app notifications and see shared items in "Shared with Me".

Acceptance Criteria:
- Share with users or teams; create DocumentShare records
- Notifications sent to recipients via existing NotificationService
- Shared docs appear in recipients' Shared view
- Shared documents respect project-level permissions

### US5 - Integration with Tasks & Dashboard (Priority: P2)
As a user, I can attach documents to tasks and see "Recent Documents" on the dashboard.

Acceptance Criteria:
- Attach uploads from a task detail page and auto-associate to the task's project
- Dashboard shows last 5 documents uploaded by the user
- Document counts included in summary cards

### US6 - Reporting & Audit (Priority: P3)
As an administrator, I can view audit logs and generate basic reports about uploads, types, and activity.

Acceptance Criteria:
- Log uploads, downloads, deletions, share actions
- Admin report endpoints for top file types, active uploaders, access patterns

---

## Non-Functional Requirements
- Must work offline with local filesystem storage
- Implement `IFileStorageService` abstraction with `LocalFileStorageService` implementation
- Database: `DocumentId` integer; `Category` stored as text; `FileType` length 255
- Performance: upload <= 30s for 25MB; list/search <= 2s for up to 500 documents

## Notes
- Storage path pattern: `{userId}/{projectId or 'personal'}/{guid}.{ext}`
- Save file to disk before inserting DB record
- Ensure malware scanning and extension whitelist
