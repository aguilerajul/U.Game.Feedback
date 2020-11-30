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
	- In addition the **Repository** project has a Seeder class that will create some dummy users for test purpose, by default there will be 10 users using: **Nickname_{index}**, **User{index}**, **user.{index}@testUsers.com**.
5. Run the Project **U.Game.Feedback.Api**
6. If everything goes well and I sure that will be :smiley: , you will be able to see a Documentation Api Library call it Swagger, and you will be able to test the controllers.
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
	
### DataBase Schema
1. There are just two tables Created:
	- Users	Fields: 
		
			Id: UniqueIdentifier PK
			
			NickName: NVARCHAR(150)
			
			Name: NVARCHAR(150)
			
			Email: NVARCHAR(250)
			
			CreatedDate: DateTime
	- UserFeedbacks	Fields:
		
			Id: UniqueIdentifier PK
			
			UserId: UniqueIdentifier FK (User Table)
			
			SessionId: NVARCHAR(MAX)
			
			Rating: INT
			
			Comments: NVARCHAR(512)
			
			CreatedDate: DateTime			
2. The relationship between Users and UserFeedbacks is One to Many, that means that 1 user can create multiples Feedbacks only if he is comming from a different session.

### API Routes and Payloads
1. Feedback API	
	- Payloads: There are only 2 Actions results
		
		1. **POST**: Will create a new Feedback 
			- **Session Id** is an String field 
			- **Ubi-UserId** Sent in the header needs to be one of the User ID (Guid) added to the Users table.	
	
	
					curl -X POST "http://localhost:63550/Feedback?sessionId=01d803d3-fe40-435b-90ad-42423bd0f42a" -H  "accept: */*" -H  "Ubi-UserId: 6D4B2BBE-3ABA-4852-B758-30AA5D32E3E6" -H  "Content-Type: application/json" -d "{\"rating\":0,\"comments\":\"string\"}"
				
				
		2. **GET**: Will returns a list of the last 15 feedbacks sents and will be order it by created date.

				curl -X GET "http://localhost:63550/Feedback/List?rating=2&totalRecords=15" -H  "accept: */*"

2. User API	
	- Payloads:
	
		1. **POST**: Will create a new user into the Users table.		
	
	
				curl -X POST "http://localhost:63550/User" -H  "accept: */*" -H  "Content-Type: application/json" -d "{\"nickName\":\"string\",\"name\":\"string\",\"email\":\"string\"}"


		2. **GET**: Will return the list of the users created.
		
		
				curl -X GET "http://localhost:63550/User/List" -H  "accept: */*"
		
		
### Unit Tests

There are 2 test projects:

1. U.Game.Feedback.Api.Tests
2. U.Game.Feedback.Repository.Tests

There are different ways to run the tests:

1. We can just go to the Test Explorer and Make click in the First Play button display it in the top on View, In case that you doesn't have open the test explorer you can find it in the Menu Bar: **View ➡️ Test Explorer** or press in the Keyboard **Ctrl+E,T** in Visual Studio Community.
2. We can run it from the Menu bar **Test ➡️ Run All Tests** or press in the Keyboard **Ctrl+R,A** in Visual Studio Community.
	
