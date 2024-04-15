using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph();
            graph.ListConnectedVertices("BFS.INP.txt"); //Bai 01
            graph.PathBFS("TimDuong.INP.txt"); //Bai 02
            graph.BFSConnected("LienThong.INP.txt"); //Bai 03
            graph.ConnectedComponents("DemLienThong.INP.txt"); //Bai 04
        }
    }
}
