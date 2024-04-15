using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab09
{
    internal class Graph
    {
        //spanning tree
        protected int n, m;
        protected LinkedList<int>[] adj;
        protected List<Tuple<int, int>> spanningTree;
        protected bool[] visited;
        //Kruskal
        protected int w;
        protected List<Tuple<int, int, int>> edgeKruskal;
        protected LinkedList<Tuple<int, int, int>> spanningTreeKruskal;
        protected int[] parent;
        protected int[] rank;
        protected int count, cost;
        //Prim
        protected List<Tuple<int, int, int>> edgePrim;
        protected LinkedList<Tuple<int, int, int>> spanningTreePrim;
        protected bool[] selected;
        //Bai1
        public void SpanningTree(string fname)
        {
            ReadSpanningTree(fname);
            List<Tuple<int, int>> list = SpanningTreeDFS(1);
            WriteStringList(fname.Substring(0, fname.Length - 7) + "OUT.txt", list);
        }
        //Bai2
        public void KruskalMST(string fname)
        {
            ReadKruskal(fname);
            LinkedList<Tuple<int, int, int>> list = KruskalOptimizer();
            WriteEdges(fname.Substring(0, fname.Length - 7) + "OUT.txt", list);
        }
        //Bai3
        public void PrimMST(string fname)
        {
            ReadPrim(fname);
            LinkedList<Tuple<int, int, int>> list = Prim();
            WriteList(fname.Substring(0, fname.Length - 7) + "OUT.txt", list);
        }
        //Bai4
        public void SpanningTreeX(string fname)
        {
            ReadKruskal(fname);
            int edges = SpanningTreeX(5);
            WriteXTree(fname.Substring(0, fname.Length - 7) + "OUT.txt", edges);
        }
        private int SpanningTreeX(int x)
        {
            // Sắp xếp các cạnh theo thứ tự tăng dần của trọng số
            edgeKruskal.Sort((e1, e2) => e1.Item3.CompareTo(e2.Item3));

            // Tạo cấu trúc dữ liệu Union-Find
            parent = new int[n + 1];
            rank = new int[n + 1];
            for (int i = 1; i <= n; i++)
            {
                MakeSet(i);
            }

            // Khởi tạo biến lưu tổng trọng số của cây khung
            int totalWeight = 0;

            // Duyệt qua các cạnh theo thứ tự đã sắp xếp
            foreach (var e in edgeKruskal)
            {
                int u = e.Item1;
                int v = e.Item2;
                int w = e.Item3;

                // Nếu cạnh đó không tạo thành chu trình và có trọng số lớn hơn hoặc bằng x
                if (Find(u) != Find(v) && w >= x)
                {
                    // Thêm trọng số của cạnh vào tổng trọng số của cây khung
                    totalWeight += w;

                    // Nối hai đỉnh u và v trong Union-Find
                    Union(u, v);
                }
            }

            // Kiểm tra xem cây khung có hợp lệ không
            bool valid = true;
            foreach (var e in edgeKruskal)
            {
                int u = e.Item1;
                int v = e.Item2;
                int w = e.Item3;

                // Nếu có cạnh có trọng số nhỏ hơn x nhưng không thuộc cây khung
                if (w < x && Find(u) != Find(v))
                {
                    valid = false;
                    break;
                }
            }

            // Nếu cây khung không hợp lệ, trả về -1
            if (!valid)
                return -1;

            // Trả về tổng trọng số của cây khung
            return totalWeight;
        }



        private void WriteXTree(string fname, int totalWeight)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fname))
            {
                if (totalWeight == -1)
                {
                    file.WriteLine(-1);
                }
                else
                {
                    file.WriteLine(totalWeight);
                }
            }
        }

        private void WriteList(string fname, LinkedList<Tuple<int, int, int>> list)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fname))
            {
                file.WriteLine(spanningTreePrim.Count + " " + cost);
                foreach (var x in spanningTreePrim)
                {
                    file.Write(x.Item1 + " " + x.Item2 + " " + x.Item3);
                    file.WriteLine();
                }
            }
        }

        private LinkedList<Tuple<int, int, int>> Prim()
        {
            cost = 0;
            selected = new bool[n + 1];
            for (int i = 0; i <= n; i++)
                selected[i] = false;
            selected[1] = true;

            spanningTreePrim = new LinkedList<Tuple<int, int, int>>();
            for (int i = 1; i <= n - 1; i++)
            {
                Tuple<int, int, int> edge = new Tuple<int, int, int>(0, 0, 0);

                foreach (var e in edgePrim)
                {
                    int u = e.Item1;
                    int v = e.Item2;
                    int w = e.Item3;

                    if ((selected[u] == true && selected[v] == false) &&
                        (edge.Item1 == 0 || edge.Item3 > w))
                        edge = new Tuple<int, int, int>(u, v, w);

                    if ((selected[v] == true && selected[u] == false) &&
                        (edge.Item1 == 0 || edge.Item3 > w))
                        edge = new Tuple<int, int, int>(v, u, w);
                }
                if (edge.Item1 == 0) break;

                selected[edge.Item2] = true;
                spanningTreePrim.AddLast(edge);
                cost += edge.Item3;

            }
            return spanningTreeKruskal;
        }

        private void ReadPrim(string fname)
        {
            string[] lines = System.IO.File.ReadAllLines(fname);
            string[] line = lines[0].Trim().Split(' '); 
            n = int.Parse(line[0]);
            m = int.Parse(line[1]);
            edgePrim = new List<Tuple<int, int, int>>();

            for (int i = 1; i <= m; i++)
            {
                line = lines[i].Trim().Split(' '); 
                int u = int.Parse(line[0]);
                int v = int.Parse(line[1]);
                int w = int.Parse(line[2]);
                edgePrim.Add(new Tuple<int, int, int>(u, v, w));

            }
        }

        private void WriteEdges(string fname, LinkedList<Tuple<int, int, int>> list)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fname))
            {
                file.WriteLine(count + " " + cost);
                foreach (var x in spanningTreeKruskal)
                {
                    file.Write(x.Item1 + " " + x.Item2 + " " + x.Item3);
                    file.WriteLine();
                }
            }
        }

        private LinkedList<Tuple<int, int, int>> KruskalOptimizer()
        {
            parent = new int[n + 1];
            rank = new int[n + 1];
            for (int i = 1; i <= n; i++)
            {
                MakeSet(i);
            }

            spanningTreeKruskal = new LinkedList<Tuple<int, int, int>>();

            edgeKruskal = edgeKruskal.OrderBy(x => x.Item3).ToList();

            cost = 0;
            count = 0;

            foreach (var e in edgeKruskal)
            {
                int u = e.Item1;
                int v = e.Item2;
                int w = e.Item3;
                if (Find(u) == Find(v)) continue;

                spanningTreeKruskal.AddLast(e);
                cost += w;
                count++;
                if (count == n - 1) break;

                Union(u, v);
            }
            return spanningTreeKruskal;
        }
        public void MakeSet(int v)
        {
            parent[v] = v;
            rank[v] = 0;
        }

        public int Find(int v)
        {
            if (v == parent[v])
                return v;
            return parent[v] = Find(parent[v]);
        }

        public void Union(int a, int b)
        {
            a = Find(a);
            b = Find(b);
            if (a != b)
            {
                if (rank[a] < rank[b])
                {
                    var t = a;
                    a = b;
                    b = t;
                }
                parent[b] = a;
                if (rank[a] == rank[b])
                    rank[a]++;
            }
        }
        private void ReadKruskal(string fname)
        {            
            string[] lines = System.IO.File.ReadAllLines(fname);
            string[] line = lines[0].Trim().Split(' '); 
            n = int.Parse(line[0]);
            m = int.Parse(line[1]);
            edgeKruskal = new List<Tuple<int, int, int>>();

            for (int i = 1; i <= m; i++)
            {
                line = lines[i].Trim().Split(' ');
                int u = int.Parse(line[0]);
                int v = int.Parse(line[1]);
                int w = int.Parse(line[2]);
                edgeKruskal.Add(new Tuple<int, int, int>(u, v, w));
            }
        }


        private void WriteStringList(string fname, List<Tuple<int, int>> list)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fname))
            {
                file.WriteLine(list.Count);
                foreach (var tuple in list)
                {
                    file.WriteLine($"{tuple.Item1} {tuple.Item2}");
                }
            }
        }


        private List<Tuple<int, int>> SpanningTreeDFS(int s)
        {
            visited[s] = true;

            foreach (var edge in adj[s])
            {
                if (visited[edge] == false)
                {
                    spanningTree.Add(new Tuple<int, int>(s, edge));
                    SpanningTreeDFS(edge);
                }
            }
            return spanningTree;
        }

        private void ReadSpanningTree(string fname)
        {
            string[] lines = System.IO.File.ReadAllLines(fname);
            string[] line = lines[0].Trim().Split(' ');
            n = int.Parse(line[0]);
            m = int.Parse(line[1]);
            adj = new LinkedList<int>[n + 1];
            spanningTree = new List<Tuple<int, int>>();
            visited = new bool[n + 1];
            for (int i = 1; i <= n; i++)
                adj[i] = new LinkedList<int>();
            for (int i = 1; i <= m; i++)
            {
                line = lines[i].Trim().Split(' ');
                adj[int.Parse(line[0])].AddLast(int.Parse(line[1]));
                adj[int.Parse(line[1])].AddLast(int.Parse(line[0]));
            }
        }
    }
}
