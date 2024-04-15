using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab04
{
    internal class Graph
    {
        LinkedList<int>[] adjList;
        List<List<int>> AdjList;
        private int[,] arrGraph;
        private bool[,] status; 
        private Tuple<int, int> sNode, eNode;
        private void InitGraphArray() 
        {
            arrGraph = new int[nRow, nCol];
        }
        private bool InBoard(int x, int y) 
        {
            return x >= 0 && x < nRow && y >= 0 && y < nCol;
        }
        int N;
        int[] color;
        int[] back;
        bool[] visited;
        int[] pre;
        int sVertex;
        int nRow, nCol;
        int removedV1, removedV2;

        // Bai 1
        public void ConnectedComponents(string fname)
        {
            ReadAdjList(fname);
            LinkedList<LinkedList<int>> components = ListConnectedComponents();
            WriteConnectedComponents(components, fname.Substring(0, fname.Length - 7) + "OUT.txt");
        }

        // Bai 2
        public void BridgeBFS(string fname)
        {
            ReadAdjListBFS2(fname);
            bool flag = IsBridge();
            WriteBridgeBFS(fname.Substring(0, fname.Length - 7) + "OUT.txt", flag);
        }

        // Bai 3
        public void CutVertexBFS(string fname)
        {
            ReadAdjBFS31(fname);
            bool flag = IsCutVertexBFS();
            WriteCutVertexBFS(fname, flag);
        }

        // Bai 4
        public void GridPathBFS(string fname)
        {
            ReadGridBFS(fname);
            List<string> list = new List<string>();
            GridBFS(sNode, eNode, ref list);
            WriteGridPathBFS(fname.Substring(0, fname.Length - 7) + "OUT.txt", list);
        }

        private void WriteGridPathBFS(string fname, List<string> list)
        {
            using (StreamWriter file = new StreamWriter(fname))
            {
                file.WriteLine($"{list.Count,-3}");
                foreach (string v in list)
                {
                    file.WriteLine($"{v, -3}");
                    Console.WriteLine($"{v,-3}");
                }
            }
        }
        private void InitBoolArray(bool value) 
        {
            status = new bool[nRow, nCol];
            for (int i = 0; i < nRow; i++)
            {
                for (int j = 0; j < nCol; j++)
                {
                    status[i, j] = value;
                }
            }
        }
        private void GridBFS(Tuple<int, int> startNode, Tuple<int, int> endNode, ref List<string> pathList)
        {
            Queue<Tuple<int, int>> queue = new Queue<Tuple<int, int>>();
            Tuple<int, int>[,] previous = new Tuple<int, int>[nRow, nCol];

            queue.Enqueue(startNode);
            InitBoolArray(false);
            status[startNode.Item1, startNode.Item2] = true;

            int[] aA = { -1, 0, 1, 0, -1 }; 

            while (queue.Count > 0)
            {
                Tuple<int, int> current = queue.Dequeue();

                for (int i = 0; i < 4; i++)
                {
                    int x = current.Item1 + aA[i];
                    int y = current.Item2 + aA[i + 1];

                    if (InBoard(x, y) && arrGraph[x, y] != 0 && !status[x, y])
                    {
                        queue.Enqueue(new Tuple<int, int>(x, y));
                        status[x, y] = true;
                        previous[x, y] = current;
                    }
                }
            }

            Tuple<int, int> node = endNode;
            while (node != null)
            {
                pathList.Add($"{node.Item1 + 1,-3}{node.Item2 + 1,-3}"); 
                int x = node.Item1;
                int y = node.Item2;
                node = (x >= 0 && x < nRow && y >= 0 && y < nCol) ? previous[x, y] : null;
            }

            pathList.Reverse(); 
        }

        private void ReadGridBFS(string fname)
        {
            string[] lines = File.ReadAllLines(fname);
            string[] line = lines[0].Split(' ');
            nRow = int.Parse(line[0].Trim());
            nCol = int.Parse(line[1].Trim());
            line = lines[1].Split(' ');
            sNode = new Tuple<int, int>(int.Parse(line[0].Trim()) - 1, int.Parse(line[1].Trim()) - 1);
            eNode = new Tuple<int, int>(int.Parse(line[2].Trim()) - 1, int.Parse(line[3].Trim()) - 1);

            InitGraphArray();
            for (int i = 0; i < nRow; i++)
            {
                line = lines[i + 2].Trim().Split(' ');
                for (int j = 0; j < nCol; j++)
                {
                    SetNode(i, j, int.Parse(line[j].Trim()));
                }
            }
        }


        private void SetNode(int i, int j, int value)
        {
            if (i < 0 || i >= nRow || j < 0 || j >= nCol)
            {
                Console.WriteLine($"Out of range ({i}, {j})");
                return;
            }
            arrGraph[i, j] = value;
        }

        private void WriteCutVertexBFS(string fname, bool flag)
        {
            using (StreamWriter file = new StreamWriter(fname.Substring(0, fname.Length - 7) + "OUT.txt"))
            {
                file.WriteLine(flag ? "YES" : "NO");
                Console.WriteLine(flag ? "YES" : "NO");
            }
        }

        private bool IsCutVertexBFS()
        {
            int prev = CountCncComponentsBFS().Count;
            int v = sVertex;
            List<int> list = RemoveVertex(v);
            int currt = CountCncComponentsBFS().Count();
            AddVertex(v, list);
            return prev < currt;
        }

        private List<int> AddVertex(int v, List<int> list)
        {
            if (v < 0 || v > AdjList.Count)
            {
                return list;
            }

            foreach (int vv in list.Where(vv => vv >= 0 && vv < AdjList.Count))
            {
                AdjList[vv].Add(v);
            }

            if (v >= 0 && v <= AdjList.Count)
            {
                AdjList.Insert(v, list);
            }

            return list;
        }

        private List<int> RemoveVertex(int v)
        {
            List<int> list = new List<int>();

            foreach (int vv in AdjList[v])
            {
                list.Add(vv);
            }

            foreach (int vv in list)
            {
                AdjList[vv].Remove(v);
            }

            AdjList.RemoveAt(v);
            return list;
        }

        private void ReadAdjBFS31(string fname)
        {
            string[] lines = File.ReadAllLines(fname);
            string[] firstLine = lines[0].Split(' ');
            N = int.Parse(firstLine[0].Trim());
            sVertex = int.Parse(firstLine[1].Trim()) - 1;
            Console.WriteLine("Number of vertices: " + N);
            AdjList = new List<List<int>>();
            for (int i = 1; i < lines.Length; i++)
            {
                string[] line = lines[i].Trim().Split(' ');
                List<int> list = new List<int>();
                for (int j = 0; j < line.Length; j++)
                {
                    if (line[j].Trim().Length > 0)
                    {
                        list.Add(int.Parse(line[j].Trim()) - 1);
                    }
                }
                AdjList.Add(list);
            }
            Console.WriteLine("Read Completely!!");
        }

        private bool IsBridge()
        {
            int prev = CountCncComponentsBFS().Count;
            AdjList[removedV1].Remove(removedV2);
            AdjList[removedV2].Remove(removedV1);
            int cur = CountCncComponentsBFS().Count;
            AdjList[removedV1].Add(removedV2);
            AdjList[removedV2].Add(removedV1);
            return prev < cur;
        }

        private List<List<int>> CountCncComponentsBFS()
        {
            List<List<int>> list = new List<List<int>>();
            InitIntArray(-2);
            for (int i = 0; i < N; i++)
            {
                if (color[i] == 0)
                {
                    List<int> res = BFSBridge(i);
                    list.Add(res);
                }
            }
            return list;
        }

        private List<int> BFSBridge(int s)
        {
            List<int> res = new List<int>();
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(s);
            color[s] = 1;

            while (queue.Count > 0)
            {
                int v = queue.Dequeue();
                if (v < 0 || v >= AdjList.Count)
                {
                    continue;
                }

                foreach (int vv in AdjList[v])
                {
                    if (vv < 0 || vv >= color.Length)
                    {
                        continue;
                    }

                    if (color[vv] == 0)
                    {
                        queue.Enqueue(vv);
                        res.Add(vv);
                        color[vv] = 1;
                    }
                }
            }

            return res;
        }

        private void InitIntArray(int value = -2)
        {
            color = new int[N];
            back = new int[N];
            for (int i = 0; i < color.Length; i++)
            {
                color[i] = 0;
                back[i] = value;
                back[i] = 0;
            }
        }

        private void WriteBridgeBFS(string fname, bool flag)
        {
            using (StreamWriter file = new StreamWriter(fname))
            {
                file.WriteLine(flag ? "YES" : "NO");
                Console.WriteLine(flag ? "YES" : "NO");
            }
        }

        private void ReadAdjListBFS2(string fname)
        {
            string[] lines = File.ReadAllLines(fname);
            string[] firstLine = lines[0].Split(' ');
            N = int.Parse(firstLine[0].Trim());
            removedV1 = int.Parse(firstLine[1].Trim()) - 1;
            removedV2 = int.Parse(firstLine[2].Trim()) - 1;
            Console.WriteLine("Number of vertices: " + N);
            AdjList = new List<List<int>>();
            for (int i = 1; i < lines.Length; i++)
            {
                string[] line = lines[i].Trim().Split(' ');
                List<int> list = new List<int>();
                for (int j = 0; j < line.Length; j++)
                {
                    if (line[j].Trim().Length > 0)
                    {
                        list.Add(int.Parse(line[j].Trim()) - 1);
                    }
                }
                AdjList.Add(list);
            }
            Console.WriteLine("Read Completely!!");
        }

        private void WriteConnectedComponents(LinkedList<LinkedList<int>> components, string fname)
        {
            using (StreamWriter file = new StreamWriter(fname))
            {
                file.WriteLine(components.Count);
                foreach (LinkedList<int> component in components)
                {
                    foreach (int v in component)
                    {
                        file.Write($"{v,-3}");
                    }
                    file.WriteLine();
                }
            }
        }

        private LinkedList<LinkedList<int>> ListConnectedComponents()
        {
            LinkedList<LinkedList<int>> component = new LinkedList<LinkedList<int>>();
            visited = new bool[N + 1];
            pre = new int[N + 1];
            for (int i = 1; i < visited.Length; i++)
            {
                if (visited[i])
                    continue;
                LinkedList<int> comp = BFS(i);
                component.AddLast(comp);
            }
            return component;
        }

        private LinkedList<int> BFS(int s)
        {
            LinkedList<int> comp = new LinkedList<int>();
            Queue<int> q = new Queue<int>();
            visited[s] = true;
            pre[s] = -1;
            q.Enqueue(s);
            while (q.Count != 0)
            {
                int u = q.Dequeue();
                comp.AddLast(u);
                foreach (int v in adjList[u - 1])
                {
                    if (visited[v])
                        continue;
                    visited[v] = true;
                    pre[v] = u;
                    q.Enqueue(v);
                }
            }
            return comp;
        }

        private void ReadAdjList(string fname)
        {
            string[] lines = File.ReadAllLines(fname);
            N = int.Parse(lines[0].Trim());
            adjList = new LinkedList<int>[N];
            for (int i = 0; i < N; i++)
            {
                adjList[i] = new LinkedList<int>();
                if (lines[i + 1].Length == 0)
                {
                    continue;
                }
                string[] line = lines[i + 1].Split(' ');
                for (int j = 0; j < line.Length; j++)
                {
                    int v = int.Parse(line[j].Trim());
                    adjList[i].AddLast(v);
                }
            }
        }
    }
}
