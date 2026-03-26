using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace DBUtils
{
    public class TabelaKlasa
    {
        /* CRC (Class Responsibility Collaboration) 
          * Responsibility - ODGOVORNOST: Rukovanje jednom tabelom baze podataka.
          * Collaboration - Saradjuje sa:
          *      - KonekcijaKlasa (za konekciju ka bazi),
          *      - SqlDataAdapter (za komunikaciju sa bazom),
          *      - DataSet (za skladištenje podataka u memoriji).
        */

        #region Atributi

        private string _nazivTabele;                 // Ime tabele u bazi
        private KonekcijaKlasa _konekcijaObjekat;    // Objekat konekcije
        private SqlDataAdapter _adapterObjekat;      // Adapter za SELECT/INSERT/UPDATE/DELETE
        private System.Data.DataSet _dataSetObjekat; // Dataset gde se drže podaci iz baze

        #endregion

        #region Konstruktor

        // Konstruktor prima objekat konekcije i ime tabele kojom upravlja
        public TabelaKlasa(KonekcijaKlasa novaKonekcija, string noviNazivTabele)
        {
            _konekcijaObjekat = novaKonekcija;
            _nazivTabele = noviNazivTabele;
        }

        #endregion

        #region Privatne metode

        // Kreira adapter i postavlja komande za SELECT, INSERT, DELETE, UPDATE
        private void KreirajAdapter(string selectUpit, string insertUpit, string deleteUpit, string updateUpit)
        {
            SqlCommand pomSelectKomanda, pomInsertKomanda, pomDeleteKomanda, pomUpdateKomanda;

            pomSelectKomanda = new SqlCommand();
            pomSelectKomanda.CommandText = selectUpit;
            pomSelectKomanda.Connection = _konekcijaObjekat.DajKonekciju();

            pomInsertKomanda = new SqlCommand();
            pomInsertKomanda.CommandText = insertUpit;
            pomInsertKomanda.Connection = _konekcijaObjekat.DajKonekciju();

            pomDeleteKomanda = new SqlCommand();
            pomDeleteKomanda.CommandText = deleteUpit;
            pomDeleteKomanda.Connection = _konekcijaObjekat.DajKonekciju();

            pomUpdateKomanda = new SqlCommand();
            pomUpdateKomanda.CommandText = updateUpit;
            pomUpdateKomanda.Connection = _konekcijaObjekat.DajKonekciju();

            // Povezujemo adapter sa komandama
            _adapterObjekat = new SqlDataAdapter();
            _adapterObjekat.SelectCommand = pomSelectKomanda;
            _adapterObjekat.InsertCommand = pomInsertKomanda;
            _adapterObjekat.UpdateCommand = pomUpdateKomanda;
            _adapterObjekat.DeleteCommand = pomDeleteKomanda;
        }

        // Kreira dataset i puni ga podacima iz baze preko adaptera
        private void KreirajDataset()
        {
            _dataSetObjekat = new System.Data.DataSet();
            _adapterObjekat.Fill(_dataSetObjekat, _nazivTabele);
        }

        // Gasi adapter i dataset (čišćenje resursa)
        private void ZatvoriAdapterDataset()
        {
            _adapterObjekat.Dispose();
            _dataSetObjekat.Dispose();
        }

        #endregion

        #region Javne metode

        // Čitanje podataka iz baze (SELECT)
        public DataSet DajPodatke(string selectUpit)
        {
            KreirajAdapter(selectUpit, "", "", "");
            KreirajDataset();
            return _dataSetObjekat;
        }

        // Vraća broj redova iz poslednjeg SELECT-a
        public int DajBrojSlogova()
        {
            int BrojSlogova = _dataSetObjekat.Tables[0].Rows.Count;
            return BrojSlogova;
        }

        // Izvršava jedan SQL upit (INSERT/UPDATE/DELETE) u transakciji
        public bool IzvrsiAzuriranje(string Upit)
        {
            bool uspeh = false;
            SqlConnection pomKonekcija;
            SqlCommand pomKomanda;
            SqlTransaction pomTransakcija = null;
            try
            {
                pomKonekcija = _konekcijaObjekat.DajKonekciju();

                // Kreiramo komandu i dodeljujemo transakciju
                pomKomanda = pomKonekcija.CreateCommand();
                pomTransakcija = pomKonekcija.BeginTransaction();
                pomKomanda.Transaction = pomTransakcija;
                pomKomanda.CommandText = Upit;

                // Izvršavamo upit
                pomKomanda.ExecuteNonQuery();
                pomTransakcija.Commit();
                uspeh = true;
            }
            catch
            {
                // Ako dođe do greške vraćamo nazad transakciju
                pomTransakcija?.Rollback();
                uspeh = false;
            }
            return uspeh;
        }

        // Izvršava više SQL upita (INSERT/UPDATE/DELETE) u jednoj transakciji
        public bool IzvrsiAzuriranje(List<string> listaUpita)
        {
            bool uspeh = false;
            SqlConnection pomKonekcija;
            SqlCommand pomKomanda;
            SqlTransaction pomTransakcija = null;
            try
            {
                pomKonekcija = _konekcijaObjekat.DajKonekciju();
                pomKomanda = pomKonekcija.CreateCommand();

                pomTransakcija = pomKonekcija.BeginTransaction();
                pomKomanda.Transaction = pomTransakcija;

                // Izvršava sve upite jedan po jedan
                foreach (string pomUpit in listaUpita)
                {
                    pomKomanda.CommandText = pomUpit;
                    pomKomanda.ExecuteNonQuery();
                }

                pomTransakcija.Commit();
                uspeh = true;
            }
            catch
            {
                // Ako padne ijedan upit – rollback cele transakcije
                pomTransakcija?.Rollback();
                uspeh = false;
            }
            return uspeh;
        }

        #endregion
    }
}
