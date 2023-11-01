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
    JOIN PurposeDate AS UP ON U.PurposeDateID = UP.ID
    JOIN SexsualOrientation AS SO ON U.SexsualOrientationID = SO.ID
    JOIN Zodiac AS Z ON U.ZodiacID = Z.ID
    JOIN Education AS E ON U.EducationID = E.ID
    JOIN FutureFamily AS FF ON U.FutureFamilyID = FF.ID
    JOIN VacxinCovid AS VC ON U.VacxinCovidID = VC.ID
    JOIN Personality AS P ON U.PersonalityID = P.ID
    JOIN Communication AS C ON U.CommunicationID = C.ID
    JOIN LoveLanguage AS LL ON U.LoveLanguageID = LL.ID
    JOIN Pet AS Pet ON U.PetID = Pet.ID
    JOIN Alcolhol AS A ON U.AlcolholID = A.ID
    JOIN Smoke AS S ON U.SmokeID = S.ID
    JOIN Workout AS W ON U.WorkoutID = W.ID
    JOIN Diet AS D ON U.DietID = D.ID
    JOIN SocialMedia AS SM ON U.SocialMediaID = SM.ID
    JOIN SleepHabit AS SH ON U.SleepHabitID = SH.ID
    WHERE U.Id = @UserId;
END

EXEC GetUserProfile 1

select * from Users