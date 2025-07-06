using Milau.Models;
using Swed64;
using System.Numerics;

namespace Milau
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Login();
        }

        static async Task Login()
        {
            Authorization auth = new Authorization("1234", "1234");
            bool isValid = await auth.Login();

            if (isValid)
            {
                Console.WriteLine("Login validated!");
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("Launching Milau...");
                System.Threading.Thread.Sleep(2000);
                Launch();
            }
            else
            {
                return;
            }
        }

        static void Launch()
        {
            Execute execute = new Execute();
            execute.Run();
        }
    }
}   
