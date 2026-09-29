<div align="center">

# MindfulJournal 🧠

### *Emotion-Based Journal System — write, reflect, understand yourself.*

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Blazor Server](https://img.shields.io/badge/Blazor-Server-512BD4?style=for-the-badge&logo=blazor&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![ASP.NET Identity](https://img.shields.io/badge/ASP.NET-Identity-2b579a?style=for-the-badge&logo=dotnet&logoColor=white)](https://learn.microsoft.com/aspnet/core/security/authentication/identity)
[![EF Core](https://img.shields.io/badge/EF%20Core-10.0-6b4fbb?style=for-the-badge&logo=nuget&logoColor=white)](https://learn.microsoft.com/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)
[![Groq AI](https://img.shields.io/badge/Groq-Llama--3.1-f55036?style=for-the-badge)](https://groq.com/)

<br/>

![MindfulJournal preview](assets/hero.webp)

> A full-stack **emotion-based journaling web app** built with Blazor Server and .NET 10.
> Write daily journal entries, track your moods over time, get **wellness suggestions**,
> see your mood patterns visualized as charts, dictate entries by voice, and even chat
> with an AI assistant that knows your journal history.

---

[Features](#-features) ·
[Tech Stack](#-tech-stack) ·
[Getting Started](#-getting-started) ·
[Project Structure](#-project-structure)

</div>

---

## ✨ Features

Everything below is in the code — nothing invented:

- **📝 Journal entries** — create, view, and edit dated journal entries with a title, content, and a mood attached to each one.
- **🎭 Mood tracking** — pick from moods like Happy 😄, Calm 😌, Anxious 😰, Sad 😢, Angry 😡; moods (with emoji + color) are stored in the database and manageable by admins.
- **💡 Wellness suggestions** — rule-based suggestions stored in the database (e.g. mood-specific tips that trigger after N days per week) plus a "How suggestions work" reference page.
- **🤖 AI-powered insights** — via the Groq API (`llama-3.1-8b-instant`): personalized wellness suggestions based on your recent entries and weekly mood pattern, a predicted mood for tomorrow, and a journal assistant you can ask questions about your own entries.
- **📊 Mood analytics** — weekly mood summaries and Chart.js charts (pie, line) rendering your mood distribution over time.
- **🎤 Voice entry** — dictate journal entries using the browser's speech recognition (works in Chrome).
- **💬 Feedback** — rate and comment on the suggestions you receive.
- **👑 Admin dashboard** — manage users, view all entries, manage moods, review feedback, and see system-wide analytics. An admin account (`admin@mindfuljournal.com`) is created automatically on first run.
- **🔐 Secure authentication** — ASP.NET Identity with roles, 6+ character passwords with a digit required.

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Frontend | Blazor Server (Interactive Server components), Bootstrap 5, custom CSS |
| Backend | ASP.NET Core / .NET 10 |
| Auth | ASP.NET Identity with roles (Admin / User) |
| Database | SQL Server (LocalDB), Entity Framework Core code-first migrations |
| AI | Groq API — `llama-3.1-8b-instant` (wellness suggestions, mood prediction, journal chat) |
| Charts | Chart.js (via `wwwroot/charts.js`) |
| Visuals | Three.js / Vanta.js animated backgrounds |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server LocalDB (comes with Visual Studio 2022/2025)
- Visual Studio 2022/2025 or any editor

### Run it

```bash
git clone https://github.com/hussnainahmedd/Emotion-Based-Journal-System.git
cd Emotion-Based-Journal-System
```

Apply the database migrations (creates `MindfulJournalDB` on LocalDB):

```bash
dotnet ef database update --project MindfulJournal
```

Run the app:

```bash
dotnet run --project MindfulJournal
```

Open **http://localhost:5175** (or https://localhost:7204) in your browser.

The app seeds an admin account at startup:

| | |
|---|---|
| Email | `admin@mindfuljournal.com` |
| Password | `Admin@123` |

> **Note:** The core app (journaling, moods, analytics, admin) works fully offline.
> The AI suggestions, mood prediction, and journal chat call the Groq API —
> make sure a valid Groq key is configured in `MindfulJournal/Services/AIService.cs`.

---

## 📂 Project Structure

```
MindfulJournal/
├── Program.cs                    # DI setup, Identity, EF Core, admin seeding
├── MindfulJournal.csproj         # .NET 10, Identity + EF Core SqlServer packages
├── appsettings.json              # LocalDB connection string
├── Models/                       # ApplicationUser, JournalEntry, Mood, Suggestion, Feedback
├── Data/                         # ApplicationDbContext
├── Migrations/                   # EF Core code-first migrations
├── Services/
│   ├── JournalService.cs         # Entry CRUD, weekly mood counts, suggestions
│   ├── AIService.cs              # Groq-powered suggestions, mood prediction, chat
│   └── AdminService.cs           # Admin management & analytics
├── Components/Pages/            # Dashboard, Add/Edit/View entries, Analytics,
│                                 # Suggestions, AISuggestions, VoiceEntry,
│                                 # Profile, Settings, Feedback, Admin*
└── wwwroot/                      # charts.js (Chart.js helpers), CSS, Bootstrap, favicon
```

---

<div align="center">

Built by **[Hussnain Ahmad](https://github.com/hussnainahmedd)** — BSCS student building full-stack apps to learn.

⭐ *Star this repo if you liked it.*

</div>
