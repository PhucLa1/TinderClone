Use TinderClone

--Create table Users
create table Users(
ID int identity(1,1) primary key,
SettingID int,
PermissionID int,
FullName nvarchar(50),
UserName varchar(100),
TagName nvarchar(100),
LikeAmount int,
Pass varchar(max),
GoogleID varchar(max),
FacebookID varchar(max),
IsBlocked bit default(0),
IsDeleted bit default(0),
AboutUser text, --Introduce yourselves by an essay
PurposeDateID smallint,
Gender bit,
SexsualOrientationID smallint,
Height smallint,
ZodiacID smallint,
EducationID smallint,
FutureFamilyID smallint,
VacxinCovidID smallint,
PersonalityID smallint,
CommunicationID smallint,
LoveLanguageID smallint,
PetID smallint,
AlcolholID smallint,
SmokeID smallint,
WorkoutID smallint,
DietID smallint,
SocialMediaID smallint,
SleepHabitID smallint,
JobTitle varchar(255),
Company nvarchar(max),
School nvarchar(255),
LiveAt nvarchar(255),
OfStatus tinyint default(1)
)


--Create table UserLanguage
create table UsersLanguages(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
LanguageID int,
UserID int
)

--Create table Languages
create table Languages(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
LName nvarchar(100),
Descriptions text
)

--Create table UsersPassion
create table UsersPassion(
	ID int identity(1,1) primary key,
	OfStatus tinyint default(1),
	PassionID int,
	UserID int
)

create table Passion(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
PName nvarchar(255),
Descriptions text
)
create table Permission(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
PerName nvarchar(100),
RoleDetails text
)

create table Setting(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
Latitute float,
PhoneNumber varchar(12),
Longtitute float,
DistancePreference int,
LookFor nvarchar(100),
AgeMin int,
AgeMax int,
DistanceUnit int,
GlobalMatches int,
HideAge int,
Email varchar(50),
HideDistance int,
)

Create table Photo(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
ImagePath varchar(100),
UserID int
)
create table SexsualOrientation(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
SOName nvarchar(50)
)
create table Communication(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
CName nvarchar(50)
)
create table LoveLanguage(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
LLName nvarchar(50)
)
create table Pet(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
PName nvarchar(50)
)
create table Alcolhol(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
AName nvarchar(100)
)
create table Diet(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
DName nvarchar(100)
)
create table VacxinCovid(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
VCName nvarchar(100)
)
create table Zodiac(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
ZName nvarchar(30)
)
create table Personality(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
PName nvarchar(100)
)
create table Smoke(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
SName nvarchar(100)
)
create table SocialMedia(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
SMName nvarchar(100)
)
create table Education(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
EName nvarchar(100)
)
create table PurposeDate(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
PDName nvarchar(100)
)
create table FutureFamily(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
FFName nvarchar(100)
)
create table Workout(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
WName nvarchar(100)
)
create table SleepHabit(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
SHName nvarchar(100)
)
create table Mess(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
SendUserID int,
ReceiveUserID int,
Content text,
SendTime datetime,
)
create table Blocks(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
BlockUserID int,
BlockedUserID int,
BlockDate datetime
)
create table Unlike(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
UnlikeUserID int,
UnlikedUserID int,
UnlikeDate datetime,
)
create table Likes(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
LikeUserID int,
LikedUserID int,
LikeDate datetime,
Matches bit
)
create table SuggestedQuestion(
	ID int identity(1,1) primary key,
	OfStatus tinyint default(1),
	Ques text
)
create table Calls(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
CallerID int,
ReceiverID int,
StartTime datetime,
EndTime datetime,
Duration smallint,
CallStatusID int
)
create table CallStatus(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
CSName varchar(30)
)
create table Admin(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
UserName nvarchar(100) not null,
Pass varchar(100) not null
)
