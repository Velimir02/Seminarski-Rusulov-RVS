using System;
using System.Data;
using KlasePodataka;           // Gde su SPEvidencijaRadaDBKlasa i SPKorisnikDBKlasa
using KlasePodataka.Entiteti;  // Gde su EvidencijaRadaKlasa, KriterijumKlasa...
using PoslovnaLogika;          // Gde je EvidencijaRadaPoslovnaLogika (validacija)

namespace PrezentacionaLogika
{
     /// <summary>
     /// Klasa koja služi za unos novih ocena/evidencija rada na formi.
     /// Povezuje UI direktno sa SP klasama i Poslovnom logikom.
     /// </summary>
     public class FormaEvidencijaUnosKlasa
     {
          private readonly string _stringKonekcije;

          public FormaEvidencijaUnosKlasa(string konekcija)
          {
               _stringKonekcije = konekcija;
          }

          /// <summary>
          /// Učitava kriterijume (šifrarnik) za padajući meni (npr. 'Timski rad', 'Produktivnost').
          /// Koristi SPEvidencijaRadaDBKlasa.
          /// </summary>
          public DataTable UcitajKriterijume()
          {
               // Instanciramo klasu koja radi sa SP i prosleđujemo string konekcije
               SPEvidencijaRadaDBKlasa db = new SPEvidencijaRadaDBKlasa(_stringKonekcije);

               // Metoda DajSveKriterijume vraća DataTable
               return db.DajSveKriterijume();
          }

          /// <summary>
          /// Učitava korisnike za padajući meni (kog radnika ocenjujemo).
          /// Koristi SPKorisnikDBKlasa.
          /// </summary>
          public DataTable UcitajKorisnike()
          {
               SPKorisnikDBKlasa db = new SPKorisnikDBKlasa(_stringKonekcije);
               return db.DajSveKorisnike();
          }

          /// <summary>
          /// Snima novu evidenciju (ocenu) uz proveru XML poslovnih pravila.
          /// </summary>
          public bool Snimi(int korisnikId, int kriterijumId, int ocena, string komentar, out string poruka)
          {
               // 1. Kreiranje objekta (DTO) sa podacima sa forme
               EvidencijaRadaKlasa novaEvidencija = new EvidencijaRadaKlasa
               {
                    KorisnikID = korisnikId,
                    KriterijumID = kriterijumId,
                    Ocena = ocena,
                    Komentar = komentar,
                    DatumUnosa = DateTime.Now // Upisujemo trenutno vreme
               };

               // 2. Validacija kroz Poslovnu Logiku (XML pravila)
               EvidencijaRadaPoslovnaLogika bl = new EvidencijaRadaPoslovnaLogika();

               // Ako validacija ne prođe (npr. dao je ocenu 1 a nije uneo komentar), vraća false i gresku
               if (!bl.ValidirajEvidenciju(novaEvidencija, out poruka))
               {
                    return false;
               }

               // 3. Ako je validacija prošla, zovemo SP klasu za upis u bazu
               SPEvidencijaRadaDBKlasa db = new SPEvidencijaRadaDBKlasa(_stringKonekcije);

               bool uspesno = db.DodajEvidenciju(novaEvidencija);

               if (!uspesno)
               {
                    poruka = "Greška pri upisu u bazu podataka (proverite konekciju ili podatke).";
                    return false;
               }

               // 4. Uspešno snimljeno - dodajemo info o preporučenoj akciji (iz BL/XML) za menadžera
               string akcija = bl.OdrediAkcijuZaOcenu(novaEvidencija.Ocena);
               poruka = $"Ocena je uspešno evidentirana.\nPreporuka sistema: {akcija}";

               return true;
          }
     }
}