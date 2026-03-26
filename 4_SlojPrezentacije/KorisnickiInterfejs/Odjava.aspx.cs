using System;
using System.Web;
using System.Web.UI;

namespace KorisnickiInterfejs
{
    public partial class Odjava : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Očisti sve podatke iz sesije
            Session.Clear();
            Session.Abandon();

            // Vrati korisnika na login stranicu
            Response.Redirect("Login.aspx");
        }
    }
}
