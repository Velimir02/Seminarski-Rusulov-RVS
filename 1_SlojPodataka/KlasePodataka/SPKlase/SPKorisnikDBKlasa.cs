using KlasePodataka.Entiteti;
using System;
using System.Data;
using System.Data.SqlClient;

namespace KlasePodataka
{
     /// <summary>
     /// Klasa za direktnu komunikaciju sa tabelom 'Korisnik' u bazi.
     /// Ne koristi spoljne biblioteke, već čist ADO.NET sa parametrima.
     /// </summary>
     public class SPKorisnikDBKlasa
     {
          private readonly string _stringKonekcije;

          public SPKorisnikDBKlasa(string noviStringKonekcije)
          {
               _stringKonekcije = noviStringKonekcije;
          }

          /// <summary>
          /// Vraća tabelu sa svim korisnicima (za prikaz u Grid-u ili popunjavanje padajućih listi).
          /// Vraća i spojeno Ime i Prezime kako bi bilo lakše za prikaz.
          /// </summary>
          public DataTable DajSveKorisnike()
          {
               DataTable tabela = new DataTable();

               using (SqlConnection conn = new SqlConnection(_stringKonekcije))
               {
                    string upit = "SELECT KorisnikID, Ime, Prezime, Ime + ' ' + Prezime AS ImePrezime, Email, Pozicija FROM Korisnik";

                    using (SqlCommand cmd = new SqlCommand(upit, conn))
                    {
                         SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                         adapter.Fill(tabela);
                    }
               }

               return tabela;
          }

          /// <summary>
          /// Metoda za Login - proverava da li postoji korisnik.
          /// Vraća DataSet kako bi Presentation sloj mogao da pročita podatke (ID, Ime, Poziciju).
          /// </summary>
          public DataSet DajKorisnikaPoKorisnickomImenuISifri(string email, string lozinka)
          {
               DataSet ds = new DataSet();

               using (SqlConnection conn = new SqlConnection(_stringKonekcije))
               {
                    string upit = "SELECT * FROM Korisnik WHERE Email = @Email AND Lozinka = @Lozinka";

                    using (SqlCommand cmd = new SqlCommand(upit, conn))
                    {
                         // Parametrizovani upit štiti od SQL Injection-a
                         cmd.Parameters.AddWithValue("@Email", email);
                         cmd.Parameters.AddWithValue("@Lozinka", lozinka);

                         SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                         adapter.Fill(ds);
                    }
               }

               return ds;
          }

          /// <summary>
          /// Dohvata tačno jednog korisnika po njegovom ID-ju (korisno za popunjavanje TextBox-ova pre izmene).
          /// </summary>
          public KorisnikKlasa UcitajPoId(int id)
          {
               KorisnikKlasa korisnik = null;

               using (SqlConnection conn = new SqlConnection(_stringKonekcije))
               {
                    string upit = "SELECT * FROM Korisnik WHERE KorisnikID = @KorisnikID";

                    using (SqlCommand cmd = new SqlCommand(upit, conn))
                    {
                         cmd.Parameters.AddWithValue("@KorisnikID", id);
                         conn.Open();

                         using (SqlDataReader reader = cmd.ExecuteReader())
                         {
                              if (reader.Read())
                              {
                                   korisnik = new KorisnikKlasa
                                   {
                                        KorisnikID = Convert.ToInt32(reader["KorisnikID"]),
                                        Ime = reader["Ime"].ToString(),
                                        Prezime = reader["Prezime"].ToString(),
                                        Email = reader["Email"].ToString(),
                                        Lozinka = reader["Lozinka"].ToString(),
                                        Pozicija = reader["Pozicija"].ToString()
                                   };
                              }
                         }
                    }
               }

               return korisnik;
          }

          /// <summary>
          /// Dodaje novog korisnika (radnika/menadžera) u bazu.
          /// </summary>
          public bool DodajKorisnika(KorisnikKlasa k)
          {
               using (SqlConnection conn = new SqlConnection(_stringKonekcije))
               {
                    string upit = @"INSERT INTO Korisnik (Ime, Prezime, Email, Lozinka, Pozicija) 
                                VALUES (@Ime, @Prezime, @Email, @Lozinka, @Pozicija)";

                    using (SqlCommand cmd = new SqlCommand(upit, conn))
                    {
                         cmd.Parameters.AddWithValue("@Ime", k.Ime);
                         cmd.Parameters.AddWithValue("@Prezime", k.Prezime);
                         cmd.Parameters.AddWithValue("@Email", k.Email);
                         cmd.Parameters.AddWithValue("@Lozinka", k.Lozinka);
                         cmd.Parameters.AddWithValue("@Pozicija", k.Pozicija);

                         conn.Open();
                         int brojRedova = cmd.ExecuteNonQuery();
                         return brojRedova > 0;
                    }
               }
          }

          /// <summary>
          /// Ažurira podatke postojećeg korisnika u bazi.
          /// </summary>
          public bool IzmeniKorisnika(KorisnikKlasa k)
          {
               using (SqlConnection conn = new SqlConnection(_stringKonekcije))
               {
                    string upit = @"UPDATE Korisnik 
                                SET Ime = @Ime, Prezime = @Prezime, Email = @Email, 
                                    Lozinka = @Lozinka, Pozicija = @Pozicija 
                                WHERE KorisnikID = @KorisnikID";

                    using (SqlCommand cmd = new SqlCommand(upit, conn))
                    {
                         cmd.Parameters.AddWithValue("@KorisnikID", k.KorisnikID);
                         cmd.Parameters.AddWithValue("@Ime", k.Ime);
                         cmd.Parameters.AddWithValue("@Prezime", k.Prezime);
                         cmd.Parameters.AddWithValue("@Email", k.Email);
                         cmd.Parameters.AddWithValue("@Lozinka", k.Lozinka);
                         cmd.Parameters.AddWithValue("@Pozicija", k.Pozicija);

                         conn.Open();
                         int brojRedova = cmd.ExecuteNonQuery();
                         return brojRedova > 0;
                    }
               }
          }

          /// <summary>
          /// Briše korisnika iz baze na osnovu ID-ja.
          /// </summary>
          public bool ObrisiKorisnika(int id)
          {
               using (SqlConnection conn = new SqlConnection(_stringKonekcije))
               {
                    string upit = "DELETE FROM Korisnik WHERE KorisnikID = @KorisnikID";

                    using (SqlCommand cmd = new SqlCommand(upit, conn))
                    {
                         cmd.Parameters.AddWithValue("@KorisnikID", id);

                         conn.Open();
                         int brojRedova = cmd.ExecuteNonQuery();
                         return brojRedova > 0;
                    }
               }
          }
     }
}