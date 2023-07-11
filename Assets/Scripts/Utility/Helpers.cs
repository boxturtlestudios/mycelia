using System.Collections;
using System.Collections.Generic;

namespace BoxTurtleStudios.Utilities
{
    public static class MathB
    {
        //Better modulo
        public static int Mod (int k, int n) {  return ((k %= n) < 0) ? k+n : k;  }
    }
}

