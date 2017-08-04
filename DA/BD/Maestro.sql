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

DROP TABLE IF EXISTS usuario_oficina;
CREATE TABLE usuario_oficina (  
  OficinaId int(11) NOT NULL,
		FOREIGN KEY(OficinaId) REFERENCES Oficina(OficinaId) on DELETE no action on UPDATE CASCADE,
  UsuarioId int(11) NOT NULL,
		FOREIGN KEY(UsuarioId) REFERENCES Usuario(UsuarioId) on DELETE no action on UPDATE CASCADE,
  PRIMARY KEY (`UsuarioId`,`OficinaId`)
); 
INSERT INTO `usuario_oficina` VALUES ('1', '1');

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
