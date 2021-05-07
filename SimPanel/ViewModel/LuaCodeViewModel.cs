using MoonSharp.Interpreter;
using SimPanel.Model;
using SimPanel.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimPanel.ViewModel
{
    public class LuaCodeViewModel : BaseViewModel
    {
        public LuaCodeViewModel()
        {
            // TODO:
            RunCommand = new RelayCommand(p =>
            {
                Run(p.ToString());
            });

            this.LuaSrcipt = new Script();

            this.LuaSrcipt.Globals["GetVariableValue"] = (Func<string, double>)GetVariableValue;
            this.LuaSrcipt.Globals["SendEvent"] = (Func<string, uint, int>)SendEvent;
        }

        public void Run(string serialdata)
        {
            if (this.Code != null)
            {
                try
                {
                    try
                    {
                        DynValue src = LuaSrcipt.DoString(this.Code);
                        DynValue res = LuaSrcipt.Call(LuaSrcipt.Globals["Process"], serialdata);
                        this.Result = res.ToString(); //.Replace("\"", "");
                    }
                    catch (SyntaxErrorException ex)
                    {
                        this.Result = $"SyntaxError: {ex.DecoratedMessage}";
                        return;
                    }
                    catch (ScriptRuntimeException ex)
                    {
                        this.Result = $"RuntimeError: {ex.DecoratedMessage}";
                        return;
                    }
                }
                catch (Exception exx)
                {
                    this.Result = $"Exception: {exx.Message}";
                }
            }
        }

        public static double GetVariableValue(string varname)
        {
            SimVar v = Globals.MainWindow.SimConnectViewModel.VarList.Where(k => k.VarName == varname).FirstOrDefault();
            if (v != null)
            {
                return v.Value == null ? double.NaN : (double)(v.Value);
            }
            return double.NaN;
        }

        public static int SendEvent(string eventname, uint value)
        {
            if (!String.IsNullOrEmpty(eventname))
            {
                SimEvent ev = Globals.MainWindow.SimConnectViewModel.EventList.Where(k => k.EventName == eventname).FirstOrDefault();
                if (ev != null)
                {
                    Globals.MainWindow.SimConnectViewModel.SendEvent(ev, value);
                    return 1;
                }
            }

            return 0;
        }


        public bool LoadFromFile(string filename)
        {
            try
            {
                this.Code = File.ReadAllText(filename);
                this.CodeModified = false;
                this.FileName = filename;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        public void SaveLuaToFile(string filename)
        {
            File.WriteAllText(filename, this.Code);
            this.FileName = filename;
            this.CodeModified = false;
        }

        public string FileName
        {
            get { return Settings.Default.LastLuaFile; }
            set
            {
                Settings.Default.LastLuaFile = value;
                Settings.Default.Save();
                this.OnPropertyChanged();
            }
        }


        private string FCode;

        public string Code
        {
            get { return FCode; }
            set
            {
                FCode = value;
                this.CodeModified = true;
                this.OnPropertyChanged();
            }
        }

        private string FResult;

        public string Result
        {
            get { return FResult; }
            set
            {
                FResult = value;
                this.OnPropertyChanged();
            }
        }

        private bool FCodeModified;

        public bool CodeModified
        {
            get { return FCodeModified; }
            set
            {
                FCodeModified = value;
                this.OnPropertyChanged();
            }
        }

        public RelayCommand RunCommand { get; }

        private readonly Script LuaSrcipt;
    }
}
