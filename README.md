# 🎓 Inquire

**Inquire** is a modern, student-first knowledge platform designed to bring together the best of **Stack Overflow**, **Reddit**, and community-driven tools — all tailored to the needs of students. Built with the latest technologies in the .NET ecosystem, Inquire makes it easier than ever for students to **ask questions**, **share insights**, **chat**, and **stay informed** within their academic environments.

---

## ✨ Overview

Inquire isn’t just a Q&A site — it’s a **unified platform for student collaboration**, built by students, for students.

Whether you're:
- Asking tough course-related questions
- Sharing code snippets or project ideas
- Posting campus-wide announcements
- Hosting informal discussions and debates

Inquire offers a clean, structured, and intuitive space to **centralize academic knowledge** and build student communities.

---


## 🌱 Project Philosophy

Inquire is built as a passion project by students, for students. It’s a space where:

Knowledge isn’t siloed — everyone can contribute and learn.

Discussions aren’t lost — threads are indexed, searchable, and permanent.

Tech matters — it’s not just about what you build, but how you build it.

We believe this platform could become a cornerstone for student communities — technical or otherwise — in the digital age.

---


## 🤝 Contributing

We welcome contributions of all kinds:

Feature ideas and requests

UI/UX suggestions

Code improvements

---



## 💡 Key Features

- 🧠 **Q&A System** — Ask and answer questions with voting and tagging, Stack Overflow-style.
- 📢 **Announcements** — Broadcast important updates across courses, departments, or student groups.
- 🧵 **Threaded Discussions** — Engage in ongoing discussions, Reddit-style, on academic or campus-related topics.
- 💬 **Real-Time Chat** — Built-in chat for casual or focused academic discussions.
- 🏷️ **Topic Tagging & Search** — Find relevant questions, threads, and announcements quickly.
- 🎓 **Student Profiles** — Highlight skills, contributions, and engagement within your academic community.
- 🔒 **Privacy-First & Campus-Aware** — Designed with institutional and personal boundaries in mind.

---

## 🧪 Built With Industry-Relevant Tech

Inquire is a showcase of modern .NET SaaS development, using current best practices and tooling:

- **.NET 8.0** — Latest LTS version of the framework for optimal performance and long-term support.
- **ASP.NET Core** — Blazing-fast web backend and APIs.
- **Entity Framework Core** — Modern ORM for secure, efficient data access.
- **SignalR** — Real-time capabilities for chat and live interactions.
- **Blazor / Razor Pages** — Component-based UI (if applicable).
- **Clean Architecture** — Maintainable codebase with separation of concerns.
- **Dockerized Deployment** — Production-ready container support.
- **CI/CD Ready** — GitHub Actions workflows for automated builds and deployments (planned/in-progress).

> 🔧 The codebase is a growing foundation meant to evolve with feedback, contributions, and student innovation.

---

## 🚀 Getting Started

### ✅ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- SQL Server or PostgreSQL (local or Docker)
- Node.js (if front-end uses modern tooling)
- Visual Studio 2022+ or VS Code

### 📦 Running Locally

```bash
git clone https://github.com/GtJohnny/Inquire.git
cd Inquire
dotnet build
dotnet run --project src/Inquire.Web

```

## ├ Project Structure
```

/Inquire
├── src/
│   ├── Inquire.Core/         # Domain models and interfaces
│   ├── Inquire.Infrastructure/  # EF Core and data logic
│   ├── Inquire.Web/          # ASP.NET Core web app
│   └── Inquire.Shared/       # Common utilities and DTOs
├── tests/                    # Unit and integration tests
├── .github/                  # Actions, workflows
└── README.md
```
