using System;
using System.Web.Mvc;
using Entidades;
using Logica;

namespace AgenciaDeVueloMVC.Controllers
{
    public class EmpleadoController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string usuario, string contrasenia)
        {
            try
            {
                Empleado emp = FabricaL.GetLogicaEmpleado().LogueoEmpleado(usuario, contrasenia);

                Session["Empleado"] = emp;

                return RedirectToAction("Menu");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        public ActionResult Menu()
        {
            if (Session["Empleado"] == null)
                return RedirectToAction("Login");

            return View();
        }

        public ActionResult Mantenimiento()
        {
            if (Session["Empleado"] == null)
                return RedirectToAction("Login");

            return View();
        }

        public ActionResult Alta()
        {
            if (Session["Empleado"] == null)
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        public ActionResult Alta(string usuario, string contrasenia, string nombreCompleto)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login");

                Empleado emp = new Empleado(
                    usuario,
                    contrasenia,
                    nombreCompleto,
                    true
                );

                FabricaL.GetLogicaEmpleado().AltaEmpleado(emp);

                ViewBag.Mensaje = "Empleado dado de alta correctamente.";
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
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        public ActionResult Buscar(string usuario)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login");

                Empleado emp = FabricaL.GetLogicaEmpleado().BuscarEmpleado(usuario);

                if (emp == null)
                    ViewBag.Mensaje = "No existe un empleado con ese usuario.";
                else
                    ViewBag.Empleado = emp;
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
                return RedirectToAction("Login");
            ViewBag.Mensaje = TempData["Mensaje"];

            return View();
        }

        [HttpPost]
        public ActionResult Modificar(string usuario)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login");

                Empleado emp = FabricaL.GetLogicaEmpleado().BuscarEmpleado(usuario);

                if (emp == null)
                    ViewBag.Mensaje = "No existe un empleado con ese usuario.";
                else
                    ViewBag.Empleado = emp;
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View();
        }

        [HttpPost]
        public ActionResult GuardarModificacion(
            string usuario,
            string contrasenia,
            string nombreCompleto,
            bool?  activo )
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login");

                Empleado emp = new Empleado(
                    usuario,
                    contrasenia,
                    nombreCompleto,
                    activo ?? false
                );

                FabricaL.GetLogicaEmpleado().ModificarEmpleado(emp);

                TempData["Mensaje"] = "Empleado modificado correctamente.";
                return RedirectToAction("Modificar");
              

            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("Modificar");
            }
        }

        public ActionResult CerrarSesion()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}