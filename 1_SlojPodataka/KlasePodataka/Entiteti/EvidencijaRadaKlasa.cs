using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlasePodataka.Entiteti
{
     /// <summary>
     /// Entitet koji predstavlja konkretnu ocenu ili evidenciju o radu zaposlenog.
     /// Mapira se na glavnu tabelu 'EvidencijeRada' u bazi.
     /// </summary>
     public class EvidencijaRadaKlasa
     {
          public int EvidencijaID { get; set; } // Ispravljeno
          public int KorisnikID { get; set; }   // Ispravljeno (veliko D)
          public int KriterijumID { get; set; } // Ispravljeno (veliko D)
          public int Ocena { get; set; }
          public string Komentar { get; set; }
          public DateTime DatumUnosa { get; set; }
     }
}