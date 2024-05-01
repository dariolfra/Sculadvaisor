using Dapper;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;

namespace schooladvisor.Models
{
    public class GestioneDati
    {
        private string s; //Connectionsszszsz
        public GestioneDati(IConfiguration configuration)
        {
            s = configuration.GetConnectionString("ScuolaConnection");
        }

        public List<Trip> GetTripList()
        {
            using var con = new MySqlConnection(s);
            return con.Query<Trip>("SELECT * from trips").ToList();
        }

        public Trip GetTrip(string id)
        {
            using var con = new MySqlConnection(s);
            return con.QueryFirstOrDefault<Trip>("SELECT * FROM trips WHERE tripID = @Id", new { Id = id });
        }

        public Review RegisterComment(Review commento)
        {
            using var con = new MySqlConnection(s);

            string sql = "INSERT INTO reviews (reviewID,UserID,reviewComment,tripID,reviewState,reviewRating) VALUES (@ID, @Email, @Comment, @TripID, @State, @Rating)";

            commento.reviewID = GetNextID();
            // Esegui l'istruzione SQL utilizzando Dapper
            con.Execute(sql, new { ID = commento.reviewID, Email = commento.userID, Comment = commento.reviewComment, TripID = commento.tripID, State = "sent", Rating = commento.reviewRating});
            return commento;
        }

        public int GetNextID()
        {
            using var con = new MySqlConnection(s);

            string sql = "SELECT * FROM reviews";
            return con.Query<Review>(sql).ToList().Count + 1;
        }

        public bool AccettaRecensioni(int id, string state)
        {
            using var con = new MySqlConnection(s);

            string query = @"UPDATE reviews SET reviewState = @Stato WHERE reviewID = @reviewID";
            var param = new { Stato = state, reviewID = id };

            bool esito;
            try
            {
                con.Execute(query, param);
                esito = true;
            }
            catch (Exception ex)
            {
                esito = false;
            }

            return esito;
        }

        public void AddTrip(Trip trip)
        {
            using var con = new MySqlConnection(s);
            string sql = "INSERT INTO trips (tripName,tripDate,tripDescription,image) VALUES (@name,@date,@descr,@img)";
            con.Execute(sql, new { name = trip.tripName, date = trip.tripDate, descr = trip.tripDescription, img = trip.image });
        }

        public List<Review>GetApprovedComments(string selectedTripId)
        {
            using var con = new MySqlConnection(s);
            string sql = "SELECT * FROM reviews JOIN trips ON trips.tripID = reviews.tripID WHERE reviewState='approved' AND trips.tripID=@Id";
            return con.Query<Review>(sql, new {Id = selectedTripId}).ToList();
        }
    }
}
