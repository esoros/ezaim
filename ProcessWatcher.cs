using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ezaim {
    public class ProcessWatcher {
                
        private readonly string[] names = new string[]{"OverwatchApplication", "CsGO"};

        ProcessWatcher() {
            Task.Run(watchForProcessLaunch);
        }

        private void watchForProcessLaunch() {
            while(true) {
                foreach(var name in names) {
                    var processses = Process.GetProcessesByName(name);
                    if(processses.Length != 0) {
                        //set the registry key to update the sensitivity based on the game
                        
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}