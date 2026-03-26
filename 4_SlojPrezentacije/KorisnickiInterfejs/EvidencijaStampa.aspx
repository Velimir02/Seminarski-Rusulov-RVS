<%@ Page Title="Štampa Izveštaja" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EvidencijaStampa.aspx.cs" Inherits="KorisnickiInterfejs.EvidencijaStampa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <style>
        @media print {
            /* Sakrij sve što nije tabela i naslov */
            .no-print, .navbar, footer, .btn {
                display: none !important;
            }
            /* Osiguraj da tabela bude crno-bela i čitljiva na papiru */
            body { background-color: white; color: black; }
            .card { border: none !important; box-shadow: none !important; }
            .card-header { background-color: white !important; color: black !important; border-bottom: 2px solid black !important; padding-left: 0 !important; }
            .main-wrapper { padding: 0 !important; border: none !important; margin: 0 !important; box-shadow: none !important; }
            
            /* Tabela za štampu - crne linije */
            .table-bordered th, .table-bordered td { border: 1px solid black !important; }
            
            /* Print Header - Prikazuje se samo na papiru */
            .print-header { display: block !important; margin-bottom: 20px; }
        }

        /* Sakrij Print Header na ekranu */
        .print-header { display: none; }
    </style>

    <div class="card border-0 shadow-sm mt-2">
        
        <div class="print-header text-center">
            <h2><i class="fas fa-truck-moving"></i> Nexus Logistika d.o.o.</h2>
            <h4>Zvanični izveštaj o kvalitetu rada zaposlenih</h4>
            <hr style="border-top: 2px solid black;"/>
        </div>

        <div class="card-header bg-dark text-warning d-flex justify-content-between align-items-center" style="border-radius: 8px 8px 0 0;">
            <h3 class="mb-0"><i class="fas fa-print me-2"></i>Izveštaj o radnicima</h3>
            
            <button type="button" class="btn btn-warning fw-bold no-print" onclick="window.print();">
                <i class="fas fa-print"></i> Odštampaj Izveštaj
            </button>
        </div>

        <div class="card-body p-0 pt-3">
           <asp:GridView ID="gvEvidencije" runat="server" CssClass="table table-bordered table-striped table-hover mb-0" 
                AutoGenerateColumns="False" EmptyDataText="Nema zabeleženih ocena za prikaz.">
                
                <HeaderStyle CssClass="bg-secondary text-white" />
                
                <Columns>
                    <asp:BoundField DataField="Zaposleni" HeaderText="Ime i Prezime (Zaposleni)" />
                    <asp:BoundField DataField="Kriterijum" HeaderText="Kriterijum Ocenjivanja" />
                    
                    <asp:BoundField DataField="Ocena" HeaderText="Ocena" ItemStyle-Font-Bold="true" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                    
                    <asp:BoundField DataField="Komentar" HeaderText="Detaljno Obrazloženje" />
                    <asp:BoundField DataField="DatumUnosa" HeaderText="Datum Evidentiranja" DataFormatString="{0:dd.MM.yyyy}" />
                    
                    <%-- Ovo je kolona koju dinamički dodaje Poslovna Logika (FormaEvidencijaStampaKlasa) --%>
                    <asp:BoundField DataField="PreporucenaAkcija" HeaderText="Sistemska Akcija / Preporuka" ItemStyle-Font-Italic="true" />
                </Columns>
            </asp:GridView>
            
            <div class="mt-4 text-end text-muted pe-3 pb-3">
                <small>Dokument generisan dana: <%: DateTime.Now.ToString("dd.MM.yyyy u HH:mm") %> časova.</small>
                <br />
                <small>Generisao: <%: Session["KorisnikImePrezime"] %></small>
            </div>
        </div>
    </div>

</asp:Content>