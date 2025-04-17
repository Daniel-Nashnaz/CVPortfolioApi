
# CVPortfolioAPI

A .NET Core Web API project that connects to a GitHub account to retrieve and display developer portfolio data.  
This application is ideal for developers who want to showcase their GitHub activity and repositories on their personal website.

---

## 📌 Project Overview

**CVPortfolioAPI** serves as a backend service that:
- Connects to the developer's personal GitHub account using **Octokit.NET**.
- Retrieves and returns useful portfolio data from GitHub.
- Allows public GitHub repository search with filters.

---

## 🚀 Features

### 🔐 Authenticated GitHub Integration
- Uses a **personal access token** to authenticate and retrieve **private account data**.
- Stores the token securely using `.NET Secret Manager` (in `secrets.json`).
- Implements the **Options Pattern** to inject GitHub credentials into services.

### 📁 Portfolio Retrieval (`GET /api/portfolio`)
Retrieves a list of repositories from the authenticated GitHub account, including:
- Programming languages used
- Last commit date
- Number of stars
- Number of pull requests
- Homepage URL (if available)
- And more

### 🔍 Public Repository Search (`GET /api/portfolio/search`)
Allows users to search all public repositories on GitHub with optional filters:
- Repository name
- Programming language
- GitHub username

### ⚡ Caching with Smart Invalidation
- Implements **in-memory caching** using Scrutor and Decorator pattern.
- Automatically invalidates cache:
  - Every few minutes _or_
  - When a new GitHub event is detected (e.g., new commit, PR, etc.)

---

## 🧱 Architecture

- **Web API Project**: Main project that exposes endpoints.
- **Class Library (Service)**: Contains GitHub integration logic (using Octokit).

```
Solution
│
├── CVPortfolioAPI         → Web API project
└── CVPortfolioService     → Class library with GitHub logic
```

---

## 🧰 Technologies Used

- ASP.NET Core Web API (.NET 6+)
- Octokit.NET (GitHub API client)
- In-memory Caching
- Scrutor (for decorators and dependency injection)
- Options Pattern
- secrets.json (for secure config management)

---

## 🔐 GitHub Token Setup

1. [Create a personal access token](https://docs.github.com/en/github/authenticating-to-github/creating-a-personal-access-token).
2. Store it securely using `dotnet user-secrets`:

```bash
dotnet user-secrets set "GitHub:Token" "your_token_here"
dotnet user-secrets set "GitHub:Username" "your_username"
```

## 📁 This will generate a structure in your secrets.json like:

```bash
{
  "GitHub": {
    "PersonalAccessToken": "your_token_here"
  }
}

```

---

## 📦 How to Run

```bash
git clone https://github.com/your-username/CVPortfolioAPI.git
cd CVPortfolioAPI

# Set secrets as shown above

dotnet restore
dotnet build
dotnet run
```

---

## 📚 Resources

- [Octokit.NET Documentation](https://octokitnet.readthedocs.io/en/latest/)
- [In-Memory Caching with Decorator Pattern](https://blog.christian-schou.dk/add-in-memory-caching-to-net-6-web-api/)
- [Scrutor for Dependency Injection](https://andrewlock.net/adding-decorated-classes-to-the-asp.net-core-di-container-using-scrutor/)
- [Options Pattern in .NET](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options)

---

## 💡 Future Improvements

- Add UI integration with a portfolio frontend (e.g., React, Angular, or Next.js)
- Support OAuth flow for dynamic user connections
- Pagination and sorting for large GitHub data sets
- Unit and integration tests

---

## 🤝 Contributing

Pull requests are welcome! Feel free to open issues or feature requests.

---
