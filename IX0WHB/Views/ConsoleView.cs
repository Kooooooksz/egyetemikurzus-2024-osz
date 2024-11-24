namespace IX0WHB.Views
{
    public static class ConsoleView
    {
        public static void ShowMenu()
        {
            Console.WriteLine("\nVálassz egy opciót:");
            Console.WriteLine("1. Mérkőzések listázása");
            Console.WriteLine("2. Mérkőzések szűrése hazai gólok szerint");
            Console.WriteLine("3. Mérkőzés hozzáadása");
            Console.WriteLine("4. Mentés fájlba");
            Console.WriteLine("0. Kilépés");
        }

        public static void ShowMatches(List<IX0WHB.Models.Match> matches)
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
