using System;
using System.Configuration;
using System.Web.UI;
using PrezentacionaLogika;

namespace KorisnickiInterfejs
{
     public partial class EvidencijaUnos : System.Web.UI.Page
     {
          // Pomoćna metoda za dohvatanje konekcije
          private string DajKonekciju()
          {
               return ConfigurationManager.ConnectionStrings["NasaKonekcija"].ConnectionString;
          }

          protected void Page_Load(object sender, EventArgs e)
          {
               // Sigurnosna provera sesije
               if (Session["KorisnikId"] == null)
               {
                    Response.Redirect("~/Login.aspx");
                    return;
               }

               if (!IsPostBack)
               {
                    NapuniPadajuceMenije();
               }
          }

          private void NapuniPadajuceMenije()
          {
               try
               {
                    // Instanciramo logiku prezentacije
                    FormaEvidencijaUnosKlasa logika = new FormaEvidencijaUnosKlasa(DajKonekciju());

                    // 1. Punjenje kriterijuma (šifrarnika)
                    ddlKriterijum.DataSource = logika.UcitajKriterijume();
                    ddlKriterijum.DataTextField = "Naziv";        // Kolona koju vidi korisnik (npr. 'Timski rad')
                    ddlKriterijum.DataValueField = "KriterijumID"; // Kolona koja je ID
                    ddlKriterijum.DataBind();

                    // 2. Punjenje korisnika (radnika)
                    ddlKorisnik.DataSource = logika.UcitajKorisnike();
                    ddlKorisnik.DataTextField = "ImePrezime";     // Spojeno ime i prezime
                    ddlKorisnik.DataValueField = "KorisnikID";
                    ddlKorisnik.DataBind();
               }
               catch (Exception ex)
               {
                    PrikaziPoruku("Greška pri učitavanju listi iz baze: " + ex.Message, false);
               }
          }

          protected void btnSnimi_Click(object sender, EventArgs e)
          {
               try
               {
                    // Provera da li su izabrani radnik i kriterijum
                    int korisnikId = int.Parse(ddlKorisnik.SelectedValue);
                    int kriterijumId = int.Parse(ddlKriterijum.SelectedValue);

                    if (korisnikId == 0 || kriterijumId == 0)
                    {
                         PrikaziPoruku("Morate izabrati zaposlenog i kriterijum ocenjivanja.", false);
                         return;
                    }

                    // Čitanje ocene iz RadioButton-a i komentara iz TextBox-a
                    int ocena = int.Parse(rblOcena.SelectedValue);
                    string komentar = txtKomentar.Text.Trim();

                    // Instanciranje logike za unos
                    FormaEvidencijaUnosKlasa logika = new FormaEvidencijaUnosKlasa(DajKonekciju());

                    string porukaIzLogike;

                    // Poziv metode Snimi (koja radi validaciju preko XML-a i upis)
                    bool uspesno = logika.Snimi(korisnikId, kriterijumId, ocena, komentar, out porukaIzLogike);

                    if (uspesno)
                    {
                         PrikaziPoruku(porukaIzLogike, true); // Zelena poruka sa predlogom akcije
                         OcistiFormu();
                    }
                    else
                    {
                         PrikaziPoruku("Greška: " + porukaIzLogike, false); // Crvena poruka (npr. ako nije uneo komentar za ocenu 1)
                    }
               }
               catch (Exception ex)
               {
                    PrikaziPoruku("Došlo je do sistemske greške: " + ex.Message, false);
               }
          }

          private void PrikaziPoruku(string tekst, bool uspesno)
          {
               pnlPoruka.Visible = true;

               // Formatiramo poruku tako da podržava prelom reda (\n iz logike pretvaramo u <br>)
               lblPoruka.Text = tekst.Replace("\n", "<br/>");

               if (uspesno)
               {
                    pnlPoruka.CssClass = "alert alert-success mt-3 fw-bold";
               }
               else
               {
                    pnlPoruka.CssClass = "alert alert-danger mt-3 fw-bold";
               }
          }

          private void OcistiFormu()
          {
               // Resetujemo padajuće menije i radio dugmad na default
               ddlKorisnik.SelectedIndex = 0;
               ddlKriterijum.SelectedIndex = 0;
               rblOcena.SelectedValue = "3"; // Vraćamo na standardnu ocenu
               txtKomentar.Text = "";
          }
     }
}