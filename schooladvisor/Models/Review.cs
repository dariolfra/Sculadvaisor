namespace schooladvisor.Models
{
    public class Review
    {
        public int reviewID { get; set; }
        public int tripID { get; set; }
        public string userID { get; set; }
        public string reviewState { get; set;}
        public int reviewRating { get; set;}
        public string reviewComment { get; set;}
    }
}
