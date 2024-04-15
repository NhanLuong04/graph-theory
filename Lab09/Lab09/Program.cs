using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab09
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph();
            graph.SpanningTree("CayKhung.INP.txt");
            graph.KruskalMST("Kruskal.INP.txt");
            graph.PrimMST("Prim.INP.txt");
            graph.SpanningTreeX("CayKhungX.INP.txt");
        }
    }
}
