using BE;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public class MatrimonioBL:Repositorio<matrimonio>
    {
        public static List<int> ObtenerUltimoLibro()
        {
            var l = new List<int>();
            using (var db = new nacEntities())
            {
                int maxlibro = db.matrimonio.Max(x => x.NroLibro);
                l.Add(maxlibro);
                l.Add(db.matrimonio.Where(x => x.NroLibro == maxlibro).Max(x => x.NroActa));
            }
            return l;
        }
        public static List<matrimonio> Listar(string clave = "")
        {
            var lista = clave.Split(char.Parse(" "));
            string s1 = string.Empty, s2 = string.Empty, s3 = string.Empty, s4 = string.Empty;
            switch (lista.Length)
            {
                case 1: s1 = lista[0]; break;
                case 2: s1 = lista[0]; s2 = lista[1]; break;
                case 3: s1 = lista[0]; s2 = lista[1]; s3 = lista[2]; break;
                case 4: s1 = lista[0]; s2 = lista[1]; s3 = lista[2]; s4 = lista[3]; break;
            }

            using (var db = new nacEntities())
            {
                if (string.IsNullOrEmpty(clave))
                    return db.matrimonio.Include("matrimonio_anexo").OrderByDescending(x => x.MatrimonioId).Take(15).ToList();

                int libro = 0;
                if (int.TryParse(clave, out libro))
                    return db.matrimonio.Include("matrimonio_anexo").Where(x => x.NroLibro == libro).OrderBy(x => x.Url).ToList();

                if (lista.Length == 2) {
                    int acta = 0;
                    if (int.TryParse(s1, out libro) && int.TryParse(s2, out acta))
                    {
                        return db.matrimonio.Include("matrimonio_anexo")
                        .Where(x => x.NroLibro == libro && x.NroActa == acta)
                        .OrderByDescending(x => x.MatrimonioId).Take(15).ToList();
                    }
                    return db.matrimonio.Include("matrimonio_anexo")
                        .Where(x => (x.ApellidoNombre.Contains(s1) && x.ApellidoNombre.Contains(s2)) || (x.Conyugue.Contains(s1) && x.Conyugue.Contains(s2)))
                        .OrderByDescending(x => x.MatrimonioId).Take(15).ToList();
                }
                if (lista.Length == 3)
                    return db.matrimonio.Include("matrimonio_anexo")
                        .Where(x => (x.ApellidoNombre.Contains(s1) && x.ApellidoNombre.Contains(s2) && x.ApellidoNombre.Contains(s3)) || (x.Conyugue.Contains(s1) && x.Conyugue.Contains(s2) && x.Conyugue.Contains(s3)))
                        .OrderByDescending(x => x.MatrimonioId).Take(15).ToList();
                if (lista.Length == 4)
                    return db.matrimonio.Include("matrimonio_anexo")
                        .Where(x => (x.ApellidoNombre.Contains(s1) && x.ApellidoNombre.Contains(s2) && x.ApellidoNombre.Contains(s3) && x.ApellidoNombre.Contains(s4)) || (x.Conyugue.Contains(s1) && x.Conyugue.Contains(s2) && x.Conyugue.Contains(s3) && x.Conyugue.Contains(s4)))
                        .OrderByDescending(x => x.MatrimonioId).Take(15).ToList();

                return db.matrimonio.Include("matrimonio_anexo").Where(x => x.ApellidoNombre.Contains(clave) || x.Conyugue.Contains(clave))
                    .OrderByDescending(x => x.MatrimonioId)
                    .Take(15).ToList();

            }
        }

    }
}
