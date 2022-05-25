namespace CashFlow.Services
{
    public class IncomeService
    {
        private static HttpClient client = new HttpClient();

        public IncomeService()
        {

        }

        public async Task<Income> GetIncome(Guid id)
        {
            var response = await client.GetAsync($"api/Income/{id}");
            if (response.IsSuccessStatusCode)
            {
                var income = await response.Content.ReadAsAsync<Income>();
                return income;
            }
            return null;
        }

    }
}
