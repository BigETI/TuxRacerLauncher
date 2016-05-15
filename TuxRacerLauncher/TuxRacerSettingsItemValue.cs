namespace TuxRacerLauncher
{
    public class TuxRacerSettingsItemValue<T> : ITuxRacerSettingsItemValue
    {
        private T val;

        public T Value
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

        public TuxRacerSettingsItemValue(T val)
        {
            this.val = val;
        }

        public override string ToString()
        {
            string ret = val.ToString();
            if (val is string)
                ret = "\"" + val.ToString() + "\"";
            if (val is bool)
                ret = val.ToString().ToLower();
            return ret;
        }
    }
}
