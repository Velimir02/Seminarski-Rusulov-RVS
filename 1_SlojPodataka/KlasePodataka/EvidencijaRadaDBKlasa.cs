using System;
using System.Data;
using DBUtils;
using KlasePodataka.Entiteti;

namespace KlasePodataka
{
     public class EvidencijaRadaDBKlasa : TabelaKlasa
     {
          public EvidencijaRadaDBKlasa(KonekcijaKlasa konekcija, string nazivTabele)
              : base(konekcija, nazivTabele) { }

          public DataSet DajSveEvidencije()
          {
               return this.DajPodatke("SELECT * FROM EvidencijaRada");
          }

          public DataSet DajSveEvidencijePrikaz()
          {
               string upit = @"
                SELECT 
                    e.EvidencijaID, 
                    k.Ime + ' ' + k.Prezime AS Zaposleni, 
                    kr.Naziv AS Kriterijum, 
                    e.Ocena, 
                    e.Komentar, 
                    e.DatumUnosa 
                FROM EvidencijaRada e
                INNER JOIN Korisnik k ON e.KorisnikID = k.KorisnikID
                INNER JOIN Kriterijum kr ON e.KriterijumID = kr.KriterijumID";

               return this.DajPodatke(upit);
          }

          // Dodato "Klasa" u parametrima i velika "ID" slova
          public bool DodajEvidenciju(EvidencijaRadaKlasa e)
          {
               string upit = $"INSERT INTO EvidencijaRada (KorisnikID, KriterijumID, Ocena, Komentar) " +
                             $"VALUES ({e.KorisnikID}, {e.KriterijumID}, {e.Ocena}, '{e.Komentar}')";
               return this.IzvrsiAzuriranje(upit);
          }

          public bool IzmeniEvidenciju(EvidencijaRadaKlasa e)
          {
               string upit = $"UPDATE EvidencijaRada SET KorisnikID = {e.KorisnikID}, KriterijumID = {e.KriterijumID}, " +
                             $"Ocena = {e.Ocena}, Komentar = '{e.Komentar}' WHERE EvidencijaID = {e.EvidencijaID}";
               return this.IzvrsiAzuriranje(upit);
          }

          public bool ObrisiEvidenciju(int id)
          {
               string upit = $"DELETE FROM EvidencijaRada WHERE EvidencijaID = {id}";
               return this.IzvrsiAzuriranje(upit);
          }
     }
}