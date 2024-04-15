using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph();
            graph.ListEdges2AdjecencyList("Canh2DSKe.INP.txt");//Bai 01
            graph.StorageTank("BonChua.INP.txt");//Bai 03
            graph.TransposeGraph("ChuyenVi.INP.txt");//Bai 04
            graph.WeightEdgeList("TrungBinhCanh.INP.txt");//Bai 05
            graph.AdjacencyMatrix2AdjacencyList("MaTran2DSKe.INP.txt");//Bai 01 lam them
        }
    }
}
