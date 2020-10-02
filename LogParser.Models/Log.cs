using Newtonsoft.Json.Linq;

namespace LogParser.models
{
    public class Log
    {
        public int ID { get; set; }
        public string TokenGUID { get; set; }
        public string TimeStamp { get; set; }
        public string Action { get; set; }
        public string Host { get; set; }
        public string Endpoint { get; set; }
        public string ResponseCode { get; set; }
        public JToken RequestBody { get; set; }
        public JToken ResponseBody { get; set; }
        public byte[] FullBody { get; set; }
    }
}