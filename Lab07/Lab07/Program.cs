using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph();
            graph.ShortestPath("Dijkstra.INP.txt");
            graph.InterVertexShortestPath("NganNhatX.INP.txt");
            graph.ShortestPathFloyd("FloydWarshall.INP.txt");

        }
    }
}
