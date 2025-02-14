namespace MS.RapidApiWebUI.Models
{
    public class WeatherViewModel
    {
        public class Rootobject
        {
            public Coord coord { get; set; }
            public Weather[] weather { get; set; }
            public string _base { get; set; }
            public Main main { get; set; }
            public int visibility { get; set; }
            public Wind wind { get; set; }
            public Clouds clouds { get; set; }
            public int dt { get; set; }
            public Sys sys { get; set; }
            public int timezone { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public int cod { get; set; }
        }

        public class Coord
        {
            public float lon { get; set; }
            public float lat { get; set; }
        }

        public class Main
        {
            public float temp { get; set; }
            public float feels_like { get; set; }
            public float temp_min { get; set; }
            public float temp_max { get; set; }
            public int pressure { get; set; }
            public int humidity { get; set; }
            public int sea_level { get; set; }
            public int grnd_level { get; set; }
        }

        public class Wind
        {
            public float speed { get; set; }
            public int deg { get; set; }
        }

        public class Clouds
        {
            public int all { get; set; }
        }

        public class Sys
        {
            public int type { get; set; }
            public int id { get; set; }
            public string country { get; set; }
            public int sunrise { get; set; }
            public int sunset { get; set; }
        }

        public class Weather
        {
            public int id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }

        //public class Rootobject
        //{
        //    public bool success { get; set; }
        //    public Data data { get; set; }
        //}
        //public class Data
        //{
        //    public string city { get; set; }
        //    public string current_weather { get; set; }
        //    public string temp { get; set; }
        //    public string expected_temp { get; set; }
        //    public string insight_heading { get; set; }
        //    public string insight_description { get; set; }
        //    public string wind { get; set; }
        //    public string humidity { get; set; }
        //    public string visibility { get; set; }
        //    public string uv_index { get; set; }
        //    public string aqi { get; set; }
        //    public string aqi_remark { get; set; }
        //    public string aqi_description { get; set; }
        //    public string last_update { get; set; }
        //    public string bg_image { get; set; }
        //}
    }
}
