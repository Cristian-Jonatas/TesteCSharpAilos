using Newtonsoft.Json;

public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        int totalGoals = 0;
        using (HttpClient client = new HttpClient())
        {
            int totalPages = 1;

            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page=1";
            var response = client.GetStringAsync(url).Result;
            RetornoDados jsonData = JsonConvert.DeserializeObject<RetornoDados>(response);
            totalPages = jsonData.total_pages;

            for (int currentPageTeam1 = 1; currentPageTeam1 <= totalPages; currentPageTeam1++)
            {
                url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={currentPageTeam1}";
                response = client.GetStringAsync(url).Result;
                jsonData = JsonConvert.DeserializeObject<RetornoDados>(response);

                foreach (var match in jsonData.data)
                {
                    totalGoals += int.Parse(match.team1goals.ToString());
                }
            }

            url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}&page=1";
            response = client.GetStringAsync(url).Result;
            jsonData = JsonConvert.DeserializeObject<RetornoDados>(response);
            totalPages = jsonData.total_pages;

            for (int currentPageTeam2 = 1; currentPageTeam2 <= totalPages; currentPageTeam2++)
            {
                url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}&page={currentPageTeam2}";
                response = client.GetStringAsync(url).Result;
                jsonData = JsonConvert.DeserializeObject<RetornoDados>(response);

                foreach (var match in jsonData.data)
                {
                    totalGoals += int.Parse(match.team2goals.ToString());
                }
            }
        }

        return totalGoals;
    }

    private class RetornoDados
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public DadosJogos[] data { get; set; }
    }

    private class DadosJogos
    {
        public string competition { get; set; }
        public int year { get; set; }
        public string round { get; set; }
        public string team1 { get; set; }
        public string team2 { get; set; }
        public string team1goals { get; set; }
        public string team2goals { get; set; }
    }
}