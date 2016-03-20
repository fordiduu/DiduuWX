--create table ReSourceRequest
--(
--Id int identity(1,1) primary key,
--Content nvarchar(500) not null default(''),
--Contact nvarchar(100) not null default(''),
--useful int not null default(0),
--AddTime datetime not null default(getdate()),
--UpdateTime datetime not null default(getdate()),
--IPAddr nvarchar(50) not null default(0.0.0.0),
--IsEnable bit not null default(1),
--OpenId nvarchar(512) NOT NULL DEFAULT('')
--)

--create table ReSourceRequest_Reply
--(
--Id int identity(1,1) primary key,
--ReplyContent nvarchar(500) not null default(''),
--AddTime datetime not null default(getdate()),
--RequestId int not null default(0)
--)


--insert into ReSourceRequest(Content,Contact,AddTime,IPAddr) values(@Content,@Contact,@AddTime,@IPAddr)

--update ReSourceRequest set IsEnable=IsEnable-1 where Id=@RId

--update ReSourceRequest set Content=@Content,Content=@Content,UpdateTime=@UpdateTime where Id=@RId

--update ReSourceRequest set useful=useful+1 where Id=@RId

select Id, Content, Contact, useful, AddTime, UpdateTime, IPAddr, IsEnable, 
(select count(Id) from dbo.ReSourceRequest_Reply where RequestId=@RId) as ReplyCount
from dbo.ReSourceRequest where Id=@RId

select Id, Content, Contact, useful, AddTime, UpdateTime, IPAddr, IsEnable




