namespace IX0WHB.Models
{
    public record Match(
        string HomeTeam,
        string AwayTeam,
        string Place,
        DateTime Date,
        int HomeGoals,
        int AwayGoals);
}
