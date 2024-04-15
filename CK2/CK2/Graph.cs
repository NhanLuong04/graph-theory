using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace CK2
{
    internal class Graph
    {
        public int N { get; set; }
        int removedV1, removedV2;
        List<List<int>> adjList;
        public int E { get; set; }
        int sVertex, eVertex;
        int[] color,back;
        LinkedList<int>[] adjlist;
        bool[] visited;
        int[] pre;
        // Bai 1
        public void BridgeBFS(string fname)
        {
            ReadAdjListBFS2(fname);
            bool flag = IsBridge();
            WriteBridgeBFS(fname.Substring(0, fname.Length - 7) + "OUT.txt", flag);
        }
        //Bai 2
        public void PathDFS(string fname)
        {
            ReadAdjListPath(fname);
            PathDFS();
            List<int> list = TracePathDFS();
            WritePathDFS(fname.Substring(0, fname.Length - 7) + "OUT.txt", list);
        }
       
        //Bai 3
        internal void Cycle(string fname)
        {
            ReadAdjList(fname);
            bool flag = Cycle();
            WriteBipartite(flag, fname.Substring(0, fname.Length - 7) + "OUT.txt");
        }
        private void ReadAdjList(string fname)
        {
            string[] lines = File.ReadAllLines(fname);
            N = int.Parse(lines[0].Trim());
            adjlist = new LinkedList<int>[N];
            for (int i = 0; i < N; i++)
            {
                adjlist[i] = new LinkedList<int>();
                if (lines[i + 1].Length > 0)
                {
                    string[] line = lines[i + 1].Split(' ');
                    for (int j = 0; j < line.Length; j++)
                    {
                        int v = int.Parse(line[j].Trim());
                        adjlist[i].AddLast(v);
                    }
                }
            }
        }
        private void WriteBipartite(bool flag, string fname)
        {
            using (StreamWriter file = new StreamWriter(fname))
            {
                if (flag)
                {
                    file.Write("Yes");
                }
                else
                    file.Write("No");
            }
        }
        private bool Cycle()
        {
            bool flag = false;
            visited = new bool[N + 1];
            pre = new int[N + 1];
            int s = 1;
            Stack<int> stack = new Stack<int>();
            visited[s] = true;
            pre[s] = 1;
            stack.Push(s);
            while (stack.Count != 0)
            {
                int u = stack.Pop();
                foreach (int v in adjlist[u - 1])
                {
                    if (!visited[v])
                    {
                        visited[v] = true;
                        pre[v] = 1 - pre[u];
                        stack.Push(u);
                        stack.Push(v);
                        continue;
                    }
                    if (stack.Contains(v))
                    {
                        return true;
                    }
                }
            }
            return flag;
        }
        private void WritePathDFS(string fname, List<int> res)
        {
            using (StreamWriter file = new StreamWriter(fname.Substring(0, fname.Length - 7) + "OUT.txt"))
            {
                file.WriteLine(String.Format("{0,-3}", res.Count));
                Console.WriteLine(String.Format("{0,-3}", res.Count));
                if (res.Count < 2)
                {
                    file.WriteLine(String.Format("Khong co duong di tu {0,-3} den {1,-3}", sVertex + 1, eVertex + 1));
                    Console.WriteLine(String.Format("Khong co duong di tu {0,-3} den {1,-3}", sVertex + 1, eVertex + 1));
                }
                else
                {
                    foreach (int v in res)
                    {
                        file.Write(String.Format("{0,-3}", v + 1));
                        Console.Write(String.Format("{0,-3}", v + 1));
                    }
                    file.WriteLine();
                    Console.WriteLine();
                }
            }
        }

        private List<int> TracePathDFS()
        {
            List<int> res = new List<int>();
            res.Add(eVertex);
            int v = back[eVertex];
            if (v != -2)
            {
                while (v != sVertex)
                {
                    res.Insert(0, v);
                    v = back[v];
                }
                if (v == sVertex)
                {
                    res.Insert(0, v);
                }
            }
            return res;
        }

        private void PathDFS()
        {
            Stack<int> stack = new Stack<int>();
            InitIntArray(-2);
            stack.Push(sVertex);
            back[sVertex] = -1;
            while (stack.Count > 0)
            {
                int v = stack.Pop();
                if (v == eVertex)
                {
                    break;
                }
                foreach (int vv in adjList[v])
                {
                    if (back[vv] == -2)
                    {
                        stack.Push(vv);
                        back[vv] = v;
                    }
                }
            }
        }
        private void ReadAdjListPath(string fname)
        {
            string[] lines = File.ReadAllLines(fname);
            string[] line = lines[0].Split(' ');
            N = int.Parse(line[0].Trim());
            if (line.Length > 1 && line[1].Trim().Length > 0)
                sVertex = int.Parse(line[1].Trim()) - 1;

            if (line.Length > 2 && line[2].Trim().Length > 0)
                eVertex = int.Parse(line[2].Trim()) - 1;
            Console.WriteLine("Number of vertices: " + N);
            adjList = new List<List<int>>();
            for (int i = 0; i < N; i++)
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
            adjList = new List<List<int>>();
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
                adjList.Add(list);
            }
            Console.WriteLine("Read Completely!!");
        }
        private bool IsBridge()
        {
            int prev = CountCncComponentsBFS().Count;
            adjList[removedV1].Remove(removedV2);
            adjList[removedV2].Remove(removedV1);
            int cur = CountCncComponentsBFS().Count;
            adjList[removedV1].Add(removedV2);
            adjList[removedV2].Add(removedV1);
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
        private void InitIntArray(int value = -2)
        {
            color = new int[N];
            back = new int[N];
            for (int i = 0; i < color.Length; i++)
            {
                color[i] = 0;
                back[i] = value;
            }
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
                if (v < 0 || v >= adjList.Count)
                {
                    continue;
                }

                foreach (int vv in adjList[v])
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
    }
}
