using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class actionSequence
{
    public struct timeInterval
    {
        private float a;
        private float b;
        public timeInterval(float a, float b)
        {
            this.a = a;
            this.b = b;
        }
        public float A { get { return a; } }
        public float B { get { return b; } }
    }

}
