using KlasePodataka.Entiteti;
using System;
using System.Data;
using System.Data.SqlClient;

namespace KlasePodataka
{
     /// <summary>
     /// SPEvidencijaRadaDBKlasa
     /// ----------------------------------------------
     /// CRC (Class–Responsibility–Collaborators)
     /// Responsibilities:
     ///   - Pristup podacima za glavnu tabelu 'EvidencijaRada' (unos, izmena, brisanje, prikaz).
     ///   - Učitavanje šifrarnika iz tabele 'Kriterijum' (za padajuće liste).
     /// Collaborators:
     ///   - SQL Server (ADO.NET: SqlConnection, SqlCommand, SqlDataAdapter).
     ///   - Konsumiraju je slojevi PrezentacionaLogika / Kontroleri za prikaz ocena.
     /// Notes:
     ///   - Klasa ne čuva stanje; bezbedno je kreirati novu instancu po potrebi.
     /// </summary>
     public class SPEvidencijaRadaDBKlasa
     {
          private readonly string _stringKonekcije;

          public SPEvidencijaRadaDBKlasa(string noviStringKonekcije)
          {
               _stringKonekcije = noviStringKonekcije;
          }

          #region Kriterijum (Šifrarnik)

          public DataTable DajSveKriterijume()
          {
               DataTable tabela = new DataTable();
               using (SqlConnection conn = new SqlConnection(_stringKonekcije))
               {
                    string upit = "SELECT KriterijumID, Naziv FROM Kriterijum";
                    SqlCommand cmd = new SqlCommand(upit, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(tabela);
               }
               return tabela;
          }

          #endregion

          #region Evidencija Rada (Glavna tabela - CRUD)

          public DataTable DajSveEvidencije()
          {
               DataTable tabela = new DataTable();
               using (SqlConnection conn = new SqlConnection(_stringKonekcije))
               {
                    string upit = @"
                    SELECT 
                        e.EvidencijaID, 
                        k.Ime + ' ' + k.Prezime AS Zaposleni, 
                        kr.Naziv AS Kriterijum, 
                        e.Ocena, 
                        e.Komentar, 
                        e.DatumUnosa 
                    FROM EvidencijaRada e
                    INNER JOIN Korisnik k ON e.KorisnikID = k.KorisnikID
                    INNER JOIN Kriterijum kr ON e.KriterijumID = kr.KriterijumID
                    ORDER BY e.DatumUnosa DESC";

                    SqlCommand cmd = new SqlCommand(upit, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(tabela);
               }
               return tabela;
          }

          // DODATO: Vraća evidencije samo za jednog specifičnog korisnika
          public DataTable DajEvidencijeZaKorisnika(int korisnikId)
          {
               DataTable tabela = new DataTable();
               using (SqlConnection conn = new SqlConnection(_stringKonekcije))
               {
                    string upit = @"
                    SELECT 
                        e.EvidencijaID, 
                        k.Ime + ' ' + k.Prezime AS Zaposleni, 
                        kr.Naziv AS Kriterijum, 
                        e.Ocena, 
                        e.Komentar, 
                        e.DatumUnosa 
                    FROM EvidencijaRada e
                    INNER JOIN Korisnik k ON e.KorisnikID = k.KorisnikID
                    INNER JOIN Kriterijum kr ON e.KriterijumID = kr.KriterijumID
                    WHERE e.KorisnikID = @KorisnikID
                    ORDER BY e.DatumUnosa DESC";

                    SqlCommand cmd = new SqlCommand(upit, conn);
                    cmd.Parameters.AddWithValue("@KorisnikID", korisnikId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(tabela);
               }
               return tabela;
          }

          // DODATO: Učitava jednu evidenciju na osnovu ID-ja (za popunjavanje polja kod izmene)
          public EvidencijaRadaKlasa UcitajPoId(int id)
          {
               EvidencijaRadaKlasa evidencija = null;
               using (SqlConnection conn = new SqlConnection(_stringKonekcije))
               {
                    string upit = "SELECT * FROM EvidencijaRada WHERE EvidencijaID = @EvidencijaID";
                    using (SqlCommand cmd = new SqlCommand(upit, conn))
                    {
                         cmd.Parameters.AddWithValue("@EvidencijaID", id);
                         conn.Open();
                         using (SqlDataReader reader = cmd.ExecuteReader())
                         {
                              if (reader.Read())
                              {
                                   evidencija = new EvidencijaRadaKlasa
                                   {
                                        EvidencijaID = Convert.ToInt32(reader["EvidencijaID"]),
                                        KorisnikID = Convert.ToInt32(reader["KorisnikID"]),
                                        KriterijumID = Convert.ToInt32(reader["KriterijumID"]),
                                        Ocena = Convert.ToInt32(reader["Ocena"]),
                                        // Provera da li je komentar null u bazi
                                        Komentar = reader["Komentar"] != DBNull.Value ? reader["Komentar"].ToString() : string.Empty,
                                        DatumUnosa = Convert.ToDateTime(reader["DatumUnosa"])
                                   };
                              }
                         }
                    }
               }
               return evidencija;
          }

          public bool DodajEvidenciju(EvidencijaRadaKlasa evidencija)
          {
               using (SqlConnection conn = new SqlConnection(_stringKonekcije))
               {
                    string upit = @"INSERT INTO EvidencijaRada (KorisnikID, KriterijumID, Ocena, Komentar) 
                                VALUES (@KorisnikID, @KriterijumID, @Ocena, @Komentar)";

                    SqlCommand cmd = new SqlCommand(upit, conn);
                    cmd.Parameters.AddWithValue("@KorisnikID", evidencija.KorisnikID);
                    cmd.Parameters.AddWithValue("@KriterijumID", evidencija.KriterijumID);
                    cmd.Parameters.AddWithValue("@Ocena", evidencija.Ocena);
                    cmd.Parameters.AddWithValue("@Komentar", string.IsNullOrEmpty(evidencija.Komentar) ? (object)DBNull.Value : evidencija.Komentar);

                    conn.Open();
                    int brojRedova = cmd.ExecuteNonQuery();
                    return brojRedova > 0;
               }
          }

          public bool IzmeniEvidenciju(EvidencijaRadaKlasa evidencija)
          {
               using (SqlConnection conn = new SqlConnection(_stringKonekcije))
               {
                    string upit = @"UPDATE EvidencijaRada 
                                SET KorisnikID = @KorisnikID, 
                                    KriterijumID = @KriterijumID, 
                                    Ocena = @Ocena, 
                                    Komentar = @Komentar 
                                WHERE EvidencijaID = @EvidencijaID";

                    SqlCommand cmd = new SqlCommand(upit, conn);
                    cmd.Parameters.AddWithValue("@EvidencijaID", evidencija.EvidencijaID);
                    cmd.Parameters.AddWithValue("@KorisnikID", evidencija.KorisnikID);
                    cmd.Parameters.AddWithValue("@KriterijumID", evidencija.KriterijumID);
                    cmd.Parameters.AddWithValue("@Ocena", evidencija.Ocena);
                    cmd.Parameters.AddWithValue("@Komentar", string.IsNullOrEmpty(evidencija.Komentar) ? (object)DBNull.Value : evidencija.Komentar);

                    conn.Open();
                    int brojRedova = cmd.ExecuteNonQuery();
                    return brojRedova > 0;
               }
          }

          public bool ObrisiEvidenciju(int evidencijaID)
          {
               using (SqlConnection conn = new SqlConnection(_stringKonekcije))
               {
                    string upit = "DELETE FROM EvidencijaRada WHERE EvidencijaID = @EvidencijaID";

                    SqlCommand cmd = new SqlCommand(upit, conn);
                    cmd.Parameters.AddWithValue("@EvidencijaID", evidencijaID);

                    conn.Open();
                    int brojRedova = cmd.ExecuteNonQuery();
                    return brojRedova > 0;
               }
          }

          #endregion
     }
}