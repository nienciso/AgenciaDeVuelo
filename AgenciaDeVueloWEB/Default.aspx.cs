using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModeloEF;

namespace AgenciaDeVueloWEB
{
    public partial class _Default : Page
    {
        private AgenciaDeVueloEntities contexto =
            new AgenciaDeVueloEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEstados();
                CargarPaquetes();
                pnlDetalle.Visible = false;
            }
        }

        private void CargarEstados()
        {
            try
            {
                ddlEstados.DataSource = contexto.Estados
                    .Where(x => x.Activo)
                    .OrderBy(x => x.Nombre)
                    .ToList();

                ddlEstados.DataTextField = "Nombre";
                ddlEstados.DataValueField = "Codigo";

                ddlEstados.DataBind();

                ddlEstados.Items.Insert(0,
                    new ListItem("-- Todos --", ""));
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void CargarPaquetes()
        {
            try
            {
                CargarGrilla("", null, null, null);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void CargarGrilla(
            string codigoEstado,
            int? maxDias,
            int? mes,
            int? anio)
        {
            var consulta = contexto.Paquetes
                .Where(p => p.Activo &&
                       p.Vuelos.FechaHoraPartida > DateTime.Now);

            if (!string.IsNullOrWhiteSpace(codigoEstado))
            {
                consulta = consulta.Where(p =>
                    p.Estados.Codigo == codigoEstado);
            }

            if (maxDias.HasValue)
            {
                consulta = consulta.Where(p =>
                    p.CantDias <= maxDias.Value);
            }

            if (mes.HasValue && anio.HasValue)
            {
                DateTime fechaInicio =
                    new DateTime(anio.Value, mes.Value, 1);

                DateTime fechaFin =
                    fechaInicio.AddMonths(1);

                consulta = consulta.Where(p =>
                    p.Vuelos.FechaHoraPartida >= fechaInicio &&
                    p.Vuelos.FechaHoraPartida < fechaFin);
            }

            var listaGrid = consulta
                .OrderBy(p => p.Vuelos.FechaHoraPartida)
                .ToList()
                .Select(p => new
                {
                    Codigo = p.Codigo,
                    Titulo = p.Titulo,
                    FechaSalida = p.Vuelos.FechaHoraPartida,
                    Estado = p.Estados.Nombre + " - " + p.Estados.Pais,
                    Precio1 = p.PrecioIndividual,
                    Precio2 = p.PrecioDoble,
                    Precio3 = p.PrecioTriple,
                    CantDias = p.CantDias
                })
                .ToList();

            gvPaquetes.DataKeyNames =
                new string[] { "Codigo" };

            gvPaquetes.DataSource = listaGrid;
            gvPaquetes.DataBind();
        }

        protected void btnFiltrar_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                lblError.Text = "";

                string codigoEstado =
                    ddlEstados.SelectedValue;

                int? maxDias = null;
                int? mes = null;
                int? anio = null;

                if (!string.IsNullOrWhiteSpace(txtMaxDias.Text))
                {
                    int valorDias;

                    if (!int.TryParse(txtMaxDias.Text,
                        out valorDias))
                    {
                        throw new Exception(
                            "La cantidad máxima de días debe ser numérica.");
                    }

                    if (valorDias <= 0)
                    {
                        throw new Exception(
                            "La cantidad máxima de días debe ser mayor a cero.");
                    }

                    maxDias = valorDias;
                }

                if (!string.IsNullOrWhiteSpace(txtMes.Text) ||
                    !string.IsNullOrWhiteSpace(txtAnio.Text))
                {
                    if (string.IsNullOrWhiteSpace(txtMes.Text) ||
                        string.IsNullOrWhiteSpace(txtAnio.Text))
                    {
                        throw new Exception(
                            "Debe ingresar mes y año.");
                    }

                    int valorMes;
                    int valorAnio;

                    if (!int.TryParse(txtMes.Text,
                        out valorMes))
                    {
                        throw new Exception(
                            "El mes debe ser numérico.");
                    }

                    if (valorMes < 1 || valorMes > 12)
                    {
                        throw new Exception(
                            "El mes debe estar entre 1 y 12.");
                    }

                    if (!int.TryParse(txtAnio.Text,
                        out valorAnio))
                    {
                        throw new Exception(
                            "El año debe ser numérico.");
                    }

                    mes = valorMes;
                    anio = valorAnio;
                }

                gvPaquetes.PageIndex = 0;

                CargarGrilla(
                    codigoEstado,
                    maxDias,
                    mes,
                    anio);

                pnlDetalle.Visible = false;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void gvPaquetes_PageIndexChanging(
            object sender,
            GridViewPageEventArgs e)
        {
            gvPaquetes.PageIndex =
                e.NewPageIndex;

            btnFiltrar_Click(null, null);
        }

        protected void gvPaquetes_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            try
            {
                int codigo =
                    Convert.ToInt32(
                        gvPaquetes.SelectedDataKey.Value);

                var p = contexto.Paquetes
                    .FirstOrDefault(x =>
                        x.Codigo == codigo);

                if (p == null)
                    return;

                pnlDetalle.Visible = true;

                string detalle = "";

                detalle += "<b>Título:</b> " +
                    p.Titulo + "<br/>";

                detalle += "<b>Descripción:</b> " +
                    p.Descripcion + "<br/>";

                detalle += "<b>Estado:</b> " +
                    p.Estados.Nombre + " - " +
                    p.Estados.Pais + "<br/>";

                detalle += "<b>Cantidad días:</b> " +
                    p.CantDias + "<br/>";

                detalle += "<b>Precio individual:</b> $" +
                    p.PrecioIndividual + "<br/>";

                detalle += "<b>Precio doble:</b> $" +
                    p.PrecioDoble + "<br/>";

                detalle += "<b>Precio triple:</b> $" +
                    p.PrecioTriple + "<br/><br/>";

                detalle += "<b>Vuelo ida:</b><br/>";

                detalle += p.Vuelos.Codigo +
                    " - " +
                    p.Vuelos.Estados.Nombre +
                    " → " +
                    p.Vuelos.Estados1.Nombre +
                    "<br/><br/>";

                detalle += "<b>Vuelo vuelta:</b><br/>";

                detalle += p.Vuelos1.Codigo +
                    " - " +
                    p.Vuelos1.Estados.Nombre +
                    " → " +
                    p.Vuelos1.Estados1.Nombre +
                    "<br/><br/>";

                detalle += "<b>Hospedajes:</b><br/>";

                foreach (var h in p.PaqueteHospedaje)
                {
                    detalle += h.Hospedajes.Nombre +
                               " - " +
                               h.Hospedajes.TipoHospedaje +
                               " - Noches: " +
                               h.CantNoches +
                               "<br/>";
                }

                lblDetalle.Text = detalle;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}