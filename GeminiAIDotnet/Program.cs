using Newtonsoft.Json.Linq;
using RestSharp;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Write("API anahtarınızı girin: ");
        string apiKey = Console.ReadLine();

        string userInput = string.Empty;

        while (true)
        {
            Console.Write("AI'ya göndermek istediğiniz metni girin (Çıkmak için 'exit' yazın): ");
            userInput = Console.ReadLine();

            if (userInput.ToLower() == "exit")
            {
                break;
            }

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = userInput }
                        }
                    }
                }
            };

            var client = new RestClient($"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent?key={apiKey}");
            var request = new RestRequest("", Method.Post);

            request.AddJsonBody(requestBody);

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var jsonResponse = JObject.Parse(response.Content);
                string modelResponseText = jsonResponse["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

                Console.WriteLine("AI'dan Gelen Cevap:");
                Console.WriteLine(modelResponseText);
            }
            else
            {
                Console.WriteLine("İstek başarısız oldu.");
            }
        }
    }
}
