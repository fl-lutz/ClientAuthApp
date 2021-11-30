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
            try
            {
                var certificate = new X509Certificate2(CERTIFICATEFILEPATH, System.IO.File.ReadAllText(PASSWORDFILEPATH));
                handler.ClientCertificates.Add(certificate);
            }
            catch(Exception e)
            {
                Console.WriteLine("There was a Problem with the Certificate");
                Console.WriteLine("Message: {0} ", e.Message);
                throw;
            }
            var client = new HttpClient(handler);
            CallEndpointAsync(client).Wait();
            Console.ReadKey();
        }

        static async Task CallEndpointAsync(HttpClient client)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await client.GetAsync(ENDPOINT);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Exception Caught!");
                Console.WriteLine("Message: {0} ", e.Message);
            }
            try
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);

                Console.WriteLine("The Certificate has worked against the specified Endpoint");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("There was a Problem with the Certificate");
                Console.WriteLine("Message: {0} ", e.Message);
            }
        }
    }
}