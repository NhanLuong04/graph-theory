using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph();
            graph.Bipartile("PhanDoi.INP.txt");
            graph.Cycle("ChuTrinh.INP.txt");
            graph.TopoSort("TopoSort.INP.txt");
        }
    }
}
