using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
   public class ReporteBL
    {
        public static List<ReporteNac> Nacimientos(int pLibroIni, int pLibroFin) {
            using (var db = new nacEntities()) {
                return db.nacimiento
                    .Where(x => x.NroLibro >= pLibroIni && x.NroLibro <= pLibroFin)
                    .Select(x => new ReporteNac { ApellidoNombre = x.ApellidoNombre, Fecha = x.Fecha, Sexo = x.Sexo, NroLibro = x.NroLibro, NroActa = x.NroActa })
                    .OrderBy(x => x.NroLibro).ThenBy(x => x.NroActa).ToList();
            }
        }
        public static List<ReporteNac> Nacimientos(DateTime pFechaIni, DateTime pFechaFin)
        {
            using (var db = new nacEntities())
            {
                return db.nacimiento
                    .Where(x => x.Fecha >= pFechaIni && x.Fecha <= pFechaFin)
                    .Select(x => new ReporteNac { ApellidoNombre = x.ApellidoNombre, Fecha = x.Fecha, Sexo = x.Sexo, NroLibro = x.NroLibro, NroActa = x.NroActa })
                    .OrderBy(x => x.NroLibro).ThenBy(x => x.NroActa).ToList();
            }
        }
        public static List<ReporteNac> NacimientosxRegistro(DateTime pFechaIni, DateTime pFechaFin)
        {
            using (var db = new nacEntities())
            {
                return db.nacimiento
                    .Where(x => x.Registro >= pFechaIni && x.Registro <= pFechaFin)
                    .Select(x => new ReporteNac { ApellidoNombre = x.ApellidoNombre, Fecha = x.Fecha, Sexo = x.Sexo, NroLibro = x.NroLibro, NroActa = x.NroActa })
                    .OrderBy(x => x.NroLibro).ThenBy(x => x.NroActa).ToList();
            }
        }

        public static List<ReporteDef> Defunciones(int pLibroIni, int pLibroFin)
        {
            using (var db = new nacEntities())
            {
                return db.defuncion
                    .Where(x => x.NroLibro >= pLibroIni && x.NroLibro <= pLibroFin)
                    .Select(x => new ReporteDef { ApellidoNombre = x.ApellidoNombre, Fecha = x.Fecha, Sexo = x.Sexo, NroLibro = x.NroLibro, NroActa = x.NroActa })
                    .OrderBy(x => x.NroLibro).ThenBy(x => x.NroActa).ToList();
            }
        }
        public static List<ReporteDef> Defunciones(DateTime pDefIni, DateTime pDefFin)
        {
            using (var db = new nacEntities())
            {
                return db.defuncion
                    .Where(x => x.Fecha >= pDefIni && x.Fecha <= pDefFin)
                    .Select(x => new ReporteDef { ApellidoNombre = x.ApellidoNombre, Fecha = x.Fecha, Sexo = x.Sexo, NroLibro = x.NroLibro, NroActa = x.NroActa })
                    .OrderBy(x => x.NroLibro).ThenBy(x => x.NroActa).ToList();
            }
        }
        public static List<ReporteDef> DefuncionesxRegistro(DateTime pDefIni, DateTime pDefFin)
        {
            using (var db = new nacEntities())
            {
                return db.defuncion
                    .Where(x => x.Registro >= pDefIni && x.Registro <= pDefFin)
                    .Select(x => new ReporteDef { ApellidoNombre = x.ApellidoNombre, Fecha = x.Fecha, Sexo = x.Sexo, NroLibro = x.NroLibro, NroActa = x.NroActa })
                    .OrderBy(x => x.NroLibro).ThenBy(x => x.NroActa).ToList();
            }
        }

        public static List<ReporteMat> Matrimonios(int pLibroIni, int pLibroFin)
        {
            using (var db = new nacEntities())
            {
                return db.matrimonio
                    .Where(x => x.NroLibro >= pLibroIni && x.NroLibro <= pLibroFin)
                    .Select(x => new ReporteMat { ApellidoNombre = x.ApellidoNombre, Conyugue = x.Conyugue, Fecha = x.Fecha, NroLibro = x.NroLibro, NroActa = x.NroActa })
                    .OrderBy(x => x.NroLibro).ThenBy(x => x.NroActa).ToList();
            }
        }
        public static List<ReporteMat> Matrimonios(DateTime pFechaIni, DateTime pFechaFin)
        {
            using (var db = new nacEntities())
            {
                return db.matrimonio
                    .Where(x => x.Fecha >= pFechaIni && x.Fecha <= pFechaFin)
                    .Select(x => new ReporteMat { ApellidoNombre = x.ApellidoNombre, Conyugue = x.Conyugue, Fecha = x.Fecha, NroLibro = x.NroLibro, NroActa = x.NroActa })
                    .OrderBy(x => x.NroLibro).ThenBy(x => x.NroActa).ToList();
            }
        }
        public static List<ReporteMat> MatrimoniosxRegistro(DateTime pFechaIni, DateTime pFechaFin)
        {
            using (var db = new nacEntities())
            {
                return db.matrimonio
                    .Where(x => x.Registro >= pFechaIni && x.Registro <= pFechaFin)
                    .Select(x => new ReporteMat { ApellidoNombre = x.ApellidoNombre, Conyugue = x.Conyugue, Fecha = x.Fecha, NroLibro = x.NroLibro, NroActa = x.NroActa })
                    .OrderBy(x => x.NroLibro).ThenBy(x => x.NroActa).ToList();
            }
        }
        public static List<ItemCombo> LibrosNacimiento()
        {
            using (var db = new nacEntities())
            {
                return db.nacimiento.Select(x => x.NroLibro).Distinct().AsEnumerable().Select(x => new ItemCombo { id = x, value = x.ToString() }).ToList();
            }
        }
        public static List<ItemCombo> LibrosDefuncion()
        {
            using (var db = new nacEntities())
            {
                return db.defuncion.Select(x => x.NroLibro).Distinct().AsEnumerable().Select(x => new ItemCombo { id = x, value = x.ToString() }).ToList();
            }
        }
        public static List<ItemCombo> LibrosMatrimonio()
        {
            using (var db = new nacEntities())
            {
                return db.matrimonio.Select(x => x.NroLibro).Distinct().AsEnumerable().Select(x => new ItemCombo { id = x, value = x.ToString() }).ToList();
            }
        }
    }
}
