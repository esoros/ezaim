using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ezaim.data;
using System.Collections.Generic;

namespace ezaim {
    public class ProcessWatcher {

        private Dictionary<string, bool> _processedMap = new Dictionary<string, bool>();
        public delegate void OnGameLoadedEventHandler(object sender, GameData game);
        public event OnGameLoadedEventHandler OnGameLoaded;
        public ProcessWatcher(GameRepository repo) {
            Task.Run(async () => {
                await watchForProcessLaunch(repo);
            });
        }
        public void setSensitivity(int value) {
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Control Panel\\Mouse","MouseSensitivity", value, Microsoft.Win32.RegistryValueKind.String);
        }
        private async Task watchForProcessLaunch(GameRepository repository) {
            GameData[] games = await repository.GetGames();
            while(true) {
                foreach(var game in games) {
                    var processses = Process.GetProcessesByName(game.ProcessName);
                    if(processses.Length > 0 && !_processedMap.ContainsKey(game.ProcessName)) {
                        System.Console.WriteLine("processes found", processses);
                        _processedMap[game.ProcessName] = true;
                        setSensitivity(game.Value);
                        if(OnGameLoaded != null) {
                            OnGameLoaded(this, game);
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}