alter table Users add Token nvarchar(255)
alter table Users add TokenCreated Datetime
alter table Users add TokenExpires Datetime
alter table Admin add Token nvarchar(255)
alter table Admin add TokenCreated Datetime
alter table Admin add TokenExpires Datetime
