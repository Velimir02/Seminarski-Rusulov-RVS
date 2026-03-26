using System;
using System.Data;
using DBUtils;
using KlasePodataka.Entiteti;

namespace KlasePodataka
{
     public class KorisnikDBKlasa : TabelaKlasa
     {
          public KorisnikDBKlasa(KonekcijaKlasa konekcija, string nazivTabele)
              : base(konekcija, nazivTabele) { }

          public DataSet DajSveKorisnike()
          {
               return this.DajPodatke("SELECT * FROM Korisnik");
          }

          public DataSet ProveraKorisnika(string email, string lozinka)
          {
               string upit = $"SELECT * FROM Korisnik WHERE Email = '{email}' AND Lozinka = '{lozinka}'";
               return this.DajPodatke(upit);
          }

          public bool DodajKorisnika(KorisnikKlasa k)
          {
               string upit = $"INSERT INTO Korisnik (Ime, Prezime, Email, Lozinka, Pozicija) " +
                             $"VALUES ('{k.Ime}', '{k.Prezime}', '{k.Email}', '{k.Lozinka}', '{k.Pozicija}')";
               return this.IzvrsiAzuriranje(upit);
          }

          public bool IzmeniKorisnika(KorisnikKlasa k)
          {
               // Ovde sada pravilno gađamo k.KorisnikID (kako piše u Entitetu)
               string upit = $"UPDATE Korisnik SET Ime = '{k.Ime}', Prezime = '{k.Prezime}', " +
                             $"Email = '{k.Email}', Lozinka = '{k.Lozinka}', Pozicija = '{k.Pozicija}' " +
                             $"WHERE KorisnikID = {k.KorisnikID}";
               return this.IzvrsiAzuriranje(upit);
          }

          public bool ObrisiKorisnika(int id)
          {
               string upit = $"DELETE FROM Korisnik WHERE KorisnikID = {id}";
               return this.IzvrsiAzuriranje(upit);
          }
     }
}