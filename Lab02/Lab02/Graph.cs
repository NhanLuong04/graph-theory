using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace Lab02
{
    internal class Graph
    {
        List<List<int>> adjList;// adjacency List
        List<Tuple<int, int>> listEdge; // List edge
        private LinkedList<Tuple<int, int, int>> lst = new LinkedList<Tuple<int, int, int>>();
        int[,] arrGraph;
        public int NumVertices {  get; set; }
        public int NumEdges { get; set; }
        public Graph()
        {
            NumVertices = 0;
        }
        //Bai 01
        public void ListEdges2AdjecencyList(string fname)
        {
            ReadListEdge(fname);
            ConvertLE2AL();
            WriteAdjecencyList(fname.Substring(0,fname.Length - 7) + "OUT" + ".txt");
        }
        //Bai 03
        public void StorageTank(string fname)
        {
            ReadAdjMatrix(fname);
            List<int> tank = FindStorageTank();
            WriteStorageTank(fname.Substring(0,fname.Length - 7) + "OUT" + ".txt", tank);
        }

        //Bai 04
        public void TransposeGraph(string fname)
        {
            ReadAdjList(fname);
            List<List<int>> transpose = Transpose();
            WriteAdjacencyList(fname.Substring(0,fname.Length-7) + "OUT" + ".txt", transpose);
        }
        //Bai05

        public void WeightEdgeList(string fname)
        {
            ReadWeightEdgeList(fname);
            double avgLength = AverageEdge();
            MaxEdge();
            WriteWeightEdgeList(fname.Substring(0, fname.Length - 7) + "OUT" + ".txt", avgLength);
        }

        private void WriteWeightEdgeList(string fname, double avgLength)
        {
            using (StreamWriter writer = new StreamWriter(fname))
            {
                writer.WriteLine($"{avgLength:F2}");

                writer.WriteLine($"{lst.Count}");

                foreach (var edge in lst)
                {
                    writer.WriteLine($"{edge.Item1} {edge.Item2} {edge.Item3}");
                }
            }
        }

        private double AverageEdge()
        {
            double sum = 0;
            foreach (Tuple<int, int, int> edge in lst)
            {
                sum += edge.Item3;
            }
            double avg = sum / lst.Count;
            return avg;
        }

        private void MaxEdge()
        {
            int max = int.MinValue;
            LinkedList<Tuple<int, int, int>> maxEdges = new LinkedList<Tuple<int, int, int>>();

            foreach (Tuple<int, int, int> edge in lst)
            {
                max = Math.Max(max, edge.Item3);
            }

            foreach (Tuple<int, int, int> edge in lst)
            {
                if (edge.Item3 == max)
                {
                    maxEdges.AddLast(edge);
                }
            }

            lst.Clear();
            foreach (var edge in maxEdges)
            {
                lst.AddLast(edge);
            }
        }

        private void ReadWeightEdgeList(string fname)
        {
            string[] lines = File.ReadAllLines(fname);
            string[] input = lines[0].Split(' ');
            int n = int.Parse(input[0].Trim());
            int m = int.Parse(input[1].Trim());
            for (int i = 1; i <= m; i++)
            {
                string[] line = lines[i].Split(' ');
                int u = int.Parse(line[0].Trim());
                int v = int.Parse(line[1].Trim());
                int w = int.Parse(line[2].Trim());
                lst.AddLast(Tuple.Create(u, v, w));
            }
        }

        //Bai 01 lam them
        public void AdjacencyMatrix2AdjacencyList(string fname)
        {
            ReadAdjMatrix(fname);
            ConvertML2AL();
            WriteAdjecencyList(fname.Substring(0, fname.Length - 7) + "OUT" + ".txt");
        }


        private void ConvertML2AL()
        {
            adjList = new List<List<int>>(NumVertices);

            for (int i = 0; i < NumVertices; i++)
            {
                adjList.Add(new List<int>());
            }

            for (int i = 0; i < NumVertices; i++)
            {
                for (int j = i + 1; j < NumVertices; j++)
                {
                    if (arrGraph[i, j] > 0)
                    {
                        adjList[i].Add(j + 1);

                        if (j < adjList.Count) 
                        {
                            adjList[j].Add(i + 1);
                        }
                        else
                        {
                            Console.WriteLine($"Error: Vertex {j} not properly initialized in adjList.");
                        }
                    }
                }
            }
        }

        private void WriteAdjacencyList(string fname, List<List<int>> aList)
        {
            using (StreamWriter file = new StreamWriter(fname))
            {
                file.WriteLine(NumVertices);
                for (int i = 0; i < NumVertices; i++)
                {
                    foreach (int v in aList[i])
                    {
                        file.Write(String.Format("{0,-3}", v));
                    }
                    file.WriteLine();
                }
            }
        }

        private List<List<int>> Transpose()
        {
            List<List<int>> tGraph = new List<List<int>>(NumVertices);

            for (int i = 0; i < NumVertices; i++)
            {
                tGraph.Add(new List<int>());
            }

            for (int i = 0; i < NumVertices; i++)
            {
                foreach (int v in adjList[i])
                {
                    tGraph[v].Add(i + 1); 
                }
            }

            return tGraph;
        }

        private void ReadAdjList(string fname)
        {
            try
            {
                string[] lines = File.ReadAllLines(fname);

                if (lines.Length < 1)
                {
                    Console.WriteLine("Error: Empty file.");
                    return;
                }

                NumVertices = int.Parse(lines[0].Trim());

                Console.WriteLine("Number of vertices: " + NumVertices);

                adjList = new List<List<int>>(NumVertices);

                for (int i = 0; i < NumVertices; i++)
                {
                    adjList.Add(new List<int>());

                    if (i + 1 < lines.Length)
                    {
                        string[] line = lines[i + 1].Split(' ');

                        for (int j = 0; j < line.Length; j++)
                        {
                            if (int.TryParse(line[j].Trim(), out int v))
                            {
                                v -= 1; 
                                adjList[i].Add(v);
                            }
                            else
                            {
                                Console.WriteLine("Error: Invalid integer in line " + (i + 2));
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Insufficient lines in the file.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }


        private void WriteStorageTank(string fname, List<int> list)
        {
            using (StreamWriter file = new StreamWriter(fname))
            {
                file.WriteLine(list.Count);
                if(list.Count < 1)
                {
                    return;
                }
                foreach(int v in list)
                {
                    file.Write(String.Format("{0,-3}", v));
                }
            }
        }

        private List<int> FindStorageTank()
        {
            List<int> list = new List<int>();

            for(int i = 0; i < NumVertices; i++)
            {
                int indegree = 0;
                int outdegree = 0;
                for(int j = 0; j < NumVertices; j++)
                {
                    indegree += arrGraph[j, i];
                    outdegree += arrGraph[i,j];
                }
                if(indegree > 0 && outdegree == 0)
                {
                    list.Add(i + 1);
                }
            }
            return list;
        }

        private void ReadAdjMatrix(string fname)
        {
            string[] lines = File.ReadAllLines(fname);
            NumVertices = int.Parse(lines[0].Trim());
            Console.WriteLine("\t Number of vertices: " + NumVertices);
            arrGraph = new int[NumVertices, NumVertices];
            for (int i = 1; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(' ');

                for (int j = 0; j < line.Length; j++)
                {
                    if (int.TryParse(line[j].Trim(), out int value))
                    {
                        SetNode(i - 1, j, value);
                        Console.Write(String.Format("{0,-3}", GetNode(i - 1, j)));
                    }
                    else
                    {
                        Console.WriteLine($"Invalid integer at line {i}, position {j}: {line[j]}");
                    }
                }
                Console.WriteLine();
            }

        }

        private void SetNode(int i, int j, int value)
        {
            if (i < 0 && i < NumVertices && j > -1 && j < NumVertices)
            {
                Console.WriteLine(String.Format("Out of range ({0}, {1})", i, j));
                return;
            }
            arrGraph[i, j] = value;
        }
        public int GetNode(int i, int j)
        {
            if (i < 0 && i < NumVertices && j > -1 && j < NumVertices)
            {
                Console.WriteLine(String.Format("Out of range ({0}, {1})", i, j));
                return int.MinValue;
            }
            return arrGraph[i, j];
        }

        private void ReadListEdge(string fname)
        {
            string[] lines = File.ReadAllLines(fname);
            string[] temp = lines[0].Split(' ');
            NumVertices = Int32.Parse(temp[0].Trim());
            NumEdges = Int32.Parse(temp[1].Trim());
            Console.WriteLine("Number of vertices: " + NumVertices);
            listEdge = new List<Tuple<int, int>>(NumEdges);
            for (int i = 1; i <= NumEdges; i++)
            {
                string[] line = lines[i].Split(' ');
                listEdge.Add(new Tuple<int, int>(
                    Int32.Parse(line[0].Trim()), Int32.Parse(line[1].Trim())
                ));
            }
        }
        private void ConvertLE2AL()
        {
            adjList = new List<List<int>>();
            for(int i = 0; i <NumVertices; i++)
            {
                adjList.Add(new List<int>());
            }
            foreach(Tuple<int, int> t in listEdge)
            {
                adjList[t.Item1 - 1].Add(t.Item2);
                adjList[t.Item2-1].Add(t.Item1);
            }
        }
        private void WriteAdjecencyList(string fname)
        {
            using (StreamWriter file = new StreamWriter(fname))
            {
                file.WriteLine(String.Format("{0,-3}", NumVertices));
                foreach(List<int> list in adjList) 
                {
                    foreach(int v in list)
                    {
                        file.Write(String.Format("{0,-3}", v));
                    }
                    file.WriteLine();
                }
            }
        }
    }
}
