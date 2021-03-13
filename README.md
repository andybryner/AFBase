# Azure Function Test Project

## Setup

### Database setup
Create database "MiViewAccounts" on (localdb)\MSSQLLocalDB

Open project in Visual Studio
Open Package Manager Console, set Default project as "MiAccount.Data" and run:
````
Update-Database
````

### Running the project
Make sure MiAccount is set as the Startup Project (right click the project, select *Set as startup project*)

Run the project, see the console output for the list of available endpoints, e.g.:
````
Functions:

        CreateAccount: [POST] http://localhost:7071/api/CreateAccount

        GetAccounts: [GET] http://localhost:7071/api/GetAccounts
````
