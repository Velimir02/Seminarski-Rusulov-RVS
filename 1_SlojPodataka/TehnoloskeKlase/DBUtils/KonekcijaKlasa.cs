using System;
using System.Data.SqlClient;

namespace DBUtils
{
    /// <summary>
    /// Klasa za upravljanje konekcijom prema SQL Server bazi podataka.
    /// Omogućava otvaranje, zatvaranje i dobijanje aktivne konekcije.
    /// </summary>
    public class KonekcijaKlasa
    {
        private SqlConnection _konekcija;       // Objekt konekcije prema SQL Serveru
        private string _putanjaBaze;            // Putanja do .mdf fajla (ako se koristi attach)
        private string _nazivBaze;              // Naziv baze
        private string _nazivDBMSinstance;      // Naziv SQL Server instance
        private string _stringKonekcije;        // Connection string (ako je prosleđen direktno)

        // =============================
        // KONSTRUKTORI
        // =============================

        /// <summary>
        /// Konstruktor koji prima parametre za instancu, putanju i ime baze.
        /// </summary>
        public KonekcijaKlasa(string nazivDBMSInstance, string putanjaBaze, string nazivBaze)
        {
            _putanjaBaze = putanjaBaze;
            _nazivBaze = nazivBaze;
            _nazivDBMSinstance = nazivDBMSInstance;
            _stringKonekcije = "";
        }

        /// <summary>
        /// Konstruktor koji prima ceo connection string.
        /// </summary>
        public KonekcijaKlasa(string noviStringKonekcije)
        {
            _putanjaBaze = "";
            _nazivBaze = "";
            _nazivDBMSinstance = "";
            _stringKonekcije = noviStringKonekcije;
        }

        // =============================
        // PRIVATNE METODE
        // =============================

        /// <summary>
        /// Dinamički pravi connection string u zavisnosti od parametara konstruktora.
        /// Ako je već prosleđen pun connection string – vraća njega.
        /// </summary>
        private string DajStringKonekcije()
        {
            string pomStringKonekcije;

            // Ako nemamo ručno zadat connection string
            if (string.IsNullOrEmpty(_stringKonekcije))
            {
                // Ako baza nije u fajlu (koristimo SQL Server bazu po imenu)
                if (string.IsNullOrEmpty(_putanjaBaze))
                {
                    pomStringKonekcije =
                        "Data Source=" + _nazivDBMSinstance +
                        ";Initial Catalog=" + _nazivBaze +
                        ";Integrated Security=True";
                }
                else
                {
                    // Ako se baza attach-uje iz fajla (.mdf)
                    pomStringKonekcije =
                        "Data Source=.\\"
                        + _nazivDBMSinstance +
                        ";AttachDbFilename=" + _putanjaBaze + "\\" + _nazivBaze +
                        ";Integrated Security=True;Connect Timeout=30;User Instance=True";
                }
            }
            else
            {
                // Ako smo dobili ceo connection string direktno
                pomStringKonekcije = _stringKonekcije;
            }

            return pomStringKonekcije;
        }

        // =============================
        // JAVNE METODE
        // =============================

        /// <summary>
        /// Otvara konekciju prema bazi. Vraća true ako je uspešno, false ako je došlo do greške.
        /// </summary>
        public bool OtvoriKonekciju()
        {
            bool uspeh;
            _konekcija = new SqlConnection();
            _konekcija.ConnectionString = DajStringKonekcije();

            try
            {
                _konekcija.Open();
                uspeh = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška pri otvaranju konekcije: " + ex.Message);
                uspeh = false;
            }

            return uspeh;
        }

        /// <summary>
        /// Vraća aktivnu SqlConnection instancu.
        /// </summary>
        public SqlConnection DajKonekciju()
        {
            return _konekcija;
        }

        /// <summary>
        /// Zatvara i oslobađa konekciju ako je trenutno otvorena.
        /// </summary>
        public void ZatvoriKonekciju()
        {
            if (_konekcija != null && _konekcija.State == System.Data.ConnectionState.Open)
            {
                _konekcija.Close();
                _konekcija.Dispose();
            }
        }
    }
}
