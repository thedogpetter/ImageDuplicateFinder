using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DuplicateImageFInder
{
    public static class Algorithm
    {
        /// <summary>
        /// For accessing a 1d array as a 2d array (MUST be for x: for y:)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static int ArrayPos2D(int x, int y, int height)
        {
            return (x * height) + y;
        }

        /// <summary>
        /// Converts an image into its float array of slice averages
        /// 
        /// </summary>
        /// <returns></returns>
        public static Color[,] AverageImage(Bitmap a, int slices)
        {
            //Get the number of sli
            int aWidthSlice = a.Width / slices;
            int aHeightSlice = a.Height / slices;

            //arrays that will hold each slice's average
            Color[,] aSlices = new Color[slices, slices];

            //Convert image into a Color Array
            Color[] pixels = new Color[a.Width * a.Height];
            for (int x = 0; x < a.Width; x++)
            {
                int adjX = x * a.Height;
                for (int y = 0; y < a.Height; y++)
                {
                    pixels[adjX + y] = a.GetPixel(x, y);
                }
            }

            Vector3 sliceAverage = new();
            int imagePosX;
            int imagePosY;
            int iPXP;//posx Plus slice
            int iPYP;//posy Plus slice
            Color px;

            //iterators
            int sx; int sy;
            int ix; int iy;
            int thisSliceWidth;
            int thisSliceHeight;

            //average down each slice
            for (sx = 0; sx < slices; sx++)
            {
                imagePosX = sx * aWidthSlice;
                iPXP = imagePosX + aWidthSlice;
                if (iPXP > a.Width) iPXP = a.Width;
                thisSliceWidth = iPXP - imagePosX;

                for (sy = 0; sy < slices; sy++)
                {
                    imagePosY = sy * aHeightSlice;
                    iPYP= imagePosY + aHeightSlice;
                    if (iPYP > a.Height) iPYP = a.Height;
                    thisSliceHeight = iPYP - imagePosY;

                    //sliceAverage = new Color[iPXP * iPYP];
                    sliceAverage.x = 0;
                    sliceAverage.y = 0;
                    sliceAverage.z = 0;

                    //image slice
                    for (ix = imagePosX; ix < iPXP; ix++)
                    {
                        for (iy = imagePosY; iy < iPYP; iy++)
                        {
                            //Per pixel
                            px = pixels[ArrayPos2D(ix, iy, a.Height)];
                            sliceAverage.x += px.R;
                            sliceAverage.y += px.G;
                            sliceAverage.z += px.B;
                        }
                    }
                    //Average it
                    int totalPixels = thisSliceWidth * thisSliceHeight;
                    sliceAverage.x /= totalPixels;
                    sliceAverage.y /= totalPixels;
                    sliceAverage.z /= totalPixels;

                    aSlices[sx, sy] = Color.FromArgb((int)sliceAverage.x, (int)sliceAverage.y, (int)sliceAverage.z);
                }
            }
            
            return (aSlices);
        }

        /// <summary>
        /// Compare two images to see if they're the same (slice count MUST be the same between both)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="tolerancePerSlice"> 0 = no difference allowed, 1 = any difference allowed</param>
        /// <returns></returns>
        public static float CompareImages(Color[,] a, Color[,] b, int slices)
        {
            float totalDiff = 0;
            float Ra; float Rb;
            float Ga; float Gb;
            float Ba; float Bb;

            float fox = (float)Math.Sqrt(Sqr(255) * 3);
            float d;
            float p;

            for (int x = 0; x < slices; x++)
            {
                for (int y = 0; y < slices; y++)
                {
                    Ra = a[x, y].R; Ga = a[x, y].G; Ba = a[x, y].B;
                    Rb = b[x, y].R; Gb = b[x, y].G; Bb = b[x, y].B;
                    d = (float)Math.Sqrt(Sqr(Rb-Ra) + Sqr(Gb - Ga) + Sqr(Bb - Ba));
                    p = d / fox;
                    totalDiff += p;
                }
            }

            return (totalDiff / a.Length) * 100;
        }

        public static Bitmap ConvertAveragesToImage(Color[,] colors, int slices, int scaleMultiplier = 10)
        {
            Bitmap b = new Bitmap(slices * scaleMultiplier, slices * scaleMultiplier);

            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                {
                    b.SetPixel(x, y, colors[x / scaleMultiplier, y / scaleMultiplier]);
                }
            }

            return b;
        }

        public static float Sqr(float a) => a * a;
    }
}
