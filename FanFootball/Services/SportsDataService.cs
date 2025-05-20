using FanFootball.Models;
using System.Net.Http;
using System.Text.Json;

namespace FanFootball.Services
{
    public class SportsDataService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "7eb120e676024668a21c631546daa43c"; //ma key
        private const string BaseUrl = "https://api.sportsdata.io/v3/nfl/projections/json/PlayerSeasonProjectionStats/2024REG";


        public SportsDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<player>> GetFantasyPlayersAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}?key={ApiKey}");
            if (!response.IsSuccessStatusCode)
                return new List<player>();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var players = new List<player>();

            foreach (var item in root.EnumerateArray())
            {
                if (item.TryGetProperty("DefensiveSnapsPlayed", out var defSnaps) && defSnaps.ValueKind != JsonValueKind.Null)
                    continue;
                var p = new player();

                if (item.TryGetProperty("PlayerID", out var id)) p.Id = id.GetInt32();
                if (item.TryGetProperty("Name", out var name)) p.Name = name.GetString() ?? "";
                if (item.TryGetProperty("Team", out var team)) p.Team = team.GetString() ?? "N/A";
                if (item.TryGetProperty("Position", out var pos)) p.Position = pos.GetString() ?? "N/A";
                p.Age = 0;
                if (item.TryGetProperty("FantasyPoints", out var points)) p.Points = points.GetDouble();
                if (item.TryGetProperty("AverageDraftPosition", out var adp) && adp.ValueKind == JsonValueKind.Number)
                    p.ADP = adp.GetDouble();
                else
                    p.ADP = 999; 


                players.Add(p);
            }

            return players;
        }

        public async Task<string> GetRawFantasyJson()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}?key={ApiKey}");
            return await response.Content.ReadAsStringAsync();
        }
    }
}