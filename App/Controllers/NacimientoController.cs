using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using BE;
using App.Models;
using System.IO;

namespace App.Controllers
{
    public class NacimientoController : Controller
    {
        private const string RUTA_BASE = "~/Actas/Nacimiento/";
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Buscar(string clave = "")
        {
            return PartialView(NacimientoBL.ListarNacimientos(clave.Trim()));
        }
        public ActionResult Mantener(int id = 0)
        {
            var nac = new nacimiento();
            if (id == 0)
            {
                var max = NacimientoBL.ObtenerUltimoLibro();
                nac.NroLibro = max[0];
                nac.NroActa = max[1] + 1;
                nac.Fecha = DateTime.Now;
                nac.Sexo = "M";
            }
            else
                nac = NacimientoBL.Obtener(id);

            return View(nac);
        }

        [HttpPost]
        public JsonResult Guardar(nacimiento nac)
        {
            var res = new ResponseModel { respuesta = true };
            nac.ApellidoNombre = nac.ApellidoNombre.ToUpper();

            if (nac.NacimientoId == 0)
            {
                nac.Url = string.Empty;
                NacimientoBL.Crear(nac);
            }
            else
            {
                if (NacimientoBL.Contar(x => x.NroLibro == nac.NroLibro && x.NroActa == nac.NroActa && x.NacimientoId != nac.NacimientoId) > 0)
                {
                    res.respuesta = false;
                    res.error = "ERROR: Ya existe el acta " + nac.NroActa + " del libro " + nac.NroLibro + ". INGRESE OTRA NUMERACIÓN!";
                    return Json(res);
                }

                var ant = NacimientoBL.Obtener(x => x.NacimientoId == nac.NacimientoId, includeProperties: "nacimiento_anexo");
                if (ant.NroLibro != nac.NroLibro || ant.NroActa != nac.NroActa)
                {
                    if (ant.Url.Length > 0)
                    {
                        string rutaAnt = Path.Combine(Server.MapPath(RUTA_BASE), ant.Url);
                        string libro = "L" + nac.NroLibro.ToString();
                        string acta = nac.NroActa.ToString() + ".pdf";
                        string rutaLibro = Path.Combine(Server.MapPath(RUTA_BASE), libro);
                        nac.Url = libro + "/" + acta;

                        if (!Directory.Exists(rutaLibro))
                            Directory.CreateDirectory(rutaLibro);

                        string adjunto = Path.Combine(rutaLibro, acta);
                        if (System.IO.File.Exists(adjunto))
                            System.IO.File.Delete(adjunto);

                        if (System.IO.File.Exists(rutaAnt))
                            System.IO.File.Move(rutaAnt, adjunto);
                    }

                    foreach (var item in ant.nacimiento_anexo)
                    {
                        string acta_nueva = "L" + nac.NroLibro + "/" + nac.NroActa + "_" + item.Nacimiento_AnexoId + Path.GetExtension(item.url);
                        string ruta_acta_anterior = Server.MapPath(RUTA_BASE + item.url);

                        if (System.IO.File.Exists(ruta_acta_anterior))
                            System.IO.File.Move(ruta_acta_anterior, Server.MapPath(RUTA_BASE + acta_nueva));

                        item.url = acta_nueva;
                        NacimientoAnexoBL.Actualizar(item);
                        res.flag = true;
                    }

                }

                if (nac.Url == null) nac.Url = string.Empty;
                NacimientoBL.Actualizar(nac);
            }

            res.valor = nac.NacimientoId.ToString();
            return Json(res);
        }

        [HttpPost]
        public JsonResult Adjuntar(int NacimientoId, HttpPostedFileBase documento)
        {
            var rpt = new ResponseModel
            {
                respuesta = true,
                error = ""
            };

            if (documento != null)
            {
                if (Path.GetExtension(documento.FileName).ToLower() != ".pdf")
                {
                    rpt.respuesta = false;
                    rpt.error = "Debe Ingresar solo Archivos PDF";
                    return Json(rpt);
                }

                var nac = NacimientoBL.Obtener(NacimientoId);
                var libro = "L" + nac.NroLibro.ToString();
                var acta = nac.NroActa + Path.GetExtension(documento.FileName);

                string rutaLibro = Path.Combine(Server.MapPath(RUTA_BASE), libro);
                if (!Directory.Exists(rutaLibro))
                    Directory.CreateDirectory(rutaLibro);

                string adjunto = Path.Combine(rutaLibro, acta);
                if (System.IO.File.Exists(adjunto))
                    System.IO.File.Delete(adjunto);

                documento.SaveAs(adjunto);

                nac.Url = libro + "/" + acta;
                NacimientoBL.Actualizar(nac);

                rpt.valor = RUTA_BASE.Substring(2, RUTA_BASE.Length - 2) + nac.Url;
            }
            else
            {
                rpt.respuesta = false;
                rpt.error = "Debe adjuntar un documento";
            }

            return Json(rpt);
        }

        public JsonResult Eliminar(int pNacimientoId)
        {
            var nac = NacimientoBL.Obtener(x => x.NacimientoId == pNacimientoId, includeProperties: "nacimiento_anexo");
            string ruta = Path.Combine(Server.MapPath(RUTA_BASE), "L" + nac.NroLibro.ToString(), nac.NroActa.ToString() + ".pdf");
            if (System.IO.File.Exists(ruta))
                System.IO.File.Delete(ruta);

            foreach (var item in nac.nacimiento_anexo)
            {
                if (System.IO.File.Exists(Server.MapPath(RUTA_BASE + item.url)))
                    System.IO.File.Delete(Server.MapPath(RUTA_BASE + item.url));
            }

            NacimientoBL.Eliminar(pNacimientoId);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult MostrarAnexos(int pNacimientoId, bool editar)
        {
            ViewBag.editar = editar;
            return PartialView(NacimientoAnexoBL.Listar(x => x.NacimientoId == pNacimientoId));
        }
        [HttpPost]
        public JsonResult Anexo(int NacimientoId, HttpPostedFileBase documento)
        {
            var rpt = new ResponseModel
            {
                respuesta = true,
                error = ""
            };

            if (documento != null)
            {
                if (Path.GetExtension(documento.FileName).ToLower() != ".pdf")
                {
                    rpt.respuesta = false;
                    rpt.error = "Debe Ingresar solo Archivos PDF";
                    return Json(rpt);
                }

                var nac = NacimientoBL.Obtener(NacimientoId);
                var anexo = NacimientoAnexoBL.Crear(new nacimiento_anexo { NacimientoId = NacimientoId, url = "" });
                string libro = "L" + nac.NroLibro;
                string acta = nac.NroActa + "_" + anexo.Nacimiento_AnexoId + Path.GetExtension(documento.FileName);


                string rutaLibro = Path.Combine(Server.MapPath(RUTA_BASE), libro);
                if (!Directory.Exists(rutaLibro))
                    Directory.CreateDirectory(rutaLibro);

                string adjunto = Path.Combine(rutaLibro, acta);
                if (System.IO.File.Exists(adjunto))
                    System.IO.File.Delete(adjunto);

                documento.SaveAs(adjunto);

                anexo.url = libro + "/" + acta;
                NacimientoAnexoBL.Actualizar(anexo);
            }
            else
            {
                rpt.respuesta = false;
                rpt.error = "Debe adjuntar un documento";
            }

            return Json(rpt);
        }

        public JsonResult EliminarAnexo(int id)
        {
            var anexo = NacimientoAnexoBL.Obtener(id);
            if (System.IO.File.Exists(Server.MapPath(RUTA_BASE + anexo.url)))
                System.IO.File.Delete(Server.MapPath(RUTA_BASE + anexo.url));

            var img = NacimientoAnexoBL.Eliminar(id);

            return Json(true);
        }


    }
}