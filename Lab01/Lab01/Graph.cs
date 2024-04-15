using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab01
{
    class Graph
    {
        int[,] arrGraph;
        List<List<int>> adjList;
        List<Tuple<int, int>> listEdge;
        public int numVertices { get; set; }
        public int numEdges { get; set; }
        public Graph()
        {
            numVertices = 0;
        }
        // Bai 1
        public void VertexDegree(string fname)
        {
            ReadMatrix2Matrix(fname);
            WriteVertexDegrees(fname.Substring(0, fname.Length - 7) + "OUT" + ".txt");
        }

        //Bai 2
        public void InOutDegrees(string fname)
        {
            ReadMatrix2Matrix(fname);
            WriteInOutDegrees(fname.Substring(0, fname.Length - 7) + "OUT"+".txt");
        }
        
        //Bai 3
        public void AdjacencyList(string fname)
        {
            ReadAdjList(fname);
            WriteAlVertexDegree(fname.Substring(0, fname.Length - 7) + "OUT" + ".txt");
        }
        //Bai 4
        public void ListEdge(string fname)
        {
            ReadListEdge(fname);
            WriteLEVertexDegree(fname.Substring(0, fname.Length - 7) + "OUT" + ".txt");
        }

        private void WriteLEVertexDegree(string fname)
        {
            List<int> degree = new List<int>(numVertices);

            foreach (Tuple<int, int> t in listEdge)
            {
                if (t.Item1 > degree.Count)
                {
                    while (t.Item1 > degree.Count)
                    {
                        degree.Add(0);
                    }
                }
                degree[t.Item1 - 1]++;

                if (t.Item2 > degree.Count)
                {
                    while (t.Item2 > degree.Count)
                    {
                        degree.Add(0);
                    }
                }
                degree[t.Item2 - 1]++;
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fname))
            {
                file.WriteLine(numVertices);
                foreach (int d in degree)
                {
                    file.Write(String.Format("{0,-3}", d));
                }
            }
        }

        private void ReadListEdge(string fname)
        {
            string[] lines = System.IO.File.ReadAllLines(fname);
            string[] firstLine = lines[0].Split(' ');
            numVertices = int.Parse(firstLine[0].Trim());
            numEdges = int.Parse(firstLine[1].Trim());
            Console.WriteLine("\t Number of vertices: " + numVertices);
            listEdge = new List<Tuple<int, int>>(numEdges);

            for (int i = 1; i <= numEdges; i++)
            {
                string[] edgeInfo = lines[i].Split(' ');

                if (edgeInfo.Length == 2)
                {
                    int vertex1, vertex2;
                    if (int.TryParse(edgeInfo[0].Trim(), out vertex1) && int.TryParse(edgeInfo[1].Trim(), out vertex2))
                    {
                        listEdge.Add(new Tuple<int, int>(vertex1, vertex2));
                    }
                    else
                    {
                        Console.WriteLine("Invalid integer format in line " + i);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input format in line " + i);
                }
            }
        }


        private void WriteAlVertexDegree(string fname)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fname))
            {
                file.WriteLine(numVertices);
                foreach(List<int> list in adjList)
                {
                    file.Write(String.Format("{0,-3}", list.Count));
                }
            }
        }

        private void ReadAdjList(string fname)
        {
            string[] lines = System.IO.File.ReadAllLines(fname);
            numVertices = int.Parse(lines[0].Trim());
            Console.WriteLine("\t Number of vertices: " + numVertices);
            adjList = new List<List<int>>(numVertices);
            for (int i = 0; i <numVertices; i++)
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

        private void WriteInOutDegrees(string fname)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fname))
            {
                file.WriteLine(String.Format("{0,-3}", numVertices));
                for (int i = 0; i < numVertices; i++)
                {
                    int indegree = 0;
                    int outDegree = 0;
                    for (int j = 0; j < numVertices; j++)
                    {
                        outDegree+= arrGraph[i,j];
                        indegree += arrGraph[j,i];
                    }
                    file.WriteLine(String.Format("{0,-3} {1,-3}", indegree, outDegree));
                }
            }
        }

        protected void ReadMatrix2Matrix(string fname)
        {
            string[] lines = System.IO.File.ReadAllLines(fname);
            numVertices = int.Parse(lines[0].Trim());
            Console.WriteLine("\t Number of vertices: " + numVertices);
            arrGraph = new int[numVertices, numVertices];
            for(int i = 1; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(' ');
                for(int j = 0; j < line.Length; j++)
                {
                    int value = int.Parse(line[j].Trim());
                    SetNode(i - 1, j, value);
                    Console.Write(String.Format("{0,-3}", GetNode(i-1,j)));
                }
                Console.WriteLine();
            }
        }

        private void SetNode(int i, int j, int value)
        {
            if(i < 0 && i < numVertices && j > -1 && j < numVertices)
            {
                Console.WriteLine(String.Format("Out of range ({0}, {1})", i,j));
                return;
            }
            arrGraph[i,j] = value;
        }
        public int GetNode(int i, int j)
        {
            if (i < 0 && i < numVertices && j > -1 && j < numVertices)
            {
                Console.WriteLine(String.Format("Out of range ({0}, {1})", i, j));
                return int.MinValue;
            }
            return arrGraph[i,j];
        }

        protected void WriteVertexDegrees(string fname)
        {
            using(System.IO.StreamWriter file = new System.IO.StreamWriter(fname))
            {
                file.WriteLine(String.Format("{0,-3}", numVertices));
                for(int i = 0; i < numVertices; i++)
                {
                    int degree = 0;
                    for(int j = 0; j < numVertices; j++)
                    {
                        degree += arrGraph[i,j];
                    }
                    file.Write(String.Format("{0,-3}", degree));
                }
                file.WriteLine();
            }
        }

        
    }
}
