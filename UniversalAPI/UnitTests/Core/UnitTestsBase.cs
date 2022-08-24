using System.Net;

namespace UnitTests.Core
{
    public abstract class UnitTestsBase
    {
        /// <summary>
        /// send request and return json from response
        /// </summary>
        protected async Task<string> GetJsonFromRequest(string requesturl)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requesturl);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                // handle exception here if needed
                throw;
            }
        }
    }
}
