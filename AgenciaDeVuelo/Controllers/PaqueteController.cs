using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Entidades;
using Logica;

namespace AgenciaDeVueloMVC.Controllers
{
    public class PaqueteController : Controller
    {
        public ActionResult Alta()
        {
            if (Session["Empleado"] == null)
                return RedirectToAction("Login", "Empleado");

            ViewBag.Estados = FabricaL.GetLogicaEstado().ListarEstados("");
            ViewBag.Vuelos = FabricaL.GetLogicaVuelo().ListarVuelos();
            ViewBag.Hospedajes = FabricaL.GetLogicaHospedaje().ListarHospedajes("");

            return View();
        }

        [HttpPost]
        public ActionResult Alta(
            string titulo,
            string descripcion,
            string codigoEstadoDestino,
            string codigoVueloIda,
            string codigoVueloVuelta,
            string[] codigosHospedaje,
            int[] cantidadesNoches)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login", "Empleado");

                if (codigosHospedaje == null || cantidadesNoches == null || codigosHospedaje.Length == 0)
                    throw new Exception("Debe agregar al menos un hospedaje.");

                Estado estadoDestino = FabricaL.GetLogicaEstado().BuscarEstado(codigoEstadoDestino);
                Vuelo vueloIda = FabricaL.GetLogicaVuelo().BuscarVuelo(codigoVueloIda);
                Vuelo vueloVuelta = FabricaL.GetLogicaVuelo().BuscarVuelo(codigoVueloVuelta);
                Empleado empleado = (Empleado)Session["Empleado"];

                List<PaqueteHospedaje> hospedajes = new List<PaqueteHospedaje>();

                for (int i = 0; i < codigosHospedaje.Length; i++)
                {
                    Hospedaje hospedaje = FabricaL.GetLogicaHospedaje().BuscarHospedaje(codigosHospedaje[i]);
                    hospedajes.Add(new PaqueteHospedaje(hospedaje, cantidadesNoches[i]));
                }

                Paquete paquete = new Paquete(
                    0,
                    titulo,
                    descripcion,
                    estadoDestino,
                    1,
                    1,
                    1,
                    1,
                    vueloIda,
                    vueloVuelta,
                    empleado,
                    hospedajes,
                    true
                );

                int codigoGenerado = FabricaL.GetLogicaPaquete().AltaPaquete(paquete);

                ViewBag.Mensaje = "Paquete dado de alta correctamente. Código generado: " + codigoGenerado;
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            ViewBag.Estados = FabricaL.GetLogicaEstado().ListarEstados("");
            ViewBag.Vuelos = FabricaL.GetLogicaVuelo().ListarVuelos();
            ViewBag.Hospedajes = FabricaL.GetLogicaHospedaje().ListarHospedajes("");

            return View();
        }

        public ActionResult PaquetesPorHospedaje(string codigoHospedaje)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login", "Empleado");

                Hospedaje hospedaje = FabricaL.GetLogicaHospedaje().BuscarHospedaje(codigoHospedaje);

                if (hospedaje == null)
                    throw new Exception("No existe el hospedaje seleccionado.");

                ViewBag.Hospedaje = hospedaje;

                List<Paquete> paquetes =
                    FabricaL.GetLogicaPaquete().ListarPaquetesPorHospedaje(hospedaje);

                return View(paquetes);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Paquete>());
            }
        }

        public ActionResult Listado(string codigoEstado)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login", "Empleado");

                if (codigoEstado == null)
                    codigoEstado = "";

                ViewBag.Estados = FabricaL.GetLogicaEstado().ListarEstados("");
                ViewBag.CodigoEstado = codigoEstado;

                List<Paquete> paquetes =
                    FabricaL.GetLogicaPaquete().ListarPaquetes(codigoEstado);

                return View(paquetes);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                ViewBag.Estados = FabricaL.GetLogicaEstado().ListarEstados("");
                return View(new List<Paquete>());
            }
        }

        public ActionResult Detalle(int codigo)
        {
            try
            {
                if (Session["Empleado"] == null)
                    return RedirectToAction("Login", "Empleado");

                Paquete paquete =
                    FabricaL.GetLogicaPaquete().BuscarPaquete(codigo);

                if (paquete == null)
                    throw new Exception("No existe el paquete seleccionado.");

                return View(paquete);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }
    }
}