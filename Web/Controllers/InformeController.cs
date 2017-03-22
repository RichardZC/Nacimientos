using BL;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Web.Filters;

namespace Web.Controllers
{
    [Autenticado]
    public class InformeController : Controller
    {      
        public ActionResult Index()
        {
            var libros = ReporteBL.LibrosNacimiento();
            var lista = new SelectList(libros, "id", "value", libros.Max(x=>x.id));
            ViewBag.LibrosNacIni = lista;
            ViewBag.LibrosNacFin = lista;

            libros = ReporteBL.LibrosDefuncion();
            lista = new SelectList(libros, "id", "value", libros.Max(x => x.id));
            ViewBag.LibrosDefIni = lista;
            ViewBag.LibrosDefFin = lista;

            libros = ReporteBL.LibrosMatrimonio();
            lista = new SelectList(libros, "id", "value", libros.Max(x => x.id));
            ViewBag.LibrosMatIni = lista;
            ViewBag.LibrosMatFin = lista;

            return View();
        }

        #region "ReportViewer"

        public ActionResult ReporteNacimiento(string pInicio, string pFin, int opcion, string pTipoReporte = "PDF")
        {
            List<ReporteNac> data = null;
            string filtro = string.Empty;
            switch (opcion)
            {
                case 1:
                    data = ReporteBL.Nacimientos(int.Parse(pInicio), int.Parse(pFin));
                    filtro = "Del Libro " + pInicio + " al Libro " + pFin; break;
                case 2:
                    data = ReporteBL.Nacimientos(DateTime.Parse(pInicio), DateTime.Parse(pFin));
                    filtro = "De la Fecha Nacimiento " + pInicio + " al " + pFin; break;
                case 3:
                    data = ReporteBL.NacimientosxRegistro(DateTime.Parse(pInicio), DateTime.Parse(pFin));
                    filtro = "De la Fecha Registro " + pInicio + " al " + pFin; break;
            }

            var rd = new ReportDataSource("dsNacimiento", data);

            var parametros = new List<ReportParameter>
                                 {
                                     new ReportParameter("Filtro", filtro)
                                 };
            
            return Reporte(pTipoReporte, "rptNacimiento.rdlc", rd, "A4Vertical0.25", parametros);
        }

        public ActionResult ReporteDefuncion(string pInicio, string pFin,int opcion, string pTipoReporte = "PDF")
        {
            List<ReporteDef> data = null;
            string filtro = string.Empty;
            switch (opcion)
            {
                case 1: data = ReporteBL.Defunciones(int.Parse(pInicio), int.Parse(pFin));
                    filtro = "Del Libro " + pInicio + " al Libro " + pFin;  break;
                case 2: data = ReporteBL.Defunciones(DateTime.Parse(pInicio), DateTime.Parse(pFin));
                    filtro = "De la Fecha Defuncion " + pInicio + " al " + pFin; break;
                case 3: data = ReporteBL.DefuncionesxRegistro(DateTime.Parse(pInicio), DateTime.Parse(pFin));
                    filtro = "De la Fecha Registro " + pInicio + " al " + pFin; break;
            }

            var rd = new ReportDataSource("dsNacimiento", data);

            var parametros = new List<ReportParameter>
                                 {
                                     new ReportParameter("Filtro", filtro)                                     
                                 };

            return Reporte(pTipoReporte, "rptDefuncion.rdlc", rd, "A4Vertical0.25", parametros);
        }

        public ActionResult ReporteMatrimonio(string pInicio, string pFin, int opcion, string pTipoReporte = "PDF")
        {
            List<ReporteMat> data = null;
            string filtro = string.Empty;
            switch (opcion)
            {
                case 1:
                    data = ReporteBL.Matrimonios(int.Parse(pInicio), int.Parse(pFin));
                    filtro = "Del Libro " + pInicio + " al Libro " + pFin; break;
                case 2:
                    data = ReporteBL.Matrimonios(DateTime.Parse(pInicio), DateTime.Parse(pFin));
                    filtro = "De la Fecha Matrimonio " + pInicio + " al " + pFin; break;
                case 3:
                    data = ReporteBL.MatrimoniosxRegistro(DateTime.Parse(pInicio), DateTime.Parse(pFin));
                    filtro = "De la Fecha Registro " + pInicio + " al " + pFin; break;
            }
        
            var rd = new ReportDataSource("dsMatrimonio", data);

            var parametros = new List<ReportParameter>
                                 {
                                     new ReportParameter("Filtro", filtro)
                                 };

            return Reporte(pTipoReporte, "rptMatrimonio.rdlc", rd, "A4Vertical0.25", parametros);
        }
        #endregion

        #region "Reporteador"
        public ActionResult Reporte(string pTipoReporte, string rdlc, ReportDataSource rds, string pPapel, List<ReportParameter> pParametros = null)
        {
            var lr = new LocalReport();
            lr.ReportPath = Path.Combine(Server.MapPath("~/Reporte"), rdlc);

            if (rds != null) lr.DataSources.Add(rds);
            if (pParametros != null) lr.SetParameters(pParametros);

            string reportType = pTipoReporte;
            string mimeType;
            string encoding;
            string fileNameExtension;

            var deviceInfo = ObtenerPapel(pPapel).Replace("[TipoReporte]", pTipoReporte);
            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = lr.Render(reportType, deviceInfo, out mimeType, out encoding,
                                             out fileNameExtension, out streams, out warnings);

            return File(renderedBytes, mimeType);
        }

        private static string ObtenerPapel(string pPapel)
        {
            switch (pPapel)
            {
                case "A4Horizontal":
                    return "<DeviceInfo>" +
                           "  <OutputFormat>[TipoReporte]</OutputFormat>" +
                           "  <PageWidth>11in</PageWidth>" +
                           "  <PageHeight>8.5in</PageHeight>" +
                           "  <MarginTop>0in</MarginTop>" +
                           "  <MarginLeft>0in</MarginLeft>" +
                           "  <MarginRight>0in</MarginRight>" +
                           "  <MarginBottom>0in</MarginBottom>" +
                           "</DeviceInfo>";
                case "A4Vertical":
                    return "<DeviceInfo>" +
                           "  <OutputFormat>[TipoReporte]</OutputFormat>" +
                           "  <PageWidth>8.5in</PageWidth>" +
                           "  <PageHeight>11in</PageHeight>" +
                           "  <MarginTop>0in</MarginTop>" +
                           "  <MarginLeft>0in</MarginLeft>" +
                           "  <MarginRight>0in</MarginRight>" +
                           "  <MarginBottom>0in</MarginBottom>" +
                           "</DeviceInfo>";
                case "A4Horizontal0.25":
                    return "<DeviceInfo>" +
                           "  <OutputFormat>[TipoReporte]</OutputFormat>" +
                           "  <PageWidth>11in</PageWidth>" +
                           "  <PageHeight>8.5in</PageHeight>" +
                           "  <MarginTop>0.25in</MarginTop>" +
                           "  <MarginLeft>0.25in</MarginLeft>" +
                           "  <MarginRight>0.25in</MarginRight>" +
                           "  <MarginBottom>0.25in</MarginBottom>" +
                           "</DeviceInfo>";
                case "A4Vertical0.25":
                    return "<DeviceInfo>" +
                           "  <OutputFormat>[TipoReporte]</OutputFormat>" +
                           "  <PageWidth>8.5in</PageWidth>" +
                           "  <PageHeight>11in</PageHeight>" +
                           "  <MarginTop>0.25in</MarginTop>" +
                           "  <MarginLeft>0.25in</MarginLeft>" +
                           "  <MarginRight>0.25in</MarginRight>" +
                           "  <MarginBottom>0.25in</MarginBottom>" +
                           "</DeviceInfo>";
                case "TicketCaja":
                    return "<DeviceInfo>" +
                           "  <OutputFormat>[TipoReporte]</OutputFormat>" +
                           "  <PageWidth>3.5in</PageWidth>" +
                           "  <PageHeight>5.0in</PageHeight>" +
                           "  <MarginTop>0in</MarginTop>" +
                           "  <MarginLeft>0.1in</MarginLeft>" +
                           "  <MarginRight>0in</MarginRight>" +
                           "  <MarginBottom>0in</MarginBottom>" +
                           "</DeviceInfo>";
                case "VoucherCaja":
                    return "<DeviceInfo>" +
                           "  <OutputFormat>[TipoReporte]</OutputFormat>" +
                           "  <PageWidth>9.0in</PageWidth>" +
                           "  <PageHeight>5.0in</PageHeight>" +
                           "  <MarginTop>0in</MarginTop>" +
                           "  <MarginLeft>0.1in</MarginLeft>" +
                           "  <MarginRight>0in</MarginRight>" +
                           "  <MarginBottom>0in</MarginBottom>" +
                           "</DeviceInfo>";
                case "CodigoBarras":
                    return "<DeviceInfo>" +
                           "  <OutputFormat>[TipoReporte]</OutputFormat>" +
                           "  <PageWidth>4.13in</PageWidth>" +
                           "  <PageHeight>2.76in</PageHeight>" +
                           "  <MarginTop>0in</MarginTop>" +
                           "  <MarginLeft>0in</MarginLeft>" +
                           "  <MarginRight>0in</MarginRight>" +
                           "  <MarginBottom>0in</MarginBottom>" +
                           "</DeviceInfo>";

            }

            return "<DeviceInfo>" +
                   "  <OutputFormat>[TipoReporte]</OutputFormat>" +
                   "  <PageWidth>8.5in</PageWidth>" +
                   "  <PageHeight>11in</PageHeight>" +
                   "  <MarginTop>0in</MarginTop>" +
                   "  <MarginLeft>0in</MarginLeft>" +
                   "  <MarginRight>0in</MarginRight>" +
                   "  <MarginBottom>0in</MarginBottom>" +
                   "</DeviceInfo>";
        }
        #endregion
    }
}