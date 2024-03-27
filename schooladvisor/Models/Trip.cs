namespace schooladvisor.Models
{
    public class Trip
    {        public int tripID { get; set; }
        public string tripName { get; set; }
        public string tripDate { get; set; }
        public string tripDescription { get; set; }
        public List<string> imagesPaths { get; set; }

        public Trip()
        {
            imagesPaths = new List<string>();
        }
    }
}
