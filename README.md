# Evowill  
![evowill-logo](https://static.wixstatic.com/media/c57a4e_07ea5b6f0656421f993e6dc8832a1ebe~mv2.jpg)  

### **MainWindow**  
![mainwindow_screen](https://github.com/zuckgrass/Evowill/blob/main/TestTask/MainWindow.png)

:white_check_mark: **Get statistics for a specific expense category for a certain user.**

*SQL query for the average price of an expense for a specific item. The user only needs to input the name and item name.*

```SQL
SELECT AVG(Price) 
FROM Notes
WHERE IDPeople = (SELECT IDPerson 
               FROM People 
WHERE Name = 'Sophia'
AND IDSpent = (SELECT IDSpent 
            FROM Spendings 
            WHERE SpentName = 'Food');
```
SQL query to calculate the percentage of expenses for a specific category out of total expenses.
```SQL
SELECT Cast(Cast(one AS decimal(18,2))/Cast(su as decimal(18,2))*100 as integer) 
FROM(SELECT SUM(Price) one FROM Notes 
WHERE IDPeople = (SELECT IDPerson FROM People WHERE Name = 'Sophia') 
AND IDSpent = (SELECT IDSpent FROM Spendings WHERE SpentName = 'Food'))
a CROSS JOIN(SELECT SUM(Price) su FROM Notes
WHERE IDPeople = (SELECT IDPerson FROM People WHERE Name = 'Sophia')) b; 
```
:white_check_mark: Ability to record expenses by category.

Query to add an expense for a category. The user needs to enter their name, category, and price.
```SQL
INSERT INTO Notes 
(IDNote, IDPeople, IDSpent, Price) 
VALUES((SELECT MAX(IDNote) 
        FROM Notes)+1,                   
(SELECT IDPerson FROM People 
WHERE Name = 'Sophia'), 
(SELECT IDspent FROM Spendings WHERE SpentName = 'Food'), 200); 
```
:white_check_mark: Ability to add expenses for a specific day.

Similar to the previous query, the user also needs to fill in the date field.
```SQL
INSERT INTO Notes 
(IDNote, IDPeople, IDSpent, Price, Date) 
VALUES((SELECT MAX(IDNote) 
FROM Notes) + 1,
(SELECT IDPerson FROM People 
WHERE Name = 'Maria'), 
(SELECT IDspent FROM Spendings 
WHERE SpentName = 'Cosmetics'), 300, '03.03.2020'); 
```
:white_check_mark: Ability to clear all data.

Created two queries to clear all data: one for a specific user and one for all data in the expense records.
```SQL
DELETE FROM Notes WHERE IDPeople=
(SELECT IDPerson 
FROM People WHERE Name='Mike');
```
```SQL
DELETE FROM Notes;
```
:white_check_mark: Ability to have multiple users.

Added multiple users to the database. The database contains three tables: users, expenses, and records. Each table contains a unique ID for each item.
----
<h3><b>Statistics</b></h3>
<img src="https://github.com/zuckgrass/Evowill/blob/main/TestTask/StatisticsYear.png" alt="statistics-per-year">
<img src="https://github.com/zuckgrass/Evowill/blob/main/TestTask/StatisticsMonth.png" alt="statistics-per-year-month">
:white_check_mark: View expense statistics: by day, month, and year.

Queries to get information about expenses: how much money was spent in the selected time period, and the percentage breakdown of expenses by category, rounded to two decimal places. The user needs to input their name, year, or year and month, or year, month, and day.

```SQL
SELECT coalesce(SUM(Price),0)
FROM Notes 
WHERE IDPeople = (
SELECT IDPerson FROM People WHERE Name = 'Sophia')
AND Year(Date)=2022 
AND Month(Date)=4 
AND Day(Date)=3;
```
```SQL
SELECT  coalesce(Cast(Round(Cast(
    one AS decimal(18,2))/Cast(
    su as decimal(18,2))*100 ,2) as decimal(18,2)),0)
FROM Spendings sp 
Left join(SELECT SUM(Price) one, IDSpent
FROM Notes
WHERE IDPeople = (
    SELECT IDPerson FROM People WHERE Name = 'Sophia') 
AND Year(Date) =2022  
AND Month(Date)=4 
AND Day(Date)=3
Group by IDSpent 
)a ON sp.IDspent = a.IDSpent 
CROSS JOIN(SELECT SUM(Price) su 
FROM Notes
WHERE IDPeople = (
SELECT IDPerson FROM People WHERE Name = 'Sophia') 
AND Year(Date)=2022  
AND Month(Date)=4 AND 
Day(Date)=3
) b;
```
----
<h3><b>Window2</b></h3>
<img src="https://github.com/zuckgrass/Evowill/blob/main/TestTask/Window2.png" alt="window2_screen">
:white_check_mark: View expense statistics — for all categories at once.

In this window, the user doesn't need to input anything. Here, the user can view the statistics of the biggest spenders and the total money they spent, as well as the categories in which the most money is spent on average.
```SQL
SELECT pep.Name, SUM(Price) 
FROM Notes 
note Left join People pep 
ON pep.IDPerson = note.IDPeople 
GROUP by pep.Name 
ORDER BY Sum(Price) desc;
```
```SQL
SELECT sp.SpentName, AVG(Price) 
FROM Notes note LEFT JOIN Spendings sp
ON note.IDSpent = sp.IDspent 
GROUP BY SpentName 
ORDER BY AVG(Price) desc;
```
----
<h3><b>Thank you for your attention!</b></h3>
