using Milau.Models;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milau
{
    public class Authorization
    {
        public string License { get; set; }
        public string Hwid { get; set; }
        public Authorization(string license, string hwid)
        {
            License = license;
            Hwid = hwid;
        }

        public async Task<bool> Login()
        {
            var postData = new UserDetails
            {
                Id = "1",
                License = License,
                Hwid = Hwid
            };

            var httpClient = new HttpClient();

            var json = JsonSerializer.Serialize(postData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("http://localhost:5160/api/v1/auth", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var postResponse = JsonSerializer.Deserialize<AuthResponse>(responseData);

                Console.WriteLine($"License: {postResponse?.User?.License}\n" +
                    $"HWID: {postResponse?.User?.Hwid}\n");

                return true;
            }
            else
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var postResponse = JsonSerializer.Deserialize<AuthResponse>(responseData);

                Console.WriteLine($"{postResponse?.Message}:\n" +
                    $"License: {postResponse?.User?.License}\n" +
                    $"HWID: {postResponse?.User?.Hwid}\n");
                return false;
            }
        }
    }
}
