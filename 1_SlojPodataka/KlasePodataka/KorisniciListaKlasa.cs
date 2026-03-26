using System.Collections.Generic;
using KlasePodataka.Entiteti;

namespace KlasePodataka
{
     public class KorisniciListaKlasa
     {
          private List<KorisnikKlasa> _listaKorisnika;

          public List<KorisnikKlasa> ListaKorisnika
          {
               get { return _listaKorisnika; }
               set { if (_listaKorisnika != value) _listaKorisnika = value; }
          }

          public KorisniciListaKlasa() { _listaKorisnika = new List<KorisnikKlasa>(); }

          public void DodajKorisnika(KorisnikKlasa k) { _listaKorisnika.Add(k); }
          public void ObrisiKorisnika(KorisnikKlasa k) { _listaKorisnika.Remove(k); }
          public void ObrisiNaPoziciji(int index) { _listaKorisnika.RemoveAt(index); }

          public void IzmeniKorisnika(KorisnikKlasa stari, KorisnikKlasa novi)
          {
               int i = _listaKorisnika.IndexOf(stari);
               if (i >= 0)
               {
                    _listaKorisnika.RemoveAt(i);
                    _listaKorisnika.Insert(i, novi);
               }
          }
     }
}