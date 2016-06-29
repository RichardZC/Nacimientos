using BL;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
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

        public ActionResult ReporteNacimiento(int pNroLibroIni, int pNroLibroFin, string pTipoReporte = "PDF")
        {
            var data = ReporteBL.Nacimientos(pNroLibroIni, pNroLibroFin);
            var rd = new ReportDataSource("dsNacimiento", data);

            var parametros = new List<ReportParameter>
                                 {
                                     new ReportParameter("LibroIni", pNroLibroIni.ToString()),
                                     new ReportParameter("LibroFin", pNroLibroFin.ToString())
                                 };
            
            return Reporte(pTipoReporte, "rptNacimiento.rdlc", rd, "A4Vertical0.25", parametros);
        }

        public ActionResult ReporteDefuncion(int pNroLibroIni, int pNroLibroFin, string pTipoReporte = "PDF")
        {
            var data = ReporteBL.Defunciones(pNroLibroIni, pNroLibroFin);
            var rd = new ReportDataSource("dsNacimiento", data);

            var parametros = new List<ReportParameter>
                                 {
                                     new ReportParameter("LibroIni", pNroLibroIni.ToString()),
                                     new ReportParameter("LibroFin", pNroLibroFin.ToString())
                                 };
            
            return Reporte(pTipoReporte, "rptDefuncion.rdlc", rd, "A4Vertical0.25", parametros);
        }

        public ActionResult ReporteMatrimonio(int pNroLibroIni, int pNroLibroFin, string pTipoReporte = "PDF")
        {
            var data = ReporteBL.Matrimonios(pNroLibroIni, pNroLibroFin);
            var rd = new ReportDataSource("dsMatrimonio", data);

            var parametros = new List<ReportParameter>
                                 {
                                     new ReportParameter("LibroIni", pNroLibroIni.ToString()),
                                     new ReportParameter("LibroFin", pNroLibroFin.ToString())
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