---
description: "Task list for Document Upload & Management feature"
---

# Tasks: Document Upload & Management

**Input**: plan.md, spec.md, data-model.md, research.md, contracts/

## Phase 1: Setup (Shared Infrastructure)

- [ ] T001 [P] Create Document model in ContosoDashboard/Models/Document.cs
- [ ] T002 [P] Create DocumentShare model in ContosoDashboard/Models/DocumentShare.cs
- [ ] T003 [P] Create DocumentAudit model in ContosoDashboard/Models/DocumentAudit.cs

---

## Phase 2: Foundational (Blocking Prerequisites)

- [ ] T004 [P] Implement IFileStorageService interface in ContosoDashboard/Services/IFileStorageService.cs
- [ ] T005 [P] Implement LocalFileStorageService in ContosoDashboard/Services/LocalFileStorageService.cs (store files outside wwwroot, implement UploadAsync/DeleteAsync/DownloadAsync/GetUrlAsync)
- [ ] T006 [P] Add DbSet<Document>, DbSet<DocumentShare>, DbSet<DocumentAudit> to Data/ApplicationDbContext.cs
- [ ] T007 Create EF Core migration for document tables and commit migration files to Migrations/ (see specs/main/quickstart.md for commands)
- [ ] T008 Implement DocumentService in ContosoDashboard/Services/DocumentService.cs (orchestrates validation, storage, DB insert, enqueue scan, notifications)
- [ ] T009 Implement DocumentsController with API endpoints in Controllers/DocumentsController.cs per specs/main/contracts/document-api.md
- [ ] T010 Implement a background scanner worker in ContosoDashboard/Services/DocumentScanWorker.cs (process queue messages, update ScanStatus, write DocumentAudit entries)

---

## Phase 3: User Story 1 - Upload Documents (Priority: P1) 🎯 MVP

**Goal**: Allow users to upload documents with metadata, store files securely, and report scan status.

**Independent Test**: Upload a supported file <=25MB and verify DB record, file saved to disk, and ScanStatus queued.

- [ ] T011 [P] [US1] Implement POST /api/documents upload handling in Controllers/DocumentsController.cs
- [ ] T012 [US1] Implement file validation (size, extension whitelist, content-type) in Services/DocumentService.cs
- [ ] T013 [US1] Implement server-side save sequence in Services/DocumentService.cs: generate unique path → save file to disk (outside wwwroot) → insert Document record → enqueue scan
- [ ] T014 [US1] Add client-side upload UI and progress indicator in Pages/DocumentUpload.razor
- [ ] T015 [P] [US1] Add a contract test for POST /api/documents in tests/contract/TestDocumentsUpload.cs

---

## Phase 4: User Story 2 - View, Search, and Organize (Priority: P1)

**Goal**: Provide listing, filtering, sorting and search of documents the user can access.

**Independent Test**: Create multiple documents and verify listing, filters, and search return correct results and respect permissions.

- [ ] T016 [US2] Implement GET /api/documents list endpoint in Controllers/DocumentsController.cs with paging, filtering, and sorting
- [ ] T017 [US2] Implement search in Services/DocumentService.cs (title, description, tags, uploader, project) and index recommended fields in DB
- [ ] T018 [US2] Implement "My Documents" and Project Documents UI in Pages/Documents.razor and Pages/ProjectDetails.razor
- [ ] T019 [P] [US2] Add integration tests for document listing and search in tests/integration/TestDocumentsListing.cs

---

## Phase 5: User Story 3 - Download, Preview, Edit, Replace, Delete (Priority: P1)

**Goal**: Allow authorized users to download/preview, edit metadata, replace file contents, and delete documents they own or manage.

**Independent Test**: Upload a PDF and verify in-browser preview, metadata edits, replacement file versioning, and deletion flow.

- [ ] T020 [US3] Implement GET /api/documents/{id}/download and preview behavior in Controllers/DocumentsController.cs (authorize, block quarantined items)
- [ ] T021 [US3] Implement metadata update and file replace (PUT /api/documents/{id}) in Controllers/DocumentsController.cs and Services/DocumentService.cs (increment Version)
- [ ] T022 [US3] Implement delete (soft delete) in Controllers/DocumentsController.cs and permanent removal job in Services/DocumentService.cs
- [ ] T023 [P] [US3] Add tests for download/preview and metadata updates in tests/integration/TestDocumentsOperations.cs

---

## Phase 6: User Story 4 - Share Documents & Notifications (Priority: P2)

**Goal**: Enable owners to share documents with users/teams and notify recipients.

**Independent Test**: Share a document with another user and verify they receive a notification and see it in "Shared with Me".

- [ ] T024 [US4] Implement POST /api/documents/{id}/share in Controllers/DocumentsController.cs (create DocumentShare records)
- [ ] T025 [US4] Integrate NotificationService: trigger in Services/DocumentService.cs when share occurs (use Services/NotificationService.cs)
- [ ] T026 [P] [US4] Add UI elements for sharing and "Shared with Me" view in Pages/SharedDocuments.razor

---

## Phase 7: Integration with Tasks & Dashboard (Priority: P2)

**Goal**: Attach documents to tasks and show recent documents on dashboard.

- [ ] T027 [US5] Add attach-from-task flow and auto-associate to the task's project in Pages/Tasks.razor and Controllers/DocumentsController.cs
- [ ] T028 [US5] Add "Recent Documents" widget to App.razor or MainLayout.razor and implement server method in Services/DocumentService.cs
- [ ] T029 [P] [US5] Update dashboard summary cards to include document counts (e.g., in Services/DashboardService.cs)

---

## Phase 8: Reporting & Audit (Priority: P3)

**Goal**: Record document activities and expose basic admin reports.

- [ ] T030 [US6] Implement DocumentAudit writes across upload/download/delete/share/replace flows in Services/DocumentService.cs
- [ ] T031 [US6] Implement admin report endpoints in Controllers/Admin/DocumentsReportController.cs and basic UI in Pages/Admin/DocumentsReport.razor

---

## Phase N: Polish & Cross-Cutting Concerns

- [ ] T032 [P] Documentation: Update specs/quickstart.md with local setup and migration steps (specs/main/quickstart.md)
- [ ] T033 [P] Add sample seed data and a seed script in Data/Seed/DocumentSeed.cs
- [ ] T034 [P] Security review and ensure endpoints validate authorization (Controllers/ and Services/ changes)
- [ ] T035 [P] Performance tuning: add DB indexes on Document(UploaderId, ProjectId, Title) in migrations

---

## Dependencies & Execution Order

- Setup (Phase 1) → Foundational (Phase 2) → User Stories (Phase 3+)
- User stories may run in parallel after foundational work completes

---

## Implementation Strategy

- MVP: Complete Phase 1 and Phase 2, then implement Phase 3 (US1) to deliver upload capability for demos

---

