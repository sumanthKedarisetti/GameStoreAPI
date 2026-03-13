Day -1 
Time spent- 1 hour 

Things done
--started a dot net project in visual studio
--configured the dev env -to develop and run the application
--configured the  rest client extension- to test apis without opening the browser
--created DTOs for GameDTO  and creategameDTO. used records(immutable by default,value type ,compares values) feature in C# which is used for storing data 
--configured 3 rest end points 
   --getgames
   --getgamebyid
   --postid
  --tested all the end points properly everything is working properly.
  --pushed the code to git and created a repository there 
  --Now all the changes can be tracked.
  --tested the code after pushing to git and cloning from the repo. working good.
Day -2
Time spent -2 hours

Things done
--want to implement authentication for the API developed.
--Explored the different IAM systems.
--Found Keycloak which is an open source Identity and Access Management .
--Installed it locally and started the server. 
--It also needs JDk to be installed. faced few challenges(environment variables for the JDK).
--Then Tried to understand Realm (isolated space for users,groups,roles,clients), and clients (represents the web app/API)
--understood Authorization code flow 
     login url(discovers the endpoints from authority) with clientid(from keycloak when we create a client)
                        |
               redirected to keycloak
                        |
        after login sucessfully server sends authorisation code
                        |
         then post request to token url(discovers endpoint from authority)-->gets token 
                        |
         ASp.Net core stores the signed token in cookie
                        |
         in susequenet requests -->valiadates tken from cookie using (metadata from authority)

---configured the authentication service middleware in program.cs
          
