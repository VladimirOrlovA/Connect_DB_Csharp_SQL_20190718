

-----------------------------------------------  Lesson 20190718  --------------------------------------------------

-- Server name -  sd\test_SQL
-- Login - stud
-- Pass - 123456789

--------------------------------------------  ������ C# � SQL server  ----------------------------------------------

--- https://teletype.in/@cozy_codespace/BkKJ4zO07 -- ���������� �� C# + ������ � MySQL ����� ������

OLEDB

connection

ORM Object Realation Model


--------------------------------------------------------------------------------------------------------------------


use InternetShop

select product_id, product_name, product_model from Product

select * from Product


CREATE PROCEDURE prod_sum
(
	@id INT,
	@res decimal OUT
)
AS
BEGIN 
 set @res=(SELECT SUM(cost) FROM Product WHERE product_id<=@id)
END 

declare 
@res decimal
exec prod_sum 1, @res out
print @res


--------------------------------------------------------------------------------------------------------------------





--------------------------------------------------------------------------------------------------------------------





--------------------------------------------------------------------------------------------------------------------