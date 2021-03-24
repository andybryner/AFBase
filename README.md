# Azure Function Test Project

## Summary
Example Azure Function running on .NET Core 3.1 with EF Core and SQL Server database.

## Setup

### Database setup
Open SSMS and connect to *(localdb)\MSSQLLocalDB*

Right-click *Databases* folder and select *New Database...* 

Enter the Database name "MiViewAccounts" and click "Ok". 

Open the project in Visual Studio

Open *Package Manager Console*, set Default project as "MiAccount.Data" and run:
````
Update-Database
````

### Running the project
Make sure MiAccount is set as the Startup Project (right click the project, select *Set as startup project*)

Run the project, see the console output for the list of available endpoints, e.g.:
````
Functions:

        CreateAccount: [POST] http://localhost:7071/api/v1/accounts

        GetAccounts: [GET] http://localhost:7071/api/v1/accounts
````
