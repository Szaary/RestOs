use [EFData]

insert into dbo.AspNetRoles (Id, [Name]) 
values (NEWID(), 'Cashier')
, (NEWID(), 'Manager')
, (NEWID(), 'Admin')


-- Do it manualy, adding admin roles
-- insert into dbo.AspNetUserRoles (UserId, RoleId)
-- values (f5aeb53c-af6f-44af-8e76-950626192266, 92F3F7C4-044A-48D3-890D-97EE27B93B5F)  -- admin
-- , (f5aeb53c-af6f-44af-8e76-950626192266 , C845328E-74B1-4619-A738-9E760DF67B94)      -- user