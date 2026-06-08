<%@ Page Title="Paquetes de viaje"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Default.aspx.cs"
    Inherits="AgenciaDeVueloWEB._Default" %>

<asp:Content ID="BodyContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <h2>Paquetes de viaje disponibles</h2>

    <hr />

    <h4>Filtros</h4>

    <table>

        <tr>
            <td>Estado:</td>

            <td>
                <asp:DropDownList ID="ddlEstados"
                    runat="server"
                    CssClass="form-control">
                </asp:DropDownList>
            </td>
        </tr>

        <tr>
            <td>Máximo cantidad días:</td>

            <td>
                <asp:TextBox ID="txtMaxDias"
                    runat="server"
                    CssClass="form-control">
                </asp:TextBox>
            </td>
        </tr>

        <tr>
            <td>Mes:</td>

            <td>
                <asp:TextBox ID="txtMes"
                    runat="server"
                    CssClass="form-control">
                </asp:TextBox>
            </td>
        </tr>

        <tr>
            <td>Año:</td>

            <td>
                <asp:TextBox ID="txtAnio"
                    runat="server"
                    CssClass="form-control">
                </asp:TextBox>
            </td>
        </tr>

    </table>

    <br />

    <asp:Button ID="btnFiltrar"
        runat="server"
        Text="Filtrar"
        CssClass="btn btn-primary"
        OnClick="btnFiltrar_Click" />

    <br />
    <br />

    <asp:Label ID="lblError"
        runat="server"
        ForeColor="Red">
    </asp:Label>

    <hr />

    <asp:GridView ID="gvPaquetes"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="table"
        AllowPaging="True"
        PageSize="5"
        OnPageIndexChanging="gvPaquetes_PageIndexChanging"
        OnSelectedIndexChanged="gvPaquetes_SelectedIndexChanged">

        <Columns>

            <asp:BoundField DataField="Codigo"
                HeaderText="Código" />

            <asp:BoundField DataField="Titulo"
                HeaderText="Título" />

            <asp:BoundField DataField="FechaSalida"
                HeaderText="Fecha salida" />

            <asp:BoundField DataField="Estado"
                HeaderText="Estado" />

            <asp:BoundField DataField="Precio1"
                HeaderText="Precio individual" />

            <asp:BoundField DataField="Precio2"
                HeaderText="Precio doble" />

            <asp:BoundField DataField="Precio3"
                HeaderText="Precio triple" />

            <asp:BoundField DataField="CantDias"
                HeaderText="Cantidad días" />

            <asp:CommandField ShowSelectButton="True"
                SelectText="Ver detalle" />

        </Columns>

    </asp:GridView>

    <hr />

    <asp:Panel ID="pnlDetalle"
        runat="server"
        Visible="false">

        <h3>Detalle del paquete</h3>

        <asp:Label ID="lblDetalle"
            runat="server">
        </asp:Label>

    </asp:Panel>

</asp:Content>