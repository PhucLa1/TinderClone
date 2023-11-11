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
Exec GetCountMess '2023-11-5'
alter table Users add RegisDate date
ALTER TABLE Users
ALTER COLUMN DOB Date;
select * from Users


create procedure getCountUser
@startDate varchar(30), @endDate varchar(30)
as 
begin
SELECT CONCAT(Month(RegisDate), '/', Year(RegisDate)) as 'Time', Count(*) as 'So nguoi'
FROM Users
WHERE RegisDate > '2021-06-18' AND RegisDate < '2023-06-18'
GROUP BY Month(RegisDate), Year(RegisDate);
end

