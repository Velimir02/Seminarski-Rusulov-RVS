using System;
using System.Configuration;
using System.Web.UI;
using PrezentacionaLogika;

namespace KorisnickiInterfejs
{
     public partial class Login : System.Web.UI.Page
     {
          protected void Page_Load(object sender, EventArgs e)
          {
               // Ako je korisnik već ulogovan, preusmeri ga odmah na Početnu stranu
               if (!IsPostBack)
               {
                    if (Session["KorisnikId"] != null)
                    {
                         Response.Redirect("~/Pocetna.aspx");
                    }
               }
          }

          protected void PrijavaButton_Click(object sender, EventArgs e)
          {
               // 1. Čitanje konekcije iz Web.config-a
               string cs = ConfigurationManager.ConnectionStrings["NasaKonekcija"].ConnectionString;

               // 2. Instanciranje klase iz Prezentacione Logike
               // Povezujemo TextBox-ove sa propertijima Email i Lozinka iz naše klase
               var login = new FormaLoginKlasa(cs)
               {
                    Email = KorisnickoImeTextBox.Text.Trim(),
                    Lozinka = SifraTextBox.Text
               };

               // 3. Provera da li su podaci tačni
               if (login.VazeciKorisnik())
               {
                    // Podaci su tačni, punimo sesiju osnovnim podacima
                    Session["KorisnikImePrezime"] = login.DajImePrezimeKorisnika();
                    Session["KorisnikId"] = login.DajIdKorisnika();
                    Session["Email"] = KorisnickoImeTextBox.Text.Trim();

                    // Opciono: Punimo sesiju pozicijom iz baze (npr. "Dispečer", "Vozač")
                    Session["Pozicija"] = login.DajPozicijuKorisnika();

                    // 4. Provera za glavnog sistemskog Admina (Supervizora) iz Web.config-a
                    string adminEmail = ConfigurationManager.AppSettings["AdminEmail"] ?? "admin@nexus.rs";

                    if (Session["Email"].ToString().Equals(adminEmail, StringComparison.OrdinalIgnoreCase))
                    {
                         Session["Uloga"] = "Admin";
                    }
                    else
                    {
                         Session["Uloga"] = "Korisnik";
                    }

                    // Redirekcija nakon uspešne prijave
                    Response.Redirect("~/Pocetna.aspx", endResponse: false);
               }
               else
               {
                    // Podaci nisu tačni - prikazujemo grešku na labeli
                    lblStatus.Text = "Neispravna email adresa ili lozinka!";
                    lblStatus.Visible = true;
               }
          }
     }
}