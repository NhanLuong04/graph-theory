using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab05
{
    internal class Graph
    {
        List<List<int>> adjList;
        public int N { get; set; }
        public int E { get; set; }
        int sVertex, eVertex;
        int[] color, back;
        //Bai 1
        public void ListCntVertices(string fname)
        {
            ReadAdjListDFS(fname);
            List<int> list = DFS();
            WriteCntVerticesDFS(fname,list);
        }
        //Bai 2
        public void PathDFS(string fname)
        {
            ReadAdjListPath(fname);
            PathDFS();
            List<int> list = TracePathDFS();
            WritePathDFS(fname.Substring(0, fname.Length - 7) + "OUT.txt", list);
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
            List<int>res = new List<int>();
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
            while(stack.Count > 0)
            {
                int v = stack.Pop();
                if(v == eVertex)
                {
                    break;
                }
                foreach(int vv in adjList[v])
                {
                    if (back[vv] == -2)
                    {
                        stack.Push(vv);
                        back[vv] =v ;
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

        private void WriteCntVerticesDFS(string fname, List<int> res)
        {
            using (StreamWriter file = new StreamWriter(fname.Substring(0, fname.Length - 7) + "OUT.txt"))
            {
                file.WriteLine(String.Format("{0,-3}", res.Count-1));
                Console.WriteLine(String.Format("{0,-3}", res.Count-1));
                foreach (int v in res)
                {
                    if(v != sVertex)
                    {
                        file.Write(String.Format("{0,-3}", v + 1));
                        Console.Write(String.Format("{0,-3}", v + 1));
                    }
                
                }
                file.WriteLine();
                Console.WriteLine();
            }
        }

        private List<int> DFS()
        {
            List<int>res = new List<int>();
            Stack<int> stack = new Stack<int>();
            InitIntArray(0);
            stack.Push(sVertex);
            color[sVertex] = 1;
            while(stack.Count > 0)
            {
                int v = stack.Pop();
                res.Add(v);
                foreach(int vv in adjList[v])
                {
                    if (color[vv] == 0)
                    {
                        stack.Push(vv);
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
            }
        }
        private void ReadAdjListDFS(string fname)
        {
            string[]lines = File.ReadAllLines(fname);
            string[] line = lines[0].Split(' ');
            N = int.Parse(line[0].Trim());
            sVertex = int.Parse(line[1].Trim()) - 1;
            Console.WriteLine("Number of vertices: " + N);
            adjList = new List<List<int>>();
            for(int i = 0; i < N; i++)
            {
                line = lines[i + 1].Trim().Split(' ');
                List<int> list = new List<int>();
                for(int j = 0; j < line.Length; j++)
                {
                    if (line[j].Trim().Length>0)
                    {
                        list.Add(int.Parse(line[j].Trim()) - 1);
                    }
                }
                adjList.Add(list);
            }
            Console.WriteLine("Read Completely!!");
        }
    }
}
