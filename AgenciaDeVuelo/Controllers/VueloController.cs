using System;
using System.Web.Mvc;
using Entidades;
using Logica;

namespace AgenciaDeVueloMVC.Controllers
{
    public class VueloController : Controller
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

            ViewBag.Estados = FabricaL.GetLogicaEstado().ListarEstados("");

            return View();
        }

        [HttpPost]
        public ActionResult Alta(
            string codigo,
            DateTime fechaHoraPartida,
            string codigoEstadoPartida,
            DateTime fechaHoraLlegada,
            string codigoEstadoLlegada,
            decimal precioPasaje)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login", "Empleado");

                Estado estadoPartida =
                    FabricaL.GetLogicaEstado().BuscarEstado(codigoEstadoPartida);

                Estado estadoLlegada =
                    FabricaL.GetLogicaEstado().BuscarEstado(codigoEstadoLlegada);

                Vuelo vuelo = new Vuelo(
                    codigo,
                    fechaHoraPartida,
                    estadoPartida,
                    fechaHoraLlegada,
                    estadoLlegada,
                    precioPasaje,
                    true
                );

                FabricaL.GetLogicaVuelo().AltaVuelo(vuelo);

                ViewBag.Mensaje = "Vuelo dado de alta correctamente.";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            ViewBag.Estados = FabricaL.GetLogicaEstado().ListarEstados("");

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

                Vuelo vuelo = FabricaL.GetLogicaVuelo().BuscarVuelo(codigo);

                if (vuelo == null)
                    ViewBag.Mensaje = "No existe un vuelo con ese código.";
                else
                    ViewBag.Vuelo = vuelo;
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

            ViewBag.Estados =
                FabricaL.GetLogicaEstado().ListarEstados("");

            return View();
        }

        [HttpPost]
        public ActionResult Modificar(string codigo)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login", "Empleado");

                Vuelo vuelo = FabricaL.GetLogicaVuelo().BuscarVuelo(codigo);

                if (vuelo == null)
                    ViewBag.Mensaje = "No existe un vuelo con ese código.";
                else
                    ViewBag.Vuelo = vuelo;
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            ViewBag.Estados = FabricaL.GetLogicaEstado().ListarEstados("");

            return View();
        }

        [HttpPost]
        public ActionResult GuardarModificacion(
            string codigo,
            DateTime fechaHoraPartida,
            string codigoEstadoPartida,
            DateTime fechaHoraLlegada,
            string codigoEstadoLlegada,
            decimal precioPasaje,
            bool? activo)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login", "Empleado");

                Estado estadoPartida =
                    FabricaL.GetLogicaEstado().BuscarEstado(codigoEstadoPartida);

                Estado estadoLlegada =
                    FabricaL.GetLogicaEstado().BuscarEstado(codigoEstadoLlegada);

                Vuelo vuelo = new Vuelo(
                    codigo,
                    fechaHoraPartida,
                    estadoPartida,
                    fechaHoraLlegada,
                    estadoLlegada,
                    precioPasaje,
                    activo ?? false
                );

                FabricaL.GetLogicaVuelo().ModificarVuelo(vuelo);

                TempData["Mensaje"] = "Vuelo modificado correctamente.";

                return RedirectToAction("Modificar");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;

                ViewBag.Estados =
                    FabricaL.GetLogicaEstado().ListarEstados("");

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

                FabricaL.GetLogicaVuelo().BajaVuelo(codigo);

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