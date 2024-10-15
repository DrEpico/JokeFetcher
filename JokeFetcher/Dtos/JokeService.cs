using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JokeFetcher.Dtos
{
    public class JokeService
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        // Method to fetch 10 jokes of a specific type
        public List<Joke> FetchJokesByType(HttpClient client, string jokeType)
        {
            var jokes = new List<Joke>();
            try
            {
                // Send a GET request to the jokes/{type}/ten endpoint
                HttpResponseMessage response = client.GetAsync($"jokes/{jokeType}/ten").Result;

                //Ensure the request was successful 
                response.EnsureSuccessStatusCode();

                //Read the response content as a string
                string responseBody = response.Content.ReadAsStringAsync().Result;

                //Deserialise the JSON response into a list of Joke objects 
                var jokeList = JsonSerializer.Deserialize<List<Joke>>(responseBody, _options);

                //Add te jokes to the list if they are not null
                if (jokeList != null)
                {
                    jokes.AddRange(jokeList);
                }
            }
            catch (HttpRequestException e)
            {
                //Handle any HTTP request error 
                Console.WriteLine($"Request error: {e.Message}");
            }

            return jokes;
        } 
    }
}
