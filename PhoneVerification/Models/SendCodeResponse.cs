using Newtonsoft.Json;

namespace PhoneVerification.Models
{
    public class SendCodeResponse
    {
        public string Carrier { get; set; }
        
        [JsonProperty("is_cellphone")]
        public bool IsCellphone { get; set; }
        
        public string Message { get; set; }
        
        [JsonProperty("seconds_to_expire")]
        public int SecondsToExpire { get; set; }
        
        public string Uuid { get; set; }
        public bool Success { get; set; }
    }
}