using ServiceReference1;
using System.Security.Cryptography.Xml;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using AltinKaynakASMXGetCurrency.Entities;
namespace AltinKaynakASMXGetCurrency
{
    class Program
    {
        static void Main(string[] args)
        {
            GetCurrencyAsync().Wait();
            Console.ReadKey();
        }
        static async Task GetCurrencyAsync()
        {
            string username = "AltinkaynakWebServis";
            string password = "AltinkaynakWebServis";
            ServiceReference1.DataServiceSoapClient client = new ServiceReference1.DataServiceSoapClient(ServiceReference1.DataServiceSoapClient.EndpointConfiguration.DataServiceSoap12);
            ServiceReference1.GetCurrencyRequest request = new ServiceReference1.GetCurrencyRequest();
            ServiceReference1.GetCurrencyResponse response = await client.GetCurrencyAsync(new AuthHeader() { Username = username, Password = password });
            if (response != null && response.GetCurrencyResult != null)
            {
                string xmldata = response.GetCurrencyResult;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmldata);
                XmlNodeList currencynodes = doc.SelectNodes("/Kurlar/Kur");
                foreach (XmlNode currencynode in currencynodes)
                {
                    //kod aciklama guncellemezamanı alis satis
                    string kod = currencynode.SelectSingleNode("Kod").InnerText;
                    string aciklama = currencynode.SelectSingleNode("Aciklama").InnerText;
                    string alis = currencynode.SelectSingleNode("Alis").InnerText;
                    string satis = currencynode.SelectSingleNode("Satis").InnerText;
                    string guncellemezamani = currencynode.SelectSingleNode("GuncellenmeZamani").InnerText;
                    Console.WriteLine($"Kod : {kod}");
                    Console.WriteLine($"Aciklama : {aciklama}");
                    Console.WriteLine($"Alis: {alis}");
                    Console.WriteLine($"Satis : {satis}");
                    Console.WriteLine($"Guncelleme Zamani: {guncellemezamani}");

                    CurDb db = new CurDb();
                    Currency  currency = new Currency()
                    {
                        Kod = kod,
                        Aciklama = aciklama,
                        Alis = alis,
                        Satis = satis,
                        GuncellenmeZamani = guncellemezamani
                    };


                    db.Add(currency);
                    db.SaveChanges();
                    Console.WriteLine("Added to database!");

                }
            }
            else
            {
                Console.WriteLine("Veri yok");
            }
        }

        public class CurDb : DbContext
        {
            public DbSet<Currency> CurrencyProp { get; set; }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=CurrencyProgram;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate = True");
            }
        }
    }
}
