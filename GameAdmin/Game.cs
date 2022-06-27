using System;

namespace DataBaseIndTask
{
    public class Game
    {
        public int game_id { get; set; }
        public string game_name { get; set; }
        public string game_description { get; set; }
        public string game_genre { get; set; }
        public DateTime game_releasedate { get; set; }
        public string game_developer { get; set; }
        public string game_publisher { get; set; }
        public byte game_rating { get; set; }
        public byte game_agelimit { get; set; }
        public string game_image { get; set; }
        public string game_storeimage { get; set; }
    }
}