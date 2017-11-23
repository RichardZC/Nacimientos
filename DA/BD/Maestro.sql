SET FOREIGN_KEY_CHECKS=0;

DROP TABLE IF EXISTS Oficina;
Create table Oficina(
	OficinaId int(11) PRIMARY KEY auto_increment not NULL,
	Denominacion VARCHAR(255) not NULL
);
INSERT INTO `oficina` VALUES ('1', 'DIRECCION EJECUTIVA');
INSERT INTO `oficina` VALUES ('2', 'ADMINISTRACION');
INSERT INTO `oficina` VALUES ('3', 'ABASTECIMIENTO');
INSERT INTO `oficina` VALUES ('4', 'ALMACEN');
INSERT INTO `oficina` VALUES ('5', 'PERSONAL');

DROP TABLE IF EXISTS Cargo;
Create table Cargo(
	CargoId int(11) PRIMARY KEY auto_increment not NULL,
	Denominacion VARCHAR(255) not NULL
);
INSERT INTO `Cargo` VALUES ('1', 'DIRECTOR EJECUTIVO');
INSERT INTO `Cargo` VALUES ('2', 'ADMINISTRADOR');
INSERT INTO `Cargo` VALUES ('3', 'JEFE ABASTECIMIENTO');
INSERT INTO `Cargo` VALUES ('4', 'JEFE ALMACEN');
INSERT INTO `Cargo` VALUES ('5', 'JEFE PERSONAL');

DROP TABLE IF EXISTS nacimiento;
Create table nacimiento(
	NacimientoId int(11) PRIMARY KEY auto_increment not NULL,
	ApellidoNombre VARCHAR(255) not NULL,
  Fecha DATE,
	Sexo enum('F','M') DEFAULT 'F',
  NroActa int(11),
  NroLibro int(11),
  Url varchar(100),
	Registro datetime
);
DROP TABLE IF EXISTS nacimiento_anexo;
Create table nacimiento_anexo(
	Nacimiento_AnexoId int(11) PRIMARY KEY auto_increment not NULL,
	NacimientoId int(11) REFERENCES nacimiento(NacimientoId),
  url varchar(100)	
);
DROP TABLE IF EXISTS matrimonio;
Create table matrimonio(
	MatrimonioId int(11) PRIMARY KEY auto_increment not NULL,
	ApellidoNombre VARCHAR(255) not NULL,
	Conyugue VARCHAR(255),
  Fecha DATE,
	NroActa int(11),
  NroLibro int(11),
  Url varchar(100),
	Registro datetime
);
DROP TABLE IF EXISTS matrimonio_anexo;
Create table matrimonio_anexo(
	Matrimonio_AnexoId int(11) PRIMARY KEY auto_increment not NULL,
	MatrimonioId int(11) REFERENCES matrimonio(MatrimonioId),
  url varchar(100)	
);
DROP TABLE IF EXISTS defuncion;
Create table defuncion(
	DefuncionId int(11) PRIMARY KEY auto_increment not NULL,
	ApellidoNombre VARCHAR(255) not NULL,
  Fecha DATE,
	Sexo enum('F','M') DEFAULT 'F',
  NroActa int(11),
  NroLibro int(11),
  Url varchar(100),
	Registro datetime
);
DROP TABLE IF EXISTS defuncion_anexo;
Create table defuncion_anexo(
	Defuncion_AnexoId int(11) PRIMARY KEY auto_increment not NULL,
	DefuncionId int(11) REFERENCES defuncion(DefuncionId),
  url varchar(100)	
);

DROP TABLE IF EXISTS Persona;
CREATE TABLE Persona (
  PersonaId int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
  Nombres varchar(60) NOT NULL,
  Paterno varchar(60) ,
  Materno varchar(60) ,
  NombreCompleto varchar(255) NOT NULL,
  DNI varchar(8) NOT NULL,
  Celular varchar(10) ,
  Correo varchar(100) ,
  Sexo enum('F','M') DEFAULT 'F',
  FechaNacimiento date
);
INSERT INTO persona VALUES ('1', 'Administrador', 'Administrador', 'Administrador', 'Administrador', '99999999', null, null, 'M', null);


DROP TABLE IF EXISTS Usuario;
create table Usuario(
	UsuarioId int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
	PersonaId int(11) NULL,
	Nombre varchar(60) NOT NULL,
	Clave varchar(60) NOT NULL,
	Activo bit(1) NOT NULL DEFAULT b'1',
    IndCambio bit(1) NOT NULL DEFAULT b'1',
	FOREIGN KEY(PersonaId) REFERENCES Persona(PersonaId) on DELETE no action on UPDATE CASCADE,
    CargoId int(11) NOT NULL,
    FOREIGN KEY(CargoId) REFERENCES Cargo(CargoId) on DELETE no action on UPDATE CASCADE,
    OficinaId int(11) NOT NULL,
    FOREIGN KEY(OficinaId) REFERENCES Oficina(OficinaId) on DELETE no action on UPDATE CASCADE
);
INSERT INTO usuario VALUES ('1', '1', 'ADMIN', '202cb962ac59075b964b07152d234b70', 1, 0,1,2);

DROP TABLE IF EXISTS rol;
create table rol(
	RolId int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
	Denominacion VARCHAR(100) NOT NULL 
);
INSERT INTO rol VALUES (1, 'ADMINISTRADOR');
INSERT INTO rol VALUES (2, 'REGISTRO CIVIL');
INSERT INTO rol VALUES (3, 'SEGURIDAD');
INSERT INTO rol VALUES (4, 'CAJERO');

DROP TABLE IF EXISTS Usuario_Rol;
CREATE TABLE Usuario_Rol(
	Id INT(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
	UsuarioId int(11) NOT NULL ,
  FOREIGN KEY(UsuarioId) REFERENCES Usuario(UsuarioId) on DELETE no action on UPDATE CASCADE,
	RolId INT(11) NOT NULL ,
  FOREIGN KEY(RolId) REFERENCES Rol(RolId) on DELETE no action on UPDATE CASCADE
	
);
INSERT INTO usuario_rol VALUES (1,1,1);

DROP TABLE IF EXISTS menu;
CREATE TABLE menu (
  MenuId int(11) PRIMARY KEY,
  Denominacion varchar(50) DEFAULT NULL,
  Modulo varchar(50) DEFAULT NULL,
  Icono varchar(200) DEFAULT NULL,
  IndPadre bit(1) DEFAULT NULL,  
  Referencia int(11) DEFAULT NULL
);
INSERT INTO menu VALUES (1, 'Mantenimientos', 'Oficina', 'mdi-action-settings', 1, null);
INSERT INTO menu VALUES (2, 'Persona', 'Persona', 			null, 0, 1);
INSERT INTO menu VALUES (3, 'Mantenimiento', 'Mantenimiento',null, 0, 1);

INSERT INTO menu VALUES (10, 'Seguridad', 'Usuario','mdi-hardware-security', 1, null);
INSERT INTO menu VALUES (11, 'Usuarios', 'Usuario', 	null,0, 10);
INSERT INTO menu VALUES (12, 'Roles', 'Rol', 			null,0, 10);

INSERT INTO menu VALUES (20, 'Registro civil', 'Nacimiento', 'mdi-action-book', 1, null);
INSERT INTO menu VALUES (21, 'Nacimientos', 'Nacimiento', null, 0, 20);
INSERT INTO menu VALUES (22, 'Matrimonios', 'Matrimonio', null, 0, 20);
INSERT INTO menu VALUES (23, 'Defunciones', 'Defuncion', 	null, 0, 20);
INSERT INTO menu VALUES (24, 'Reportes', 'Informe', 		null, 0, 20);

INSERT INTO menu VALUES (30, 'Caja', 		'Cajadiario', 	'mdi-action-book', 	1, null);
INSERT INTO menu VALUES (31, 'Diario', 		'Diario', 		null, 0, 30);
INSERT INTO menu VALUES (32, 'Caja Diario', 'Cajadiario', 	null, 0, 30);

INSERT INTO menu VALUES (40, 'Oficina', 		'Oficina', 	'mdi-action-book', 	1, null);
INSERT INTO menu VALUES (41, 'Bandeja', 		'Bandeja', 		null, 0, 40);

DROP TABLE IF EXISTS rol_menu;
CREATE TABLE rol_menu(
	Id int(11) primary key AUTO_INCREMENT,	
	RolId int(11) NOT NULL ,
	FOREIGN KEY(RolId) REFERENCES Rol(RolId) on DELETE no action on UPDATE CASCADE,
    MenuId int(11) NOT NULL ,
	FOREIGN KEY(MenuId) REFERENCES Menu(MenuId) on DELETE no action on UPDATE CASCADE
);
INSERT INTO rol_menu(MenuId,RolId) VALUES (1, 1);
INSERT INTO rol_menu(MenuId,RolId) VALUES (2, 1);
INSERT INTO rol_menu(MenuId,RolId) VALUES (3, 1);

INSERT INTO rol_menu(MenuId,RolId) VALUES (10, 1);
INSERT INTO rol_menu(MenuId,RolId) VALUES (11, 1);
INSERT INTO rol_menu(MenuId,RolId) VALUES (12, 1);

INSERT INTO rol_menu(MenuId,RolId) VALUES (20, 1);
INSERT INTO rol_menu(MenuId,RolId) VALUES (21, 1);
INSERT INTO rol_menu(MenuId,RolId) VALUES (22, 1);
INSERT INTO rol_menu(MenuId,RolId) VALUES (23, 1);
INSERT INTO rol_menu(MenuId,RolId) VALUES (24, 1);

INSERT INTO rol_menu(MenuId,RolId) VALUES (30, 1);
INSERT INTO rol_menu(MenuId,RolId) VALUES (31, 1);
INSERT INTO rol_menu(MenuId,RolId) VALUES (32, 1);

INSERT INTO rol_menu(MenuId,RolId) VALUES (40, 1);
INSERT INTO rol_menu(MenuId,RolId) VALUES (41, 1);


DROP TABLE IF EXISTS ConceptoPago;
Create table ConceptoPago(
	ConceptoPagoId int(11) PRIMARY KEY auto_increment not NULL,
	Denominacion VARCHAR(255) not NULL,
	Importe decimal(15,2) not NULL default 0,
	OficinaId int(11) ,
		FOREIGN KEY(OficinaId) REFERENCES Oficina(OficinaId) on DELETE no action on UPDATE CASCADE,
	Estado bit(1) not null
);
/*CAJA*/
DROP TABLE IF EXISTS Caja;
create table Caja(
	CajaId int(11) PRIMARY KEY AUTO_INCREMENT NOT NULL,
	Denominacion VARCHAR(100) NOT NULL,
	IndAbierto bit(1) NOT NULL,
	IndBoveda bit(1) NOT NULL,
	Estado bit(1) NOT NULL	
);

DROP TABLE IF EXISTS CajaDiario;
create table CajaDiario(
	CajaDiarioId int(11) PRIMARY KEY AUTO_INCREMENT NOT NULL,
	CajaId int(11) not null,
		FOREIGN KEY(CajaId) REFERENCES Caja(CajaId) on DELETE no action on UPDATE CASCADE,
	PersonaId int(11) ,
		FOREIGN KEY(PersonaId) REFERENCES Persona(PersonaId) on DELETE no action on UPDATE CASCADE,
	SaldoInicial DECIMAL(15,2) not null DEFAULT 0,
	Entradas DECIMAL(15,2) not null DEFAULT 0,
	Salidas DECIMAL(15,2) not null DEFAULT 0,
	SaldoFinal DECIMAL(15,2) not null DEFAULT 0,
	FechaInicio datetime not null,
	FechaFin datetime,
	IndAbierto bit(1) not null
);

DROP TABLE IF EXISTS CajaMov;
create table CajaMov(
	CajaMovId int(11) PRIMARY KEY AUTO_INCREMENT NOT NULL,
	CajaDiarioId int(11) ,
		FOREIGN KEY(CajaDiarioId) REFERENCES CajaDiario(CajaDiarioId) on DELETE no action on UPDATE CASCADE,
	PersonaId int(11) not null,
		FOREIGN KEY(PersonaId) REFERENCES Persona(PersonaId) on DELETE no action on UPDATE CASCADE,
	Operacion char(3) not null, -- TRA tranferencia, INI Saldo Inicial
	Monto decimal(15,2) not null,
	Glosa VARCHAR(100) not null,
	IndEntrada bit(1) not null,
	Estado char(1) not null, -- P Pendiente, C Cobrado, T terminado, X anulado
	UsuarioRegId int(11) not null,
		FOREIGN KEY(UsuarioRegId) REFERENCES Usuario(UsuarioId) on DELETE no action on UPDATE CASCADE,
	FechaReg DateTime not null,
	FechaCobro DateTime,
	FechaAnulacion DateTime,
	UsuarioDespachoId int(11) ,
		FOREIGN KEY(UsuarioDespachoId) REFERENCES Usuario(UsuarioId) on DELETE no action on UPDATE CASCADE,
	FechaDespacho DateTime	
);
DROP TABLE IF EXISTS CajaMovDetalle;
create table CajaMovDetalle(
	CajaMovDetalleId int(11) PRIMARY KEY AUTO_INCREMENT NOT NULL,
	CajaMovId int(11) not null,
		FOREIGN KEY(CajaMovId) REFERENCES CajaMov(CajaMovId) on DELETE no action on UPDATE CASCADE,
	Cantidad int(11) not null,
	ConceptoPagoId int(11) not null,
		FOREIGN KEY(ConceptoPagoId) REFERENCES ConceptoPago(ConceptoPagoId) on DELETE no action on UPDATE CASCADE,
	PU decimal(15,2) not null,
	Monto decimal(15,2) not null
);
/*
 DROP TABLE IF EXISTS CajaTransferencia;
create table CajaTransferencia(
	Id int(11) PRIMARY KEY AUTO_INCREMENT NOT NULL,
	OrigenCajaDiarioId int(11) not null,
		FOREIGN KEY(OrigenCajaDiarioId) REFERENCES CajaDiario(CajaDiarioId) on DELETE no action on UPDATE CASCADE,
	DestinoCajaDiarioId int(11) not null,
		FOREIGN KEY(DestinoCajaDiarioId) REFERENCES CajaDiario(CajaDiarioId) on DELETE no action on UPDATE CASCADE,
	Monto decimal(15,2) not null,
	Fecha DateTime not null,
    Estado CHAR(1) not NULL,
    IndSaldoInicial bit(1) not NULL
);*/

-- carga inicial
insert into caja(Denominacion,IndAbierto,IndBoveda,Estado)
VALUES('BOVEDA',1,1,1);
insert into caja(Denominacion,IndAbierto,IndBoveda,Estado)
VALUES('CAJA 1',0,0,1);
insert into caja(Denominacion,IndAbierto,IndBoveda,Estado)
VALUES('CAJA 2',0,0,1);
insert into CajaDiario(CajaId,PersonaId,FechaInicio,IndAbierto)
values(1,null,now(),1);

insert into conceptopago(Denominacion,Importe,OficinaId,Estado)
VALUES('COPIA PARTIDA NACIMIENTO',10,1,1);
insert into conceptopago(Denominacion,Importe,OficinaId,Estado)
VALUES('COPIA PARTIDA MATRIMONIO',5,1,1);
insert into conceptopago(Denominacion,Importe,OficinaId,Estado)
VALUES('COPIA PARTIDA DEFUNCION',4,1,1);

