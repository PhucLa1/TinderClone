delete from Users
delete from Photo
delete from UsersLanguages
delete from UsersPassion
delete from Setting

DBCC CHECKIDENT ('Users',  RESEED, 0);
DBCC CHECKIDENT ('Photo',  RESEED, 0);
DBCC CHECKIDENT ('UsersLanguages',  RESEED, 0);
DBCC CHECKIDENT ('UsersPassion',  RESEED, 0);
DBCC CHECKIDENT ('Setting',  RESEED, 0);

DBCC CHECKIDENT ('Users', NORESEED);
DBCC CHECKIDENT ('Photo', NORESEED);
DBCC CHECKIDENT ('UsersLanguages', NORESEED);
DBCC CHECKIDENT ('UsersPassion', NORESEED);
DBCC CHECKIDENT ('Setting', NORESEED);