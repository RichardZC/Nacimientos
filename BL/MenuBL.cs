using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Modelo;
using System.Data.Entity;

namespace BL
{
    public class MenuBL : Repositorio<menu>
    {
        public static List<Menu> ListarMenu()
        {
            using (var bd = new nacEntities())
            {
                var menu = bd.menu.Select(x => new Menu()
                {
                    MenuId = x.MenuId,
                    Denominacion = x.Denominacion,
                    Estado = false
                }).ToList();

                return menu;
            }
        }

        
    }
}