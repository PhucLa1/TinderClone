alter procedure getCountUser
@startDate varchar(30), @endDate varchar(30)
as 
begin
SELECT CONCAT(Month(RegisDate), '-', Year(RegisDate)) as 'Time', Count(*) as 'UserCount'
FROM Users
WHERE RegisDate >= @startDate AND RegisDate <= @endDate
GROUP BY Month(RegisDate), Year(RegisDate);
end

exec getCountUser '2023-11-03','2023-11-09'
'2021-06-18'
'2023-06-18'