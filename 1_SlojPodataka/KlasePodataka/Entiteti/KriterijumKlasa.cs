using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlasePodataka.Entiteti
{
     /// <summary>
     /// Entitet koji predstavlja šifrarnik kriterijuma za ocenjivanje rada.
     /// Mapira se na tabelu 'Kriterijumi' u bazi.
     /// </summary>
     public class KriterijumKlasa
     {
          public int KriterijumID { get; set; } // Ispravljeno sa Id na KriterijumID
          public string Naziv { get; set; }
          public string Opis { get; set; }
     }
}