using IX0WHB.Controllers;

namespace IX0WHB
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Sporteredmények Nyilvántartó");
            var controller = new MatchController("matches.json");
            controller.Run();
        }
    }
}
