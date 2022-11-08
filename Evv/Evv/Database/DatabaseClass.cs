using System.Data.SqlClient;

namespace Evv.Database
{
    public class DatabaseClass
    {
        private string ConnectionString = "Data Source=cgievv.database.windows.net;Initial Catalog=FHICT-EVV;Persist Security Info=True;User ID=login;Password=CGIevv123";

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
    }
}
