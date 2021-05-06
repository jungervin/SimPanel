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
    public class VariableEditorViewModel : ObservableObject
    {

        public VariableEditorViewModel()
        {
            OKCommand = new RelayCommand<Window>(p => OK(p), p => { return this.SelectedSimVar != null; });
            CancelCommand = new RelayCommand<Window>(p => Cancel(p));

            this.VarList = new ObservableCollection<SimVar>(Utility.StaticLists.SimConnectVariables);
        }

        private void Cancel(object p)
        {
            (p as Window).DialogResult = false;
        }

        private void OK(object p)
        {
            (p as Window).DialogResult = true;
        }
        private ObservableCollection<SimVar> FVarList;

        public RelayCommand<Window> OKCommand { get; private set; }
        public RelayCommand<Window> CancelCommand { get; private set; }

        public ObservableCollection<SimVar> VarList
        {
            get { return FVarList; }
            set
            {
                FVarList = value;
                this.OnPropertyChanged();
            }
        }

        private SimVar FSelectedSimvar;

        public SimVar SelectedSimVar
        {
            get { return FSelectedSimvar; }
            set
            {
                FSelectedSimvar = value;
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

                this.VarList = new ObservableCollection<SimVar>(Utility.StaticLists.SimConnectVariables.Where(k => k.VarName.Contains(FFilter)).ToList());

                this.OnPropertyChanged();
            }
        }

        private int FVarIndex;

        public int VarIndex
        {
            get { return FVarIndex; }
            set
            {
                FVarIndex = value;
                this.OnPropertyChanged();
            }
        }

    }
}
