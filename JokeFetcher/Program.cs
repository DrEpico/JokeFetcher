
//Set the base URL for the Web Sevice (API)
using JokeFetcher.Dtos;
using System.Text.Json;
using System.Text.Json.Nodes;

const string baseUrl = "https://official-joke-api.appspot.com/";

//Create custom options for JsonSerialiser to ignore case when deserialising
var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
};

var client = new HttpClient { BaseAddress = new Uri(baseUrl) };

try
{
    //Send a GET request to the "random_joke" endpoint
    HttpResponseMessage response = client.GetAsync("jokes/random").Result;

    //Ensure the request was successful (throws an exception if unsuccessful) 
    response.EnsureSuccessStatusCode();

    //Read the response content as a string 
    string responseBody = response.Content.ReadAsStringAsync().Result;

    //Deserialise the JSON response into a joke object (ignore case on key names)
    var joke = JsonSerializer.Deserialize<Joke>(responseBody, options);

    //Check if the joke is null
    if (joke != null)
    {
        //Display the joke in the console
        Console.WriteLine("Here's a joke for you.");
        Console.WriteLine(joke.Setup);
        Console.WriteLine(joke.Punchline);
    }
    else
    {
        Console.WriteLine("Failed to fetch a joke.");
    }
}
catch (HttpRequestException e)
{
    //Handle and HTTP request errors
    Console.WriteLine($"Request error: {e.Message}");
}

