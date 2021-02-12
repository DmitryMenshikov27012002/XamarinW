using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            
        }
    }

    public class MainPageViewModel : ViewModelBase
    {
        private string _buttonText = string.Empty;

        public string ButtonText 
        {
            get => _buttonText;
            set
            {
                _buttonText = value;
                InvokePropertyChanged();
            }
        }

        private ICommand _buttonClicked = default;
        public ICommand ButtonClicked
        {
            get
            {
                if (_buttonClicked == default)
                    _buttonClicked = new RelayCommand((_) => 
                    {
                        ButtonText = _rand.Next(0, 1000).ToString();
                    });
                return _buttonClicked;
            }
        }


        private ObservableCollection<ListViewItemModel> _itemsCollection = new ObservableCollection<ListViewItemModel>();
        public ObservableCollection<ListViewItemModel> ItemsCollection 
        {
            get => _itemsCollection;
            set
            {
                _itemsCollection = value;
                InvokePropertyChanged();
            }
        }

        private string _entryText = string.Empty;
        public string EntryText
        {
            get => _entryText;
            set
            {
                _entryText = value;
                InvokePropertyChanged();
            }
        }

        private ICommand _addEntityCommand = default;
        public ICommand AddEntityCommand
        {
            get
            {
                return _addEntityCommand ?? (_addEntityCommand = new RelayCommand((_)=> 
                {
                    if (ItemsCollection.FirstOrDefault(m => m.StringField == EntryText) == default)
                        ItemsCollection.Insert(0, new ListViewItemModel() { Id = _rand.Next(0, 10000), StringField = EntryText });
                }));
            }
        }

        private Random _rand = new Random();
    }

    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;


        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action<object> execute) : this(execute, (_) => true) { }

        public bool CanExecute(object parameter) => _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);

        private Action<object> _execute;
        private Func<object, bool> _canExecute;
    }


    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    
        protected void InvokePropertyChanged([CallerMemberName] string memberName = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(memberName));
        }
    }

    public class ListViewItemModel : ViewModelBase
    {
        public int Id { get; set; }
        public string StringField { get; set; }


    }

}
