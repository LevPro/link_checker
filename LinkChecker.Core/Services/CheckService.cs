using LinkChecker.Core.Models;
using LinkChecker.Core.Helpers;
using System.Net.Http;
using System.Threading.Tasks;

namespace LinkChecker.Core.Services
{
    public class CheckService
    {
        public async Task<ResponseInfo> CheckAsync(string url)
        {
            using var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync(url);
                var code = (int)response.StatusCode;
                return new ResponseInfo
                {
                    ResponseCode = code,
                    ResponseDescription = ResponseDescriptionHelper.GetDescription(code)
                };
            }
            catch
            {
                return new ResponseInfo
                {
                    ResponseCode = 0,
                    ResponseDescription = "Connection Error"
                };
            }
        }
    }
}