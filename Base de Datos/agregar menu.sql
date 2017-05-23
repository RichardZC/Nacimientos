DROP TABLE IF EXISTS `nac`.`menu` ;

CREATE TABLE IF NOT EXISTS `nac`.`menu` (
  `MenuId` INT NOT NULL,
  `Denominacion` VARCHAR(50) NULL,
  `Modulo` VARCHAR(50) NULL,
  `Url` VARCHAR(200) NULL,
  `Icono` VARCHAR(200) NULL,
  `IndPadre` BIT(1) NULL,
  `Orden` INT NULL,
  `Referencia` INT NULL,
  PRIMARY KEY (`MenuId`))
ENGINE = InnoDB;


DROP TABLE IF EXISTS `nac`.`rol_menu` ;

CREATE TABLE IF NOT EXISTS `nac`.`rol_menu` (
  `RolMenuId` INT NOT NULL,
  `MenuId` INT NOT NULL,
  `RolId` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`RolMenuId`),
  CONSTRAINT `fk_rolMenu_menu`
    FOREIGN KEY (`MenuId`)
    REFERENCES `nac`.`Menu` (`MenuId`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_rolMenu_menu_idx` ON `nac`.`rol_menu` (`MenuId` ASC);