namespace UniversalAPI.Logger.Models
{
    public class LogModel
    {
        // for elastic search
        public string _index_id { get => "id"; }
        /// <summary>
        /// Logzeit in UTS
        /// </summary>
        public string asctime
        {
            get
            {
                return DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss,fff");
            }
        }

        public long latency { get; set; }
        public string service { get; set; }
        public int responseLength { get; set; }
        public int statusCode { get; set; }
        public string? message { get; set; }
    }
}
