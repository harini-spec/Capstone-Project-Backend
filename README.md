# Health Tracker Application:

### Use Case:
Health Tracker App:
    Dashboard with the records of a week should be present. User can choose what to track. (BP, Sugar, Sleep)

### Installations:
- Frontend:

```
npm install bootstrap -save
npm install axios 
npm install react-router-dom
npm i @fortawesome/react-fontawesome
npm i @fortawesome/free-solid-svg-icons
npm install react-toastify --save  
npm install @coreui/react-chartjs
npm install jwt-decode
npm install react-loader-spinner --save
npm install @react-oauth/google@latest
```

- Backend:
```
Azure storage blobs
Entity Framework core 
EF Core - SQL Server
EF Core - Tools
EF Core - JWT Bearer 
EF Core - InMemory
MOQ
log4net
MS log4net
Azure OpenAI 
```

<hr/>

### Testing
- Line Coverage: 75.7% 
- Branch Coverage: 89%

### Tech Stack:
- Frontend: ReactJS
- Backend: .Net
- Database: SQL Server 
- GFit: OAuth

### Azure Services
- Backend: App Service (Web App)
- Frontend: App Service (Static Web App)
- Storage: Storage account 
- Database: SQL Databases
- Secrets: Key Vault
- ChatBot: Azure OpenAI API

<hr/>

## Models:
```
Enums:

ENUM Health_status  - Good, Fair, Poor

ENUM Target_status  - Achieved, Not_Achieved

ENUM Role           - User, Coach, Admin

ENUM User_status    - Active, Inactive

ENUM Gender         - Female, Male, Others
```
### Common:
- **User Details:** [ID], Password Hash, Password encrypted, Status (for soft delete), created_at, updated_at
- **User:** [User Detail ID], Name, Age, Gender, Phone, Email ID, Role, created_at, updated_at

### For User:
- **User Preferences:** [ID], User ID, Metric ID, created_at, updated_at
- **Metrics:** [ID], Metric_Type, Unit, created_at, updated_at
- **Target:** [ID], Preference ID, Target Min Value, Target Max Value, status, start date, end date, created_at, updated_at 
- **Health Log:** [ID], Preference ID, value, Heath status, created_at, updated_at
- **Ideal data:** [ID], Health status, Metric Id, Min_val, Max_val, created_at, updated_at 

### For health coach:
- **Monitor Preferences:** [ID], User ID, Metric Id, created_at, updated_at 
- **Suggestions:** [ID], User ID, Coach ID, Suggestion, created_at, updated_at 

## Endpoints:
### Common:
`POST/Register user or coach`
- Fill User and UserDetails table, get height and weight too
- Fill in health log
- user always active, coach - inactive
- O/P: User ID 

`POST/Login user or coach` 
- I/P: Email, Password 
- check if user active
- O/P: Msg - successfully logged in

`GET/All Metrics` 
- I/P: - 
- O/P: List of metrics

`GET/Get Metric Type and Unit from Pref Id` 
- I/P: Pref Id
- O/P: Pref DTO

### User:
`POST/User Preferences` 
- I/P: Metric list
- Get user ID from token 
- Add them to User Preferences table one by one 
- O/P: Msg - successfully added 

`GET/User Preference`
- I/P: UserID
- O/P: List of user preferences

`POST/Health log` [No Health status for height (NoStatus will be health status), for weight -> BMI]
- I/P: value, pref Id
- Metric ID, User Id - user preference ID 
- Check if log already added for today i.e., check if preference id and today's date are present already 
    - If so, send error - Already added

- Check data value against ideal value 
    - Update the health status by comparing with ideal val table
    - Add to output DTO

- Check data value against target if any set 
    - Preference ID, start date, end date - recent datetime (latest in that range) - using this find target data GET/Target endpoint
    - Check if value inside target range
    - update target status 
    - Add target status to Output DTO

- If height, check if its reducing - error

- Add to health log 
- O/P: output DTO: Ideal status, target status 

`GET/Health Log`
- I/P: Pref Id
- O/p: Health log data 

`PUT/Update Health log` 
- I/P: Health log ID, updated data 
- Do ideal value check and target value check - update both status in the DTO 
- O/P: output DTO: Ideal status, target status 

`POST/Target`
- I/P: Target data 
- Populate target table
- Check if target already set for the given date
- O/P: Msg - status based on the ideal range 

`GET/Today's Target` 
- I/P: Metric type
- Calculate preference ID 
- Not achieved target in earliest future
- Get target data for current date between start and end date 
- O/P: Target DTO 

`GET/Target By ID` 
- I/P: Target ID
- O/P: Target DTO 

`DELETE/Target By ID` 
- I/P: Target ID
- O/P: Success message

`GET/Get All Targets of Pref ID` 
- I/P: Pref ID
- Sort Targets based on Target Date
- O/P: Target DTO List

`PUT/Target`
- I/P: Target updated data 
- Check if target already set for the given date
- Status set back to not achieved
- O/P: Update and msg status based on ideal range 

`Get/Graph Data` 
- I/P (params): duration, metric_type 
- Duration - This Week, Last Week, This Month, Last Month, Overall
- Calculate start and end date based on duration 
- Get all health logs of user within the given date range 
- O/P: Health log list 

`Get/All Suggestions`
- I/P: -
- Get User ID from token 
- suggestions for user
- O/P: List of suggestions of user 

`POST/Add google fit data to DB`

`POST/Add OAuth Access token to DB`

`GET/Get OAuth Access token from DB`

### Coach:
`POST/Coach monitor Preferences` 
- I/P: Metric list
- Get user ID from localstorage (stored after register) 
- Add them to User Preferences table one by one 
- O/P: Msg - successfully added

`GET/Coach monitor preferences`

`GET/GetProblemsOfUserId`

`GET/Problems`
- I/P: -
- Get health logs 
- Health logs with status as Poor (Today) - Get User Id 
- O/P: list of user ids, metrics

`POST/Suggestions` 
- I/P: User ID, Suggestion
- Add to suggestion table 
- O/P: Successfully added 

`GET/My Suggestions` 
- I/P: UserId
- Get coach ID from token 
- O/P: List of suggestion DTOs

### Admin:
`GET/Get all inactive coaches`

`PUT/Activate Coach Account`

<hr/>
