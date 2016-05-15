namespace TuxRacerLauncher
{
    public class Resolution
    {
        private int width;
        private int height;
        private int bpp;
        private int frequency;

        public int Width
        {
            get
            {
                return width;
            }
        }
        public int Height
        {
            get
            {
                return height;
            }
        }
        public int BPP
        {
            get
            {
                return bpp;
            }
        }
        public int Frequency
        {
            get
            {
                return frequency;
            }
        }

        public Resolution(int width, int height, int bpp, int frequency)
        {
            this.width = width;
            this.height = height;
            this.bpp = bpp;
            this.frequency = frequency;
        }

        public override string ToString()
        {
            return width + "x" + height + " (" + frequency + " Hz, " + bpp + " bpp)";
        }
    }
}
