using System.Data.SqlClient;
using static System.Formats.Asn1.AsnWriter;
using System.Security.Cryptography.Xml;
using System.Globalization;
using System.Data.Common;
using Evv.Classes;
using System.Security.Principal;
using static System.Net.Mime.MediaTypeNames;

namespace Evv.Database
{
    public class DatabaseClass
    {
       private string ConnectionString = "Data Source=mssqlstud.fhict.local;Initial Catalog=dbi494690_evv;Persist Security Info=True;User ID=dbi494690_evv;Password=CGIevv123";

        private double totalscores;
        public void AddTrip(double score, double lenght, DateTime dag, string accountId, string transport)
        {
            string Query = "INSERT INTO [dbo].[Trip]([Date],[Score],[acountId],[Distance],[Transport])VALUES(@date,@score,@acountId,@distance,@transport)";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand comm = conn.CreateCommand();
                comm.CommandText = Query;
                comm.Parameters.AddWithValue("@date", dag);
                comm.Parameters.AddWithValue("@score", score);
                comm.Parameters.AddWithValue("@acountId", accountId);
                comm.Parameters.AddWithValue("@distance", lenght);
                comm.Parameters.AddWithValue("@transport", transport);
                comm.ExecuteNonQuery();
            }
        }
        public void AddTripFromFavorites(string accountId, string favorite)
        {
            string Query = "SELECT * FROM Trip WHERE acountId = @userId and FavoriteName = @favorite";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand comm = new SqlCommand(Query, conn);
                comm.Parameters.AddWithValue("@userId", accountId);
                comm.Parameters.AddWithValue("@favorite", favorite);

                conn.Open();

                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AddTrip(Convert.ToDouble(reader["Score"]), Convert.ToDouble(reader["Distance"]), DateTime.Now, accountId, reader["Transport"].ToString());
                    }

                    conn.Close();
                }
            }
        }

        public Account GetCurentUserData(string userId)
        {
            Account account = new Account();

            string Query = "SELECT * FROM Account WHERE id = @userId";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand comm = new SqlCommand(Query, conn);
                comm.Parameters.AddWithValue("@userId", userId);

                conn.Open();

                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        account.ID = reader["id"].ToString();
                        account.FirstName = reader["first_name"].ToString();
                        account.LastName = reader["last_name"].ToString();
                    }

                    conn.Close();
                }
            }

            return account;
        }

        public void CreateUser(string firstname, string lastname, string auth0id)
        {
            string Query = "INSERT INTO Account (id, first_name, last_name, isAdmin) VALUES(@id, @first_name, @last_name, @isAdmin)";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand comm = conn.CreateCommand();
                comm.CommandText = Query;
                comm.Parameters.AddWithValue("@id", auth0id);
                comm.Parameters.AddWithValue("@first_name", firstname);
                comm.Parameters.AddWithValue("@last_name", lastname);
                comm.Parameters.AddWithValue("@isAdmin", false);
                comm.ExecuteNonQuery();
            }
        }

        public int UpdateUser(string firstname, string lastname, string auth0id)
        {
            int rowsaffected = 0;

            string Query = "UPDATE Account SET first_name = @first_name, last_name = @last_name WHERE id = @id";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand comm = conn.CreateCommand();
                comm.CommandText = Query;
                comm.Parameters.AddWithValue("@first_name", firstname);
                comm.Parameters.AddWithValue("@last_name", lastname);
                comm.Parameters.AddWithValue("@id", auth0id);
                rowsaffected = comm.ExecuteNonQuery();
            }

            return rowsaffected;
        }

        public int GetNextId()
        {
            int count = 0;

            string Query = "SELECT * FROM Account";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand comm = new SqlCommand(Query, conn);
                conn.Open();

                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        count++;
                    }

                    conn.Close();
                }
            }

            return count + 1;
        }
        public List<Trip> GetTrips(string userId)
        {
            List<Trip> ListWithTrips = new();

            string Query = "SELECT * FROM Trip WHERE acountId = @userId and FavoriteName IS NULL";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand comm = new SqlCommand(Query, conn);
                comm.Parameters.AddWithValue("@userId", userId);

                conn.Open();

                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        Trip trip = new Trip((int)reader["tripId"],Convert.ToDouble(reader["Distance"]), reader["Transport"].ToString(), Convert.ToDateTime(reader["Date"]), Convert.ToDouble(reader["Score"]));
                        ListWithTrips.Add(trip);
                    }

                    conn.Close();
                }
            }

            return ListWithTrips;
        }

        public double TotalScore(string userId)
        {
            string Query = "SELECT Score FROM Trip WHERE acountId = @userId and FavoriteName IS NULL ";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand comm = new SqlCommand(Query, conn);
                comm.Parameters.AddWithValue("@userId", userId);

                conn.Open();

                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        totalscores += Convert.ToDouble(reader["Score"]);
                        //double test = Convert.ToDouble(reader["Score"];
                        //totalscores =+ test;
                    }
                    conn.Close();
                }
            }
            return Math.Round(totalscores,2);
        }

        public List<Person> GetAllPeople()
        {
            List<Person> list = new List<Person>();
            list.Add(new Person());

            string Query = "SELECT Account.id, Account.first_name, Account.last_name, Trip.Score, Trip.distance FROM Account INNER JOIN Trip ON Account.id = Trip.acountId WHERE Trip.FavoriteName IS NULL ORDER BY Account.id ";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand comm = new SqlCommand(Query, conn);

                conn.Open();

                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    int i = 0;

                    while (reader.Read())
                    {
                        if (list[i].firstname == "")
                        {
                            list[i].firstname = reader.GetString(1);
                            list[i].lastname = reader.GetString(2);
                            list[i].points = Decimal.ToDouble(reader.GetDecimal(3));
                            list[i].distance = Decimal.ToDouble(reader.GetDecimal(4));
                            list[i].tripAmount++;
                        }
                        else
                        {
                            if (list[i].firstname == reader.GetString(1))
                            {
                                list[i].points += Decimal.ToDouble(reader.GetDecimal(3));
                                list[i].distance += Decimal.ToDouble(reader.GetDecimal(4));
                                list[i].tripAmount++;
                            }
                            else
                            {
                                list.Add(new Person());
                                i++;
                                list[i].firstname = reader.GetString(1);
                                list[i].lastname = reader.GetString(2);
                                list[i].points = Decimal.ToDouble(reader.GetDecimal(3));
                                list[i].distance = Decimal.ToDouble(reader.GetDecimal(4));
                                list[i].tripAmount++;
                            }
                        }
                    }

                    conn.Close();
                }
            }

            return list;
        }

        public void AddFavorite(string userid, string favoriteName)
        {
            string Query = "INSERT INTO [dbo].[Favorites]([FavoriteName],[accountId])VALUES(@favoriteName,@userid)";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand comm = conn.CreateCommand();
                comm.CommandText = Query;
                comm.Parameters.AddWithValue("@userid", userid);
                comm.Parameters.AddWithValue("@favoriteName", favoriteName);
                comm.ExecuteNonQuery();
            }
        }

        public List<string> GetFavoriteName(string userId)
        {
            string Query = "SELECT FavoriteName FROM Favorites WHERE accountId = @userId";
            List<string> Favorites = new List<string>();
            string Favorite = "";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand comm = new SqlCommand(Query, conn);
                comm.Parameters.AddWithValue("@userId", userId);

                conn.Open();

                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Favorites.Add(Convert.ToString(reader["FavoriteName"]));
                    }
                    conn.Close();
                }
            }
            return Favorites;
        }
        public void AddTripToFavorite(double score, double lenght, DateTime dag, string accountId, string transport, string FavoriteName)
        {
            string Query = "INSERT INTO [dbo].[Trip]([Date],[Score],[acountId],[Distance],[Transport],[FavoriteName])VALUES(@date,@score,@acountId,@distance,@transport,@FavoriteName)";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand comm = conn.CreateCommand();
                comm.CommandText = Query;
                comm.Parameters.AddWithValue("@date", dag);
                comm.Parameters.AddWithValue("@score", score);
                comm.Parameters.AddWithValue("@acountId", accountId);
                comm.Parameters.AddWithValue("@distance", lenght);
                comm.Parameters.AddWithValue("@transport", transport);
                comm.Parameters.AddWithValue("@FavoriteName", FavoriteName);
                comm.ExecuteNonQuery();
            }
        }

        public string GetUserName(string Userid)
        {
            string name = "";

            string Query = "SELECT first_name, last_name FROM Account WHERE id = @userId";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand comm = new SqlCommand(Query, conn);
                comm.Parameters.AddWithValue("@userId", Userid);

                conn.Open();

                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        name = reader["first_name"].ToString() + " " + reader["last_name"].ToString();
                    }

                    conn.Close();
                }
            }

            return name;
        }

        public Trip GetTripByID(int TripId)
        {
            string Query = "SELECT * FROM Trip WHERE tripId = @TripID";

            int tripID = 0;
            double distance = 0;
            string transport = "";
            DateTime dateCreated = DateTime.Now;
            double score = 0;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand comm = new SqlCommand(Query, conn);
                comm.Parameters.AddWithValue("@tripId", TripId);

                conn.Open();

                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tripID = (int)reader["tripId"];
                        distance = Convert.ToDouble(reader["Distance"]);
                        transport = reader["Transport"].ToString();
                        dateCreated = Convert.ToDateTime(reader["Date"]);
                        score = Convert.ToDouble(reader["Score"]);
                    }

                    conn.Close();
                }
            }

            return new Trip(tripID, distance, transport, dateCreated, score);
        }

        public int UpdateTrip(Trip _trip)
        {
            int rowsaffected = 0;

            //string Query = "INSERT INTO [dbo].[Trip]([Date],[Score],[acountId],[Distance],[Transport])VALUES(@date,@score,@acountId,@distance,@transport)";
            string Query = "UPDATE Trip SET Date = @date, Score = @score, Distance = @distance, Transport = @transport WHERE tripId = @id";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand comm = conn.CreateCommand();



                comm.CommandText = Query;
                comm.Parameters.AddWithValue("@id", _trip.Id);
                comm.Parameters.AddWithValue("@date", _trip.DateCreated);
                comm.Parameters.AddWithValue("@score", _trip.Score);
                comm.Parameters.AddWithValue("@distance", _trip.Distance);
                comm.Parameters.AddWithValue("@transport", _trip.Vehicle);
                comm.ExecuteNonQuery();
            }

            return rowsaffected;
        }

        public void DeleteTrip(int id)
        {
            string Query = "DELETE FROM Trip WHERE tripId = @id";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand comm = conn.CreateCommand();
                comm.CommandText = Query;
                comm.Parameters.AddWithValue("@id", id);
                comm.ExecuteNonQuery();
            }
        }
    }
}
