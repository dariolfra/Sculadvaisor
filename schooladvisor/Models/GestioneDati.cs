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

        public void RegisterComment(string comment,string email,string selectedTripId,string rating)
        {
            using var con = new MySqlConnection(s);

            string sql = "INSERT INTO reviews (UserID,reviewComment,tripID,reviewState,reviewRating) VALUES (@Email, @Comment, @TripID, @State, @Rating)";

            // Esegui l'istruzione SQL utilizzando Dapper
            con.Execute(sql, new { Email = email, Comment = comment, TripID = selectedTripId, State = "sent", Rating = rating});
        }

        public List<Review>GetApprovedComments(string selectedTripId)
        {
            using var con = new MySqlConnection(s);
            string sql = "SELECT * FROM reviews JOIN trips ON trips.tripID = reviews.tripID WHERE reviewState='approved' AND trips.tripID=@Id";
            return con.Query<Review>(sql, new {Id = selectedTripId}).ToList();
        }

        public void AddTrip(Trip trip)
        {
            using var con = new MySqlConnection(s);
            string sql = "INSERT INTO trips (tripName,tripDate,tripDescription,image) VALUES (@name,@date,@descr,@img)";
            con.Execute(sql, new { name = trip.tripName, date = trip.tripDate, descr = trip.tripDescription, img = trip.image });
        }

        public List<Trip> SearchTrips(string search)
        {
            using var con = new MySqlConnection(s);
            string sql = "SELECT * FROM trips WHERE tripName LIKE CONCAT('%', @searchText, '%') OR tripDate LIKE CONCAT('%', @searchText, '%') OR tripDescription LIKE CONCAT('%', @searchText, '%')";
            return con.Query<Trip>(sql, new { searchText = search }).ToList();
        }

    }
}
