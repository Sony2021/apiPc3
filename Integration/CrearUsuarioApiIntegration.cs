using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using apiPc3.Integration.dto;

namespace apiPc3.Integration
{
    public class CrearUsuarioApiIntegration
    {
        private readonly ILogger<CrearUsuarioApiIntegration> _logger;

        private const string API_URL = "https://reqres.in/api/users";
        private readonly HttpClient httpClient;

        public CrearUsuarioApiIntegration(ILogger<CrearUsuarioApiIntegration> logger)
        {
            _logger = logger;
            httpClient = new HttpClient();

        }

        public async Task<ApiResponse> CreateUser(string name, string job)
        {
            string requestUrl = API_URL;
            ApiResponse apiResponse = null;
            try
            {
                var userData = new { name, job };
                var jsonUserData = JsonSerializer.Serialize(userData);
                var requestContent = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(requestUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadFromJsonAsync<ApiResponse>();

                    apiResponse = responseBody ?? new ApiResponse();

                }
                else
                {
                    // Manejar el error si la solicitud no fue exitosa
                    _logger.LogDebug($"La solicitud POST a la API no fue exitosa. Código de estado: {response.StatusCode}");
                }
            }
            catch(Exception ex)
            {
                // Manejar cualquier excepción que ocurra durante la solicitud
                _logger.LogDebug($"Error al llamar a la API: {ex.Message}");
            }
            return apiResponse;
        }

        public class ApiResponse
        {
            public string Name { get; set; }
            public string Job { get; set; }
            public string Id { get; set; }
            public string CreatedAt { get; set; } // Cambiado a string
        }
    }
}