using System.Text.Json;

namespace IX0WHB.Models
{
    internal class MatchFileHandler
    {
        private readonly string _filePath;

        public MatchFileHandler(string filePath)
        {
            _filePath = filePath;
        }

        public List<Match> LoadMatches()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    return JsonSerializer.Deserialize<List<Match>>(json) ?? new List<Match>();
                }
                Console.WriteLine("Nincs meglévő fájl. Új lista létrehozása.");
                return new List<Match>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba a fájl betöltésekor: {ex.Message}");
                return new List<Match>();
            }
        }

        public void SaveMatches(List<Match> matches)
        {
            try
            {
                string directory = "IX0WHB";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(matches, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
                Console.WriteLine("Adatok sikeresen mentve.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba a fájl mentésekor: {ex.Message}");
            }
        }

    }
}
