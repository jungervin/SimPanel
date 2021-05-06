using SimPanel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimPanel.ViewModel
{
    public class EventEditorViewModel : ObservableObject
    {
        public EventEditorViewModel()
        {
            OKCommand = new RelayCommand<Window>(p => OK(p), p => { return this.SelectedSimEvent != null; });

            CancelCommand = new RelayCommand<Window>(p => Cancel(p));

            this.EventList = new ObservableCollection<SimEvent>(Utility.StaticLists.SimConnectEvents);
        }

        private void Cancel(object p)
        {
            (p as Window).DialogResult = false;
        }

        private void OK(object p)
        {
            (p as Window).DialogResult = true;
        }

        private ObservableCollection<SimEvent> FEventList;

        public ObservableCollection<SimEvent> EventList
        {
            get { return FEventList; }
            set
            {
                FEventList = value;
                this.OnPropertyChanged();
            }
        }

        private SimEvent FSelectedSimEvent;

        public SimEvent SelectedSimEvent
        {
            get { return FSelectedSimEvent; }
            set
            {
                FSelectedSimEvent = value;
                this.OnPropertyChanged();
            }
        }

        private string FFilter;

        public string Filter
        {
            get { return FFilter; }
            set
            {
                FFilter = value.ToUpper();

                this.EventList = new ObservableCollection<SimEvent>(Utility.StaticLists.SimConnectEvents.Where(k => k.EventName.Contains(FFilter)).ToList());
                this.OnPropertyChanged();
            }
        }

        public RelayCommand<Window> OKCommand { get; }
        public RelayCommand<Window> CancelCommand { get; }
    }
}
