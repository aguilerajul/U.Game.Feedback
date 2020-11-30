# U.Game.Feedback.Api
Api to provide feedback about some game sessions, allow to send feedback and creation of new users

The soluction was implemented using the DDD Patterns with Entity Framework and was divided in these folders.
1. Apps: .NET Core WEB API
2. Domain: Contains all the Models that are going to be used between Repository, API and Unit Tests
3. Infrastructure: Contains all the logic related with Logging, Data Persistence, in addition have a project call it Repository that was created using Entity Framework Core and Code First approach.
5. Tests: Contains all unit tests related with the project.

## Pre-Requisistes
:ballot_box_with_check: You need to have installed **Microsoft® SQL Server® 2017 Express** or superior.

:ballot_box_with_check: You need to have **Visual Studio 2019 Community**, **Visual Studio 2019 Professional** or **Visual Studio 2019 Enterprice**

:ballot_box_with_check: You need to have the **.NET Core 3.1** version enabled in your Visual Studio.


### How To Test it
1. Change the connection :key: value **Game.Feedback.DataBase** contain it in the projects: **U.Game.Feedback.Api ➡️ appsettings.json**
3. Set **U.Game.Feedback.Api** as startup project: 
	- You don't need to exectute the manually migrations or update database using the package console, it's being handle it in the **U.Game.Feedback.Api** project, 
	- In addition the **Repository** project has a Seeder class that will create some dummy users for test purpose, by default there will be 10 users using: **Nickname_{index}**, **User{index}**, **user.{i}@testUsers.com**.
5. Run the Project **U.Game.Feedback.Api**
6. If everything goes well, you will be able to see a Documentation Api Library call it Swagger, and you will be able to test the controllers.
	### 1. User: 
		1. Create new users.
		2. List all users and set how many records or users do you wants to see, by default the list returns the last 15 users.
	### 2. Feedback:
		1. Send Feedback, using UserId, SessionId and rating with comments.
		2. List last 15 feedbacks and can be filter it by Rating.
		
### Entity Framework Migrations
1. If you wants to made changes to the Database the best way is use the Repository project with migrations	
	- Made changes to the model create on **U.Game.Feedback.Domain ➡️ Entities**
	- Update RepositoryDbContext just in case that you add a new model.
	- Open Package Manager Console. If you don't kwnow where it's, no worries just go to the Menus bar: **Tools ➡️ Nugget Package Manager ➡️  Package Manager Console**
	- Once it's open choose as Default Project: **Infrastructure\U.Feedback.Repository**
	- and run the command: **update-database**
