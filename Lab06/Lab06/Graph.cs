using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab06
{
    internal class Graph
    {
        public int N { get;set; }
        LinkedList<int>[] adjList;
        bool[] visited;
        int[] pre;
        //Bai 1
        internal void Bipartile(string fname)
        {
            ReadAdjList(fname);
            bool flag = Bipartile();
            WriteBipartite(flag, fname.Substring(0,fname.Length - 7) +"OUT.txt");
        }

        private bool Bipartile()
        {
            bool flag = true;
            visited = new bool[N+1];
            pre = new int[N +1];
            int s = 1;
            Stack<int> stack = new Stack<int>();
            visited[s] = true;
            pre[s] = 1;
            stack.Push(s);
            while (stack.Count != 0)
            {
                int u = stack.Pop();
                foreach(int v in adjList[u - 1])
                {
                    if (!visited[v])
                    {
                        visited[v] = true;
                        pre[v] = 1 - pre[u];
                        stack.Push(u);
                        stack.Push(v);
                        continue;
                    }
                    if (pre[u] == pre[v])
                    {
                        return false;
                    }
                }
            }
            return flag;
        }

        private void WriteBipartite(bool flag, string fname)
        {
            using(StreamWriter file = new StreamWriter(fname))
            {
                if (flag)
                {
                    file.Write("Yes");
                }
                else
                    file.Write("No");
            }
        }

        private void ReadAdjList(string fname)
        {
            string[] lines = File.ReadAllLines(fname);
            N = int.Parse(lines[0].Trim());
            adjList = new LinkedList<int>[N];
            for(int i = 0; i < N; i++)
            {
                adjList[i] = new LinkedList<int>();
                if (lines[i + 1].Length > 0)
                {
                    string[] line = lines[i + 1].Split(' ');
                    for (int j = 0; j < line.Length; j++)
                    {
                        int v = int.Parse(line[j].Trim());
                        adjList[i].AddLast(v);
                    }
                }
            }
        }
        //Bai 2
        internal void Cycle(string fname)
        {
            ReadAdjList(fname);
            bool flag = Cycle();
            WriteBipartite(flag, fname.Substring(0, fname.Length - 7) + "OUT.txt");
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
                foreach (int v in adjList[u - 1])
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
        //Bai 3
        internal void TopoSort(string fname)
        {
            ReadAdjList(fname);
            List<int> list = TopoSorting();
            WriteTopologicalSorting(list,fname.Substring(0, fname.Length - 7) + "OUT.txt");
        }

        private void WriteTopologicalSorting(List<int> list, string fname)
        {
            using (StreamWriter file = new StreamWriter(fname))
            {
                if(list.Count == 0)
                {
                    file.WriteLine("Not TopologicalSorting");
                    Console.WriteLine("Not TopologicalSorting");
                    return;
                }
                foreach(int v in list)
                {
                    file.Write(String.Format("{0,-3}", v));
                    Console.Write(String.Format("{0,-3}", v));
                }
                file.WriteLine();
                Console.WriteLine();
            }
        }
        private List<int> TopoSorting()
        {
            List<int> result = new List<int>();
            Stack<int> stack = new Stack<int>();
            visited = new bool[N + 1];
            bool[] inStack = new bool[N + 1];

            bool DFS(int u)
            {
                visited[u] = true;
                inStack[u] = true;
                foreach (int v in adjList[u - 1])
                {
                    if (!visited[v])
                    {
                        if (DFS(v))
                            return true;
                    }
                    else if (inStack[v])
                    {
                        return true; 
                    }
                }
                inStack[u] = false;
                stack.Push(u);
                return false;
            }

            for (int i = 1; i <= N; i++)
            {
                if (!visited[i])
                {
                    if (DFS(i))
                    {
                        result.Clear(); 
                        return result;
                    }
                }
            }

            while (stack.Count > 0)
            {
                result.Add(stack.Pop());
            }

            return result;
        }

    }
}
