# Health Tracker Application:

## Models:
```
Enums:

ENUM Metric_Type    - Blood_Pressure_Diastolic, Blood_Pressure_Systolic, Sugar_level, Water_Intake, Steps_Count, Sleep_hours, Height, Weight, Calories_Intake, Calories_Burned

ENUM Range_category - Weight, Height, Age

ENUM Health_status  - Excellent, Good, Fair, Poor, Critical

ENUM Target_status  - Ongoing, Achieved, Not_Achieved

ENUM Role           - User, Coach 

ENUM User_status    - Active, Inactive

ENUM Gender         - Female, Male, Others
```
### For User:
- **User Details:** [ID], Password Hash, Password encrypted, Status (for soft delete)
- **User:** [User Detail ID], Name, Age, Gender, Phone, Email ID, notification permission
- **User Preferences:** [ID], User ID, Metric Type (enum)
- **Units:** [ID], Metric_Type, unit 
- **Target:** [ID], Preference ID, Target Min Value, Target Max Value, Unit ID, status, start date, end date, datetime 
- **Health Log:** [ID], Preference ID, value, unit ID, date, Heath status
- **Ideal data:** [ID], Health status, Gender, Metric_Type, Weight_Range, Height_Range, Age_Range, Min_val, Max_val, unit ID 
- **Metric_Ranges:** [ID], Category, Min_val, Max_val, unit ID 

### For health coach:
- **Coach:** [User Detail ID], Name, Age, Gender, Phone, Email ID, Certificate, Trustability score
- **Monitor Preferences:** [ID], Coach ID, Metric Type (enum)
- **Suggestions:** [ID], Coach ID, Health Log ID, Suggestion, Date, is_like

## Endpoints:
### User:

`POST/Register user`
- Fill User and UserDetails table 
- O/P: User ID 

`POST/Add and Update Height`
- I/P: Get height - add to health log (no checking needed)
- O/P: Msg - successfully added 
        
`POST/Add and Update Weight` 
- I/P: Get weight - add to health log (no checking needed)
- O/P: Msg - successfully added 
        
`POST/User Preferences` 
- I/P: Metric list
- Get user ID from localstorage (stored after register) 
- Add them to User Preferences table one by one 
- O/P: Msg - successfully added

`POST/Login` 
- I/P: Email, Password 
- O/P: Msg - successfully logged in 

`POST/Health log`
- I/P: Health log data - category, value, unit, date 
- Find preference ID 
- Check if log already added for today i.e., check if preference id and today's date are present already 
    - If so, send error - Already added

- Check data value against ideal value 
    - Get weight, height, age range IDs from values 
    - Metric type and gender use - Get the ideal value record 
    - Get the health status 
    - Add health status
    - Add to output DTO

- Check data value against target if any set 
    - Preference ID, start date, end date - recent datetime (latest in that range) - using this find target data 
    - Check if value inside target range 
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
- Get health logs of user - check if those health log ids are present in suggestions 
- O/P: List of suggestions of user 

`PUT/Update Coach Score`
- I/P: Suggestion ID, is_like
- Get coach ID and update to trustability score (add or minus) 
- Update like to true or false
- O/P: Msg - Successfully updated 

### Coach:
`POST/Register user`
- Fill User and UserDetails table 
- O/P: User ID 

`POST/Coach monitor Preferences` 
- I/P: Metric list
- Get user ID from localstorage (stored after register) 
- Add them to User Preferences table one by one 
- O/P: Msg - successfully added

`POST/Login` 
- I/P: Email, Password 
- Check if user active 
- O/P: Msg - successfully logged in 

`GET/Problems`
- I/P: -
- Get health logs 
- Health logs with coach's suggestions as critical
- If health log has no suggestions by coach, add to output dto  
- O/P: list of health logs 

`POST/Suggestions` 
- I/P: User ID, Suggestion
- Add to suggestion table 
- O/P: Successfully added 

`GET/My Suggestions` 
- I/P: -
- Get coach ID from token 
- O/P: List of suggestion DTOs with like 