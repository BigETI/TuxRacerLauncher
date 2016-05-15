using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace TuxRacerLauncher
{
    public class TuxRacerSettingsItem
    {
        private List<string> comments = new List<string>();
        private string key;
        private ITuxRacerSettingsItemValue val;

        public string Key
        {
            get
            {
                return key;
            }
        }

        public ITuxRacerSettingsItemValue Value
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
            }
        }

        public string Comments
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (string i in comments)
                {
                    sb.Append("#");
                    if (i.Length > 0)
                    {
                        sb.Append(" ");
                        sb.Append(i.ToString());
                    }
                    sb.Append("\r\n");
                }
                return sb.ToString();
            }
        }

        public TuxRacerSettingsItem(string key, ITuxRacerSettingsItemValue val)
        {
            this.key = key;
            this.val = val;
        }

        public void AddComment(string comment)
        {
            comments.Add(comment);
        }

        public T GetValue<T>()
        {
            return ((TuxRacerSettingsItemValue<T>)val).Value;
        }

        public ToolTip SetToolTip(Control ctrl)
        {
            ToolTip ret = new ToolTip();
            ret.AutoPopDelay = 5000;
            ret.InitialDelay = 1000;
            ret.ReshowDelay = 500;
            ret.ShowAlways = true;
            ret.ToolTipTitle = key;
            ret.SetToolTip(ctrl, Comments);
            return ret;
        }

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append("\r\n");
            ret.Append(Comments);
            ret.Append(key);
            ret.Append(" ");
            ret.Append(val.ToString());
            ret.Append("\r\n");
            return ret.ToString();
        }
    }
}
