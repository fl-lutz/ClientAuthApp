using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace TestClientAuthCert
{
    class Program
    {
        static HttpClientHandler handler = new HttpClientHandler();
        static readonly string CERTIFICATEFILEPATH = "../../../certificate.pfx";
        static readonly string PASSWORDFILEPATH = "../../../pwd.txt";
        static readonly string ENDPOINT = "https://pki.zeiss.org/pkiWebDev/test";


        static void Main()
        {
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var certificate = new X509Certificate2(CERTIFICATEFILEPATH, System.IO.File.ReadAllText(PASSWORDFILEPATH));
            handler.ClientCertificates.Add(certificate);
            var client = new HttpClient(handler);
            CallEndpointAsync(client).Wait();
            Console.ReadKey();
        }

        static async Task CallEndpointAsync(HttpClient client)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(ENDPOINT);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Exception Caught!");
                Console.WriteLine("Message: {0} ", e.Message);
            }
        }
    }
}