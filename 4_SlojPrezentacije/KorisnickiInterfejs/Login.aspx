<%@ Page Title="Prijava" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="KorisnickiInterfejs.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="d-flex justify-content-center align-items-center" style="min-height: 70vh;">
        
        <div class="card shadow-lg" style="width: 400px; border-radius: 10px;">
            <div class="card-header bg-dark text-white text-center" style="border-top-left-radius: 10px; border-top-right-radius: 10px;">
                <h3><i class="fas fa-truck-moving me-2"></i>Nexus Logistika</h3>
                <h6 class="text-muted mb-0">Portal za zaposlene</h6>
            </div>
            
            <div class="card-body p-4">
                
                <div class="mb-3 text-center">
                    <asp:Label ID="lblStatus" runat="server" CssClass="text-danger fw-bold" Visible="false"></asp:Label>
                </div>

                <div class="mb-3">
                    <label for="KorisnickoImeTextBox" class="form-label">Email adresa:</label>
                    <div class="input-group">
                        <span class="input-group-text"><i class="fas fa-envelope"></i></span>
                        <asp:TextBox ID="KorisnickoImeTextBox" runat="server" CssClass="form-control" placeholder="ime.prezime@nexus.rs" />
                    </div>
                </div>

                <div class="mb-4">
                    <label for="SifraTextBox" class="form-label">Lozinka:</label>
                    <div class="input-group">
                        <span class="input-group-text"><i class="fas fa-lock"></i></span>
                        <asp:TextBox ID="SifraTextBox" runat="server" TextMode="Password" CssClass="form-control" placeholder="Unesite lozinku" />
                    </div>
                </div>

                <div class="d-grid">
                    <asp:Button ID="PrijavaButton" runat="server" Text="Prijavi se na sistem" OnClick="PrijavaButton_Click" CssClass="btn btn-warning btn-lg fw-bold" />
                </div>

            </div>
            
            <div class="card-footer text-center text-muted">
                <small>Sistem za upravljanje flotom v2.1<br/>Problemi sa prijavom? Kontaktirajte IT podršku.</small>
            </div>
        </div>

    </div>

</asp:Content>