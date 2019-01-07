## Description

SoftUni Motor Insurance is a project assignment for the C# Web Development ASP.NET Core course at Softuni.bg. $UMI is a system for managing insurance policies and claims between an Insurance company, its clients and insurance agents.


## How to run the project

1. Open the project with Visual Studio 2017 (15.8 or later)
2. Build the project (all packages including the client-side dependencies) will be restored automatically.
3. Change connection string
4. Uncomment line 138 in startup.cs (// ApplicationDbContextSeeder.Seed(dbContext, serviceScope.ServiceProvider);) You only need it the first time you start the application. Then you can comment it out again.
