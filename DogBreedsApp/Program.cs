using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DogBreedsApp
{
    class Program
    {
        static readonly HttpClient httpClient = new HttpClient();
        const string domain = "https://dog.ceo/api/breeds/";
        
        static void Main(string[] args)
        {
            bool status = false;
            string url = "";

            DisplayCommandText();

            string command = Console.ReadLine();
           
            while (!status)
            {
                if (command.Equals("All", StringComparison.OrdinalIgnoreCase))
                {
                    url = $"{domain}list/all";
                    GetAllDogBreeds(url);

                }
                else if (command.Equals("Random", StringComparison.OrdinalIgnoreCase))
                {
                    url = $"{domain}image/random";
                    GetRandomDogBreed(url);
                }
                else if (command.Equals("Subbreeds", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Please enter the breed's name: ");
                    string breedName = Console.ReadLine();
                    url = $"https://dog.ceo/api/breed/{breedName}/list";
                    GetSubBreeds(url);


                }
                else if (command.Equals("Photo", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Please enter the breed's name: ");
                    string breedName = Console.ReadLine();
                    url = $"https://dog.ceo/api/breed/{breedName}/images/random";
                    GetBreedPhoto(url);


                }
                else if (command.Equals("Exit", StringComparison.OrdinalIgnoreCase))
                {
                    status = true;
                }
                else
                {
                    Console.Write("Invalid Command.\n");
                    DisplayCommandText();
                }

                if (!status)
                {
                    Console.WriteLine("Please select a command:");
                    command = Console.ReadLine();
                }
            }
            
        }
        /// <summary>
        /// Gets all dog breeds.
        /// </summary>
        /// <param name="url">The URL.</param>
        static void GetAllDogBreeds(string url)
        {
            ApiMessage apiMessage = GetBreedInfo(url);
            Dictionary<string,JToken> breeds =
          ((IEnumerable<KeyValuePair<string, JToken>>)apiMessage.message)
                     .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            foreach(KeyValuePair<string, JToken>breed in breeds)
            {
                if(breed.Value.HasValues)
                    Console.WriteLine($"Breed: {breed.Key}, Sub-Breeds: {breed.Value.ToString()}");
                else
                    Console.WriteLine($"Breed: {breed.Key}");
            }
            Console.WriteLine(Convert.ToString(apiMessage.message));
        }
        /// <summary>
        /// Gets the sub breeds.
        /// </summary>
        /// <param name="url">The URL.</param>
        static void GetSubBreeds(string url)
        {
            ApiMessage apiMessage = GetBreedInfo(url);
            Console.WriteLine(Convert.ToString(apiMessage.message));
        }
        /// <summary>
        /// Gets the random dog breed.
        /// </summary>
        /// <param name="url">The URL.</param>
        static void GetRandomDogBreed(string url)
        {
            GetBreedPhoto(url);
        }
        /// <summary>
        /// Gets the breed information.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        static ApiMessage GetBreedInfo(string url)
        {
            ApiMessage jsonItem = null;
            try
            {
                var response = httpClient.GetAsync(url).Result;
                jsonItem = JsonConvert.DeserializeObject<ApiMessage>(response.Content.ReadAsStringAsync().Result);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return jsonItem;
        }
        /// <summary>
        /// Gets the breed photo.
        /// </summary>
        /// <param name="url">The URL.</param>
        static void GetBreedPhoto(string url)
        {
            try
            {
                ApiMessage jsonItem = GetBreedInfo(url);
                string[] words = jsonItem.message.ToString().Split('/');
                using (WebClient client = new WebClient())
                {
                    string downloadedPath = $"{Path.GetTempPath()}{words[words.Count() - 1]}";
                    client.DownloadFile(new Uri(jsonItem.message.ToString()), downloadedPath);
                    Console.WriteLine($"The image is downloaded to the {downloadedPath}");

                    System.Diagnostics.Process.Start(downloadedPath);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
        /// <summary>
        /// Displays the command text.
        /// </summary>
        static void DisplayCommandText()
        {
            Console.WriteLine("Please select one of the available commands:");
            Console.WriteLine("All - list all available dog breeds.");
            Console.WriteLine("Random - retrieve a random dog breed.");
            Console.WriteLine("Subbreeds - list all breed's sub-breeds.");
            Console.WriteLine("Photo - get an image of selected breed");
            Console.WriteLine("Exit - exit the program");
        }
    }
}
