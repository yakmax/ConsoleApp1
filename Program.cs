using System;
using System.Collections.Generic;
using System.Drawing;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            int l = 10;
            int[,] p = new int[l, l];
            Point s = new Point(0, 0);
            Point g = new Point(l-1, l-1);

            for (int i = 0; i < l; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    p[i, j] = 0;
                }
            }

            SearchPoints.FindPath(p, s, g);

            for (int i = 0; i < l; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    Console.Write(p[i, j]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}
