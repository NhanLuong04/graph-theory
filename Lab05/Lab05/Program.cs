using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph();
            graph.ListCntVertices("LienThongDFS.INP.txt"); //Bai 1
            graph.PathDFS("TimDuongDFS.INP.txt");
        }
    }
}
