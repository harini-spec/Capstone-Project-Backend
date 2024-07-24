# Health Tracker Application:

## Models:
```
Enums:

ENUM Health_status  - Good, Fair, Poor

ENUM Target_status  - Ongoing, Achieved, Not_Achieved

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

### User:
`POST/User Preferences` 
- I/P: Metric list
- Get user ID from token 
- Add them to User Preferences table one by one 
- O/P: Msg - successfully added 

`GET/User Preference`
- I/P: -
- Get user ID from token 
- O/P: List of user preferences

`POST/Health log`
- I/P: value, metric
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

- Add to health log 
- O/P: output DTO: Ideal status, target status 

`GET/Health Log`
- I/P: Health log ID 
- O/p: Health log data 

`PUT/Update Health log` 
- I/P: Health log ID, updated data 
- Do ideal value check and target value check - update both status in the DTO 
- O/P: output DTO: Ideal status, target status 

`POST/Target`
- I/P: Target data 
- Populate target table
- Check if it is in ideal range - if not, send a msg 
- O/P: Msg - status based on the ideal range 

`GET/Target` 
- I/P: Metric type
- Calculate preference ID 
- Get target data for current date between start and end date 
- O/P: Target DTO 

`PUT/Target`
- I/P: Target updated data 
- O/P: Update and msg status based on ideal range 

`Get/Graph Data` 
- I/P (params): duration, metric_type 
- Calculate start and end date based on duration 
- Get all health logs of user within the given date range 
- O/P: Health log list 

`Get/Notifications` 
- I/P: -
- Get user ID from token 
- Get user preference metrics 
- Get health logs of today 
- Check if all user pref metrics have logs 
- If not, add them to output dto  
- O/P: List of metric types 

`Get/Suggestions`
- I/P: -
- Get User ID from token 
- suggestions for user
- O/P: List of suggestions of user 

### Coach:
`POST/Coach monitor Preferences` 
- I/P: Metric list
- Get user ID from localstorage (stored after register) 
- Add them to User Preferences table one by one 
- O/P: Msg - successfully added

`GET/Coach monitor preferences`

`GET/Problems`
- I/P: -
- Get health logs 
- Health logs with status as critical
- If health log has no suggestions by coach, add user id to output dto 
- O/P: list of user ids

`POST/Suggestions` 
- I/P: User ID, Suggestion
- Add to suggestion table 
- O/P: Successfully added 

`GET/My Suggestions` 
- I/P: -
- Get coach ID from token 
- O/P: List of suggestion DTOs with like 