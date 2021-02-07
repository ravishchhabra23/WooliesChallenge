using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WooliesChallenge.Application.Helpers;
using WooliesChallenge.Application.Interfaces;

namespace WooliesChallenge.Application.Services
{
    public class ResourceService : IResourceService
    {
        private readonly HttpClient _httpClient;
        public ResourceService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException();
        }
        public async Task<string> GetProducts()
        {
            var uri = _httpClient.BaseAddress + Constants.ResourceBaseUrl + Constants.ProductResourceUrl + Constants.Token;
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> GetShopperHistory()
        {
            var uri = _httpClient.BaseAddress + Constants.ResourceBaseUrl + Constants.ShopperHistoryUrl + Constants.Token;
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> GetTrolleyCalculation(string input)
        {
            if (string.IsNullOrEmpty(input)) throw new ArgumentNullException();
            var uri = _httpClient.BaseAddress + Constants.ResourceBaseUrl + Constants.TrolleyCalculatorUrl + Constants.Token;
            var response = await _httpClient.PostAsync(uri,new StringContent(input, Encoding.UTF8, Constants.ApplicationJsonType));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
