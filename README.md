# Evowill
 <img src="https://static.wixstatic.com/media/c57a4e_07ea5b6f0656421f993e6dc8832a1ebe~mv2.jpg" alt="evowill-logo">
<h2><i>Test Task of Sophia Pisotska</i></h2>
<h3><b>MainWindow</b></h3>
<img src="https://github.com/zuckgrass/Evowill/blob/main/TestTask/MainWindow.png" alt="mainwindow_screen">
:white_check_mark: Отримання статистики по одній категорії витрат для певного користувача.

<i>SQL запит для середньої ціни витрати на певний товар. Від користувача потрібно лише введення імені та назви товару.</i>

```SQL
SELECT AVG(Price) 
FROM Notes
WHERE IDPeople = (SELECT IDPerson 
               FROM People 
WHERE Name = 'Sophia'
AND IDSpent = (SELECT IDSpent 
            FROM Spendings 
            WHERE SpentName = 'їжа');
```
<i>SQL запит для дізнавання про відсоток витрат на певну категорію від витрат на усі категорій.</i>

```SQL
SELECT Cast(Cast(one AS decimal(18,2))/Cast(su as decimal(18,2))*100 as integer) 
FROM(SELECT SUM(Price) one FROM Notes 
WHERE IDPeople = (SELECT IDPerson FROM People WHERE Name = 'Sophia') 
AND IDSpent = (SELECT IDSpent FROM Spendings WHERE SpentName = 'їжа'))
a CROSS JOIN(SELECT SUM(Price) su FROM Notes
WHERE IDPeople = (SELECT IDPerson FROM People WHERE Name = 'Sophia')) b; 
```
:white_check_mark:Можливість записувати витрати по категоріям.

<i>Запит на додавання витрати по категорії. Користувача потрібно ввести ім'я, категорію витрати та ціну.</i>

```SQL
INSERT INTO Notes 
(IDNote, IDPeople, IDSpent, Price) 
VALUES((SELECT MAX(IDNote) 
        FROM Notes)+1,                   
(SELECT IDPerson FROM People 
WHERE Name = 'Sophia'), 
(SELECT IDspent FROM Spendings WHERE SpentName = 'їжа'), 200); 
```
:white_check_mark: Можливість внести витрати за певний день.

<i>Схожий на попередній запит, користувачу потрібно заповнити ще поле з датою.</i>

```SQL
INSERT INTO Notes 
(IDNote, IDPeople, IDSpent, Price, Date) 
VALUES((SELECT MAX(IDNote) 
FROM Notes) + 1,
(SELECT IDPerson FROM People 
WHERE Name = 'Maria'), 
(SELECT IDspent FROM Spendings 
WHERE SpentName = 'косметика'), 300, '03.03.2020'); 
```
:white_check_mark: Можливість очистити всі дані.

<i>Створила два запити на очищення всіх даних: для певного користувача та всіх даних у записах витрат.</i>

```SQL
DELETE FROM Notes WHERE IDPeople=
(SELECT IDPerson 
FROM People WHERE Name='Mike');
```
```SQL
DELETE FROM Notes;
```
:white_check_mark: Додати можливість мати кілька користувачів.

<i>Додала декілька користувачів у базу даних. У базу зберігається три таблиці: з користувачами, витратами та записами щодо витрат. У кожній таблиці міститься унікальний ID елемента таблиці.</i>

----
<h3><b>Statistics</b></h3>
:white_check_mark: Подивитися статистику витрат: за день, за місяць, за рік.

<i>Запити для дізнавання інформації про витрати: скільки витратили в цьому проміжку часу грощей та розподілення у відсотках витрат на кожну категорію з точністю до сотих відсотка. Користувачу потрібно ввести ім'я, рік або рік і місяць, або рік, місяць і день.</i>

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
:white_check_mark:Отримувати статистику витрат — по всім категоріям одразу.

<i>У цьому вікні користувачу нічого не потрібно вводити. Тут можна переглянути статистику найбільших витратників та суму грошей вони віддали, а також категорії витрат, на які в середньому витрачають найбільше грошей.</i>

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
<h3><b>Дякую за увагу!</b></h3>