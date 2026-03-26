using System;
using System.Linq;
using System.Web.Services;
using System.Xml.Linq;

namespace WebServis
{
     /// <summary>
     /// DTO klasa koja prebacuje pravila ocenjivanja (iz XML-a ka aplikaciji).
     /// </summary>
     public class PraviloOcenjivanjaDto
     {
          public int Ocena { get; set; }       // Npr. 1, 2, 3...
          public string Akcija { get; set; }   // Npr. "HR_Upozorenje"
          public string Opis { get; set; }     // Poruka šta ta ocena znači
     }

     /// <summary>
     /// Web servis koji čita poslovna pravila i ograničenja iz XML fajla.
     /// Centralizovano mesto za validaciju Evidencije Rada, Korisnika i Kriterijuma.
     /// </summary>
     [WebService(Namespace = "http://softverprojekat.rs/")]
     [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
     public class OgranicenjaServis : WebService
     {
          // Putanja do XML fajla (fajl nazovi OgranicenjaPracenjeRada.xml i stavi u XML folder)
          private string XmlPutanja => Server.MapPath("~/XML/Ogranicenja.xml");

          #region Helpers (Pomoćne metode za čitanje XML-a)

          private static XDocument LoadXml(string path) => XDocument.Load(path);

          private static string ReadScalar(XDocument doc, params string[] path)
          {
               XElement cur = doc.Root;
               foreach (var p in path)
                    cur = cur?.Element(p);
               return cur?.Value ?? string.Empty;
          }

          private static bool ReadBool(XDocument doc, params string[] path)
          {
               string val = ReadScalar(doc, path);
               return !string.IsNullOrEmpty(val) && bool.Parse(val);
          }

          private static int ReadInt(XDocument doc, params string[] path)
          {
               string val = ReadScalar(doc, path);
               return string.IsNullOrEmpty(val) ? 0 : int.Parse(val);
          }

          #endregion

          // ===========================
          //  VALIDACIJA EVIDENCIJE RADA
          // ===========================

          [WebMethod(Description = "Vraća minimalnu dozvoljenu ocenu.")]
          public int OcenaMin()
          {
               var doc = LoadXml(XmlPutanja);
               return ReadInt(doc, "EvidencijaRada", "OcenaMin");
          }

          [WebMethod(Description = "Vraća maksimalnu dozvoljenu ocenu.")]
          public int OcenaMax()
          {
               var doc = LoadXml(XmlPutanja);
               return ReadInt(doc, "EvidencijaRada", "OcenaMax");
          }

          [WebMethod(Description = "Poruka ako ocena nije u dozvoljenom rasponu.")]
          public string PorukaOcena()
          {
               var doc = LoadXml(XmlPutanja);
               return ReadScalar(doc, "EvidencijaRada", "PorukaOcena");
          }

          [WebMethod(Description = "Da li treba zabraniti odabir datuma u budućnosti za unos ocene?")]
          public bool ZabraniBuduciDatum()
          {
               var doc = LoadXml(XmlPutanja);
               return ReadBool(doc, "EvidencijaRada", "ZabraniBuduciDatum");
          }

          [WebMethod(Description = "Vraća poruku greške ako je izabran datum u budućnosti.")]
          public string PorukaDatum()
          {
               var doc = LoadXml(XmlPutanja);
               return ReadScalar(doc, "EvidencijaRada", "PorukaDatum");
          }

          [WebMethod(Description = "Da li je obavezno uneti komentar za loše ocene?")]
          public bool KomentarObavezanZaLoshuOcenu()
          {
               var doc = LoadXml(XmlPutanja);
               return ReadBool(doc, "EvidencijaRada", "KomentarObavezanZaLoshuOcenu");
          }

          [WebMethod(Description = "Prag ispod koga se ocena smatra lošom i zahteva komentar.")]
          public int PragLoseOcene()
          {
               var doc = LoadXml(XmlPutanja);
               return ReadInt(doc, "EvidencijaRada", "PragLoseOcene");
          }

          [WebMethod(Description = "Poruka za obavezan komentar kod loših ocena.")]
          public string PorukaKomentar()
          {
               var doc = LoadXml(XmlPutanja);
               return ReadScalar(doc, "EvidencijaRada", "PorukaKomentar");
          }

          [WebMethod(Description = "Vraća listu pravila za značenje svake ocene i akcije menadžera.")]
          public PraviloOcenjivanjaDto[] DajPravilaOcenjivanja()
          {
               var doc = LoadXml(XmlPutanja);

               var lista = doc.Root?
                   .Element("EvidencijaRada")?
                   .Element("PravilaOcenjivanja")?
                   .Elements("Pravilo")
                   .Select(x => new PraviloOcenjivanjaDto
                   {
                        Ocena = int.Parse(x.Attribute("ocena")?.Value ?? "0"),
                        Akcija = x.Attribute("akcija")?.Value ?? "",
                        Opis = (x.Value ?? "").Trim()
                   })
                   .ToArray();

               return lista ?? new PraviloOcenjivanjaDto[0];
          }

          // ===========================
          //  VALIDACIJA KORISNIKA
          // ===========================

          [WebMethod(Description = "Minimalna dužina lozinke koju sistem zahteva.")]
          public int MinDuzinaLozinke()
          {
               var doc = LoadXml(XmlPutanja);
               return ReadInt(doc, "Korisnik", "MinDuzinaLozinke");
          }

          [WebMethod(Description = "Poruka greške za nevalidnu lozinku.")]
          public string PorukaLozinka()
          {
               var doc = LoadXml(XmlPutanja);
               return ReadScalar(doc, "Korisnik", "PorukaLozinka");
          }

          [WebMethod(Description = "Da li je kompanijski email obavezan?")]
          public bool EmailObavezan()
          {
               var doc = LoadXml(XmlPutanja);
               return ReadBool(doc, "Korisnik", "EmailObavezan");
          }

          [WebMethod(Description = "Vraća dozvoljeni domen za email, npr. @firma.rs")]
          public string DozvoljenDomen()
          {
               var doc = LoadXml(XmlPutanja);
               return ReadScalar(doc, "Korisnik", "DozvoljenDomen");
          }

          [WebMethod(Description = "Poruka ukoliko email ne ispunjava pravilo domena.")]
          public string PorukaEmail()
          {
               var doc = LoadXml(XmlPutanja);
               return ReadScalar(doc, "Korisnik", "PorukaEmail");
          }

          // ===========================
          //  VALIDACIJA KRITERIJUMA
          // ===========================

          [WebMethod(Description = "Maksimalna dozvoljena dužina naziva kriterijuma.")]
          public int MaxDuzinaNaziva()
          {
               var doc = LoadXml(XmlPutanja);
               return ReadInt(doc, "Kriterijum", "MaxDuzinaNaziva");
          }

          [WebMethod(Description = "Da li je opis kriterijuma obavezno polje?")]
          public bool OpisObavezan()
          {
               var doc = LoadXml(XmlPutanja);
               return ReadBool(doc, "Kriterijum", "OpisObavezan");
          }

          [WebMethod(Description = "Poruka za predugačak naziv kriterijuma.")]
          public string PorukaNaziv()
          {
               var doc = LoadXml(XmlPutanja);
               return ReadScalar(doc, "Kriterijum", "PorukaNaziv");
          }
     }
}