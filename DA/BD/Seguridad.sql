SET FOREIGN_KEY_CHECKS=0;

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
