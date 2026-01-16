## Database setup

This project uses SQL Server.

### 1. Create database
Create an empty database in SQL Server, for example:

BookStore

### 2. Run SQL script
Run the script located at:

/SQL/BookStore.sql

The script will:
- Create all tables
- Insert test data
- Create views, procedures and user-defined types

### 3. Configure connection string

The application uses .NET User Secrets for the database connection string.
Right-click on solution and open terminal.
Run the following command:

```bash
dotnet user-secrets --project .\BookStore.Infrastructure\BookStore.Infrastructure.csproj set "ConnectionString" "Initial Catalog=BookStore;Integrated Security=True;Trust Server Certificate=True;Server SPN=localhost"
```

### 4. Run the application
Start the application.  
You can now log in using any employee and store from the database.
