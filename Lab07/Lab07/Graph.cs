using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Lab07
{
    internal class Graph
    {
        protected const int INF = 1000;
        protected int maxWeight = 0;
        int[,] arrGraph;
        int[,] floydMatrix;
        List<List<Tuple<int, int>>> adjList;
        public int N {  get; set; }
        public int E { get; set; }
        int sVertex, eVertex, iVertex;
        int[] color, back, dist;

        internal void ShortestPath(string fname)
        {
            ReadAdjListSP(fname);
            Dijkstra();
            List<int> list = TracePath();
            WriteShortestPath(fname.Substring(0, fname.Length - 7) + "OUT.txt", list);
        }

        private void WriteShortestPath(string fname, List<int> list)
        {
            using(System.IO.StreamWriter file = new System.IO.StreamWriter(fname))
            {
                if(list.Count == 3)
                {
                    file.WriteLine(String.Format("Khong co duong di tu {0,-3} den {1,-3}", sVertex + 1, eVertex + 1));
                    Console.WriteLine(String.Format("Khong co duong di tu {0,-3} den {1,-3}", sVertex + 1, eVertex + 1));
                }
                else
                {
                    file.WriteLine(String.Format("{0,-3}", dist[eVertex]));
                    Console.WriteLine(String.Format("{0,-3}", dist[eVertex]));
                    foreach(int v in list)
                    {
                        file.Write(String.Format("{0,-3}", v + 1));
                        Console.Write(String.Format("{0,-3}", v + 1));
                    }
                    file.WriteLine();
                    Console.WriteLine();
                }
            }
        }

        private void ReadAdjListSP(string fname)
        {
            string[]lines  = System.IO.File.ReadAllLines(fname);
            string[] line = lines[0].Split(' ');
            N = int.Parse(line[0].Trim());
            E = int.Parse(line[1].Trim());
            sVertex = int.Parse(line[2].Trim()) - 1;
            eVertex = int.Parse(line[3].Trim()) - 1;
            Console.WriteLine("Number of vertices: " + N);
            adjList = new List<List<Tuple<int,int>>>();
            for(int i = 0; i <N; i++)
            {
                adjList.Add(new List<Tuple<int, int>>());
            }
            for(int i = 0; i < E;i++)
            {
                line = lines[i + 1].Trim().Split(' ');
                int v1 = int.Parse(line[0].Trim()) - 1;
                int v2 = int.Parse(line[1].Trim()) - 1;
                int w = int.Parse(line[2].Trim());
                adjList[v1].Add(new Tuple<int, int>(v2, w));
                adjList[v2].Add(new Tuple<int, int>(v1, w));
            }
            Console.WriteLine("Read Completely!!");
        }
        public void Dijkstra()
        {
            InitIntArray(-1);
            int g = sVertex;
            dist[g] = 0;
            back[g] = -1;
            do
            {
                g = eVertex;
                for (int i = 0; i < N; i++)
                {
                    if ((color[i] == 0) && (dist[g] > dist[i]))
                    {
                        g = i;
                    }
                }
                color[g] = 1;
                if ((dist[g] == INF) || g == eVertex)
                    break;
                foreach(Tuple<int, int> vv in adjList[g])
                {
                    if (color[vv.Item1] == 0)
                    {
                        int d = dist[g] + vv.Item2;
                        if (dist[vv.Item1] > d)
                        {
                            dist[vv.Item1] = d;
                            back[vv.Item1] = g;
                        }
                    }
                }
            } while (true);
        }
        private void InitIntArray(int value = -2)
        {
            color = new int[N];
            back = new int[N];
            dist = new int[N];
            for (int i = 0; i < color.Length; i++)
            {
                color[i] = 0;
                back[i] = value;
                dist[i] = INF;
            }
        }
        private List<int> TracePath()
        {
            int y = eVertex;
            int x = sVertex;
            List<int> res = new List<int>();
            res.Add(y);
            int v = back[y];
            while (v != x && v != -1)
            {
                res.Insert(0, v);
                v = back[v];
            }
            if (v == x)
            {
                res.Insert(0, v);
            }
            return res;
        }

        internal void InterVertexShortestPath(string fname)
        {
            ReadAdjListIVSP(fname);
            List<int> list = IVShortestPath();
            WriteInterVertexShortestPath(fname.Substring(0,fname.Length-7) + "OUT.txt", list);
        }

        private void WriteInterVertexShortestPath(string fname, List<int> list)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fname))
            {
                if (list.Count == 3)
                {
                    file.WriteLine($"Khong co duong di tu {sVertex + 1,-3} den {eVertex + 1,-3}");
                    Console.WriteLine($"Khong co duong di tu {sVertex + 1,-3} den {eVertex + 1,-3}");
                }
                else
                {
                    file.WriteLine($"{list[list.Count - 1],-3}");
                    Console.WriteLine($"{list[list.Count - 1],-3}");
                    foreach (int v in list)
                    {
                        if (v == list[list.Count - 1])
                            break;
                        file.Write($"{v + 1,-3}");
                        Console.Write($"{v + 1,-3}");
                    }
                    file.WriteLine();
                    Console.WriteLine();
                }
            }
        }
        private List<int> IVShortestPath()
        {
            int temp = eVertex;
            eVertex = iVertex;
            Dijkstra();
            int distance = dist[eVertex];
            List<int> res = TracePath();
            eVertex = temp;
            temp = sVertex;
            sVertex = iVertex;
            for(int i = 0; i < N; i++)
            {
                if(!res.Contains(i + 1))
                {
                    color[i] = 0;
                    back[i] = -2;
                    dist[i] = INF;
                }
            }
            color[sVertex] = 0;
            SubDijkstra();
            List<int> phase2 = TracePath();
            distance += dist[eVertex];
            iVertex = sVertex;
            sVertex = temp;
            res.RemoveAt(res.Count - 1);
            res.AddRange(phase2);
            res.Add(distance);
            return res;
        }

        private void SubDijkstra()
        {
            int g = sVertex;
            dist[g] = 0;
            do
            {
                g = eVertex;
                for (int i = 0; i < N; i++)
                {
                    if ((color[i] == 0) && (dist[g] > dist[i]))
                    {
                        g = i;
                    }
                }
                color[g] = 1;
                if ((dist[g] == INF) || g == eVertex)
                    break;
                foreach (Tuple<int, int> vv in adjList[g])
                {
                    if (color[vv.Item1] == 0)
                    {
                        int d = dist[g] + vv.Item2;
                        if (dist[vv.Item1] > d)
                        {
                            dist[vv.Item1] = d;
                            back[vv.Item1] = g;
                        }
                    }
                }
            } while (true);
        }

        private void ReadAdjListIVSP(string fname)
        {
            string[] lines = System.IO.File.ReadAllLines(fname);
            string[] line = lines[0].Split(' ');
            N = int.Parse(line[0].Trim());
            E = int.Parse(line[1].Trim());
            sVertex = int.Parse(line[2].Trim()) - 1;
            eVertex = int.Parse(line[3].Trim()) - 1;
            iVertex = int.Parse(line[4].Trim()) - 1;
            Console.WriteLine("Number of vertices: " + N);
            adjList = new List<List<Tuple<int, int>>>();
            for (int i = 0; i < N; i++)
            {
                adjList.Add(new List<Tuple<int, int>>());
            }
            for (int i = 0; i < E; i++)
            {
                line = lines[i + 1].Trim().Split(' ');
                int v1 = int.Parse(line[0].Trim()) - 1;
                int v2 = int.Parse(line[1].Trim()) - 1;
                int w = int.Parse(line[2].Trim());
                adjList[v1].Add(new Tuple<int, int>(v2, w));
                adjList[v2].Add(new Tuple<int, int>(v1, w));
            }
            Console.WriteLine("Read Completely!!");
        }

        internal void ShortestPathFloyd(string fname)
        {
            ReadMatrix2Matrix(fname);
            FloyAlgorithm();
            WriteFloydMatrix(fname.Substring(0, fname.Length - 7) + "OUT.txt");
        }

        private void WriteFloydMatrix(string fname)
        {
            using(System.IO.StreamWriter file = new System.IO.StreamWriter(fname))
            {
                file.WriteLine(String.Format("{0,-3}", N));
                for(int i = 0; i < N; i++)
                {
                    for(int j = 0; j < N; j++)
                    {
                        string str = String.Format("{0,-5}", floydMatrix[i, j]);
                        if ((floydMatrix[i, j] >= (INF - maxWeight)) && (floydMatrix[i, j] <= (INF + maxWeight)))
                        {
                            str = "Inf";
                        }
                        file.Write(str);

                    }
                    file.WriteLine();
                }
            }
        }

        private void FloyAlgorithm()
        {
            floydMatrix = new int[N, N];
            for(int i =0; i < N; i++)
            {
                floydMatrix[i, i] = 0;
                for(int j = i + 1; j < N; j++)
                {
                    floydMatrix[i, j] = INF;
                    floydMatrix[j, i] = INF;
                    if (arrGraph[i,j] != 0)
                    {
                        floydMatrix[i, j] = arrGraph[i, j];
                        floydMatrix[j, i] = arrGraph[j, i];
                    }
                }
            }
            for(int k = 0; k <N; k++)
            {
                for(int i = 0; i<N; i++)
                {
                    for(int j = 0; j <N; j++)
                    {
                        int d = floydMatrix[i, k] + floydMatrix[k, j];
                        if (floydMatrix[i,j] > d)
                        {
                            floydMatrix[i, j] = d;
                        }
                    }
                }
            }
        }

        private void ReadMatrix2Matrix(string fname)
        {
            string[]lines = System.IO.File.ReadAllLines(fname);
            N = int.Parse(lines[0].Trim());
            Console.WriteLine("Number of vertices: " + N);
            arrGraph = new int[N, N];
            for(int i = 1; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(' ');
                for(int j = 0; j < line.Length; j++)
                {
                    int value = int.Parse(line[j].Trim());
                    SetNode(i - 1, j, value);
                    Console.Write(String.Format("{0,-3}", GetNode(i - 1, j)));
                }
                Console.WriteLine();
            }
        }

        private int GetNode(int i, int j)
        {
            if (i < 0 && i < N && j > -1 && j < N)
            {
                Console.WriteLine(String.Format("Out of range ({0}, {-1})", i, j));
                return Int32.MinValue;
            }
            return arrGraph[i,j];
        }

        private void SetNode(int i, int j, int value)
        {
            if(i < 0 && i < N && j > -1 && j < N)
            {
                Console.WriteLine(String.Format("Out of range ({0}, {-1})", i ,j));
                return;
            }
            arrGraph[i, j] = value;
        }
    }
}
