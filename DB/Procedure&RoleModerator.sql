
go 
--create role Moderator
Create ROLE Moderator;
Grant Select, Update, Insert On dbo.UserInfo to Moderator
Grant Select On dbo.Province to Moderator
Grant Select On dbo.Users to Moderator
Grant Select, Insert On dbo.District to Moderator
Grant Select, Insert On dbo.Commune to Moderator
Grant Select, Insert On dbo.Address to Moderator

-- P -- R -- O -- C -- E -- D -- U -- R -- E -- 