using Newtonsoft.Json;

namespace _106_A2_M1.Interfaces
{
    public class RequestBody<T> where T: RequestBody<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">in the vast majority scenario, this will be <c>this</c></param>
        /// <returns></returns>
        public static string ToJSONString(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
