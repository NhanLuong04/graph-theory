using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CK2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph();
            graph.BridgeBFS("CanhCau.INP.txt");
            graph.PathDFS("TimDuongDFS.INP.txt");
            graph.Cycle("ChuTrinh.INP.txt");
        }
    }
}
