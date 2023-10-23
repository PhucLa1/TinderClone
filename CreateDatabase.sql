Use TinderClone

--Create table Users
create table Users(
ID int identity(1,1) primary key,
SettingID int,
PermissionID int,
FullName nvarchar(50),
Email varchar(50),
UserName varchar(100),
TagName nvarchar(100),
LikeAmount int,
Pass varchar(max),
GoogleID varchar(max),
FacebookID varchar(max),
PhoneNumber varchar(12),
IsBlocked bit default(0),
IsDeleted bit default(0),
AboutUser text, --Introduce yourselves by an essay
PassionID int,
JobTitle varchar(255),
LookingFor varchar(255),
Gender bit,
SexsualOrientation varchar(255),
Height int,
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
Longtitute float,
DistancePreference int,
LookFor nvarchar(100),
AgeMin int,
AgeMax int,
DistanceUnit int,
GlobalMatches int,
HideAge int,
HideDistance int,
)

Create table Photo(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
ImagePath varchar(100),
UserID int
)

create table UserWork(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
UserID int,
WorkID int
)
create table UserLifeStyle(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
UserID int,
LifeStyleID int
)
create table Work(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
WName nvarchar(255),
Descriptions text
)
create table LifeStyle(
ID int identity(1,1) primary key,
OfStatus tinyint default(1),
LSName nvarchar(255),
Descriptions text
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
