namespace DuplicateImageFInder
{
    internal static class Program
    {
        //There is no GUI for selecting images yet
        //Most of the program is inside Form1.cs and Algorithm.cs <--------------

        //if true it will compare two images, if false it will compare 1 image to a folder of images
        public static bool compareTwo = true;

        //paths for if compareTwo = false
        public static string FolderToCheckPath = @"Enter a valid path here";
        public static string ImageToFindDuplicatesOfPath = @"Enter a valid path here";

        //paths for if compareTwo = true
        public static string imageA = @"Enter a valid path here";
        public static string imageB = @"Enter a valid path here";

        //threads only make a difference when comparing 1 image to a folder of images
        public static int numThreads = -1;//-1 will auto detect

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (numThreads == -1) numThreads = System.Environment.ProcessorCount;
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        public static Bitmap OpenFileAsBmp(string path)
        {
            Bitmap b = null;
            try
            {
                Image a = Image.FromFile(path);
                b = new Bitmap(a);
            }
            catch { }
            return b;
        }
    }
}