using App.Models;
using BE;
using BL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class MatrimonioController : Controller
    {
        private const string RUTA_BASE = "~/Actas/Matrimonio/";
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Buscar(string clave = "")
        {
            return PartialView(MatrimonioBL.Listar(clave.Trim()));
        }
        public ActionResult Mantener(int id = 0)
        {
            var def = new matrimonio();
            if (id == 0)
            {
                var max = MatrimonioBL.ObtenerUltimoLibro();
                def.NroLibro = max[0];
                def.NroActa = max[1] + 1;
                def.Fecha = DateTime.Now;
            }
            else
                def = MatrimonioBL.Obtener(id);

            return View(def);
        }

        [HttpPost]
        public JsonResult Guardar(matrimonio def)
        {
            var res = new ResponseModel { respuesta = true };
            def.ApellidoNombre = def.ApellidoNombre.ToUpper();
            def.Conyugue = def.Conyugue.ToUpper();

            if (def.MatrimonioId == 0)
            {
                def.Url = string.Empty;
                MatrimonioBL.Crear(def);
            }
            else
            {
                if (MatrimonioBL.Contar(x => x.NroLibro == def.NroLibro && x.NroActa == def.NroActa && x.MatrimonioId != def.MatrimonioId) > 0)
                {
                    res.respuesta = false;
                    res.error = "ERROR: Ya existe el acta " + def.NroActa + " del libro " + def.NroLibro + ". INGRESE OTRA NUMERACIÓN!";
                    return Json(res);
                }

                var ant = MatrimonioBL.Obtener(x => x.MatrimonioId == def.MatrimonioId, includeProperties: "matrimonio_anexo");
                if (ant.NroLibro != def.NroLibro || ant.NroActa != def.NroActa)
                {
                    if (ant.Url.Length > 0)
                    {
                        string rutaAnt = Path.Combine(Server.MapPath(RUTA_BASE), ant.Url);
                        string libro = "L" + def.NroLibro.ToString();
                        string acta = def.NroActa.ToString() + ".pdf";
                        string rutaLibro = Path.Combine(Server.MapPath(RUTA_BASE), libro);
                        def.Url = libro + "/" + acta;

                        if (!Directory.Exists(rutaLibro))
                            Directory.CreateDirectory(rutaLibro);

                        string adjunto = Path.Combine(rutaLibro, acta);
                        if (System.IO.File.Exists(adjunto))
                            System.IO.File.Delete(adjunto);

                        if (System.IO.File.Exists(rutaAnt))
                            System.IO.File.Move(rutaAnt, adjunto);
                    }
                    foreach (var item in ant.matrimonio_anexo)
                    {
                        string acta_nueva = "L" + def.NroLibro + "/" + def.NroActa + "_" + item.Matrimonio_AnexoId + Path.GetExtension(item.url);
                        string ruta_acta_anterior = Server.MapPath(RUTA_BASE + item.url);

                        if (System.IO.File.Exists(ruta_acta_anterior))
                            System.IO.File.Move(ruta_acta_anterior, Server.MapPath(RUTA_BASE + acta_nueva));

                        item.url = acta_nueva;
                        MatrimonioAnexoBL.Actualizar(item);
                        res.flag = true;
                    }

                }
                if (def.Url == null) def.Url = string.Empty;
                MatrimonioBL.Actualizar(def);
            }

            res.valor = def.MatrimonioId.ToString();
            return Json(res);
        }

        [HttpPost]
        public JsonResult Adjuntar(int MatrimonioId, HttpPostedFileBase documento)
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

                var def = MatrimonioBL.Obtener(MatrimonioId);
                var libro = "L" + def.NroLibro.ToString();
                var acta = def.NroActa + Path.GetExtension(documento.FileName);

                string rutaLibro = Path.Combine(Server.MapPath(RUTA_BASE), libro);
                if (!Directory.Exists(rutaLibro))
                    Directory.CreateDirectory(rutaLibro);

                string adjunto = Path.Combine(rutaLibro, acta);
                if (System.IO.File.Exists(adjunto))
                    System.IO.File.Delete(adjunto);

                documento.SaveAs(adjunto);

                def.Url = libro + "/" + acta;
                MatrimonioBL.Actualizar(def);

                rpt.valor = RUTA_BASE.Substring(2, RUTA_BASE.Length - 2) + def.Url;
            }
            else
            {
                rpt.respuesta = false;
                rpt.error = "Debe adjuntar un documento";
            }

            return Json(rpt);
        }

        public JsonResult Eliminar(int pMatrimonioId)
        {
            var nac = MatrimonioBL.Obtener(x => x.MatrimonioId == pMatrimonioId, includeProperties: "matrimonio_anexo");
            string ruta = Path.Combine(Server.MapPath(RUTA_BASE), "L" + nac.NroLibro.ToString(), nac.NroActa.ToString() + ".pdf");
            if (System.IO.File.Exists(ruta))
                System.IO.File.Delete(ruta);

            foreach (var item in nac.matrimonio_anexo)
            {
                if (System.IO.File.Exists(Server.MapPath(RUTA_BASE + item.url)))
                    System.IO.File.Delete(Server.MapPath(RUTA_BASE + item.url));
            }

            MatrimonioBL.Eliminar(pMatrimonioId);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult MostrarAnexos(int pMatrimonioId, bool editar)
        {
            ViewBag.editar = editar;
            return PartialView(MatrimonioAnexoBL.Listar(x => x.MatrimonioId == pMatrimonioId));
        }
        [HttpPost]
        public JsonResult Anexo(int MatrimonioId, HttpPostedFileBase documento)
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

                var def = MatrimonioBL.Obtener(MatrimonioId);
                var anexo = MatrimonioAnexoBL.Crear(new matrimonio_anexo { MatrimonioId = MatrimonioId, url = "" });
                string libro = "L" + def.NroLibro;
                string acta = def.NroActa + "_" + anexo.Matrimonio_AnexoId + Path.GetExtension(documento.FileName);


                string rutaLibro = Path.Combine(Server.MapPath(RUTA_BASE), libro);
                if (!Directory.Exists(rutaLibro))
                    Directory.CreateDirectory(rutaLibro);

                string adjunto = Path.Combine(rutaLibro, acta);
                if (System.IO.File.Exists(adjunto))
                    System.IO.File.Delete(adjunto);

                documento.SaveAs(adjunto);

                anexo.url = libro + "/" + acta;
                MatrimonioAnexoBL.Actualizar(anexo);
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
            var anexo = MatrimonioAnexoBL.Obtener(id);
            if (System.IO.File.Exists(Server.MapPath(RUTA_BASE + anexo.url)))
                System.IO.File.Delete(Server.MapPath(RUTA_BASE + anexo.url));

            var img = MatrimonioAnexoBL.Eliminar(id);

            return Json(true);
        }


    }
}