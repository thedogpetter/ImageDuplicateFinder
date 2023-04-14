namespace DuplicateImageFInder
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
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
                a.Dispose();
            }
            catch { }
            return b;
        }
    }
}