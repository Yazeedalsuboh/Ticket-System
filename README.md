# 🎫 Client Support Ticketing System

A modern web-based ticketing platform to streamline client support operations across company products. Built using **Angular** (frontend) and **.NET Web API** (backend) following a **3-tier architecture** with a clean, modular, and secure design.

---

## 🚀 Features Overview

### ✅ Core System Features
- **Client Ticket Submission**: Clients submit and manage support tickets.
- **Support Team Collaboration**: Employees view, comment, and resolve tickets.
- **Manager Controls**:
  - Assign/reassign tickets
  - Manage client and employee accounts
  - Dashboard with analytics and KPIs
- **Ticket Lifecycle Management**: Creation → Assignment → Interaction → Closure.

### 🔒 Authentication & Registration
- Manual DB registration for **Support Managers**.
- Self-registration for **External Clients**.
- In-system registration by manager for **Support Employees**.
- Login using **username/email + password**.
- **Account Activation/Deactivation** feature.
- **Forgot/Change Password** for clients and employees.
- System-generated password sent via email to new employees with mandatory change on first login.

### 💡 AI-Powered Enhancements
- 🔍 **Ticket Priority Detection** to help employees decide which ticket to start with.
- 🌐 **Language Translation** for ticket titles/descriptions.
- ✨ **Content Enhancement** for ticket titles and descriptions.
- 🤖 **AI Chat Assistant** available for all system roles.

### 🎨 User Experience
- 📱 Responsive, user-friendly UI/UX with toast notifications for errors.
- 🌍 Multi-language support (bonus).
- 📊 Interactive dashboard with charts and counters.
- 🎯 Advanced ticket filtering (by status, date, client, assignee, etc.).
- 🏠 Public-facing landing page.

---

## 🧱 Architecture

The system is based on a **3-Tier Architecture**:
- **UI Layer (Angular)**  
  Handles user interactions, views, and real-time feedback.

- **Business Logic Layer (ASP.NET Web API)**  
  Manages application logic, AI services, and validation.

- **Data Access Layer (MSSQL)**  
  Handles database interaction and persistence.

---

## 🧪 Tech Stack

- **Frontend**: Angular (TypeScript, HTML, SCSS)
- **Backend**: .NET 6+ Web API (C#)
- **Database**: Microsoft SQL Server
- **AI Services**: Integrated using internal logic (can be linked to external APIs like Azure AI or OpenAI)
