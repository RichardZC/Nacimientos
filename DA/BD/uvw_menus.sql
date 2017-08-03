ALTER VIEW uvw_menus AS 
(
	SELECT
		ur.UsuarioId AS `UsuarioId`,
		ur.RolId AS `RolId`,
		m.MenuId AS `MenuId`,
		m.Denominacion AS `Denominacion`,
		m.Modulo AS `Modulo`,
		m.Icono AS `Icono`,
		m.IndPadre AS `IndPadre`,
		m.Orden AS `Orden`,
		m.Referencia AS `Referencia`
	FROM
		(
			(
				`usuario_rol` `ur`
				JOIN `menu_rol` `mr` ON (
					(`mr`.`RolId` = `ur`.`RolId`)
				)
			)
			JOIN `menu` `m` ON (
				(`m`.`MenuId` = `mr`.`MenuId`)
			)
		)
	GROUP BY
		`m`.`MenuId`,
		`ur`.`UsuarioId`
	ORDER BY
		m.MenuId
) ;