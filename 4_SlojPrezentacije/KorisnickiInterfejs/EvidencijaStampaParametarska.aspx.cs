using System;
using System.Configuration;
using System.Data;
using PrezentacionaLogika;
using KlasePodataka;

namespace KorisnickiInterfejs
{
     public partial class EvidencijaStampaParametarska : System.Web.UI.Page
     {
          private string DajKonekciju()
          {
               return ConfigurationManager.ConnectionStrings["NasaKonekcija"].ConnectionString;
          }

          protected void Page_Load(object sender, EventArgs e)
          {
               // Sigurnosna provera
               if (Session["KorisnikId"] == null)
               {
                    Response.Redirect("~/Login.aspx");
                    return;
               }

               if (!IsPostBack)
               {
                    NapuniKorisnike();

                    // Odmah prikazujemo sve podatke čim se stranica učita
                    FiltrirajIPrikazi();
               }
          }

          private void NapuniKorisnike()
          {
               // Koristimo postojeću DB klasu za korisnike da napunimo padajući meni
               SPKorisnikDBKlasa dbKorisnici = new SPKorisnikDBKlasa(DajKonekciju());
               DataTable dt = dbKorisnici.DajSveKorisnike();

               ddlKorisnici.DataSource = dt;
               ddlKorisnici.DataTextField = "ImePrezime"; // Spojeno ime i prezime iz baze
               ddlKorisnici.DataValueField = "KorisnikID";
               ddlKorisnici.DataBind();
          }

          protected void btnPrikazi_Click(object sender, EventArgs e)
          {
               FiltrirajIPrikazi();
          }

          private void FiltrirajIPrikazi()
          {
               FormaEvidencijaStampaKlasa logika = new FormaEvidencijaStampaKlasa(DajKonekciju());
               DataTable dt;

               int odabraniKorisnikId = int.Parse(ddlKorisnici.SelectedValue);
               string tekstFilter = txtPretraga.Text.Trim();

               // LOGIKA FILTRIRANJA:
               if (odabraniKorisnikId > 0)
               {
                    // 1. Ako je izabran konkretan zaposleni iz padajućeg menija
                    dt = logika.DajEvidencijeZaKorisnika(odabraniKorisnikId);
               }
               else
               {
                    // 2. Ako su izabrani "Svi zaposleni", primenjujemo tekstualni filter (ako je unet)
                    dt = logika.DajEvidencije(tekstFilter);
               }

               // Post-procesiranje: Dodavanje kolone "PreporucenaAkcija" na osnovu ocene
               logika.PrimeniAkcijeNaTabelu(dt);

               // Povezivanje sa GridView-om
               gvRezultat.DataSource = dt;
               gvRezultat.DataBind();
          }
     }
}