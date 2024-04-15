using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab08
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph();
            graph.GoMargin("RaBien.INP.txt");
            graph.ChooseCity("ChonThanhPho.INP.txt");
            graph.MoveCircle("DuongTron.INP.txt");
            graph.Go2School("SCHOOL.INP.txt");
        }
    }
}