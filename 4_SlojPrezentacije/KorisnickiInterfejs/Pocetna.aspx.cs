using System;
using System.Web.UI;

namespace KorisnickiInterfejs
{
     public partial class Pocetna : System.Web.UI.Page
     {
          protected void Page_Load(object sender, EventArgs e)
          {
               // Provera sesije - Da li je korisnik ulogovan?
               if (Session["KorisnikId"] == null)
               {
                    // Ako nije ulogovan, vrati ga na Login stranu
                    Response.Redirect("~/Login.aspx");
               }
               else
               {
                    // Ako je ulogovan, prikazi njegovo ime
                    if (!IsPostBack)
                    {
                         string imePrezime = Session["KorisnikImePrezime"] as string;
                         string pozicija = Session["Pozicija"] as string; // Pozicija (npr. Menadžer)

                         if (!string.IsNullOrEmpty(imePrezime))
                         {
                              // Ako ima poziciju, ispišemo i nju u zagradi
                              if (!string.IsNullOrEmpty(pozicija))
                              {
                                   lblPozdrav.Text = $"Zdravo, {imePrezime} ({pozicija})!";
                              }
                              else
                              {
                                   lblPozdrav.Text = $"Zdravo, {imePrezime}!";
                              }
                         }
                         else
                         {
                              lblPozdrav.Text = "Zdravo, dobrodošli na Nexus portal!";
                         }
                    }
               }
          }
     }
}