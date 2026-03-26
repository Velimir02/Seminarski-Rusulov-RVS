using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using KlasePodataka;
using KlasePodataka.Entiteti;
using PoslovnaLogika; // Tu se nalazi logika za određivanje akcija na osnovu ocene

namespace PrezentacionaLogika
{
     public class FormaEvidencijaStampaKlasa
     {
          private readonly string _stringKonekcije;

          public FormaEvidencijaStampaKlasa(string konekcija)
          {
               _stringKonekcije = konekcija;
          }

          /// <summary>
          /// Vraća sve evidencije (ocene), uz mogućnost filtriranja po Imenu radnika, Kriterijumu ili Komentaru.
          /// </summary>
          public DataTable DajEvidencije(string filter = "")
          {
               // Naša SP klasa za Evidenciju direktno vraća DataTable
               DataTable tabela = new SPEvidencijaRadaDBKlasa(_stringKonekcije).DajSveEvidencije();

               if (tabela == null || tabela.Rows.Count == 0) return new DataTable();

               // Ako postoji filter string, radimo pretragu unutar RAM memorije (LINQ)
               if (!string.IsNullOrWhiteSpace(filter))
               {
                    // Proveravamo da li kolone postoje (ovo su alijasi koje smo napravili u SQL JOIN-u)
                    bool imaZaposleni = tabela.Columns.Contains("Zaposleni");
                    bool imaKriterijum = tabela.Columns.Contains("Kriterijum");
                    bool imaKomentar = tabela.Columns.Contains("Komentar");

                    var filtrirani = tabela.AsEnumerable()
                        .Where(r =>
                            (imaZaposleni && (r.Field<string>("Zaposleni") ?? "").IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                            (imaKriterijum && (r.Field<string>("Kriterijum") ?? "").IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                            (imaKomentar && (r.Field<string>("Komentar") ?? "").IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
                        );

                    return filtrirani.Any() ? filtrirani.CopyToDataTable() : tabela.Clone();
               }

               return tabela;
          }

          /// <summary>
          /// Vraća evidencije/ocene samo za određenog zaposlenog.
          /// </summary>
          public DataTable DajEvidencijeZaKorisnika(int korisnikId)
          {
               return new SPEvidencijaRadaDBKlasa(_stringKonekcije).DajEvidencijeZaKorisnika(korisnikId);
          }

          /// <summary>
          /// Post-proces: Prolazi kroz učitanu tabelu i dodaje novu kolonu "PreporucenaAkcija".
          /// Tekst se povlači iz Poslovne Logike (XML-a) na osnovu toga da li je radnik dobio 1, 3 ili 5.
          /// Pozovi ovo pre nego što vežeš tabelu za Grid (DataBind).
          /// </summary>
          public void PrimeniAkcijeNaTabelu(DataTable tabela)
          {
               if (tabela == null || tabela.Rows.Count == 0) return;

               var bl = new EvidencijaRadaPoslovnaLogika();

               // 1. Dodajemo novu kolonu za prikaz u Grid-u/Izveštaju
               if (!tabela.Columns.Contains("PreporucenaAkcija"))
                    tabela.Columns.Add("PreporucenaAkcija", typeof(string));

               // 2. Iteriramo kroz svaki red i računamo akciju
               foreach (DataRow r in tabela.Rows)
               {
                    // Izvlačimo ocenu sigurno pomoću Helper metode
                    int ocena = SafeInt(r, "Ocena");

                    if (ocena > 0)
                    {
                         // 3. Pozivamo BL da izvuče značenje ocene (npr. 5 -> "Predlog za bonus")
                         string akcija = bl.OdrediAkcijuZaOcenu(ocena);
                         r["PreporucenaAkcija"] = akcija;
                    }
                    else
                    {
                         r["PreporucenaAkcija"] = "Nema ocene";
                    }
               }
          }

          #region Helpers (Pomoćne metode za bezbedno čitanje iz DataRow-a)

          private static int SafeInt(DataRow r, params string[] cols)
          {
               foreach (var c in cols)
                    if (r.Table.Columns.Contains(c) && r[c] != DBNull.Value)
                         return Convert.ToInt32(r[c]);
               return 0;
          }

          private static decimal SafeDecimal(DataRow r, params string[] cols)
          {
               foreach (var c in cols)
                    if (r.Table.Columns.Contains(c) && r[c] != DBNull.Value)
                         try { return Convert.ToDecimal(r[c]); } catch { /* ignore */ }
               return 0m;
          }

          private static DateTime SafeDate(DataRow r, params string[] cols)
          {
               foreach (var c in cols)
                    if (r.Table.Columns.Contains(c) && r[c] != DBNull.Value)
                         try { return Convert.ToDateTime(r[c]); } catch { /* ignore */ }
               return DateTime.MinValue;
          }

          private static string SafeString(DataRow r, params string[] cols)
          {
               foreach (var c in cols)
                    if (r.Table.Columns.Contains(c) && r[c] != DBNull.Value)
                         return r[c].ToString();
               return string.Empty;
          }

          #endregion
     }
}