using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using ezaim.data;

namespace ezaim {    
    public class MainWindowViewModel : INotifyPropertyChanged {
                
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand ClickCommand {get; set;}
        public ICommand CalibrateCommand {get; set;}

        public ICommand ResetCommand {get; set;}
        
        private GameData[] _models {get; set;}

        private string[] _items;
        public string[] Items 
        {
            get {
                return _items;
            } 
            set {
                _items = value;
                if(PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Items)));
                }
            }
        }

        private string _selectedItem;
        public string SelectedItem {
            get {
                return _selectedItem;
            } set {
                _selectedItem = value;
                selectedItemChange();
                if(PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
                }
            }
        }

        private int _value = 0;
        public int Value {
            get {return _value;}
            set {
                _value = value;
                if(PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
        }

        private readonly ProcessWatcher _watcher;

        public MainWindowViewModel() {
            ClickCommand = new DelegateCommand((param) => true, (param) => onClick());
            CalibrateCommand = new DelegateCommand((param) => true, (param) => calibrate(Value));
            ResetCommand = new DelegateCommand((param) => true, (param) => Reset());
            _watcher = new ProcessWatcher(new GameRepository());
            _watcher.OnGameLoaded += OnGameLoaded;
            Task.Run(async () => {
                await init();
            });
        }

        private void OnGameLoaded(object sender, GameData game) {
            SelectedItem = game.Name;
        }

        private void Reset() {

        }

        private async Task init() {
            _models = await new GameRepository().GetGames();            
            Items = _models.Select(model => model.Name).ToArray();
            SelectedItem = Items[0];
            Value = _models[0].Value;
        }

        private void selectedItemChange() {
            Value = _models.First(item => item.Name == SelectedItem).Value;
        }

        private void calibrate(int sensitivity) {
            _watcher.setSensitivity(sensitivity);
        }

        private async Task onClick() {
            GameData data = new GameData {
                Name = SelectedItem,
                Value = Value
            };
            _models = await new GameRepository().UpdateGame(data);
            System.Windows.MessageBox.Show("Your settings have been saved");
        }
    }
}