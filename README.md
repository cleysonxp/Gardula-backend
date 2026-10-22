# 🚀 Gardula Backend

Backend for **Gardula**, a personal finance management application developed with **C# and .NET 8**.

The project follows a modular architecture, separating responsibilities across the API, application logic, domain, and infrastructure layers.

---

## 📌 About the Project

Gardula aims to centralize personal financial management, allowing users to manage accounts, credit cards, transactions, and other financial activities.

The backend provides a REST API for the frontend and handles business rules, authentication, data persistence, and data access.

---

## ✨ Features

The backend currently supports the following features:

### 🔐 Authentication and User Management

- User registration
- User authentication and login
- Refresh token
- Logout
- Authenticated user information

### 💰 Financial Management

- Account management
- Credit card management
- Transaction creation, updates, and deletion
- Transfers between accounts
- Credit card purchases
- Installment purchases
- Credit card invoice payments
- Installment management

### 📊 Financial Insights

- Financial summaries
- Date range filtering
- Transaction filtering
- Pagination
- Transaction sorting
- Consolidated dashboard data
- Financial reports

---

## 🏗️ Architecture

The backend is organized into separate projects, each with its own responsibilities.

```text
Gardula
│
├── Gardula.Api
├── Gardula.Application
├── Gardula.Domain
├── Gardula.Infrastructure
└── Gardula.UnitTests
```

### 📂 Project Responsibilities

- **Gardula.Api** — API endpoints and HTTP request handling.
- **Gardula.Application** — application services and use cases.
- **Gardula.Domain** — domain entities and business concepts.
- **Gardula.Infrastructure** — data persistence and infrastructure implementations.
- **Gardula.UnitTests** — unit tests for application components.

---

## 🛠️ Technologies

- 💻 **C#**
- ⚙️ **.NET 8**
- 🌐 **ASP.NET Core Web API**
- 🗃️ **Entity Framework Core**
- 🐬 **MySQL**
- 🔑 **JWT Authentication**

---

## 🚀 Getting Started

### 📋 Prerequisites

- .NET 8 SDK
- MySQL
- Git

### 📥 Installation

Clone the repository:

```bash
git clone <repository-url>
```

Navigate to the project directory:

```bash
cd <repository-folder>
```

Configure the database connection and required authentication settings.

Run the API:

```bash
dotnet run --project Gardula.Api
```

The API address will be displayed in the terminal.

---

## 📖 API Documentation

If Swagger is enabled in the current environment, it can be used to explore the available endpoints and test API requests.

---

## 📍 Project Status

**MVP — Personal Validation Phase**

The project has reached the MVP stage. The next steps focus on using the application in real-world scenarios, identifying issues, and implementing improvements based on actual usage.

---

## 👨‍💻 About

Gardula is a personal project developed to apply software engineering concepts to a practical financial management system, focusing on modular architecture, business rules, REST API development, and maintainable code.
