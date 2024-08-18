use [DB20222000953]

CREATE TABLE Usuarios (
  UsuarioId int,
  Nombre varchar(50) not null,
  Contrase�a varchar(50) not null,

  constraint pkUsuario primary key(UsuarioId)
)

CREATE TABLE Empleado 
(
  EmpleadoId int ,
  Nombre varchar(50) not null,
  DNI varchar(50) not null,
  E_mail varchar(20) not null,
  Telefono varchar(50) not null,
  Direccion varchar(50) not null,
  Especializacion varchar(50) not null,
  Sueldo float not null,
  jefeId int null,
  UsuarioId int not null

    constraint pkEmpleado primary key(EmpleadoId)
)
GO

alter table Empleado add constraint fkUsurioEmpleado foreign key(UsuarioId) references Usuarios
alter table Empleado add constraint fkEmpleadoJefe foreign key(jefeId) references Empleado
alter table Empleado alter column E_mail varchar(50)



CREATE TABLE Cliente (
  Clienteid int,
  Nombre varchar(50) not null,
  DNI varchar(50) not null,
  E_mail varchar(20) not null,
  Telefono varchar(50) not null,
  Direccion varchar(50) not null,
  UsuarioId int not null

  constraint pkCliente primary key(Clienteid)
)

alter table Cliente add constraint fkUsurioCliente foreign key(UsuarioId) references Usuarios
alter table Cliente alter column E_mail varchar(50)



go
create or alter proc spUsuarios @Nombre varchar(50), @Pass varchar(50)
as
begin
select * from Usuarios where Nombre=@Nombre and Contrase�a=@Pass;
end

go
create or alter procedure spRegistrarUsuarios @Name varchar(50), @Contra varchar(50),@DNI varchar(50),@Email varchar(50),@Cel varchar(50),@Direccion varchar(50),@user varchar(50)
as
begin
declare @usuariosid int
declare @usuarioFinal int
declare @clienteid int

select @usuariosid =  ISNULL(MAX(UsuarioId),0) from Usuarios 
select @usuarioFinal= (@usuariosid+1)
select @clienteid = ISNULL(MAX(Clienteid),0) from Cliente

insert into Usuarios values (@usuarioFinal,@user,@Contra,'A')
insert into Cliente values ((@clienteid+1),@Name,@DNI,@Email,@Cel,@Direccion,@usuarioFinal)

end

select * from Empleado
go
create or alter procedure spValidarUsuario @Nombre varchar(50), @Password varchar(50)
as
	begin
		SET NOCOUNT ON;

		select UsuarioId
		from Usuarios
		where Nombre = @Nombre and Contrase�a = @Password;
	end
go


select * from Empleado
select * from Usuarios
insert into Usuarios values(15, 'Carlos Daniel Montoya','LaringeHueso04')
insert into Empleado values (1,'Carlos Montoya','0502-2004-02578','dcarlosmontoya04@gmail.com','8987-7075','Colonia Aurora','Administracion Gerencial','30000',null,15)


create or alter proc spJefes @userid int
as
begin 
select jefeId from Empleado where UsuarioId = @userid
end

create or alter proc spRegistrarEmpleadosJefes @Name varchar(50), @DNI varchar(50),@Email varchar(50),@Cel varchar(50),@Direccion varchar(50),@Area varchar(50),@Sueldo float, @Jefe int,@Pass varchar(50),@user varchar(50)
as
begin
declare @EMPLEADOMAX int
declare @USERMAX int
declare @BOSSID int

select @EMPLEADOMAX  = ISNULL(MAX(EmpleadoId),0) +1 from Empleado 
select @USERMAX =  ISNULL(MAX(UsuarioId),0) +1 from Usuarios 
select @BOSSID = 0

if @Jefe =0
begin
select @BOSSID = NULL
end

else
begin
select @BOSSID = @Jefe
end

select * from Usuarios
insert into Usuarios values(@USERMAX,@user,@Pass,'A')
insert into Empleado values(@EMPLEADOMAX,@Name,@DNI,@Email,@Cel,@Direccion,@Area,@Sueldo,@BOSSID,@USERMAX)
end

select * from Usuarios
select * from Cliente
select * from Empleado 

create or alter proc spDeleteEmpleados @user int
as
begin
			 update Usuarios set Estado = 'N' where UsuarioId = @user
end

-- Funcion para verificar si un usuario ya existe
GO
CREATE FUNCTION dbo.fnUsuarioExiste (@NombreUsuario NVARCHAR(50))
RETURNS BIT
AS
BEGIN
    DECLARE @Existe BIT;
    
    IF EXISTS (SELECT 1 FROM usuarios WHERE nombre = @NombreUsuario)
    BEGIN
        SET @Existe = 1;  -- True, el usuario existe
    END
    ELSE
    BEGIN
        SET @Existe = 0;  -- False, el usuario no existe
    END

    RETURN @Existe;
END
GO

create or alter procedure spEditarEmpleados @Name varchar(50), @DNI varchar(50),@Email varchar(50),@Cel varchar(50),@Direccion varchar(50),
@Area varchar(50),@Sueldo float, @Estado varchar(1),@EmpleadoID int, @Jefe int = null
as

     declare @UsuarioID int
	 select @UsuarioID = UsuarioId from Empleado where EmpleadoId = @EmpleadoID 


     update Empleado set Nombre = @Name, DNI = @DNI, E_mail = @Email, Telefono = @Cel, Direccion = @Direccion, Especializacion = @Area, Sueldo = @Sueldo, jefeId = @Jefe where EmpleadoId = @EmpleadoID

	 update Usuarios set Estado = @Estado where UsuarioId = @UsuarioID

go

