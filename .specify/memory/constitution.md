<!--
Sync Impact Report
Version change: placeholder → 1.0.0
Modified principles: initial constitution (no prior named principles)
Added sections: Core Principles, Additional Constraints, Development Workflow, Governance
Removed sections: none
Deferred items: none
-->

# ContosoDashboard Constitution

## Core Principles

### I. Training-First Security and Ethics
This project exists for learning and demonstration. All implementation, data, and authentication choices MUST prioritize safe, understandable training scenarios over production assumptions. Features that imply real-world security or deployment MUST be clearly labeled as training-only and MUST NOT be treated as production guidance.

### II. User-Value and Business Clarity
Every feature MUST map to a concrete user workflow or training objective. We deliver work that helps teams manage tasks, coordinate projects, and understand access boundaries. Unclear or speculative abstractions are not accepted without a clear user need.

### III. Test-First Quality
New logic and policy changes MUST be validated by executable tests or reproducible verification before they are accepted. When behavior changes, the team MUST confirm the scenario, confirm the expected result, and then implement the minimal fix. Regression testing is required for security checks, authorization rules, and user-isolation logic.

### IV. Access Integrity and Least Privilege
Authentication, authorization, and service checks MUST enforce user isolation. Any page, service, or data lookup that depends on user identity MUST verify that the current user has permission to view or modify the requested resource. IDOR prevention and role-based access checks are non-negotiable.

### V. Simplicity and Maintainability
The codebase MUST favor clear names, explicit dependencies, and small focused components over hidden behavior or over-engineering. Shared logic MUST be organized into reusable services and data models with documentation sufficient for training usage. The system MUST remain approachable for learners.

## Additional Constraints

ContosoDashboard is a training application and MUST remain offline-first, local-only, and intentionally limited. Production deployment patterns, cloud dependencies, and external identity services are not in scope unless explicitly labeled as future migration guidance. Mock authentication is allowed only for learning exercises and MUST NOT represent a production security model.

The repository MUST keep the architecture understandable: Models, Data, Services, Pages, and Shared components remain distinct; business logic stays separate from UI concerns; and any infrastructure abstraction remains explicit and well documented.

## Development Workflow

All work MUST be driven by small, reviewable changes. Requirements and implementation decisions MUST be documented in the repository or feature artifacts before code is considered done. Pull requests MUST state the user value, affected area, and verification performed.

For user-facing behavior and security-sensitive features, verification MUST include the relevant scenario and expected outcome. The team MUST use small iterative changes, validate each change, and avoid unreviewed shortcuts that bypass access controls or data isolation rules.

## Governance

This Constitution supersedes informal practices and governs all project decisions. Any rule in this document takes precedence over convenience, speed, or demo-only shortcuts. A feature may not bypass an access rule, testing requirement, or ethical constraint simply because it is easier to ship.

Amendments require a documented pull request that explains the reason for the change, identifies affected principles, and includes migration or compatibility notes if behavior changes. Reviews MUST confirm the amendment is consistent with this Constitution and does not weaken user safety, data boundaries, or training clarity.

Versioning uses semantic versioning:
- MAJOR: backward-incompatible removal or redefinition of a core principle or governance rule
- MINOR: new principle or section added, or material expansion of a requirement
- PATCH: wording, clarification, or non-semantic refinement

Compliance review is required for all changes that touch security, authorization, workflow, or architecture. Reviewers MUST verify that the change preserves the intent of the affected principles and that the repository remains aligned with the training-only constraints of the project.

**Version**: 1.0.0 | **Ratified**: 2026-09-15 | **Last Amended**: 2026-09-15
