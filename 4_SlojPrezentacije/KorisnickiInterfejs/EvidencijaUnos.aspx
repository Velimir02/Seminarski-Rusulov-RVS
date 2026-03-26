<%@ Page Title="Nova Ocena" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EvidencijaUnos.aspx.cs" Inherits="KorisnickiInterfejs.EvidencijaUnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row justify-content-center">
        <div class="col-md-8">
            <div class="card shadow-sm border-0">
                <div class="card-header bg-dark text-warning">
                    <h4 class="mb-0"><i class="fas fa-star-half-alt me-2"></i>Ocenjivanje zaposlenog</h4>
                </div>
                
                <div class="card-body bg-light">
                    
                    <asp:Panel ID="pnlPoruka" runat="server" Visible="false" CssClass="alert alert-info">
                        <asp:Label ID="lblPoruka" runat="server"></asp:Label>
                    </asp:Panel>

                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label for="ddlKorisnik" class="form-label fw-bold">Zaposleni koga ocenjujete <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlKorisnik" runat="server" CssClass="form-select border-dark">
                                <asp:ListItem Text="-- Izaberite radnika --" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-6 mb-3">
                            <label for="ddlKriterijum" class="form-label fw-bold">Kriterijum (Oblast ocene) <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlKriterijum" runat="server" CssClass="form-select border-dark">
                                <asp:ListItem Text="-- Izaberite kriterijum --" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="mb-4">
                        <label class="form-label fw-bold">Brojčana ocena (1 do 5) <span class="text-danger">*</span></label>
                        <div class="d-flex justify-content-between px-3">
                            <asp:RadioButtonList ID="rblOcena" runat="server" RepeatDirection="Horizontal" CssClass="w-100 d-flex justify-content-between" style="font-size: 1.2rem;">
                                <asp:ListItem Text=" 1 (Kritično)" Value="1"></asp:ListItem>
                                <asp:ListItem Text=" 2 (Loše)" Value="2"></asp:ListItem>
                                <asp:ListItem Text=" 3 (Standardno)" Value="3" Selected="True"></asp:ListItem>
                                <asp:ListItem Text=" 4 (Vrlo dobro)" Value="4"></asp:ListItem>
                                <asp:ListItem Text=" 5 (Odlično)" Value="5"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <small class="text-muted"><i class="fas fa-info-circle"></i> Napomena: Za ocene 1 i 2, komentar je obavezan.</small>
                    </div>

                    <div class="mb-3">
                        <label for="txtKomentar" class="form-label fw-bold">Obrazloženje / Komentar ocene</label>
                        <asp:TextBox ID="txtKomentar" runat="server" CssClass="form-control border-dark" TextMode="MultiLine" Rows="4" placeholder="Detaljno obrazložite učinak radnika..."></asp:TextBox>
                    </div>

                    <div class="mt-4 d-flex justify-content-between">
                        <a href="EvidencijaEdit.aspx" class="btn btn-outline-dark fw-bold">
                            <i class="fas fa-list"></i> Lista Ocena
                        </a>
                        <asp:Button ID="btnSnimi" runat="server" Text="Sačuvaj ocenu" OnClick="btnSnimi_Click" CssClass="btn btn-warning text-dark fw-bold px-4" />
                    </div>

                </div>
            </div>
        </div>
    </div>

</asp:Content>