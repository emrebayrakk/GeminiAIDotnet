using Newtonsoft.Json.Linq;
using RestSharp;
using System.Text;

namespace GeminiAIWinFormApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string apiKey = textBox1.Text;
            string userInput = textBox2.Text;

            if (apiKey == null || apiKey == "")
            {
                DialogResult result1 = MessageBox.Show("Api key and message cannot be empty!",
               "Exit", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    this.Close();
                }
                else
                {
                    Close();
                }
            }

            button1.Enabled = false;

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

            var response = client.ExecuteAsync(request).Result;

            if (response.IsSuccessful)
            {
                var jsonResponse = JObject.Parse(response.Content);
                string modelResponseText = jsonResponse["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"You : {userInput}");
                sb.AppendLine($"AI : {modelResponseText}");
                richTextBox1.Text += sb.ToString();
            }
            else
            {
                richTextBox1.Text += $"Error \n";
            }
            button1.Enabled = true;

        }
    }
}
