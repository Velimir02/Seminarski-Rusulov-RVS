using System.Collections.Generic;
using KlasePodataka.Entiteti;

namespace KlasePodataka
{
     public class KriterijumiListaKlasa
     {
          private List<KriterijumKlasa> _listaKriterijuma;

          public List<KriterijumKlasa> ListaKriterijuma
          {
               get { return _listaKriterijuma; }
               set { if (_listaKriterijuma != value) _listaKriterijuma = value; }
          }

          public KriterijumiListaKlasa() { _listaKriterijuma = new List<KriterijumKlasa>(); }

          public void DodajKriterijum(KriterijumKlasa k) { _listaKriterijuma.Add(k); }
          public void ObrisiKriterijum(KriterijumKlasa k) { _listaKriterijuma.Remove(k); }
          public void ObrisiNaPoziciji(int index) { _listaKriterijuma.RemoveAt(index); }

          public void IzmeniKriterijum(KriterijumKlasa stari, KriterijumKlasa novi)
          {
               int i = _listaKriterijuma.IndexOf(stari);
               if (i >= 0)
               {
                    _listaKriterijuma.RemoveAt(i);
                    _listaKriterijuma.Insert(i, novi);
               }
          }
     }
}