using System;
using System.Data;
using DBUtils;
using KlasePodataka.Entiteti;

namespace KlasePodataka
{
     public class KriterijumDBKlasa : TabelaKlasa
     {
          public KriterijumDBKlasa(KonekcijaKlasa konekcija, string nazivTabele)
              : base(konekcija, nazivTabele) { }

          public DataSet DajSveKriterijume()
          {
               return this.DajPodatke("SELECT * FROM Kriterijum");
          }

          public bool DodajKriterijum(KriterijumKlasa k)
          {
               string upit = $"INSERT INTO Kriterijum (Naziv, Opis) VALUES ('{k.Naziv}', '{k.Opis}')";
               return this.IzvrsiAzuriranje(upit);
          }

          public bool IzmeniKriterijum(KriterijumKlasa k)
          {
               // Ovde sada pravilno gađamo k.KriterijumID
               string upit = $"UPDATE Kriterijum SET Naziv = '{k.Naziv}', Opis = '{k.Opis}' WHERE KriterijumID = {k.KriterijumID}";
               return this.IzvrsiAzuriranje(upit);
          }

          public bool ObrisiKriterijum(int id)
          {
               string upit = $"DELETE FROM Kriterijum WHERE KriterijumID = {id}";
               return this.IzvrsiAzuriranje(upit);
          }
     }
}