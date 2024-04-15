using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKLTDT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph();
            graph.AdjacencyList("DanhSachKe.INP.txt"); //Bai 1
            graph.AdjacencyList2ListEdges("Ke2Canh.INP.txt");//Bai 2
            graph.ConnectedComponents("DemLienThong.INP.txt");//Bai 3
        }
    }
}
