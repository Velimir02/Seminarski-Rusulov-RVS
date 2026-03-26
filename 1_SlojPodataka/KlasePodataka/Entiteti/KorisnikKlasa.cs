using System;

namespace KlasePodataka.Entiteti
{
     /// <summary>
     /// Entitet koji predstavlja zaposlenog ili menadžera.
     /// Mapira se na tabelu 'Korisnik' u bazi.
     /// </summary>
     public class KorisnikKlasa
     {
          public int KorisnikID { get; set; } // Ispravljeno sa Id na KorisnikID
          public string Ime { get; set; }
          public string Prezime { get; set; }
          public string Email { get; set; }
          public string Lozinka { get; set; }
          public string Pozicija { get; set; }
     }
}