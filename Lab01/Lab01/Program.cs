using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.AccessControl;

namespace Lab01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph();
            graph.VertexDegree("BacDoThiVoHuong.INP.txt"); //Bai 1
            graph.InOutDegrees("BacVaoRa.INP.txt");//Bai 2
            graph.AdjacencyList("DanhSachKe.INP.txt");//Bai3
            graph.ListEdge("DanhSachCanh.INP.txt");//Bai 4

        }
    }
}
