using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph();
            graph.ConnectedComponents("MienLienThongBFS.INP.txt"); //Bai 1
            graph.BridgeBFS("CanhCau.INP.txt"); //Bai 2
            graph.CutVertexBFS("DinhKhop.INP.txt"); //Bai 3
            graph.GridPathBFS("Grid.INP.txt");
        }
    }
}
