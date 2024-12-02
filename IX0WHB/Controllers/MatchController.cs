﻿using System.Text.Json;
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
                    case "5":
                        ShowTable();
                        break;
                    case "0":
                        exit = true;
                        break;
                    case "6":
                        DeleteMatch();
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

            Console.WriteLine("Meccs hozzáadva:");
            Console.WriteLine(JsonSerializer.Serialize(_matches, new JsonSerializerOptions { WriteIndented = true }));

            SaveMatches();
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
            Console.WriteLine("Meccsek mentve.");
        }

        private void DeleteMatch()
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
                Console.WriteLine("Érvénytelen index.");
            }
        }


        private void ShowTable()
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
