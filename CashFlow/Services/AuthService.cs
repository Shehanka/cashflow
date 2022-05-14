using CashFlow.Models;

namespace CashFlow.Services
{
    public class AuthService
    {
        private static HttpClient client = new HttpClient();

        public AuthService()
        {
            this.client.BaseAddress = new Uri("http://localhost:5000/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client
                .DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async User Login(string username, string password)
        {
            var response =
                await client
                    .PostAsJsonAsync("login", new { username, password });
            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadAsAsync<User>();
                return user;
            }
            return null;
        }

        public async bool Register(User user)
        {
            var response =
                await client
                    .PostAsJsonAsync("register", user);
            return response.IsSuccessStatusCode;
        }
    }
}
