using Models;
using BE;
using BL;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Web.Filters;

namespace Web.Controllers
{
    [Autenticado]
    public class DefuncionController : Controller
    {
        private const string RUTA_BASE = "~/Actas/Defuncion/";
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Buscar(string clave = "")
        {
            return PartialView(DefuncionBL.Listar(clave.Trim()));
        }
        public ActionResult Mantener(int id = 0)
        {
            var def = new defuncion();
            if (id == 0)
            {
                var max = DefuncionBL.ObtenerUltimoLibro();
                def.NroLibro = max[0];
                def.NroActa = max[1] + 1;
                def.Fecha = DateTime.Now;
                def.Sexo = "M";
            }
            else
                def = DefuncionBL.Obtener(id);

            return View(def);
        }

        [HttpPost]
        public JsonResult Guardar(defuncion def)
        {
            var res = new Respuesta { respuesta = true };
            def.ApellidoNombre = def.ApellidoNombre.ToUpper();
            if (def.DefuncionId == 0)
            {
                def.Url = string.Empty;
                DefuncionBL.Crear(def);
            }
            else
            {
                if (DefuncionBL.Contar(x => x.NroLibro == def.NroLibro && x.NroActa == def.NroActa && x.DefuncionId != def.DefuncionId) > 0)
                {
                    res.respuesta = false;
                    res.error = "ERROR: Ya existe el acta " + def.NroActa + " del libro " + def.NroLibro + ". INGRESE OTRA NUMERACIÓN!";
                    return Json(res);
                }

                var ant = DefuncionBL.Obtener(x => x.DefuncionId == def.DefuncionId, includeProperties: "defuncion_anexo");
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
                    foreach (var item in ant.defuncion_anexo)
                    {
                        string acta_nueva = "L" + def.NroLibro + "/" + def.NroActa + "_" + item.Defuncion_AnexoId + Path.GetExtension(item.url);
                        string ruta_acta_anterior = Server.MapPath(RUTA_BASE + item.url);

                        if (System.IO.File.Exists(ruta_acta_anterior))
                            System.IO.File.Move(ruta_acta_anterior, Server.MapPath(RUTA_BASE + acta_nueva));

                        item.url = acta_nueva;
                        DefuncionAnexoBL.Actualizar(item);
                        res.flag = true;
                    }

                }
                if (def.Url == null) def.Url = string.Empty;
                DefuncionBL.Actualizar(def);
            }

            res.valor = def.DefuncionId.ToString();
            return Json(res);
        }

        [HttpPost]
        public JsonResult Adjuntar(int DefuncionId, HttpPostedFileBase documento)
        {
            var rpt = new Respuesta
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

                var def = DefuncionBL.Obtener(DefuncionId);
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
                DefuncionBL.Actualizar(def);

                rpt.valor = RUTA_BASE.Substring(2, RUTA_BASE.Length - 2) + def.Url;
            }
            else
            {
                rpt.respuesta = false;
                rpt.error = "Debe adjuntar un documento";
            }

            return Json(rpt);
        }

        public JsonResult Eliminar(int pDefuncionId)
        {
            var nac = DefuncionBL.Obtener(x => x.DefuncionId == pDefuncionId, includeProperties: "defuncion_anexo");
            string ruta = Path.Combine(Server.MapPath(RUTA_BASE), "L" + nac.NroLibro.ToString(), nac.NroActa.ToString() + ".pdf");
            if (System.IO.File.Exists(ruta))
                System.IO.File.Delete(ruta);

            foreach (var item in nac.defuncion_anexo)
            {
                if (System.IO.File.Exists(Server.MapPath(RUTA_BASE + item.url)))
                    System.IO.File.Delete(Server.MapPath(RUTA_BASE + item.url));
            }

            DefuncionBL.Eliminar(pDefuncionId);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult MostrarAnexos(int pDefuncionId, bool editar)
        {
            ViewBag.editar = editar;
            return PartialView(DefuncionAnexoBL.Listar(x => x.DefuncionId == pDefuncionId));
        }
        [HttpPost]
        public JsonResult Anexo(int DefuncionId, HttpPostedFileBase documento)
        {
            var rpt = new Respuesta
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

                var def = DefuncionBL.Obtener(DefuncionId);
                var anexo = DefuncionAnexoBL.Crear(new defuncion_anexo { DefuncionId = DefuncionId, url = "" });
                string libro = "L" + def.NroLibro;
                string acta = def.NroActa + "_" + anexo.Defuncion_AnexoId + Path.GetExtension(documento.FileName);


                string rutaLibro = Path.Combine(Server.MapPath(RUTA_BASE), libro);
                if (!Directory.Exists(rutaLibro))
                    Directory.CreateDirectory(rutaLibro);

                string adjunto = Path.Combine(rutaLibro, acta);
                if (System.IO.File.Exists(adjunto))
                    System.IO.File.Delete(adjunto);

                documento.SaveAs(adjunto);

                anexo.url = libro + "/" + acta;
                DefuncionAnexoBL.Actualizar(anexo);
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
            var anexo = DefuncionAnexoBL.Obtener(id);
            if (System.IO.File.Exists(Server.MapPath(RUTA_BASE + anexo.url)))
                System.IO.File.Delete(Server.MapPath(RUTA_BASE + anexo.url));

            var img = DefuncionAnexoBL.Eliminar(id);

            return Json(true);
        }


    }
}