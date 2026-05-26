using System;
using System.Web.Mvc;
using Entidades;
using Logica;

namespace AgenciaDeVueloMVC.Controllers
{
    public class EstadoController : Controller
    {
        public ActionResult Mantenimiento()
        {
            if (Session["Empleado"] == null)
                return RedirectToAction("Login", "Empleado");

            return View();
        }

        public ActionResult Alta()
        {
            if (Session["Empleado"] == null)
                return RedirectToAction("Login", "Empleado");

            return View();
        }

        [HttpPost]
        public ActionResult Alta(string codigo, string nombre, string pais)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login", "Empleado");

                Estado est = new Estado(
                    codigo,
                    nombre,
                    pais,
                    true
                );

                FabricaL.GetLogicaEstado().AltaEstado(est);

                ViewBag.Mensaje = "Estado dado de alta correctamente.";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View();
        }

        public ActionResult Buscar()
        {
            if (Session["Empleado"] == null)
                return RedirectToAction("Login", "Empleado");

            return View();
        }

        [HttpPost]
        public ActionResult Buscar(string codigo)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login", "Empleado");

                Estado est = FabricaL.GetLogicaEstado().BuscarEstado(codigo);

                if (est == null)
                    ViewBag.Mensaje = "No existe un estado con ese código.";
                else
                    ViewBag.Estado = est;
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View();
        }

        public ActionResult Modificar()
        {
            if (Session["Empleado"] == null)
                return RedirectToAction("Login", "Empleado");

            ViewBag.Mensaje = TempData["Mensaje"];

            return View();
        }

        [HttpPost]
        public ActionResult Modificar(string codigo)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login", "Empleado");

                Estado est = FabricaL.GetLogicaEstado().BuscarEstado(codigo);

                if (est == null)
                    ViewBag.Mensaje = "No existe un estado con ese código.";
                else
                    ViewBag.Estado = est;
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View();
        }

        [HttpPost]
        public ActionResult GuardarModificacion(
            string codigo,
            string nombre,
            string pais,
            bool? activo)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login", "Empleado");

                Estado est = new Estado(
                    codigo,
                    nombre,
                    pais,
                    activo ?? false
                );

                FabricaL.GetLogicaEstado().ModificarEstado(est);

                TempData["Mensaje"] = "Estado modificado correctamente.";

                return RedirectToAction("Modificar");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("Modificar");
            }
        }

        public ActionResult Baja()
        {
            if (Session["Empleado"] == null)
                return RedirectToAction("Login", "Empleado");

            return View();
        }

        [HttpPost]
        public ActionResult Baja(string codigo)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login", "Empleado");

                FabricaL.GetLogicaEstado().BajaEstado(codigo);

                ViewBag.Mensaje = "Baja realizada correctamente.";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View();
        }
    }
}