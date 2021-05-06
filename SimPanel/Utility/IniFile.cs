using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPanel.Utility
{
    public class IniFile
    {
        private string[] Text = null;
        public IniFile()
        {

        }

        public bool LoadFromFile(string filename)
        {
            if (File.Exists(filename))
            {
                this.Text = File.ReadAllLines(filename);
                return this.Text.Length > 0;
            }

            return false;
        }

        public string GetValue(string section, string keyword, string def = "")
        {
            section = ("[" + section + "]").ToLower();
            keyword = keyword.ToLower();
            bool found = false;
            for (int i = 0; i < this.Text.Length; i++)
            {
                string text = this.Text[i].Trim();
                if (text != "")
                {
                    if (!found)
                    {
                        found = text.StartsWith("[") && text.Trim().ToLower() == section;
                    }
                    else
                    {
                        if (text.StartsWith("["))
                        {
                            return def;
                        }
                        else
                        {
                            string[] parts = text.Split('=');
                            if (parts.Length == 2 && parts[0].Trim().ToLower() == keyword)
                            {
                                return parts[1].Trim();
                            }
                        }
                    }

                }
            }
            return def;
        }

        public int GetValue(string section, string keyword, int def)
        {
            string res = this.GetValue(section, keyword, def.ToString());
            if (int.TryParse(res, out def))
            {
                return def;
            }
            return def;
        }
        public Single GetValue(string section, string keyword, Single def)
        {
            string res = this.GetValue(section, keyword, def.ToString());
            if (Single.TryParse(res, out def))
            {
                return def;
            }
            return def;
        }
        public double GetValue(string section, string keyword, double def)
        {
            string res = this.GetValue(section, keyword, def.ToString());
            if (double.TryParse(res, out def))
            {
                return def;
            }
            return def;
        }

        public bool GetValue(string section, string keyword, bool def)
        {
            return this.GetValue(section, keyword, def.ToString()).ToLower() == "true";
        }

    }
}
