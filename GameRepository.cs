using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ezaim.data {
    
    public struct GameDataJson {
        public GameData[] Games {get; set;}
    }

    public struct GameData {
        public string Name {get; set;}
        public string ProcessName {get; set;}
        public int Value {get; set;}
    }
    
    public class GameRepository {
        public async Task<GameData[]> GetGames() {
            var text = await File.ReadAllTextAsync("./db.json");
            return JsonSerializer.Deserialize<GameDataJson>(text).Games;
        }

        public async Task<GameData[]> UpdateGame(GameData game) {
            var games = await GetGames();
            var stream = File.OpenWrite("./db.json");
            games = games.Where(item => item.Name != game.Name).ToArray();
            games = games.Append(game).ToArray();
            await JsonSerializer.SerializeAsync<GameDataJson>(stream, new GameDataJson {
                Games = games
            });
            stream.Close();
            return games;
        }
    }
}