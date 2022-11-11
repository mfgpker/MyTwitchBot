using System.ComponentModel.DataAnnotations;


namespace TwitchBot2.Model
{
    public class Users

    {
        [Key]
        public string Userid { get; set; }
        public string UserName { get; set; }
        public string channel { get; set; }
        public int rolelevel { get; set; }
        public int Score { get; set; }
    }
}
