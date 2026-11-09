using System.Net.Http.Headers;
namespace RaceDay_MVC.Services
{// start of namespace
    public class ApiService
    {// start of class

        private readonly IHttpClientFactory factory;
        private readonly IHttpContextAccessor accessor;

        public ApiService(IHttpClientFactory factory, IHttpContextAccessor accessor)
        {
            this.factory = factory;
            this.accessor = accessor;
        }

        // the MVC session of the person using the site
        private ISession Session => accessor.HttpContext!.Session;

        // sends a request to the Part 2 API and returns the response text
        public async Task<string> SendAsync(HttpMethod method, string url, HttpContent? content = null)
        {// start of method

            HttpClient client = factory.CreateClient("RaceDayApi");
            HttpRequestMessage request = new HttpRequestMessage(method, url);

            if (content != null)
            {
                request.Content = content;
            }

            // pass the API's session cookie along so the API knows who is logged in
            string? cookie = Session.GetString("ApiCookie");
            if (cookie != null)
            {
                request.Headers.Add("Cookie", cookie);
            }

            HttpResponseMessage response = await client.SendAsync(request);

            // keep the API's session cookie when it sends one
            if (response.Headers.TryGetValues("Set-Cookie", out IEnumerable<string>? values))
            {
                string? sessionCookie = values.FirstOrDefault(v => v.StartsWith(".AspNetCore.Session"));
                if (sessionCookie != null)
                {
                    Session.SetString("ApiCookie", sessionCookie.Split(';')[0]);
                }
            }

            return await response.Content.ReadAsStringAsync();

        }// end of method
         // sends an image to the API as a file upload
        public async Task<string> UploadAsync(string url, IFormFile file)
        {// start of method

            using MultipartFormDataContent form = new MultipartFormDataContent();
            StreamContent fileContent = new StreamContent(file.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            form.Add(fileContent, "file", file.FileName);

            return await SendAsync(HttpMethod.Post, url, form);

        }// end of method
    }// end of class
}// end of namespace