EXEC GetUserProfile 1
alter PROCEDURE GetUserProfile
    @UserId INT
AS
BEGIN
    SELECT
        U.ID,
        U.FullName,
        U.TagName,
        U.LikeAmount,
        U.AboutUser,
        DATEDIFF(YEAR, U.DOB, GETDATE()) - 
        CASE WHEN (MONTH(U.DOB) > MONTH(GETDATE()) OR 
                  (MONTH(U.DOB) = MONTH(GETDATE()) AND DAY(U.DOB) > DAY(GETDATE())))
            THEN 1
            ELSE 0
        END  as 'Age',
        UP.PDName as 'PurposeDate',
        U.Gender,
        SO.SOName as 'SexsualOrientation',
        U.Height,
        Z.ZName as 'Zodiac',
        E.EName as 'Education',
        FF.FFName as 'FutureFamily',
        VC.VCName as 'VacxinCovid',
        P.PName as 'Personality',
        C.CName as 'Communication',
        LL.LLName as 'LoveLanguage',
        Pet.PName as 'Pet',
        A.AName as 'Alcolhol',
        S.SName as 'Smoke',
        W.WName as 'Workout',
        D.DName as 'Diet',
        SM.SMName as 'SocialMedia',
        SH.SHName as 'SleepHabit',
        U.JobTitle,
        U.Company,
        U.School,
        U.LiveAt
    FROM Users AS U
    LEFT JOIN PurposeDate AS UP ON U.PurposeDateID = UP.ID
    LEFT JOIN SexsualOrientation AS SO ON U.SexsualOrientationID = SO.ID
    LEFT JOIN Zodiac AS Z ON U.ZodiacID = Z.ID
    LEFT JOIN Education AS E ON U.EducationID = E.ID
    LEFT JOIN FutureFamily AS FF ON U.FutureFamilyID = FF.ID
    LEFT JOIN VacxinCovid AS VC ON U.VacxinCovidID = VC.ID
    LEFT JOIN Personality AS P ON U.PersonalityID = P.ID
    LEFT JOIN Communication AS C ON U.CommunicationID = C.ID
    LEFT JOIN LoveLanguage AS LL ON U.LoveLanguageID = LL.ID
    LEFT JOIN Pet AS Pet ON U.PetID = Pet.ID
    LEFT JOIN Alcolhol AS A ON U.AlcolholID = A.ID
    LEFT JOIN Smoke AS S ON U.SmokeID = S.ID
    LEFT JOIN Workout AS W ON U.WorkoutID = W.ID
    LEFT JOIN Diet AS D ON U.DietID = D.ID
    LEFT JOIN SocialMedia AS SM ON U.SocialMediaID = SM.ID
    LEFT JOIN SleepHabit AS SH ON U.SleepHabitID = SH.ID
    WHERE U.Id = @UserId;
END


//Get all user in database
alter PROCEDURE GetAllUserProfile
AS
BEGIN
    SELECT
        U.ID,
        U.FullName,
        U.TagName,
        U.LikeAmount,
        U.AboutUser,
DATEDIFF(YEAR, U.DOB, GETDATE()) - 
        CASE WHEN (MONTH(U.DOB) > MONTH(GETDATE()) OR 
                  (MONTH(U.DOB) = MONTH(GETDATE()) AND DAY(U.DOB) > DAY(GETDATE())))
            THEN 1
            ELSE 0
        END  as 'Age',

        UP.PDName as 'PurposeDate',
        U.Gender,
        SO.SOName as 'SexsualOrientation',
        U.Height,
        Z.ZName as 'Zodiac',
        E.EName as 'Education',
        FF.FFName as 'FutureFamily',
        VC.VCName as 'VacxinCovid',
        P.PName as 'Personality',
        C.CName as 'Communication',
        LL.LLName as 'LoveLanguage',
        Pet.PName as 'Pet',
        A.AName as 'Alcolhol',
        S.SName as 'Smoke',
        W.WName as 'Workout',
        D.DName as 'Diet',
        SM.SMName as 'SocialMedia',
        SH.SHName as 'SleepHabit',
        U.JobTitle,
        U.Company,
        U.School,
        U.LiveAt
    FROM Users AS U
    LEFT JOIN PurposeDate AS UP ON U.PurposeDateID = UP.ID
    LEFT JOIN SexsualOrientation AS SO ON U.SexsualOrientationID = SO.ID
    LEFT JOIN Zodiac AS Z ON U.ZodiacID = Z.ID
    LEFT JOIN Education AS E ON U.EducationID = E.ID
    LEFT JOIN FutureFamily AS FF ON U.FutureFamilyID = FF.ID
    LEFT JOIN VacxinCovid AS VC ON U.VacxinCovidID = VC.ID
    LEFT JOIN Personality AS P ON U.PersonalityID = P.ID
    LEFT JOIN Communication AS C ON U.CommunicationID = C.ID
    LEFT JOIN LoveLanguage AS LL ON U.LoveLanguageID = LL.ID
    LEFT JOIN Pet AS Pet ON U.PetID = Pet.ID
    LEFT JOIN Alcolhol AS A ON U.AlcolholID = A.ID
    LEFT JOIN Smoke AS S ON U.SmokeID = S.ID
    LEFT JOIN Workout AS W ON U.WorkoutID = W.ID
    LEFT JOIN Diet AS D ON U.DietID = D.ID
    LEFT JOIN SocialMedia AS SM ON U.SocialMediaID = SM.ID
    LEFT JOIN SleepHabit AS SH ON U.SleepHabitID = SH.ID
END

//Get count mess in one day
alter procedure GetCountMess 
@Date varchar(30)
as
begin
WITH AllHours AS (
  SELECT 0 AS Hour
  UNION ALL SELECT 1
  UNION ALL SELECT 2
  UNION ALL SELECT 3
  UNION ALL SELECT 4
  UNION ALL SELECT 5
  UNION ALL SELECT 6
  UNION ALL SELECT 7
  UNION ALL SELECT 8
  UNION ALL SELECT 9
  UNION ALL SELECT 10
  UNION ALL SELECT 11
  UNION ALL SELECT 12
  UNION ALL SELECT 13
  UNION ALL SELECT 14
  UNION ALL SELECT 15
  UNION ALL SELECT 16
  UNION ALL SELECT 17
  UNION ALL SELECT 18
  UNION ALL SELECT 19
  UNION ALL SELECT 20
  UNION ALL SELECT 21
  UNION ALL SELECT 22
  UNION ALL SELECT 23
)

SELECT AllHours.Hour, COUNT(Mess.SendTime) AS MessageCount
FROM AllHours
LEFT JOIN Mess ON AllHours.Hour = DATEPART(HOUR, Mess.SendTime)
WHERE CAST(Mess.SendTime AS DATE) = @Date OR Mess.SendTime IS NULL
GROUP BY AllHours.Hour
ORDER BY AllHours.Hour;
end

//Get user by regis date
alter procedure getCountUser
@startDate varchar(30), @endDate varchar(30)
as 
begin
SELECT CONCAT(Month(RegisDate), '-', Year(RegisDate)) as 'Time', Count(*) as 'UserCount'
FROM Users
WHERE RegisDate >= @startDate AND RegisDate <= @endDate
GROUP BY Month(RegisDate), Year(RegisDate);
end

