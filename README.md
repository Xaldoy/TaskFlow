# TaskFlow

**TaskFlow is currently in the early stages of development and has not yet been released.**

TaskFlow is a highly customizable and personal task manager app built with ASP.NET Core and React. It aims to provide users with a flexible and intuitive interface to manage their tasks effectively, making it easier to organize, prioritize, and track their progress.

## Planned Features

# Core Features

-   **User Authentication**: User registration and login functionality using ASP.NET Identity, OAuth integration for social logins (Google, Facebook, etc.).
-   **Dark/Light Mode**: Toggle between dark and light themes.
-   **Task Management**: Create, edit, and delete tasks.
-   **Custom Task Types**: Create tasks with custom fields.
-   **Categories & Tags**: Organize tasks into categories and tag them for easy filtering.
-   **Due Dates & Recurring Tasks**: Set due dates and add complex recurrence rules for repeated tasks.
-   **Task Dependecies**: Specify the order in which tasks must be completed, critical path analysis.
-   **Multiple Views**: Different ways of displaying your tasks (list, grid, calendar, kanban, etc.).
-   **Custom Dashboard Layout**: Create custom page layouts for each dashboard.

# Advanced Features

-   **Smart Scheduling**: Automatically allocate tasks into available calendar slots.
-   **Automated Workflows**: Conditional workflows (e.g. if Task A is done, Set Task B Prioriy to High).
-   **Nested Subtasks**: Unlimited nested tasks for complex hierarchies.
-   **Custom Reports**: Reports on productivity, completion rates, time spent and similar.
-   **Task Versioning**: Keep history of task modifications and allow users to revert to previous versions.

## Technologies Used

-   **Backend**: ASP.NET Core
-   **Frontend**: React + Vite
-   **Database**: PostgreSQL
-   **Authentication**: Microsoft.AspNetCore.Identity
-   **State Management**: Redux, Context
-   **UI Library**: -

## Getting Started

### Prerequisites

-   [.NET SDK](https://dotnet.microsoft.com/download)
-   [Node.js](https://nodejs.org/)
-   [PostgreSQL](https://www.postgresql.org/)

### Installation / Running in development environment

#### 1. Clone the Repository

Begin by cloning the repository:

```bash
git clone https://github.com/Xaldoy/TaskFlow.git
cd TaskFlow
```

#### 2. Set Up the Backend environment

1. Navigate to Taskflow.API directory and rename `appsettings.development.json.txt` to `appsettings.development.json`.
2. Insert the appropriate JWT Key in the configuration file.

#### 3. Database Setup

1. Create a new PostgreSQL database named **taskflow** with a schema named **backend**.
2. Update the databse connection string in the `appsettings.development.json` file.
3. If not already installed, execute the following command:

```bash
dotnet tool install --global dotnet-ef --version 8.*
```

4. To set up the database schema, run the following command from the TaskFlow folder:

```bash
dotnet ef database update -s Taskflow.API -p Taskflow.Model
```

#### 4. Run the Backend

1. Run the following commands from the Taskflow.API firectory:

```bash
dotnet restore
dotnet run
```

#### 5. Set Up and run the Frontend

1. Got to TaskFlow.Front and rename **.env.development.txt** to **.env.development**.
2. Run the following commands from the Taskflow.Front:

```bash
npm install
npm run dev
```

## Usage

After setting up the project, open your web browser and navigate to [http://localhost:3000](http://localhost:3000) to access the TaskFlow application.

## Test User Login

To log in, you may use the following test user credentials:

-   Username: test
-   Password: Password0

## Contact

For any questions or feedback, please reach out to [mjurisic812@gmail.com](mailto:mjurisic812@gmail.com).
