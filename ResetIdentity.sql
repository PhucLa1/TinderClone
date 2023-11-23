delete from Users
delete from Photo
delete from UsersLanguages
delete from UsersPassion
delete from Setting
--5 bang
delete from Likes
delete from Unlike
delete from Mess
delete from Calls
delete from SuggestedQuestion
--16 bang
delete from SexsualOrientation
delete from Communication
delete from LoveLanguage
delete from Pet
delete from Alcolhol
delete from Diet
delete from VacxinCovid
delete from Zodiac
delete from Personality
delete from Smoke
delete from SocialMedia
delete from Education
delete from PurposeDate
delete from FutureFamily
delete from Workout
delete from SleepHabit

DBCC CHECKIDENT ('Users',  RESEED, 0);
DBCC CHECKIDENT ('Photo',  RESEED, 0);
DBCC CHECKIDENT ('UsersLanguages',  RESEED, 0);
DBCC CHECKIDENT ('UsersPassion',  RESEED, 0);
DBCC CHECKIDENT ('Setting',  RESEED, 0);

--16 bang
DBCC CHECKIDENT ('SexsualOrientation',  RESEED, 0);
DBCC CHECKIDENT ('Communication',  RESEED, 0);
DBCC CHECKIDENT ('LoveLanguage',  RESEED, 0);
DBCC CHECKIDENT ('Pet',  RESEED, 0);
DBCC CHECKIDENT ('Alcolhol',  RESEED, 0);
DBCC CHECKIDENT ('Diet',  RESEED, 0);
DBCC CHECKIDENT ('VacxinCovid',  RESEED, 0);
DBCC CHECKIDENT ('Zodiac',  RESEED, 0);
DBCC CHECKIDENT ('Personality',  RESEED, 0);
DBCC CHECKIDENT ('Smoke',  RESEED, 0);
DBCC CHECKIDENT ('SocialMedia',  RESEED, 0);
DBCC CHECKIDENT ('Education',  RESEED, 0);
DBCC CHECKIDENT ('PurposeDate',  RESEED, 0);
DBCC CHECKIDENT ('FutureFamily',  RESEED, 0);
DBCC CHECKIDENT ('Workout',  RESEED, 0);
DBCC CHECKIDENT ('SleepHabit',  RESEED, 0);

--4 bang
DBCC CHECKIDENT ('Likes',  RESEED, 0);
DBCC CHECKIDENT ('Unlike',  RESEED, 0);
DBCC CHECKIDENT ('Mess',  RESEED, 0);
DBCC CHECKIDENT ('Calls',  RESEED, 0);
DBCC CHECKIDENT ('SuggestedQuestion',  RESEED, 0);


DBCC CHECKIDENT ('Users', NORESEED);
DBCC CHECKIDENT ('Photo', NORESEED);
DBCC CHECKIDENT ('UsersLanguages', NORESEED);
DBCC CHECKIDENT ('UsersPassion', NORESEED);
DBCC CHECKIDENT ('Setting', NORESEED);

--16 bang
DBCC CHECKIDENT ('SexsualOrientation',  NORESEED);
DBCC CHECKIDENT ('Communication',  NORESEED);
DBCC CHECKIDENT ('LoveLanguage',  NORESEED);
DBCC CHECKIDENT ('Pet',  NORESEED);
DBCC CHECKIDENT ('Alcolhol',  NORESEED);
DBCC CHECKIDENT ('Diet',  NORESEED);
DBCC CHECKIDENT ('VacxinCovid',  NORESEED);
DBCC CHECKIDENT ('Zodiac',  NORESEED);
DBCC CHECKIDENT ('Personality',  NORESEED);
DBCC CHECKIDENT ('Smoke',  NORESEED);
DBCC CHECKIDENT ('SocialMedia',  NORESEED);
DBCC CHECKIDENT ('Education',  NORESEED);
DBCC CHECKIDENT ('PurposeDate',  NORESEED);
DBCC CHECKIDENT ('FutureFamily',  NORESEED);
DBCC CHECKIDENT ('Workout',  NORESEED);
DBCC CHECKIDENT ('SleepHabit',  NORESEED);

--5 bang
DBCC CHECKIDENT ('Likes',  NORESEED);
DBCC CHECKIDENT ('Unlike',  NORESEED);
DBCC CHECKIDENT ('Mess',  NORESEED);
DBCC CHECKIDENT ('Calls',  NORESEED);
DBCC CHECKIDENT ('SuggestedQuestion',  NORESEED);