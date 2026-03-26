using KlasePodataka;
using KlasePodataka.Entiteti;
using System;
using System.Data;

namespace PrezentacionaLogika
{
     public class FormaEvidencijaEditKlasa
     {
          private string _stringKonekcije; // konekcioni string za bazu

          // konstruktor – prima konekciju i čuva je za dalje
          public FormaEvidencijaEditKlasa(string konekcija)
          {
               _stringKonekcije = konekcija;
          }

          // vraća sve evidencije (pregled svih ocena)
          public DataTable DajSveEvidencije()
          {
               // Koristimo SPEvidencijaRadaDBKlasa koju smo ranije napravili
               return new SPEvidencijaRadaDBKlasa(_stringKonekcije).DajSveEvidencije();
          }

          // vraća evidencije/ocene samo za jednog specifičnog korisnika
          public DataTable DajEvidencijeZaKorisnika(int korisnikId)
          {
               // Napomena: Za ovo je potrebno dodati metodu 'DajEvidencijeZaKorisnika' u SPEvidencijaRadaDBKlasa
               // koja radi: SELECT * FROM EvidencijaRada WHERE KorisnikID = @KorisnikID
               return new SPEvidencijaRadaDBKlasa(_stringKonekcije).DajEvidencijeZaKorisnika(korisnikId);
          }

          // briše evidenciju po ID-u
          public bool Obrisi(int id)
          {
               return new SPEvidencijaRadaDBKlasa(_stringKonekcije).ObrisiEvidenciju(id);
          }

          // učitava sve korisnike (za padajući meni - kog radnika ocenjujemo)
          public DataTable UcitajKorisnike()
          {
               return new SPKorisnikDBKlasa(_stringKonekcije).DajSveKorisnike();
          }

          // učitava sve kriterijume iz šifrarnika (za padajući meni - npr. "Timski rad")
          public DataTable UcitajKriterijume()
          {
               // U ranijem kodu smo metodu za šifrarnik stavili u SPEvidencijaRadaDBKlasa
               return new SPEvidencijaRadaDBKlasa(_stringKonekcije).DajSveKriterijume();
          }

          // vraća jednu evidenciju po ID-u (za popunjavanje forme pre izmene)
          public EvidencijaRadaKlasa DajPoId(int id)
          {
               // Napomena: Potrebno je implementirati 'UcitajPoId' u SPEvidencijaRadaDBKlasa
               // koja vraća jedan popunjen objekat EvidencijaRadaKlasa na osnovu ID-ja
               return new SPEvidencijaRadaDBKlasa(_stringKonekcije).UcitajPoId(id);
          }

          // izmena postojeće evidencije (ocene)
          public bool Izmeni(int evidencijaId, int korisnikId, int kriterijumId, int ocena, string komentar)
          {
               // kreiramo objekat evidencije i punimo ga novim podacima sa forme
               EvidencijaRadaKlasa evidencija = new EvidencijaRadaKlasa
               {
                    EvidencijaID = evidencijaId,
                    KorisnikID = korisnikId,
                    KriterijumID = kriterijumId,
                    Ocena = ocena,
                    Komentar = komentar
                    // DatumUnosa se obično ne menja kod editovanja, ostaje onaj stari
               };

               // prosleđujemo objekat bazi na izmenu
               return new SPEvidencijaRadaDBKlasa(_stringKonekcije).IzmeniEvidenciju(evidencija);
          }
     }
}