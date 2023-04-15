using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateImageFInder
{
    public class Vector3
    {
        public float x;
        public float y; 
        public float z;

        public Vector3(float x = 0, float y = 0, float z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float Magnitude()
        {
            float xS = x * x;
            float yS = y * y;
            float zS = z * z;

            return (float)Math.Sqrt(xS + yS + zS);
        }
    }

    public class Vector2
    {
        public float x; public float y;
        public Vector2(float x = 0, float y = 0)
        {
            this.x = x;
            this.y = y;
        }
    }
}
