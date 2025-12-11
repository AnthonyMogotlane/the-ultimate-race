# The Ultimate Race

![Project Overview](https://firebasestorage.googleapis.com/v0/b/anthonymogotlane.appspot.com/o/Screenshot%202025-12-11%20063604.png?alt=media&token=c22c4ab7-0cf3-43ac-bbfb-a58b08a9cbf5)


## Overview
This is improvements to modernise the stack, enhance maintainability, and streamline integration with frontend. The following sections outline the key changes and the reasoning behind them.

---

## 🚀 Migration from .NET Core 2.1 to .NET 8
### Change
- Upgraded the backend from **.NET Core 2.1** to **.NET 8**.

### Reasons
- **Long-term support**: .NET Core 2.1 is out of support, while .NET 8 provides active support and security patches.
- **Performance improvements**: .NET 8 introduces optimized runtime performance, faster APIs, and reduced memory usage.
- **Modern features**: Access to new language features (C# 12), improved minimal APIs, and enhanced cloud-native capabilities.
- **Security**: Staying current ensures the latest security fixes and compliance with industry standards.

---

## 🧩 Creation of Common Class
### Change
- Introduced a **Common Class Library** to centralize shared logic.

### Reasons
- **Code reusability**: Eliminates duplication by consolidating shared methods and utilities.
- **Maintainability**: Easier to update and extend common functionality in one place.
- **Consistency**: Ensures uniform behavior across different modules of the application.
- **Scalability**: Provides a foundation for future features without rewriting core logic.

---

## 🌐 API Integration with Angular While keeping the console app
### Change
- Designed and exposed APIs specifically for use with the **Angular frontend**.

### Reasons
- **Seamless communication**: Enables Angular to consume backend services efficiently via endpoints.
- **Decoupling**: Keeps frontend and backend independent, allowing parallel development and easier upgrades.
- **Scalability**: APIs can be reused by other clients (mobile apps, services) beyond Angular.
- **Standardization**: Follows best practices for modern web applications, ensuring predictable data exchange.

---

## ✨ Improved Naming Conventions
### Change
- Refactored codebase to adopt **clear and consistent naming conventions**.

### Reasons
- **Readability**: Developers can quickly understand the purpose of classes, methods, and variables.
- **Maintainability**: Reduces confusion and errors when extending or debugging code.
- **Collaboration**: Makes onboarding new developers easier by providing intuitive naming patterns.

---

## 🛠️ Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js (LTS)](https://nodejs.org/) and npm
- [Angular CLI](https://angular.io/cli) installed globally (`npm install -g @angular/cli`)

---
