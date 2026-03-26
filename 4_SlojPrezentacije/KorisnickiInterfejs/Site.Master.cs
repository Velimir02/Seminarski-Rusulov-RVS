using System;
using System.Web.UI;

namespace KorisnickiInterfejs
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ovde možeš dodati globalnu proveru sesije ako želiš da 
            // zaštitiš apsolutno sve stranice koje koriste ovaj Master.
        }

        protected void btnOdjava_Click(object sender, EventArgs e)
        {
            // 1. Čistimo sve podatke iz sesije (ID korisnika, ime, ulogu)
            Session.Clear();
            Session.Abandon();

            // 2. Preusmeravamo korisnika na login stranu
            // Koristimo ~/ da bi putanja bila ispravna bez obzira na folder
            Response.Redirect("~/Login.aspx");
        }
    }
}