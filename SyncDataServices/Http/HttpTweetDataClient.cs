// using System;
// using System.Net.Http;
// using System.Text;
// using System.Text.Json;
// using System.Threading.Tasks;
// using lidl_twitter_user_service.DTOs;
// using Microsoft.Extensions.Configuration;
//
// namespace lidl_twitter_user_service.SyncDataServices.Http
// {
//     public class HttpTweetDataClient : ITweetDataClient
//     {
//         private readonly HttpClient _httpClient;
//         private readonly IConfiguration _configuration;
//
//         public HttpTweetDataClient(HttpClient httpClient, IConfiguration configuration)
//         {
//             _httpClient = httpClient;
//             _configuration = configuration;
//         }
//         
//         public async Task SendUserToTweet(ReadUser user)
//         {
//             var httpContent = new StringContent(
//                 JsonSerializer.Serialize(user),
//                 Encoding.UTF8,
//                 "application/json");
//
//             var response = await _httpClient.PostAsync($"{_configuration["TweetService"]}", httpContent);
//
//             if(response.IsSuccessStatusCode)
//             {
//                 Console.WriteLine("--> Sync POST to tweet service was OK!");
//             }
//             else
//             {
//                 Console.WriteLine("--> Sync POST to tweet service was NOT OK!");
//             }
//         }
//     }
// }