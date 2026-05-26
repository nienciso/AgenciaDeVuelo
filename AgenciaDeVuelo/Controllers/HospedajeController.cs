using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Entidades;
using Logica;

namespace AgenciaDeVueloMVC.Controllers
{
    public class HospedajeController : Controller
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
            string codigoInterno,
            string nombre,
            string direccion,
            string tipoHospedaje,
            decimal precioNochePersona,
            string codigoEstado)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login", "Empleado");

                Estado estado = FabricaL.GetLogicaEstado().BuscarEstado(codigoEstado);

                Hospedaje hospedaje = new Hospedaje(
                    codigoInterno,
                    nombre,
                    direccion,
                    tipoHospedaje,
                    precioNochePersona,
                    estado,
                    true
                );

                FabricaL.GetLogicaHospedaje().AltaHospedaje(hospedaje);

                ViewBag.Mensaje = "Hospedaje dado de alta correctamente.";
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
        public ActionResult Buscar(string codigoInterno)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login", "Empleado");

                Hospedaje hospedaje = FabricaL.GetLogicaHospedaje().BuscarHospedaje(codigoInterno);

                if (hospedaje == null)
                    ViewBag.Mensaje = "No existe un hospedaje con ese código.";
                else
                    ViewBag.Hospedaje = hospedaje;
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
        public ActionResult Modificar(string codigoInterno)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login", "Empleado");

                Hospedaje hospedaje = FabricaL.GetLogicaHospedaje().BuscarHospedaje(codigoInterno);

                if (hospedaje == null)
                    ViewBag.Mensaje = "No existe un hospedaje con ese código.";
                else
                    ViewBag.Hospedaje = hospedaje;
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
            string codigoInterno,
            string nombre,
            string direccion,
            string tipoHospedaje,
            decimal precioNochePersona,
            string codigoEstado,
            bool? activo)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login", "Empleado");

                Estado estado =
                    FabricaL.GetLogicaEstado().BuscarEstado(codigoEstado);

                Hospedaje hospedaje = new Hospedaje(
                    codigoInterno,
                    nombre,
                    direccion,
                    tipoHospedaje,
                    precioNochePersona,
                    estado,
                    activo ?? false
                );

                FabricaL.GetLogicaHospedaje().ModificarHospedaje(hospedaje);

                TempData["Mensaje"] = "Hospedaje modificado correctamente.";

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
        public ActionResult Baja(string codigoInterno)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login", "Empleado");

                FabricaL.GetLogicaHospedaje().BajaHospedaje(codigoInterno);

                ViewBag.Mensaje = "Baja realizada correctamente.";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View();
        }

        public ActionResult ListadoHospedajes(string filtroNombre)
        {
            if (Session["Empleado"] == null)
                return RedirectToAction("Login", "Empleado");

            if (filtroNombre == null)
                filtroNombre = "";

            ViewBag.FiltroNombre = filtroNombre;

            List<Hospedaje> lista = FabricaL.GetLogicaHospedaje().ListarHospedajes(filtroNombre);

            return View(lista);
        }
    }
}