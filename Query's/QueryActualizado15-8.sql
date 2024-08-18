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

--Creacion de las tablas para el inventario

create table Compra
(
	CompraId int not null,
	Fecha varchar(50) not null,

	constraint pkCompra primary key(CompraId)
)

Create table Factura
(
	FacturaID int not null,
	Fecha varchar(50) not null,
	Tipo varchar not null,
	ClienteId int not null,

	constraint pkFactura primary key(FacturaID)
)

alter table Factura add constraint FkFacturaCliente foreign key(ClienteId) references Cliente

Create Table FDArticulo
(
	FacturaId int not null,
	ArticuloId int not null,
	Precio float not null,
	Tasa float not null,
	Cantidad int not null,
	MedidaId int not null
)

alter table FDArticulo alter column Cantidad float

alter table FDArticulo add constraint FkFDArticuloMedida foreign key(MedidaId) references Medida
alter table FDArticulo add constraint FkFDArticuloFactura foreign key(FacturaId) references Factura
alter table FDArticulo add constraint FkFDArticuloArticulo foreign key(ArticuloId) references Articulo

create table Medida
(
	MedidaId int not null,
	Nombre varchar(50) not null

	constraint pkMedida primary key(MedidaId)
)

create Table Articulo
(
	ArticuloId int not null,
	Nombre varchar(50) not null,
	Descripcion varchar(50) not null,
	MedidaId int not null

	constraint pkArticulo primary key(ArticuloId)
)

alter table Articulo add constraint FkArticuloMedida foreign key(MedidaId) references Medida

create table CompraDetalle
(
	Compraid int not null,
	Articuloid int not null,
	Proveedor varchar(50) not null,
	Cantidad int  not null,
	Precio float not null,
	Tasa float not null
)

alter table CompraDetalle alter column Cantidad float

alter table CompraDetalle add constraint FkCompraDetalleCompra foreign key(Compraid) references Compra
alter table CompraDetalle add constraint FkCompraDetalleArticulo foreign key(Articuloid) references Articulo


create table Inventario 
(
	ArticuloId int not null,
	Existencia int not null,
	Medidaid int not null,
	Rentabilidad float not null
) 
	alter table Inventario add constraint FkInventarioAriculo foreign key(ArticuloId) references Articulo
	alter table Inventario add constraint FkInventarioMedida foreign key(Medidaid) references Medida

go


create or alter procedure spInventario @articulolId int
as
	declare @CantidadComprada float, @CantidadFacturada float, @Existencias float, 
			@CostoTotal float, @PrecioTotal float, @Rentabilidad float

	select @CantidadComprada= cast(sum(isnull(Cantidad,0)) as numeric(12)) from CompraDetalle where Articuloid= @articulolId
	select @CantidadFacturada= cast(sum(isnull(Cantidad,0)) as numeric(12))  from FDArticulo where Articuloid= @articulolId
	select @Existencias = @CantidadComprada-@CantidadFacturada
	update Inventario set Existencia= @Existencias where ArticuloId=@articulolId
	
	select @CostoTotal= sum(((Cantidad*Precio)+(Cantidad*Precio*Tasa))) from CompraDetalle where Articuloid= @articulolId
	select @PrecioTotal= sum(((Cantidad*Precio)+(Cantidad*Precio*Tasa))) from FDArticulo where Articuloid= @articulolId
	select @Rentabilidad = @PrecioTotal-@CostoTotal
	update Inventario set Rentabilidad= @Rentabilidad where ArticuloId=@articulolId

	select *from Inventario
go

spInventario 1

select *from CompraDetalle
select *from FDArticulo
select *from Inventario

insert into Compra values(1,'15-8-2024')
insert into Compra values(2,'20-8-2024')
insert into Compra values(3,'21-8-2024')
insert into Medida values(1,'Unidad'),(2,'Metro')
insert into Articulo values(1,'Fibra Optica','Cable para internet',2)
insert into CompraDetalle values(1,1,'LA gran bodega',2,25,0.15)
insert into CompraDetalle values(2,1,'LA gran bodega',5,25,0.15)
insert into CompraDetalle values(3,1,'LA gran bodega',10,25,0.15)

insert into Factura values(1,'13-10-2024','P',1)
insert into Factura values(2,'16-11-202','P',1)
insert into FDArticulo values(1,1,40,0.15,2,2)
insert into FDArticulo values(2,1,42,0.15,12,2)

insert into Inventario values(1,0,2,0)