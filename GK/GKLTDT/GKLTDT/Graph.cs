using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GKLTDT
{
    internal class Graph
    {
        List<List<int>> adjList;
        List<Tuple<int, int>> listEdge;
        public int numVertices { get; set; }
        public int numEdges { get; set; }
        int sVertex;
        int eVertex;
        int[] color;
        int[] back;
        public Graph()
        {
            numVertices = 0;
            numEdges = 0;
        }
        //Buoi 1 Bai 3
        public void AdjacencyList(string fname)
        {
            ReadAdjList(fname);
            WriteAlVertexDegree(fname.Substring(0, fname.Length - 7) + "OUT" + ".txt");
        }
        //Buoi 2 bai 2
        public void AdjacencyList2ListEdges(string fname)
        {
            ReadAdjList(fname);
            ConvertAL2LE();
            WriteListEdge(fname.Substring(0, fname.Length - 7) + "OUT.txt");
        }
        //Buoi 3 bai 4
        public void ConnectedComponents(string fname)
        {
            ReadAdjListBFS(fname);
            List<List<int>> list = CoutConnectedComponents();
            WriteConnectedComponents(fname, list);
        }
        private void ReadAdjListBFS(string fname)
        {
            string[] lines = File.ReadAllLines(fname);
            string[] line = lines[0].Split(' ');
            numVertices = int.Parse(line[0].Trim());
            if (line.Length > 1 && line[1].Trim().Length > 0)
                sVertex = int.Parse(line[1].Trim()) - 1;

            if (line.Length > 2 && line[2].Trim().Length > 0)
                eVertex = int.Parse(line[2].Trim()) - 1;
            Console.WriteLine("Number of vertices: " + numVertices);
            adjList = new List<List<int>>();
            for (int i = 0; i < numVertices; i++)
            {
                line = lines[i + 1].Trim().Split(' ');
                List<int> list = new List<int>();
                for (int j = 0; j < line.Length; j++)
                {
                    if (line[j].Trim().Length > 0)
                    {
                        list.Add(int.Parse(line[j].Trim()) - 1);
                    }
                }
                adjList.Add(list);
            }
            Console.WriteLine("Read Completely!!");
        }
        private void WriteConnectedComponents(string fname, List<List<int>> list)
        {
            using (StreamWriter file = new StreamWriter(fname.Substring(0, fname.Length - 7) + "OUT.txt"))
            {
                file.WriteLine(String.Format("{0,-3}", list.Count));
                Console.WriteLine(String.Format("{0,-3}", list.Count));
            }
        }

        private List<List<int>> CoutConnectedComponents()
        {
            List<List<int>> list = new List<List<int>>();
            InitIntArray(-2);
            for (int i = 0; i < numVertices; i++)
            {
                if (color[i] == 0)
                {
                    List<int> res = BFSBai4(i);
                    list.Add(res);
                }
            }
            return list;
        }

        private List<int> BFSBai4(int s)
        {
            List<int> res = new List<int>();
            Queue<int> q = new Queue<int>();
            q.Enqueue(s);
            color[s] = 1;
            while (q.Count > 0)
            {
                int v = q.Dequeue();
                foreach (int i in adjList[v])
                {
                    if (color[i] == 0)
                    {
                        q.Enqueue(i);
                        res.Add(i);
                        color[i] = 1;
                    }
                }
            }
            return res;
        }
        private void InitIntArray(int value = -2)
        {
            color = new int[numVertices];
            back = new int[numVertices];
            for (int i = 0; i < color.Length; i++)
            {
                color[i] = 0;
                back[i] = value;
            }
        }
        private void WriteListEdge(string fname)
        {
            using(StreamWriter file = new StreamWriter(fname))
            {
                file.WriteLine(String.Format("{0,-3} {1,-3}", numVertices, numEdges));
                Console.WriteLine(String.Format("{0,-3} {1,-3}", numVertices, numEdges));
                foreach (Tuple<int,int> edge in listEdge)
                {
                    file.WriteLine(String.Format("{0,-3} {1,-3}", edge.Item1, edge.Item2));
                    Console.WriteLine(String.Format("{0,-3} {1,-3}", edge.Item1, edge.Item2));
                }
            }
        }

        private void ConvertAL2LE()
        {
            listEdge = new List<Tuple<int, int>>();
            numEdges = 0;

            for (int i = 0; i < adjList.Count; i++)
            {
                int v1 = i + 1;

                foreach (int v2 in adjList[i])
                {
                    if (!listEdge.Contains(new Tuple<int, int>(v1, v2 + 1)) && !listEdge.Contains(new Tuple<int, int>(v2 + 1, v1)))
                    {
                        listEdge.Add(new Tuple<int, int>(v1, v2 + 1)); 
                        numEdges++;
                    }
                }
            }
        }



        private void ReadAdjList(string fname)
        {
            string[] lines = System.IO.File.ReadAllLines(fname);
            numVertices = int.Parse(lines[0].Trim());
            Console.WriteLine("\t Number of vertices: " + numVertices);
            adjList = new List<List<int>>(numVertices);
            for (int i = 0; i < numVertices; i++)
            {
                adjList.Add(new List<int>());

                string[] line = lines[i + 1].Split(' ');

                for (int j = 0; j < line.Length; j++)
                {
                    int v = int.Parse(line[j].Trim()) - 1;
                    adjList[i].Add(v);
                }
            }
        }
        private void WriteAlVertexDegree(string fname)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fname))
            {
                file.WriteLine(numVertices);
                foreach (List<int> list in adjList)
                {
                    file.Write(String.Format("{0,-3}", list.Count));
                    Console.Write(String.Format("{0,-3}", list.Count));
                }
                Console.WriteLine();
            }
        }
    }
}
