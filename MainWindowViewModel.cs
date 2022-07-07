using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Text.Json;
using System.Linq;
using Microsoft.Win32;

namespace ezaim {
    
    struct GameModel {
        public string Name {get; set;}
        public int Value {get; set;}
    }

    struct GameModelJson {
        public GameModel[] Models {get; set;}
    }
    
    public class MainWindowViewModel : INotifyPropertyChanged {
                
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand ClickCommand {get; set;}
        
        public string[] Items 
        {
            get; set;
        }


        private string _selectedItem;
        public string SelectedItem {
            get {
                return _selectedItem;
            } set {
                _selectedItem = value;
                if(PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
                }
            }
        }

        private int _value;
        public int Value {
            get {return _value;}
            set {
                _value = value;
                if(PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
        }

        public MainWindowViewModel() {
            ClickCommand = new DelegateCommand((param) => true, (param) => onClick());
            init();
        }

        private async Task init() {
            var stream = File.OpenRead("./db.json");
            var models = await JsonSerializer.DeserializeAsync<GameModelJson>(stream);
            Items = models.Models.Select(model => model.Name).ToArray();
            SelectedItem = Items[0];
        }

        private void onClick() {
            //actually setting the registry key and loading and saving changes to json
        }
    }
}