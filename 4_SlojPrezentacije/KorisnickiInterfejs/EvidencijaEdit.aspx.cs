using System;
using System.Configuration;
using System.Web.UI;
using PrezentacionaLogika;
using KlasePodataka.Entiteti;

namespace KorisnickiInterfejs
{
     public partial class EvidencijaEdit : System.Web.UI.Page
     {
          private string DajKonekciju() => ConfigurationManager.ConnectionStrings["NasaKonekcija"].ConnectionString;

          protected void Page_Load(object sender, EventArgs e)
          {
               if (Session["KorisnikId"] == null) Response.Redirect("~/Login.aspx");

               if (!IsPostBack)
               {
                    OsveziTabelu();
                    NapuniListe();
               }
          }

          private void OsveziTabelu()
          {
               FormaEvidencijaEditKlasa logika = new FormaEvidencijaEditKlasa(DajKonekciju());
               gvEvidencije.DataSource = logika.DajSveEvidencije();
               gvEvidencije.DataBind();
          }

          private void NapuniListe()
          {
               FormaEvidencijaEditKlasa logika = new FormaEvidencijaEditKlasa(DajKonekciju());

               ddlKriterijum.DataSource = logika.UcitajKriterijume();
               ddlKriterijum.DataTextField = "Naziv";
               ddlKriterijum.DataValueField = "KriterijumID";
               ddlKriterijum.DataBind();

               ddlKorisnik.DataSource = logika.UcitajKorisnike();
               ddlKorisnik.DataTextField = "ImePrezime"; // Koristimo alijas iz SQL-a
               ddlKorisnik.DataValueField = "KorisnikID";
               ddlKorisnik.DataBind();
          }

          protected void gvEvidencije_SelectedIndexChanged(object sender, EventArgs e)
          {
               int id = Convert.ToInt32(gvEvidencije.SelectedDataKey.Value);
               UcitajPodatkeUFormu(id);
               pnlUredi.Visible = true;
          }

          private void UcitajPodatkeUFormu(int id)
          {
               FormaEvidencijaEditKlasa logika = new FormaEvidencijaEditKlasa(DajKonekciju());
               EvidencijaRadaKlasa e = logika.DajPoId(id);

               if (e != null)
               {
                    hfEvidencijaID.Value = e.EvidencijaID.ToString();
                    txtOcena.Text = e.Ocena.ToString();
                    txtKomentar.Text = e.Komentar;
                    ddlKriterijum.SelectedValue = e.KriterijumID.ToString();
                    ddlKorisnik.SelectedValue = e.KorisnikID.ToString();
               }
          }

          protected void btnSacuvaj_Click(object sender, EventArgs e)
          {
               FormaEvidencijaEditKlasa logika = new FormaEvidencijaEditKlasa(DajKonekciju());
               bool uspeh = logika.Izmeni(int.Parse(hfEvidencijaID.Value), int.Parse(ddlKorisnik.SelectedValue),
                                         int.Parse(ddlKriterijum.SelectedValue), int.Parse(txtOcena.Text), txtKomentar.Text);

               if (uspeh) { OsveziTabelu(); PrikaziPoruku("Uspešno izmenjeno!", true); }
               else PrikaziPoruku("Greška pri izmeni.", false);
          }

          protected void btnObrisi_Click(object sender, EventArgs e)
          {
               FormaEvidencijaEditKlasa logika = new FormaEvidencijaEditKlasa(DajKonekciju());
               if (logika.Obrisi(int.Parse(hfEvidencijaID.Value)))
               {
                    pnlUredi.Visible = false;
                    OsveziTabelu();
               }
          }

          private void PrikaziPoruku(string t, bool u) { pnlPoruka.Visible = true; lblPoruka.Text = t; pnlPoruka.CssClass = u ? "alert alert-success" : "alert alert-danger"; }
     }
}