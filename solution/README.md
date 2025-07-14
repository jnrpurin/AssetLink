# Inventory Tracker: Computer Management System

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Architecture](#architecture)
  - [Backend](#backend)
  - [Frontend](#frontend)
- [Setup and Installation](#setup-and-installation)
  - [Prerequisites](#prerequisites)
  - [Backend Setup](#backend-setup)
  - [Frontend Setup](#frontend-setup)
- [Running the Application](#running-the-application)
- [Testing](#testing)
  - [Unit Tests](#unit-tests)
  - [Integration Tests](#integration-tests)
- [API Endpoints](#api-endpoints)
- [Future Enhancements](#future-enhancements)

## Introduction

This project delivers a robust Inventory Tracker system designed for managing computer assets. It enables tracking of computers, manufacturers, current status, and user assignments. The application uses a **.NET Core backend** for API services and **vanilla JavaScript frontend** for a responsive UI.

## Features

- **Computer Management:** Create, read, update, delete (CRUD) computer records.
- **Detailed Computer Information:** Serial number, manufacturer, specs, image URL, purchase date, warranty expiration.
- **User Assignment:** Assign computers to users, track assignment periods, handle reassignments.
- **Status Tracking:** Track and display current operational status.
- **Filtering & Pagination:** Client-side filtering and pagination for large inventories.
- **Responsive UI:** Bootstrap for adaptability across devices.

## Architecture

### Backend

Developed in **.NET 9 (ASP.NET Core)** with a layered architecture:

- **Framework:** .NET 9 (ASP.NET Core)
- **Database:** SQLite
- **ORM:** Entity Framework Core
- **Mapping:** AutoMapper
- **DI & Logging:** Built-in ASP.NET Core
- **Structure:**
  - **Controllers:** Expose RESTful API endpoints (`ComputersController`, `UsersController`).
  - **Services:** Business logic orchestration (`ComputerService`, `UserService`).
  - **Repositories:** Abstract EF Core access (`IComputerRepository`, `ComputerRepository`, `IUserRepository`, `UserRepository`).
  - **Models:** Core entities (`Computer`, `User`, etc.).
  - **DTOs:** Controlled data contracts.
  - **Profiles:** AutoMapper configurations.
  - **Data:** `InventoryDbContext` for EF configuration.

### Frontend

A **vanilla JavaScript SPA** with Bootstrap:

- **Language:** JavaScript (ES Modules)
- **Styling:** Bootstrap 5.3.3 + custom CSS
- **Structure:**
  - `index.html`: Main entry point.
  - `app.js`: Initializes app and routing.
  - `services/api.js`: Interacts with backend API.
  - `components/`: UI components (lists, forms).
  - `utils/utils.js`: Utilities.
  - `styles/main.css`: Custom styles.

# Setup and Installation

## Prerequisites

- .NET 9 SDK
- Node.js and npm (for frontend development)
- Git (for cloning the repository)
- A code editor like Visual Studio Code

### Backend Setup

1.Clone the repository:

```bash
git clone <repository-url>
cd Inventory-Tracker/solution
```

2.Navigate to the backend project:

```bash
cd backend/InventoryTracker
```

3.Restore NuGet packages:

```bash
dotnet restore
```

4.Apply database migrations:

```bash
dotnet ef database update
```

This will create the SQLite database file (InventoryTracker.db) in backend/InventoryTracker/Database.

### Frontend Setup

1.Navigate to the frontend project:

```bash
 cd ../../frontend/Inventory-frontend
```

(Assuming you are in backend/InventoryTracker from the previous step)

2.Install npm dependencies (if any, though this project uses vanilla JS):

```bash
npm install # (If you add any npm packages in the future)
```

# Running the Application

## Start the Backend API

1. Open a terminal in the `backend/InventoryTracker` directory.
2. Run:

    ```bash
    dotnet run
    ```

3. The API will typically run on [http://localhost:5072](http://localhost:5072).

---

## Start the Frontend

1. Open a new terminal in the `frontend/Inventory-frontend` directory.
2. Since it uses vanilla JS, you can:
   - Open `index.html` directly in your browser, **or**
   - Use a simple HTTP server, such as:

    ```bash
    npx http-server .
    ```

    (If you have `http-server` installed globally, or use VS Code’s Live Server extension.)

3. The frontend will typically be accessible via:
   - The file system (when opening directly), **or**
   - [http://localhost:8080](http://localhost:8080) (if using `http-server`).

---

Now you are ready to run and test your **Inventory Tracker** locally.

# Testing

## Unit Tests

They are in a Pull request ready to be merged

- **Local:** `backend/InventoryTracker.Service.Tests/`
- **Frameworks:** xUnit, Moq

### Execution

```bash
cd D:\inventory-tracker\solution
dotnet test backend/InventoryTracker.Service.Tests/InventoryTracker.Service.Tests.csproj
```

# API Endpoints

O backend expõe os seguintes endpoints principais:

| Resource  | Method | Endpoint                   | Description                                                                      |
|-----------|--------|----------------------------|----------------------------------------------------------------------------------|
| Computers | GET    | `/api/Computers`          | Retrieve a list of all computers.                                               |
| Computers | GET    | `/api/Computers/{id}`     | Retrieve a specific computer by ID.                                             |
| Computers | POST   | `/api/Computers`          | Create a new computer.                                                          |
| Computers | PUT    | `/api/Computers/{id}`     | Update an existing computer.                                                    |
| Computers | DELETE | `/api/Computers/{id}`     | Delete a computer by ID.                                                        |
| Computers | POST   | `/api/Computers/assign`   | Assign a computer to a user. Ends any previous active assignment for that computer. |
| Users     | GET    | `/api/Users`              | Retrieve a list of all users.                                                   |
| Users     | GET    | `/api/Users/{id}`         | Retrieve a specific user by ID.                                                 |
| Users     | POST   | `/api/Users`              | Create a new user.                                                              |
| Users     | PUT    | `/api/Users/{id}`         | Update an existing user.                                                        |
| Users     | DELETE | `/api/Users/{id}`         | Delete a user by ID.                                                            |


# Future Enhancements

...answering the question...
If I had more time, the next steps for this project would focus on enhancing robustness, scalability, and user experience:

1. Backend Pagination and Filtering

Implement server-side better pagination and filtering for **Computers** and **Users** endpoints to significantly improve performance for large datasets by reducing the amount of data transferred and processed on the client.  
This would involve:
- Adding parameters to API endpoints (e.g., `/api/Computers?page=1&pageSize=10&serialNumber=ABC`).
- Modifying repository/service layers to handle these parameters.

2. Authentication and Authorization

Introduce a robust authentication system (e.g., **JWT-based authentication**) to secure API endpoints.  
Implement **role-based authorization** to control access:
- Only admins can delete computers.
- Only authorized users can assign computers.

3. User Interface Refinements

- **Dropdown for Manufacturer ID**: Replace the plain `ComputerManufacturerId` input field with a dynamic dropdown populated from `GET /api/ComputerManufacturers` for better UX and data integrity.
- **User Interface for User Management**: Create dedicated CRUD screens for managing users in the frontend, similar to computer management.
- **Enhanced Assign Form**: Allow selecting an `AssignEndDt` to manually end an assignment during the assignment process.
- **Notifications/Toasts**: Replace `alert()` calls with user-friendly, non-intrusive notifications (e.g., **Bootstrap Toasts**).
- **Error Handling and User Feedback**: Implement granular error handling on both frontend and backend, providing clear, actionable error messages to users.

5. Audit Logging

Implement comprehensive audit logging to track changes to critical entities, capturing:
- Who changed what
- When the change occurred

6. Containerization (Docker)

Containerize the backend and frontend applications using **Docker** to:
- Simplify deployment
- Ensure environment consistency across development, staging, and production

7. CI/CD Pipeline

Set up a **Continuous Integration/Continuous Deployment pipeline** to automate:
- Building
- Testing
- Deploying the application

8. Frontend Framework

Consider migrating the frontend to a modern JavaScript framework (e.g., **React**, **Angular**, **Vue.js**) for:
- Better state management
- Component reusability
- Scalable and maintainable UI for complex interactions
