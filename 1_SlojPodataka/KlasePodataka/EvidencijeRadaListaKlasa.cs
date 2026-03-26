using System.Collections.Generic;
using KlasePodataka.Entiteti;

namespace KlasePodataka
{
     public class EvidencijeRadaListaKlasa
     {
          private List<EvidencijaRadaKlasa> _listaEvidencija;

          public List<EvidencijaRadaKlasa> ListaEvidencija
          {
               get { return _listaEvidencija; }
               set { if (_listaEvidencija != value) _listaEvidencija = value; }
          }

          public EvidencijeRadaListaKlasa() { _listaEvidencija = new List<EvidencijaRadaKlasa>(); }

          public void DodajEvidenciju(EvidencijaRadaKlasa e) { _listaEvidencija.Add(e); }
          public void ObrisiEvidenciju(EvidencijaRadaKlasa e) { _listaEvidencija.Remove(e); }
          public void ObrisiNaPoziciji(int index) { _listaEvidencija.RemoveAt(index); }

          public void IzmeniEvidenciju(EvidencijaRadaKlasa stara, EvidencijaRadaKlasa nova)
          {
               int i = _listaEvidencija.IndexOf(stara);
               if (i >= 0)
               {
                    _listaEvidencija.RemoveAt(i);
                    _listaEvidencija.Insert(i, nova);
               }
          }
     }
}