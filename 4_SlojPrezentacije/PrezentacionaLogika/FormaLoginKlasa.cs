using KlasePodataka;
using System.Data;
using System;

namespace PrezentacionaLogika
{
     public class FormaLoginKlasa
     {
          private string _stringKonekcije;
          private string _email;
          private string _lozinka;

          public string Email
          {
               get { return _email; }
               set { _email = value; }
          }

          public string Lozinka
          {
               get { return _lozinka; }
               set { _lozinka = value; }
          }

          public FormaLoginKlasa(string novaKonekcija)
          {
               _stringKonekcije = novaKonekcija;
          }

          // Proverava direktno preko SPKorisnikDBKlasa
          public bool VazeciKorisnik()
          {
               // Koristimo SPKorisnikDBKlasa direktno, bez DBUtils
               SPKorisnikDBKlasa db = new SPKorisnikDBKlasa(_stringKonekcije);
               DataSet ds = db.DajKorisnikaPoKorisnickomImenuISifri(_email, _lozinka);

               return ds.Tables[0].Rows.Count > 0;
          }

          public string DajImePrezimeKorisnika()
          {
               SPKorisnikDBKlasa db = new SPKorisnikDBKlasa(_stringKonekcije);
               DataSet ds = db.DajKorisnikaPoKorisnickomImenuISifri(_email, _lozinka);

               if (ds.Tables[0].Rows.Count > 0)
               {
                    string ime = ds.Tables[0].Rows[0]["Ime"].ToString();
                    string prezime = ds.Tables[0].Rows[0]["Prezime"].ToString();
                    return $"{ime} {prezime}";
               }
               return "";
          }

          public int DajIdKorisnika()
          {
               SPKorisnikDBKlasa db = new SPKorisnikDBKlasa(_stringKonekcije);
               DataSet ds = db.DajKorisnikaPoKorisnickomImenuISifri(_email, _lozinka);

               if (ds.Tables[0].Rows.Count > 0)
               {
                    return Convert.ToInt32(ds.Tables[0].Rows[0]["KorisnikID"]);
               }
               return -1;
          }

          public string DajPozicijuKorisnika()
          {
               SPKorisnikDBKlasa db = new SPKorisnikDBKlasa(_stringKonekcije);
               DataSet ds = db.DajKorisnikaPoKorisnickomImenuISifri(_email, _lozinka);

               if (ds.Tables[0].Rows.Count > 0)
               {
                    return ds.Tables[0].Rows[0]["Pozicija"].ToString();
               }
               return "";
          }
     }
}