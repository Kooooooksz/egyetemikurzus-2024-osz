using IX0WHB.Models;
using IX0WHB.Views;

namespace IX0WHB.Controllers
{
    public class MatchController
    {
        private readonly MatchFileHandler _fileHandler;
        private readonly List<Match> _matches;

        public MatchController(string filePath)
        {
            _fileHandler = new MatchFileHandler(filePath);
            _matches = _fileHandler.LoadMatches();
        }

        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                ConsoleView.ShowMenu();
                string choice = Console.ReadLine() ?? "0";

                switch (choice)
                {
                    case "1":
                        ConsoleView.ShowMatches(_matches);
                        break;
                    case "2":
                        FilterMatches();
                        break;
                    case "3":
                        AddMatch();
                        break;
                    case "4":
                        SaveMatches();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Érvénytelen opció!");
                        break;
                }
            }
        }

        private void AddMatch()
        {
            string homeTeam = ConsoleView.GetStringInput("Hazai csapat neve: ");
            string awayTeam = ConsoleView.GetStringInput("Vendég csapat neve: ");
            string place = ConsoleView.GetStringInput("Helyszín: ");
            DateTime date = ConsoleView.GetDateInput("Dátum (YYYY-MM-DD): ");
            int homeGoals = ConsoleView.GetIntegerInput("Hazai gólok száma: ");
            int awayGoals = ConsoleView.GetIntegerInput("Vendég gólok száma: ");

            _matches.Add(new Match(homeTeam, awayTeam, place, date, homeGoals, awayGoals));
        }

        private void FilterMatches()
        {
            int minGoals = ConsoleView.GetIntegerInput("Minimum hazai gólok száma: ");
            var filteredMatches = _matches
                .Where(m => m.HomeGoals >= minGoals)
                .OrderByDescending(m => m.HomeGoals)
                .ToList();

            ConsoleView.ShowMatches(filteredMatches);
        }

        private void SaveMatches()
        {
            _fileHandler.SaveMatches(_matches);
        }
    }
}
