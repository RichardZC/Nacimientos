DROP TABLE IF EXISTS CajaTransferencia;
DROP TABLE IF EXISTS CajaMovDetalle;
DROP TABLE IF EXISTS ConceptoPago;
DROP TABLE IF EXISTS CajaMov;
DROP TABLE IF EXISTS CajaDiario;
DROP TABLE IF EXISTS Caja;


Create table ConceptoPago(
	ConceptoPagoId int(11) PRIMARY KEY auto_increment not NULL,
	Denominacion VARCHAR(255) not NULL,
	Importe decimal(15,2) not NULL default 0,
	OficinaId int(11) ,
		FOREIGN KEY(OficinaId) REFERENCES Oficina(OficinaId) on DELETE no action on UPDATE CASCADE,
	Estado bit(1) not null
);

create table Caja(
	CajaId int(11) PRIMARY KEY AUTO_INCREMENT NOT NULL,
	Denominacion VARCHAR(100) NOT NULL,
	IndAbierto bit(1) NOT NULL,
	IndBoveda bit(1) NOT NULL,
	Estado bit(1) NOT NULL,
	PersonaId int ,
		FOREIGN KEY(PersonaId) REFERENCES Persona(PersonaId) on DELETE no action on UPDATE CASCADE,
	FechaInicioOperacion datetime 
);

create table CajaDiario(
	CajaDiarioId int(11) PRIMARY KEY AUTO_INCREMENT NOT NULL,
	CajaId int(11) not null,
		FOREIGN KEY(CajaId) REFERENCES Caja(CajaId) on DELETE no action on UPDATE CASCADE,
	PersonaId int(11) not null,
		FOREIGN KEY(PersonaId) REFERENCES Persona(PersonaId) on DELETE no action on UPDATE CASCADE,
	SaldoInicial DECIMAL(15,2) not null DEFAULT 0,
	Entradas DECIMAL(15,2) not null DEFAULT 0,
	Salidas DECIMAL(15,2) not null DEFAULT 0,
	SaldoFinal DECIMAL(15,2) not null DEFAULT 0,
	FechaInicio datetime not null,
	FechaFin datetime,
	IndAbierto bit(1) not null
);

create table CajaMov(
	CajaMovId int(11) PRIMARY KEY AUTO_INCREMENT NOT NULL,
	CajaDiarioId int(11) ,
		FOREIGN KEY(CajaDiarioId) REFERENCES CajaDiario(CajaDiarioId) on DELETE no action on UPDATE CASCADE,
	PersonaId int(11) not null,
		FOREIGN KEY(PersonaId) REFERENCES Persona(PersonaId) on DELETE no action on UPDATE CASCADE,
	Operacion char(3) not null,
	Monto decimal(15,2) not null,
	Glosa VARCHAR(100) not null,
	IndEntrada bit(1) not null,
	Estado char(1) not null,
	UsuarioRegId int(11) not null,
		FOREIGN KEY(UsuarioRegId) REFERENCES Usuario(UsuarioId) on DELETE no action on UPDATE CASCADE,
	FechaReg DateTime not null,
	FechaCobro DateTime,
	FechaAnulacion DateTime,
	UsuarioDespachoId int(11) ,
		FOREIGN KEY(UsuarioDespachoId) REFERENCES Usuario(UsuarioId) on DELETE no action on UPDATE CASCADE,
	FechaDespacho DateTime	
);

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

create table CajaTransferencia(
	Id int(11) PRIMARY KEY AUTO_INCREMENT NOT NULL,
	OrigenCajaDiarioId int(11) not null,
		FOREIGN KEY(OrigenCajaDiarioId) REFERENCES CajaDiario(CajaDiarioId) on DELETE no action on UPDATE CASCADE,
	DestinoCajaDiarioId int(11) not null,
		FOREIGN KEY(DestinoCajaDiarioId) REFERENCES CajaDiario(CajaDiarioId) on DELETE no action on UPDATE CASCADE,
	Monto decimal(15,2) not null,
	Fecha DateTime not null
);


insert into caja(Denominacion,IndAbierto,IndBoveda,Estado)
VALUES('BOVEDA',1,1,1);
insert into caja(Denominacion,IndAbierto,IndBoveda,Estado)
VALUES('CAJA 1',0,0,1);
insert into caja(Denominacion,IndAbierto,IndBoveda,Estado)
VALUES('CAJA 2',0,0,1);

insert into conceptopago(Denominacion,Importe,OficinaId,Estado)
VALUES('COPIA PARTIDA NACIMIENTO',10,1,1);
insert into conceptopago(Denominacion,Importe,OficinaId,Estado)
VALUES('COPIA PARTIDA MATRIMONIO',5,1,1);
insert into conceptopago(Denominacion,Importe,OficinaId,Estado)
VALUES('COPIA PARTIDA DEFUNCION',4,1,1);

