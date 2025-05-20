namespace FanFootball.Models
{
    public class player
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int Age { get; set; } //didnt use do to site not having age data
        public string Team { get; set; } = string.Empty;
        public double Points { get; set; }
        public double ADP { get; set; }

    }
}
