using KlasePodataka.Entiteti;
using PoslovnaLogika.WSOgranicenja; // Zameni sa stvarnim namespace-om gde ti je servis
using System;
using System.Linq;

namespace PoslovnaLogika
{
     public class EvidencijaRadaPoslovnaLogika
     {
          /// <summary>
          /// Glavna validacija pre unosa ili izmene ocene (Evidencije Rada).
          /// Proverava raspon ocena, datume i da li je komentar obavezan za loše ocene.
          /// </summary>
          public bool ValidirajEvidenciju(EvidencijaRadaKlasa evidencija, out string poruka)
          {
               var servis = new OgranicenjaServis();
               poruka = "";

               // 1) Provera raspona ocene (npr. da li je između 1 i 5)
               int minOcena = servis.OcenaMin();
               int maxOcena = servis.OcenaMax();

               if (evidencija.Ocena < minOcena || evidencija.Ocena > maxOcena)
               {
                    poruka = servis.PorukaOcena();
                    return false;
               }

               // 2) Provera datuma (da li menadžer pokušava da oceni radnika u budućnosti)
               if (servis.ZabraniBuduciDatum() && evidencija.DatumUnosa.Date > DateTime.Now.Date)
               {
                    poruka = servis.PorukaDatum();
                    return false;
               }

               // 3) Provera obaveznog komentara za loše ocene
               if (servis.KomentarObavezanZaLoshuOcenu())
               {
                    int prag = servis.PragLoseOcene(); // npr. ocene 1 i 2
                    if (evidencija.Ocena <= prag && string.IsNullOrWhiteSpace(evidencija.Komentar))
                    {
                         poruka = servis.PorukaKomentar();
                         return false;
                    }
               }

               return true;
          }

          /// <summary>
          /// Validacija pri kreiranju novog zaposlenog (Korisnika).
          /// Proverava dužinu lozinke i pravila za email domen.
          /// </summary>
          public bool ValidirajKorisnika(KorisnikKlasa korisnik, out string poruka)
          {
               var servis = new OgranicenjaServis();
               poruka = "";

               // 1) Validacija dužine lozinke
               int minDuzina = servis.MinDuzinaLozinke();
               if (string.IsNullOrEmpty(korisnik.Lozinka) || korisnik.Lozinka.Length < minDuzina)
               {
                    poruka = servis.PorukaLozinka();
                    return false;
               }

               // 2) Validacija Email domena (npr. mora biti @firma.rs)
               if (servis.EmailObavezan())
               {
                    if (string.IsNullOrWhiteSpace(korisnik.Email))
                    {
                         poruka = "Email adresa je obavezna.";
                         return false;
                    }

                    string dozvoljenDomen = servis.DozvoljenDomen(); // Npr. "@firma.rs"
                    if (!korisnik.Email.EndsWith(dozvoljenDomen, StringComparison.OrdinalIgnoreCase))
                    {
                         poruka = servis.PorukaEmail();
                         return false;
                    }
               }

               return true;
          }

          /// <summary>
          /// Validacija pri unosu novog kriterijuma za ocenjivanje (Šifrarnik).
          /// </summary>
          public bool ValidirajKriterijum(KriterijumKlasa kriterijum, out string poruka)
          {
               var servis = new OgranicenjaServis();
               poruka = "";

               // 1) Provera maksimalne dužine naziva
               int maxDuzina = servis.MaxDuzinaNaziva();
               if (string.IsNullOrWhiteSpace(kriterijum.Naziv) || kriterijum.Naziv.Length > maxDuzina)
               {
                    poruka = servis.PorukaNaziv();
                    return false;
               }

               // 2) Provera da li je opis obavezan
               if (servis.OpisObavezan() && string.IsNullOrWhiteSpace(kriterijum.Opis))
               {
                    poruka = "Opis kriterijuma je obavezan prema pravilima sistema.";
                    return false;
               }

               return true;
          }

          /// <summary>
          /// Pomoćna metoda koja na osnovu unete ocene vraća akciju koju menadžer 
          /// treba da preduzme (čita iz XML-a, npr. Ocena 1 -> HR Upozorenje).
          /// </summary>
          public string OdrediAkcijuZaOcenu(int ocena)
          {
               var servis = new OgranicenjaServis();
               var pravila = servis.DajPravilaOcenjivanja();

               if (pravila == null || pravila.Length == 0)
                    return "Nema definisanih akcija.";

               var praviloZaOcenu = pravila.FirstOrDefault(p => p.Ocena == ocena);

               if (praviloZaOcenu != null)
               {
                    // Vraćamo formatiran string: Akcija - Opis
                    return $"{praviloZaOcenu.Akcija}: {praviloZaOcenu.Opis}";
               }

               return "Nepoznata ocena.";
          }
     }
}