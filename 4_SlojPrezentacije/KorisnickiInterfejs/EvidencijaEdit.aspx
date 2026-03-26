<%@ Page Title="Upravljanje Ocenama" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EvidencijaEdit.aspx.cs" Inherits="KorisnickiInterfejs.EvidencijaEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <div class="card shadow-sm mb-4">
            <div class="card-header bg-dark text-warning">
                <h4 class="mb-0"><i class="fas fa-clipboard-list me-2"></i>Pregled i izmena ocena rada</h4>
            </div>
            <div class="card-body">
                <asp:GridView ID="gvEvidencije" runat="server" CssClass="table table-hover table-bordered" 
                    AutoGenerateColumns="False" OnSelectedIndexChanged="gvEvidencije_SelectedIndexChanged" DataKeyNames="EvidencijaID">
                    <Columns>
                        <asp:BoundField DataField="EvidencijaID" HeaderText="ID" />
                        <asp:BoundField DataField="Zaposleni" HeaderText="Zaposleni" />
                        <asp:BoundField DataField="Kriterijum" HeaderText="Kriterijum" />
                        <asp:BoundField DataField="Ocena" HeaderText="Ocena" />
                        <asp:BoundField DataField="DatumUnosa" HeaderText="Datum" DataFormatString="{0:dd.MM.yyyy}" />
                        <asp:CommandField ShowSelectButton="True" SelectText="Izmeni/Obriši" ControlStyle-CssClass="btn btn-sm btn-warning fw-bold" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <asp:Panel ID="pnlUredi" runat="server" Visible="false">
            <div class="card shadow border-warning">
                <div class="card-header bg-warning text-dark">
                    <h4 class="mb-0"><i class="fas fa-edit me-2"></i>Izmena ili brisanje odabrane ocene</h4>
                </div>
                <div class="card-body">
                    <asp:Panel ID="pnlPoruka" runat="server" Visible="false" CssClass="alert alert-info">
                        <asp:Label ID="lblPoruka" runat="server"></asp:Label>
                    </asp:Panel>

                    <asp:HiddenField ID="hfEvidencijaID" runat="server" />

                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label class="form-label fw-bold">Zaposleni:</label>
                            <asp:DropDownList ID="ddlKorisnik" runat="server" CssClass="form-select"></asp:DropDownList>
                        </div>
                        <div class="col-md-6 mb-3">
                            <label class="form-label fw-bold">Kriterijum:</label>
                            <asp:DropDownList ID="ddlKriterijum" runat="server" CssClass="form-select"></asp:DropDownList>
                        </div>
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-bold">Ocena (1-5):</label>
                        <asp:TextBox ID="txtOcena" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-bold">Komentar:</label>
                        <asp:TextBox ID="txtKomentar" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>

                    <div class="mt-4 d-flex justify-content-end">
                        <asp:Button ID="btnObrisi" runat="server" Text="Obriši evidenciju" CssClass="btn btn-danger me-2" 
                            OnClientClick="return confirm('Da li ste sigurni da želite obrisati ovu ocenu?');" OnClick="btnObrisi_Click" />
                        <asp:Button ID="btnSacuvaj" runat="server" Text="Sačuvaj izmene" CssClass="btn btn-success" OnClick="btnSacuvaj_Click" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>

</asp:Content>