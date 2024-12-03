using IX0WHB.Models;
namespace IX0WHB.Views
{
    internal static class ConsoleView
    {
        public static void ShowMenu()
        {
            Console.WriteLine("\nVálassz egy opciót:");
            Console.WriteLine("1. Mérkőzések listázása");
            Console.WriteLine("2. Mérkőzések szűrése hazai gólok szerint");
            Console.WriteLine("3. Mérkőzés hozzáadása");
            Console.WriteLine("4. Mentés fájlba");
            Console.WriteLine("5. Tabella megjelenítése");
            Console.WriteLine("6. Mérkőzés törlése");
            Console.WriteLine("0. Kilépés");
        }

        public static void ShowMatches(List<Match> matches)
        {
            Console.WriteLine("\nMérkőzések:");
            if (matches.Count == 0)
            {
                Console.WriteLine("Nincs elérhető adat.");
                return;
            }

            foreach (var match in matches)
            {
                Console.WriteLine(
                    $"Hazai: {match.HomeTeam}, Vendég: {match.AwayTeam}, " +
                    $"Helyszín: {match.Place}, Dátum: {match.Date:yyyy-MM-dd}, " +
                    $"Hazai gólok: {match.HomeGoals}, Vendég gólok: {match.AwayGoals}");
            }
        }

        public static void ShowTable(List<TeamStats> table)
        {
            if (table == null || table.Count == 0)
            {
                Console.WriteLine("Nincs adat a tabella megjelenítéséhez.");
                return;
            }
            Console.WriteLine("\nTabella:");
            Console.WriteLine(new string('-', 90));
            Console.WriteLine(
                $"{"Helyezés",-10} | {"Csapat neve",-20} | {"Lejátszott",-15} | {"Győzelem",-10} | {"Döntetlen",-10} | {"Vereség",-10} | {"Lőtt gólok",-10} | {"Kapott gólok",-10} | {"Pontszám",-10}");
            Console.WriteLine(new string('-', 90));
            foreach (var row in table)
            {
                Console.WriteLine(
                    $"{row.Position,-10} | {row.TeamName,-20} | {row.Played,-15} | {row.Wins,-10} | {row.Draws,-10} | {row.Losses,-10} | {row.Scored,-10} | {row.Conceded,-10} | {row.Points,-10}");
            }

            Console.WriteLine(new string('-', 90));
        }




        public static int GetIntegerInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    return result;
                }
                Console.WriteLine("Érvénytelen szám, próbáld újra.");
            }
        }

        public static DateTime GetDateInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (DateTime.TryParse(Console.ReadLine(), out DateTime result))
                {
                    return result;
                }
                Console.WriteLine("Érvénytelen dátum, próbáld újra (YYYY-MM-DD formátumban).");
            }
        }

        public static string GetStringInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? string.Empty;
        }
    }
}
