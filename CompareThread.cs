using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace DuplicateImageFInder
{
    public class CompareThread
    {
        public Thread thread;
        string[] filesToCheck;
        Color[,] ctrl;
        int slices;
        float percentDifferenceToFail;

        public List<TReturn> matches = new();

        public CompareThread(Color[,] control, string[] filesToCheck, int slices, float percentDifferenceToFail)
        {
            this.filesToCheck = filesToCheck;
            this.ctrl = control;
            this.slices = slices;
            this.percentDifferenceToFail = percentDifferenceToFail;
        }

        public void DoWork()
        {
            Bitmap test;
            Color[,] tst;
            float difference;

            for (int i = 0; i < filesToCheck.Length; i++)
            {
                test = Program.OpenFileAsBmp(filesToCheck[i]);
                if (test == null) continue;//move along if test failed to open
                tst = Algorithm.AverageImage(test, slices);

                difference = Algorithm.CompareImages(ctrl, tst, slices);

                if (difference <= percentDifferenceToFail) matches.Add(new(filesToCheck[i], difference));
            }
        }

        public class TReturn
        {
            public string path;
            public float percentDifference;

            public TReturn(string path, float percentDifference)
            {
                this.path = path;
                this.percentDifference = percentDifference;
            }
        }
    }
}
