using System.Text.Json;
using IX0WHB.Models;
using IX0WHB.Views;

namespace IX0WHB.Controllers
{
    internal class MatchController
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
                var menuActions = new Dictionary<string, Action>
                {
                    { "1", () => ConsoleView.ShowMatches(_matches) },
                    { "2", FilterMatches },
                    { "3", AddMatch },
                    { "4", ShowTable },
                    { "5", DeleteMatch },
                    { "0", () => exit = true }
                };

                while (!exit)
                {
                    ConsoleView.ShowMenu();
                    string choice = Console.ReadLine() ?? "0";

                    if (menuActions.TryGetValue(choice, out var action))
                    {
                        action();
                    }
                    else
                    {
                        Console.WriteLine("Érvénytelen opció!");
                    }
                }

            }
        }

        public void AddMatch()
        {
            try
            {
                string homeTeam = ConsoleView.GetStringInput("Hazai csapat neve: ");
                string awayTeam = ConsoleView.GetStringInput("Vendég csapat neve: ");
                string place = ConsoleView.GetStringInput("Helyszín: ");
                DateTime date = ConsoleView.GetDateInput("Dátum (YYYY-MM-DD): ");
                int homeGoals = ConsoleView.GetIntegerInput("Hazai gólok száma: ");
                int awayGoals = ConsoleView.GetIntegerInput("Vendég gólok száma: ");

                _matches.Add(new Match(homeTeam, awayTeam, place, date, homeGoals, awayGoals));
                Console.WriteLine("Meccs hozzáadva:");
               

                SaveMatches();
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Hiba: Érvénytelen adatbevitel. {ex.Message}");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Hiba: Egy vagy több kötelező mező üres. {ex.Message}");
            }
        }

        public void FilterMatches()
        {
            int minGoals = ConsoleView.GetIntegerInput("Minimum hazai gólok száma: ");
            var filteredMatches = _matches
                .Where(m => m.HomeGoals >= minGoals)
                .OrderByDescending(m => m.HomeGoals)
                .ToList();

            ConsoleView.ShowMatches(filteredMatches);
        }

        public void SaveMatches()
        {
            try
            {
                _fileHandler.SaveMatches(_matches);
                Console.WriteLine("Meccsek mentve.");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Mentési hiba: Nincs megfelelő jogosultság a fájl mentéséhez. {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Mentési hiba: A fájl írása közben hiba történt. {ex.Message}");
            }
        }

        public void DeleteMatch()
        {
            try
            {
                ConsoleView.ShowMatches(_matches);
                int index = ConsoleView.GetIntegerInput("Add meg a törlendő mérkőzés sorszámát (0-tól kezdődően): ");

                if (index >= 0 && index < _matches.Count)
                {
                    Console.WriteLine($"A következő mérkőzés törlésre kerül: {_matches[index]}");
                    _matches.RemoveAt(index);

                    SaveMatches();
                    Console.WriteLine("Mérkőzés sikeresen törölve.");
                }
                else
                {
                    Console.WriteLine("Hiba: Érvénytelen index.");
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Hiba: Index túlmutat a lista határain. {ex.Message}");
            }
        }


        public void ShowTable()
        {
            var teamStats = new Dictionary<string, (int Played, int Scored, int Conceded, int Points, int Wins, int Draws, int Losses)>();

            foreach (var match in _matches)
            {
                if (!teamStats.ContainsKey(match.HomeTeam))
                {
                    teamStats[match.HomeTeam] = (0, 0, 0, 0, 0, 0, 0);
                }

                if (!teamStats.ContainsKey(match.AwayTeam))
                {
                    teamStats[match.AwayTeam] = (0, 0, 0, 0, 0, 0, 0);
                }
                teamStats[match.HomeTeam] = (
                    teamStats[match.HomeTeam].Played + 1,
                    teamStats[match.HomeTeam].Scored + match.HomeGoals,
                    teamStats[match.HomeTeam].Conceded + match.AwayGoals,
                    teamStats[match.HomeTeam].Points + (match.HomeGoals > match.AwayGoals ? 3 : match.HomeGoals == match.AwayGoals ? 1 : 0),
                    teamStats[match.HomeTeam].Wins + (match.HomeGoals > match.AwayGoals ? 1 : 0),
                    teamStats[match.HomeTeam].Draws + (match.HomeGoals == match.AwayGoals ? 1 : 0),
                    teamStats[match.HomeTeam].Losses + (match.HomeGoals < match.AwayGoals ? 1 : 0)
                );

                teamStats[match.AwayTeam] = (
                    teamStats[match.AwayTeam].Played + 1,
                    teamStats[match.AwayTeam].Scored + match.AwayGoals,
                    teamStats[match.AwayTeam].Conceded + match.HomeGoals,
                    teamStats[match.AwayTeam].Points + (match.AwayGoals > match.HomeGoals ? 3 : match.AwayGoals == match.HomeGoals ? 1 : 0),
                    teamStats[match.AwayTeam].Wins + (match.AwayGoals > match.HomeGoals ? 1 : 0),
                    teamStats[match.AwayTeam].Draws + (match.AwayGoals == match.HomeGoals ? 1 : 0),
                    teamStats[match.AwayTeam].Losses + (match.AwayGoals < match.HomeGoals ? 1 : 0)
                );
            }

            var table = teamStats
                .OrderByDescending(t => t.Value.Points)
                .ThenByDescending(t => t.Value.Scored - t.Value.Conceded)
                .ThenByDescending(t => t.Value.Scored)                
                .Select((team, index) => new TeamStats
                {
                    Position = index + 1,
                    TeamName = team.Key,
                    Played = team.Value.Played,
                    Wins = team.Value.Wins,
                    Draws = team.Value.Draws,
                    Losses = team.Value.Losses,
                    Scored = team.Value.Scored,
                    Conceded = team.Value.Conceded,
                    Points = team.Value.Points
                })
                .ToList();

            ConsoleView.ShowTable(table);
        }

    }
}
