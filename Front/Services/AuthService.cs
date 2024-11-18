using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Front.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://seu-servidor.com"); // URL da sua API
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var model = new
            {
                Email = email,
                Password = password
            };

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("/account/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(responseBody);
                return result.token;
            }
            else
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<dynamic>(errorBody);
                throw new Exception(error.message.ToString());
            }
        }

        public async Task<string> SignupAsync(string name, string email, string password)
        {
            var model = new
            {
                Name = name,
                Email = email,
                Password = password
            };

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("/account/signup", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(responseBody);
                return $"Usuário registrado com sucesso! ID: {result.userId}";
            }
            else
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<dynamic>(errorBody);
                throw new Exception(error.message.ToString());
            }
        }

    }
}
