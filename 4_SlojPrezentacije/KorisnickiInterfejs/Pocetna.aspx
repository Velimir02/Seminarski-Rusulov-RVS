<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pocetna.aspx.cs" Inherits="KorisnickiInterfejs.Pocetna" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="row align-items-center mb-5">
        <div class="col-md-8">
            <h1 class="h2 fw-bold text-dark">
                <i class="fas fa-gauge-high me-2" style="color: var(--brand-color);"></i>Kontrolna Tabla
            </h1>
            <p class="text-muted fs-5">
                <asp:Label ID="lblPozdrav" runat="server" Text="Dobrodošli!"></asp:Label>
            </p>
        </div>
        <div class="col-md-4 text-md-end">
            <span class="badge bg-soft-teal text-teal p-2 px-3">
                <i class="fas fa-calendar-alt me-1"></i> <%: DateTime.Now.ToString("dd. MMM yyyy") %>
            </span>
        </div>
    </div>

    <div class="row g-4">
        
        <div class="col-lg-3 col-md-6">
            <a href="EvidencijaUnos.aspx" class="text-decoration-none">
                <div class="card p-3 border-start border-4 border-primary h-100 transition-hover">
                    <div class="card-body">
                        <i class="fas fa-circle-plus fa-2x mb-3 text-primary"></i>
                        <h5 class="fw-bold text-dark">Nova Ocena</h5>
                        <p class="small text-muted mb-0">Dodaj novu evidenciju o radu</p>
                    </div>
                </div>
            </a>
        </div>

        <div class="col-lg-3 col-md-6">
            <a href="EvidencijaEdit.aspx" class="text-decoration-none">
                <div class="card p-3 border-start border-4 border-info h-100 transition-hover">
                    <div class="card-body">
                        <i class="fas fa-list-check fa-2x mb-3 text-info"></i>
                        <h5 class="fw-bold text-dark">Tabela Ocena</h5>
                        <p class="small text-muted mb-0">Izmena i brisanje zapisa</p>
                    </div>
                </div>
            </a>
        </div>

        <div class="col-lg-3 col-md-6">
            <a href="EvidencijaStampa.aspx" class="text-decoration-none">
                <div class="card p-3 border-start border-4 border-warning h-100 transition-hover">
                    <div class="card-body">
                        <i class="fas fa-file-pdf fa-2x mb-3 text-warning"></i>
                        <h5 class="fw-bold text-dark">Izveštaji</h5>
                        <p class="small text-muted mb-0">PDF priprema i štampa</p>
                    </div>
                </div>
            </a>
        </div>

        <div class="col-lg-3 col-md-6">
            <a href="EvidencijaStampaParametarska.aspx" class="text-decoration-none">
                <div class="card p-3 border-start border-4 border-success h-100 transition-hover">
                    <div class="card-body">
                        <i class="fas fa-magnifying-glass fa-2x mb-3 text-success"></i>
                        <h5 class="fw-bold text-dark">Pretraga</h5>
                        <p class="small text-muted mb-0"> Napredni filteri po radniku</p>
                    </div>
                </div>
            </a>
        </div>
    </div>

    <style>
        .transition-hover { transition: transform 0.2s, box-shadow 0.2s; }
        .transition-hover:hover { transform: translateY(-5px); box-shadow: 0 10px 15px rgba(0,0,0,0.1) !important; }
        .bg-soft-teal { background-color: #dcfce7; }
        .text-teal { color: #0f766e; }
    </style>

</asp:Content>