using Newtonsoft.Json;

namespace _106_A2_M1.Interfaces
{
    public class ResponseBody<T> where T: ResponseBody<T> {
        public static T FromJSONString(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
