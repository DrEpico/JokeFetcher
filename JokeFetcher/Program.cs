
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

//Start with an empty list
var jokes = new List<Joke>();

try
{
    //Send a GET request to the "jokes/{type}/ten" endpoint
    HttpResponseMessage response = client.GetAsync($"jokes/ten").Result;

    //Ensure the request was successful 
    response.EnsureSuccessStatusCode();

    //Read the response content as a string 
    string responseBody = response.Content.ReadAsStringAsync().Result;

    //Desrialise the JSON response into a list of Joke objects
    var jokeList = JsonSerializer.Deserialize<List<Joke>>(
            responseBody, options
        );
    if (jokeList != null)
    {
        jokes.AddRange(jokeList);
    }
    //Add the jokes to the list if they are not null
} catch (HttpRequestException e)
{
    //Handle any HTTP request errors 
    Console.WriteLine($"Request error: {e.Message}");
}

//Display list of jokes
Console.WriteLine("\n10 More Jokes");
Console.WriteLine("-----------------");
foreach (var joke in jokes)
{
    Console.WriteLine(joke.Setup);
    Console.WriteLine(joke.Punchline);
    Console.WriteLine();
}

//Create an instance of the JokeService
var jokeService = new JokeService();

//Fetch 10 programming jokes
List<Joke> programmingJokes = jokeService.FetchJokesByType(client, "programming");

//Display the programming jokes
Console.WriteLine("\n10 Programming Jokes:");
Console.WriteLine("------------------------");
foreach (var joke in programmingJokes)
{
    Console.WriteLine(joke.Setup);
    Console.WriteLine(joke.Punchline);
    Console.WriteLine();
}

//Fetch another 10 examples (dad jokes)
List<Joke> dadJokes = jokeService.FetchJokesByType(client, "dad");

// Display the dad jokes
Console.WriteLine("\n10 Dad Jokes:");
Console.WriteLine("----------------");
foreach (var joke in dadJokes)
{
    Console.WriteLine(joke.Setup);
    Console.WriteLine(joke.Punchline);
    Console.WriteLine();
}


