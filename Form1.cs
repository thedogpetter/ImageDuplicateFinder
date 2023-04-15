using System.IO;
using System.Text;

namespace DuplicateImageFInder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            if (Program.compareTwo) Compare2();
            else Start(10, 4, 24);

            /*
            Bitmap a = Program.OpenFileAsBmp(Program.ImageToFindDuplicatesOfPath);
            a = Algorithm.DownscaleImage(a, Algorithm.GetDownscalePercentage(a.Width, a.Height));
            Color[,] c = Algorithm.AverageImage(a, 10);
            //a = Algorithm.ConvertAveragesToImage(c, 10, 1);
            

            pictureBox1.Image = a;
            */
        }


        void Start(int slices, float percentDifferenceToFail, int threads = 1)
        {
            string[] files = Directory.GetFiles(Program.FolderToCheckPath, "*", SearchOption.AllDirectories);
            string[] files2 = new string[files.Length];

            //Get an array of only accepted image types
            string type;
            int passed = 0;
            for (int i = 0; i < files.Length; i++)
            {
                string[] splt = files[i].Split(".");
                type = splt[splt.Length-1];

                switch (type)
                {
                    case "png":
                    case "jpg":
                    case "bmp":
                    case "webp":
                        files2[passed] = files[i];
                        passed++;
                        break;
                }
            }
            files = new string[passed];
            Array.Copy(files2, files, passed);

            //test them all

            //get the control image
            Bitmap control = Program.OpenFileAsBmp(Program.ImageToFindDuplicatesOfPath);
            control = Algorithm.DownscaleImage(control, Algorithm.GetDownscalePercentage(control.Width, control.Height));
            Color[,] ctrl = Algorithm.AverageImage(control, slices);

            pictureBox1.Image = Algorithm.ConvertAveragesToImage(ctrl, 10);
            //Threading

            //break up workload
            string[] workLoad;
            int indx=0;
            int range = (int)Math.Ceiling((decimal)files.Length/threads);

            CompareThread[] workers = new CompareThread[threads];

            //Start threads
            for (int i = 0; i < threads; i++)
            {
                //break up workload
                if (indx + range > files.Length) range = files.Length - indx;
                workLoad = new string[range];
                for (int x = 0; x < workLoad.Length; x++)
                {
                    workLoad[x] = files[indx];
                    indx++;
                }

                CompareThread cT = new CompareThread(ctrl, workLoad, slices, percentDifferenceToFail);
                workers[i] = cT;
                Thread t = new Thread(() => cT.DoWork());
                workers[i].thread = t;
                workers[i].thread.Start();
            }

            //Await threads
            for (int i = 0; i < threads; i++)
            {
                workers[i].thread.Join();
            }

            List<string> matches = new();
            //add all matches
            for (int i = 0; i < threads; i++)
            {
                for (int x = 0; x < workers[i].matches.Count; x++)
                {
                    matches.Add(workers[i].matches[x].path);
                }
            }

            

            StringBuilder sB = new();
            sB.Append($"Matches found: {matches.Count} -> ");
            
            for (int i = 0; i < matches.Count; i++)
            {
                sB.Append($"{matches[i]}{(i < matches.Count-1 ? ", " : "")}");
            }
            
            label1.Text = sB.ToString();
            control.Dispose();
        }


        void Compare2()
        {
            int mult = 10;//how much to multiply resolution by
            int slices = 10;//how many slices x & y

            Bitmap a = Program.OpenFileAsBmp(Program.imageA);
            Bitmap b = Program.OpenFileAsBmp(Program.imageB);

            Color[,] cA = Algorithm.AverageImage(a, slices);
            Color[,] cB = Algorithm.AverageImage(b, slices);

            pictureBox1.Image = Algorithm.ConvertAveragesToImage(cA, slices);
            pictureBox2.Image = Algorithm.ConvertAveragesToImage(cB, slices);
            
            float percentDiff = Algorithm.CompareImages(cA, cB, slices);

            label1.Text = $"Difference: {percentDiff}%";
        }
    }
}