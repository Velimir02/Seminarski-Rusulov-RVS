using System;
using System.Configuration;
using System.Data;
using PrezentacionaLogika;

namespace KorisnickiInterfejs
{
     public partial class EvidencijaStampa : System.Web.UI.Page
     {
          protected void Page_Load(object sender, EventArgs e)
          {
               // Sigurnosna provera: Ako korisnik nije ulogovan, vrati ga na Login
               if (Session["KorisnikId"] == null)
               {
                    Response.Redirect("~/Login.aspx");
                    return;
               }

               // Učitavamo podatke samo prvi put kada se stranica otvori
               if (!IsPostBack)
               {
                    UcitajEvidencije();
               }
          }

          private void UcitajEvidencije()
          {
               // 1. Čitamo konekciju za bazu
               string cs = ConfigurationManager.ConnectionStrings["NasaKonekcija"].ConnectionString;

               // 2. Instanciramo klasu iz sloja Prezentacione Logike namenjenu za štampu
               FormaEvidencijaStampaKlasa logika = new FormaEvidencijaStampaKlasa(cs);

               // 3. Dobijamo sirove podatke (INNER JOIN tabele Evidencija, Korisnik i Kriterijum)
               DataTable dt = logika.DajEvidencije();

               // 4. Primenjujemo poslovnu logiku na tabelu
               // Ovo prolazi kroz svaki red, čita Ocenu, i dodaje kolonu "PreporucenaAkcija"
               logika.PrimeniAkcijeNaTabelu(dt);

               // 5. Povezujemo napunjenu tabelu sa GridView-om na ekranu
               gvEvidencije.DataSource = dt;
               gvEvidencije.DataBind();
          }
     }
}