# MDVision EHR — Clinical Management System

A full-featured, enterprise-grade Electronic Health Records (EHR) platform built on **.NET Framework 4.8**, **ASP.NET MVC 5**, and **SQL Server**. Covers the complete clinical, billing, and administrative lifecycle of an ambulatory practice.

---

## Table of Contents

- [Architecture Overview](#architecture-overview)
- [Solution Structure](#solution-structure)
- [Project Descriptions](#project-descriptions)
- [Tech Stack](#tech-stack)
- [Key Features](#key-features)
- [Healthcare Standards & Integrations](#healthcare-standards--integrations)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)

---

## Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                        Clients                              │
│          Browser (MVC)          Mobile / Third-party        │
└────────────────┬────────────────────────┬───────────────────┘
                 │                        │
    ┌────────────▼──────────┐  ┌──────────▼────────────┐
    │   MDVision.IEHR       │  │   MDVision.WebAPI      │
    │   ASP.NET MVC 5       │  │   ASP.NET Web API 2    │
    │   (Hospital / Clinic  │  │   OWIN / OAuth2        │
    │    User Interface)    │  │   REST + JWT           │
    └────────────┬──────────┘  └──────────┬────────────┘
                 └──────────┬─────────────┘
                            │
              ┌─────────────▼─────────────┐
              │    MDVision.Business       │
              │    Business Logic Layer    │
              │  BLL classes, EDI, CCDA,  │
              │  CCM, Claims, Batch        │
              └─────────────┬─────────────┘
                            │
         ┌──────────────────┼──────────────────┐
         │                  │                  │
┌────────▼───────┐ ┌────────▼───────┐ ┌────────▼───────┐
│ MDVision.      │ │ MDVision.      │ │ MDVision.      │
│ DataAccess     │ │ Dataset        │ │ EDIParser      │
│ DAL + DBManager│ │ Typed DataSets │ │ X12/HIPAA      │
│ Stored Procs   │ │ (ADO.NET XSD)  │ │ 270/271/835/837│
└────────┬───────┘ └────────────────┘ └────────────────┘
         │
┌────────▼───────────────────────────────────────────────┐
│                  SQL Server                             │
│   Schemas: Clinical · Billing · Admin · Patient · CCM  │
│   All data access via named stored procedures          │
└────────────────────────────────────────────────────────┘

Cross-cutting (all layers):
  MDVision.Model   — Domain models, ViewModels, DTOs
  MDVision.Common  — Logging, session, encryption, utilities
```

---

## Solution Structure

```
MDVision.sln
│
├── MDVision.IEHR/                  # ASP.NET MVC 5 web application (primary UI)
│   ├── Controllers/
│   │   ├── Admin/
│   │   ├── Billing/
│   │   ├── Patient/
│   │   ├── Scheduler/
│   │   ├── DashBoard/
│   │   ├── Messages/
│   │   └── Batch/
│   ├── Views/
│   │   ├── AOETemplate/
│   │   ├── ReviewOfSystemRMP/
│   │   ├── Scheduler/
│   │   └── Shared/
│   ├── Areas/
│   │   └── CCM/                    # Chronic Care Management module
│   │       ├── Controllers/
│   │       ├── Services/
│   │       └── Helpers/
│   ├── Content/                    # Themes: Black, Blue, Gray, Default
│   ├── App_Start/                  # RouteConfig, BundleConfig, WebApiConfig
│   └── Web.config
│
├── MDVision.WebAPI/                # REST API — mobile / third-party access
│   ├── Controllers/
│   │   ├── AccountController.cs
│   │   ├── ClinicalController.cs
│   │   ├── PatientController.cs
│   │   ├── SchedulerController.cs
│   │   ├── NotesController.cs
│   │   ├── APILookupsController.cs
│   │   └── PrivilegesController.cs
│   ├── Providers/                  # OAuth2: Simple, Facebook, Google
│   ├── Helpers/
│   ├── Models/
│   ├── Filters/
│   └── Startup.cs                  # OWIN pipeline
│
├── MDVision.Business/              # Business Logic Layer
│   ├── BLL/
│   │   ├── BLLClinical.cs
│   │   ├── BLLAdmin.cs
│   │   ├── BLLPatient.cs
│   │   ├── BLLSchedule.cs
│   │   ├── BLLBillingClaim.cs
│   │   ├── BLLPayment.cs
│   │   ├── BLLERA.cs
│   │   ├── BLLCCDA.cs
│   │   ├── BLLCCM.cs
│   │   ├── BLLPQRS.cs
│   │   ├── BLLCQM.cs
│   │   ├── BLLBatch.cs
│   │   └── BLLReports.cs
│   ├── BCommon/
│   │   ├── DBManager.cs
│   │   ├── ClaimSubmission.cs
│   │   ├── EDI270Parser.cs
│   │   ├── EDI271Parser.cs
│   │   ├── FTP.cs / SFTP.cs
│   │   └── BarCode39.cs
│   ├── ClaimScrubber/
│   ├── AppointmentReminders/       # TeleVox SMS/voice integration
│   └── MedTextReferrals/
│
├── MDVision.DataAccess/            # Data Access Layer
│   ├── DCommon/
│   │   ├── DBManager.cs            # SQL Server command/connection engine
│   │   ├── DataProvider.cs
│   │   └── ClientConfiguration.cs
│   └── DAL/
│       ├── Admin/                  # DALLogin, DALUser, DALProvider, DALInsurance …
│       ├── Clinical/               # DALAllergies, DALMedications, DALVitals, DALNotes …
│       ├── Patient/                # DALPatient, DALPatientDocument, DALPatientReferral …
│       ├── Appointment/
│       ├── Schedule/
│       ├── Billing/                # DALCharge, DALPayment, DALClaim, DAL837, DAL835 …
│       ├── CCM/
│       ├── ERA/
│       ├── FollowUp/
│       └── Document/
│
├── MDVision.Model/                 # Domain models, ViewModels, DTOs (~350+ classes)
│   ├── Admin/
│   ├── Clinical/
│   │   ├── Medical/               # Allergies, Medications, Vitals, Labs, Orders
│   │   └── Notes/
│   ├── Billing/                   # ERA, EOB, Claims, Statements
│   ├── Patient/
│   ├── CCM/
│   ├── CCDA/
│   ├── Dashboard/
│   ├── Lookups/
│   └── Native/                    # API request/response DTOs
│
├── MDVision.Common/                # Cross-cutting utilities
│   ├── Logging/                   # MDVLogger, ERALogger
│   ├── Shared/                    # MDVSession, MDVApplication, MDVCustomException
│   └── Utilities/                 # CommonFunc, MDVUtility (encryption, formatting)
│
├── MDVision.Dataset/               # Typed ADO.NET DataSets (XSD-generated, 50+ sets)
│   └── DS*.xsd / DS*Designer.cs
│
└── MDVision.EDIParser/             # X12/HIPAA EDI transaction parsers
    ├── EDI270Parser.cs             # Eligibility inquiry
    ├── EDI271Parser.cs             # Eligibility response
    ├── EDI835Parser.cs             # Remittance Advice (ERA)
    ├── EDI837Parser.cs             # Professional claim submission
    ├── EDI837Segment/              # Claim segment models
    └── XSD/                        # DS270, DS271, DS277, DSHCFA typed datasets
```

---

## Project Descriptions

| Project | Type | Purpose |
|---|---|---|
| **MDVision.IEHR** | ASP.NET MVC 5 Web App | Primary clinical UI for providers and staff |
| **MDVision.WebAPI** | ASP.NET Web API 2 (OWIN) | REST API for mobile and third-party integrations |
| **MDVision.Business** | Class Library | Orchestrates workflows, claims, CCM, CCDA, batch jobs |
| **MDVision.DataAccess** | Class Library | Repository pattern over SQL Server stored procedures |
| **MDVision.Model** | Class Library | Shared domain models, ViewModels, API DTOs |
| **MDVision.Common** | Class Library | Logging, session management, encryption, file I/O |
| **MDVision.Dataset** | Class Library | Typed ADO.NET DataSets for SQL result sets |
| **MDVision.EDIParser** | Class Library | Parse and generate X12 EDI 270/271/835/837 transactions |

---

## Tech Stack

| Concern | Technology | Version |
|---|---|---|
| Runtime | .NET Framework | 4.8 |
| Web UI | ASP.NET MVC | 5.2.3 |
| REST API | ASP.NET Web API | 5.2.3 |
| Auth middleware | Microsoft OWIN | 4.2.2 |
| Identity | ASP.NET Identity | 2.0.1 |
| Token auth | jose-jwt | 2.4.0 |
| Real-time | SignalR | 2.2.1 |
| ORM (identity only) | Entity Framework | 6.1.0 |
| Clinical data access | ADO.NET + stored procs | — |
| JSON | Newtonsoft.Json | 13.0.3 |
| PDF generation | iTextSharp + XMLWorker | 5.5.9 |
| HL7 parsing | NHapi | 2.4.0 |
| HTML parsing | HtmlAgilityPack | 1.4.9 |
| SFTP | WinSCP.NET | 1.2.8 |
| Error logging | ELMAH | 1.2.2 |
| Parallel processing | SmartThreadPool | — |
| Database | SQL Server | (clustered) |
| Client scripting | TypeScript / jQuery | 3.2 / — |
| Rich text | CKEditor | — |

---

## Key Features

### Clinical
- Patient demographics, insurance, referrals, and document management
- Problem lists, vital signs, physical exams, and history (family, surgical, social)
- Medications with clinical decision support (CDS) alerts
- Allergy tracking and reconciliation
- Lab and radiology order management with result routing
- Immunization tracking with registry integration
- SOAP clinical notes with customizable templates
- Custom AOE (Area of Exam) and ROS (Review of Systems) templates
- Consultation and procedure orders

### Billing & Revenue Cycle
- Charge capture and posting
- EDI 837 claim generation and submission (Professional)
- Claim scrubbing (AlphaII integration)
- Claim submission via clearinghouse (Navicure)
- EDI 835 remittance advice (ERA) processing and auto-posting
- Patient statements and AR follow-up workflows
- Insurance eligibility verification (EDI 270/271, real-time)
- Payment allocation and refund processing

### Scheduling
- Provider schedule management with block hours
- Visit type and duration groups
- Appointment reminders via TeleVox (SMS and voice)
- Dashboard with tasks, messages, and appointment views

### Quality & Compliance
- PQRS (Physician Quality Reporting System) export
- CQM (Continuous Quality Measures) tracking and reporting
- Meaningful Use (MU) alert dashboard
- CCDA (Consolidated Clinical Document Architecture) generation
- Direct Protocol integration for secure clinical data exchange
- Full audit event logging

### Chronic Care Management (CCM)
- Patient enrollment and care plan management
- Health Risk Assessment (HRA)
- Monthly care coordination tracking

### Administration
- Multi-provider, multi-facility configuration
- Role-based access control (RBAC)
- EDI and HL7 sender/receiver setup
- Batch import/export processing (parallel via SmartThreadPool)
- Multi-theme UI (Black, Blue, Gray, Default)

---

## Healthcare Standards & Integrations

| Standard / System | Purpose |
|---|---|
| **EDI X12 837P** | Professional claim submission |
| **EDI X12 835** | Electronic Remittance Advice (ERA) |
| **EDI X12 270/271** | Real-time insurance eligibility |
| **EDI X12 277** | Claim status inquiry |
| **HL7 v2.x (NHapi)** | Immunization data exchange |
| **CCDA / C-CDA** | Clinical summary documents for HIE |
| **Direct Protocol** | Secure clinical messaging (CCDA exchange) |
| **OAuth2 / Bearer tokens** | Mobile and third-party API access |
| **SSRS (Report Viewer)** | Crystal/SQL reporting |
| **AlphaII** | Claim scrubbing service (WCF) |
| **Navicure** | Clearinghouse claim submission (WCF) |
| **TeleVox** | Appointment reminder (SMS/voice) |
| **Apple APNS / Google FCM** | Mobile push notifications |

---

## Prerequisites

- Visual Studio 2019 or later
- .NET Framework 4.8 SDK
- SQL Server 2016 or later
- NuGet Package Manager (packages restore automatically on build)
- IIS or IIS Express for local hosting

---

## Getting Started

1. **Clone the repository**
   ```bash
   git clone git@github.com:ZiyadMehmoodDBA/clinical.git
   cd clinical
   ```

2. **Restore NuGet packages**
   Open `MDVision.sln` in Visual Studio. NuGet will restore all packages on first build, or run:
   ```bash
   nuget restore MDVision.sln
   ```

3. **Configure the database connection**
   Update the connection string in:
   - `MDVision.IEHR/Web.config` → `<connectionStrings>`
   - `MDVision.WebAPI/Web.config` → `<connectionStrings>`
   - `MDVision.DataAccess/App.config` → `<connectionStrings>`

4. **Set startup projects**
   In Visual Studio → right-click solution → *Set Startup Projects* → select both `MDVision.IEHR` and `MDVision.WebAPI` as startup projects.

5. **Build and run**
   Press `F5` or `Ctrl+F5`. The MVC application and Web API will start on separate ports.

> **Note:** The OCR scanner asset (`MDVision.IEHR/Resources/OCRScanner/id_scanner_ocr.zip`) exceeds GitHub's 100 MB file limit and is excluded from the repository. Obtain it separately and place it in that folder before building features that depend on it.
