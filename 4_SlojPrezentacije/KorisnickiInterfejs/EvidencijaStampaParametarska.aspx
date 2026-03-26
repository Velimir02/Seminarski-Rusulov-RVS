<%@ Page Title="Napredna Pretraga" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EvidencijaStampaParametarska.aspx.cs" Inherits="KorisnickiInterfejs.EvidencijaStampaParametarska" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        @media print {
            .no-print, .navbar, footer { display: none !important; }
            body { background-color: white; color: black; }
            .card { border: none !important; box-shadow: none !important; }
            .table-bordered th, .table-bordered td { border: 1px solid black !important; }
        }
    </style>

    <div class="card mt-3 shadow-sm border-0">
        <div class="card-header bg-dark text-warning">
            <h4 class="mb-0"><i class="fas fa-filter me-2"></i>Napredna pretraga i izveštaji</h4>
        </div>

        <div class="card-body">
            
            <div class="row mb-4 p-4 bg-light border rounded no-print">
                <div class="col-md-5">
                    <label class="form-label fw-bold">Filtriraj po zaposlenom:</label>
                    <asp:DropDownList ID="ddlKorisnici" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                        <asp:ListItem Text="-- Svi zaposleni --" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="col-md-5">
                    <label class="form-label fw-bold">Pretraga po tekstu (Komentar/Kriterijum):</label>
                    <asp:TextBox ID="txtPretraga" runat="server" CssClass="form-control" placeholder="Unesite reč za pretragu..."></asp:TextBox>
                </div>

                <div class="col-md-2 d-flex align-items-end">
                    <asp:Button ID="btnPrikazi" runat="server" Text="Prikaži" CssClass="btn btn-primary w-100 fw-bold" OnClick="btnPrikazi_Click" />
                </div>
            </div>

            <div class="d-flex justify-content-between align-items-center mb-3">
                <h5 class="text-secondary mb-0"><i class="fas fa-list me-2"></i>Rezultati pretrage:</h5>
                <button type="button" class="btn btn-outline-dark no-print" onclick="window.print();">
                    <i class="fas fa-print"></i> Štampaj tabelu
                </button>
            </div>

            <asp:GridView ID="gvRezultat" runat="server" CssClass="table table-bordered table-hover" 
                AutoGenerateColumns="False" EmptyDataText="Nema rezultata za zadate parametre pretrage.">
                
                <HeaderStyle CssClass="bg-secondary text-white" />
                
                <Columns>
                    <asp:BoundField DataField="Zaposleni" HeaderText="Zaposleni" />
                    <asp:BoundField DataField="Kriterijum" HeaderText="Kriterijum Ocenjivanja" />
                    
                    <asp:BoundField DataField="Ocena" HeaderText="Ocena" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                    
                    <asp:BoundField DataField="Komentar" HeaderText="Komentar" />
                    <asp:BoundField DataField="DatumUnosa" HeaderText="Datum Evidencije" DataFormatString="{0:dd.MM.yyyy}" />
                    
                    <asp:BoundField DataField="PreporucenaAkcija" HeaderText="Sistemska Akcija" ItemStyle-Font-Italic="true" />
                </Columns>
            </asp:GridView>

        </div>
    </div>

</asp:Content>