using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
namespace Lab03
{
    internal class Graph
    {
        List<List<int>> adjlist;
        int numVertices;
        public Graph() { numVertices = 0; }
        int sVertex;
        int eVertex;
        int[] color;
        int[] back;
        //Bai 01
        public void ListConnectedVertices(string fname)
        {
            ReadAdjListBFS(fname);
            List<int> res = BFS(sVertex);
            WriteBFS(fname, res);
        }
        //Bai 02
        public void PathBFS(string fname)
        {
            ReadAdjListBFS(fname);
            List<int> res = PathBFS(sVertex, eVertex);
            WritePathBFS(fname, res);
        }
        //Bai 03
        public void BFSConnected(string fname)
        {
            ReadAdjListBFS(fname);
            sVertex = 0;
            List<int> res = BFS(sVertex);
            WriteBFSBai3(fname, res);
        }

        //Bai 04 
        public void ConnectedComponents(string fname)
        {
            ReadAdjListBFS(fname);
            List<List<int>> list = CoutConnectedComponents();
            WriteConnectedComponents(fname,list);
        }

        private void WriteConnectedComponents(string fname, List<List<int>> list)
        {
            using(StreamWriter file = new StreamWriter(fname.Substring(0,fname.Length-7) + "OUT.txt"))
            {
                file.WriteLine(String.Format("{0,-3}", list.Count));
                Console.WriteLine(String.Format("{0,-3}", list.Count));
            }
        }

        private List<List<int>> CoutConnectedComponents()
        {
            List<List<int>> list = new List<List<int>>();
            InitIntArray(-2);
            for(int i = 0; i < numVertices; i++)
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
            while(q.Count > 0)
            {
                int v = q.Dequeue();
                foreach(int i in adjlist[v])
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

        private void WriteBFSBai3(string fname, List<int> res)
        {
            using(StreamWriter file = new StreamWriter(fname.Substring(0, fname.Length-7) + "OUT.txt"))
            {
                if(res.Count == (numVertices - 1))
                {
                    file.WriteLine("YES");
                }
                else
                {
                    file.WriteLine("NO");
                }
            }
        }

        private void WritePathBFS(string fname, List<int> res)
        {
            using(StreamWriter file = new StreamWriter(fname.Substring(0,fname.Length-7) + "OUT.txt"))
            {
                file.WriteLine(String.Format("{0,-3}", res.Count));
                Console.WriteLine(String.Format("{0,-3}", res.Count));
                if(res.Count < 2)
                {
                    file.WriteLine(String.Format("Khong co duong di tu {0,-3} den {1,-3}", sVertex + 1, eVertex + 1));
                    Console.WriteLine(String.Format("Khong co duong di tu {0,-3} den {1,-3}", sVertex + 1, eVertex+1));
                }
                else
                {
                    foreach(int v in res)
                    {
                        file.Write(String.Format("{0,-3}", v + 1));
                        Console.Write(String.Format("{0,-3}", v + 1));
                    }
                    file.WriteLine();
                    Console.WriteLine();
                }
            }
        }

        private List<int> PathBFS(int sVertex, int eVertex)
        {
            BFSBai2(sVertex, eVertex);
            List<int> res = TracePath(sVertex,eVertex);
            return res;
        }

        private List<int> TracePath(int sVertex, int eVertex)
        {
            List<int> res = new List<int>();
            res.Add(eVertex);
            int v = back[eVertex];
            if(v != -2)
            {
                while(v != sVertex)
                {
                    res.Insert(0, v);
                    v = back[v];
                }
                if(v == sVertex)
                {
                    res.Insert(0, v);
                }
            }
            return res;
        }

        private void BFSBai2(int sVertex, int eVertex)
        {
            Queue<int> queue = new Queue<int>();
            InitIntArray(-2);
            color[sVertex] = 1;
            back[sVertex] = -1;
            bool isBreak = false;

            // Enqueue the starting vertex
            queue.Enqueue(sVertex);

            while (queue.Count > 0)
            {
                int v = queue.Dequeue();

                foreach (int i in adjlist[v])
                {
                    if (color[i] == 0)
                    {
                        queue.Enqueue(i);
                        back[i] = v;
                        color[i] = 1;
                    }
                    if (i == eVertex)
                    {
                        isBreak = true;
                        break;
                    }
                }

                if (isBreak)
                {
                    break;
                }
                color[v] = 2;
            }
        }

        private void WriteBFS(string fname, List<int> res)
        {
            using(StreamWriter file = new StreamWriter(fname.Substring(0, fname.Length - 7) + "OUT.txt"))
            {
                file.WriteLine(String.Format("{0,-3}", res.Count));
                Console.WriteLine(String.Format("{0,-3}", res.Count));
                foreach(int v in res)
                {
                    file.Write(String.Format("{0,-3}", v + 1));
                    Console.Write(String.Format("{0,-3}", v + 1));
                }
                file.WriteLine();
                Console.WriteLine();
            }
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
            adjlist = new List<List<int>>();
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
                adjlist.Add(list);
            }
            Console.WriteLine("Read Completely!!");
        }

        private List<int> BFS(int sVertex)
        {
            List<int> res = new List<int>();
            Queue<int> q = new Queue<int>();
            q.Enqueue(sVertex);
            InitIntArray(0);
            color[sVertex] = 1;
            while(q.Count > 0)
            {
                int v = q.Dequeue();
                foreach(int i in adjlist[v])
                {
                    if (color[i] == 0)
                    {
                        q.Enqueue(i);
                        color[i] = 1;
                        res.Add(i);
                    }
                }
                color[v] = 2;
            }
            return res;
        }

        private void InitIntArray(int value = -2)
        {
            color = new int[numVertices];
            back = new int[numVertices];
            for(int i = 0;i < color.Length;i++)
            {
                color[i] = 0;
                back[i] = value;
            }
        }
    }
}
