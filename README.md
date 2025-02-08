# TaskBoard.Framework.Core
[![Build](https://github.com/niolikon/TaskBoard.Framework.Core/actions/workflows/dotnet.yml/badge.svg)](https://github.com/niolikon/TaskBoard.Framework.Core/actions)
[![Package](https://github.com/niolikon/TaskBoard.Framework.Core/actions/workflows/publish-nuget.yml/badge.svg)](https://github.com/niolikon/TaskBoard.Framework.Core/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

Task Board Framework and Common components (.NET Case Study)


# Overview

TaskBoard.Framework.Core is a framework for task board management in .NET, designed to provide common components and a solid foundation for applications based on Entity Framework Core and ASP.NET.

---

## ✨ Features
- 📌 **Task Board Management** with support for users and roles.
- 📊 **Compatible with Entity Framework Core** for integration with SQL Server databases.
- 🔐 **JWT Authentication** for secure access management.
- 📑 **REST API** for task management.
- ✅ **Testing with xUnit and Testcontainers** to ensure framework stability.

---

## 🛠️ Getting Started

### Prerequisites

- **.NET 8 SDK** ([Download](https://dotnet.microsoft.com/en-us/download))
- **NuGet CLI** ([Download](https://www.nuget.org/downloads)) (optional for manual package management)
- **Git** ([Download](https://git-scm.com/)) (for cloning and contributing)

### Quickstart Guide

1. Clone the repository:
   ```bash
   git clone https://github.com/niolikon/TaskBoard.Framework.Core.git
   cd TaskBoard.Framework.Core
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```
   
3. Build the project:
   ```bash
   dotnet build --configuration Release
   ```
   
4. Pack the NuGet package to a local repository:
   ```bash
   dotnet pack --configuration Release --output ~/.nuget/local
   ```

5. Add the local NuGet source (only needed once):
   ```bash
   dotnet nuget add source ~/.nuget/local --name TaskBoardLocal
   ```

6. Install the package in another project 
   ```bash
   dotnet add package TaskBoard.Framework.Core --source TaskBoardLocal
   ```

## 📬 Feedback

If you have suggestions or improvements, feel free to open an issue or create a pull request. Contributions are welcome!

---

## 📝 License

This project is licensed under the MIT License.

---
🚀 **Developed by Simone Andrea Muscas | Niolikon**

