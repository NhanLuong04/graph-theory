using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab08
{
    internal class Graph
    {
        private int[,] direction = new int[,] { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

        private int m, n, x, y;
        private int[,] a;
        private bool[,] visited;
        private int[,] cost;

        const int INF = (int)1e9;

        // FloyWarshall Update Varriables
        private int[,] pre;
        private int[,] dist;

        // Circle Update Varriables
        private int s, t;
        private List<Tuple<int, int, int>> rawData;
        private LinkedList<Tuple<int, double>>[] adj;
        private bool[] processed;
        private double[] distances;

        //School Update Varriables
        private int k;
        private LinkedList<Tuple<int, int>>[] graphCar;
        private LinkedList<Tuple<int, int>>[] graphWalk;
        private LinkedList<Tuple<int, int>>[] graphCarReverse;

        private int[] costCar;
        private int[] costWalk;
        private int[] costCarReverse;
        //Bai1
        public void GoMargin(string fname)
        {
            ReadGoMargin(fname);
            double distance = Dijsktra(x, y);
            WriteGoMargin(fname.Substring(0, fname.Length - 7) + "OUT.txt", distance);
        }
        //Bai2
        public void ChooseCity(string fname)
        {
            ReadMatrix2Adjlist(fname);
            ChooseCity();
        }
        //Bai3
        public void MoveCircle(string fname)
        {
            ReadMoveCircle(fname);
            GeneratedAdjMatrix();
            double res = DjstraForCircle(s);
            WriteGoMargin(fname.Substring(0, fname.Length - 7) + "OUT.txt", res);
        }
        //Bai4
        public void Go2School(string fname)
        {
            ReadG25(fname);
            double value = Go2School();
            WriteGoMargin(fname.Substring(0, fname.Length - 7) + "OUT.txt", value);
        }

        private double Go2School()
        {
            costCar = new int[n + 1];
            costWalk = new int[n + 1];
            costCarReverse = new int[n + 1];

            DijsktraPromise(1, graphCar, costCar);
            DijsktraPromise(k, graphWalk, costWalk);
            DijsktraPromise(n, graphCarReverse, costCarReverse);

            int result = int.MaxValue, _index = -1;
            for (int i = 2; i <= n - 1; i++)
                if (costCar[i] + costWalk[i] <= 59 && result > costCarReverse[i] && i != k)
                {
                    result = costCarReverse[i];
                    _index = i;
                }
            return result + costCar[_index];
        }
        public void DijsktraPromise(int start, LinkedList<Tuple<int, int>>[] graph, int[] costByGraph)
        {
            processed = new bool[n + 1];

            for (int i = 1; i <= n; i++)
            {
                costByGraph[i] = INF;
                processed[i] = false;
            }

            costByGraph[start] = 0;

            for (int k = 1; k <= n; k++)
            {
                int a = -1;
                for (int i = 1; i <= n; i++)
                    if (processed[i] == false && (a == -1 || costByGraph[a] > costByGraph[i]))
                        a = i;
                if (costByGraph[a] == INF) break;
                processed[a] = true;

                foreach (Tuple<int, int> edge in graph[a])
                {
                    int b = edge.Item1;
                    int w = edge.Item2;

                    if (costByGraph[b] > costByGraph[a] + w)
                        costByGraph[b] = costByGraph[a] + w;
                }
            }
        }

        private void ReadG25(string fname)
        {
            string[] lines = System.IO.File.ReadAllLines(fname);
            string[] line = lines[0].Trim().Split(' ');
            n = int.Parse(line[0]);
            m = int.Parse(line[1]);
            k = int.Parse(line[2]);

            graphCar = new LinkedList<Tuple<int, int>>[n + 1];
            graphWalk = new LinkedList<Tuple<int, int>>[n + 1];
            graphCarReverse = new LinkedList<Tuple<int, int>>[n + 1];



            for (int i = 1; i <= n; i++)
            {
                graphCar[i] = new LinkedList<Tuple<int, int>>();
                graphWalk[i] = new LinkedList<Tuple<int, int>>();
                graphCarReverse[i] = new LinkedList<Tuple<int, int>>();
            }

            for (int i = 1; i <= m; i++)
            {
                line = lines[i].Trim().Split(' ');
                int u = int.Parse(line[0]);
                int v = int.Parse(line[1]);
                int walk = int.Parse(line[2]);
                int car = int.Parse(line[3]);
                graphCar[u].AddLast(new Tuple<int, int>(v, car));
                graphWalk[v].AddLast(new Tuple<int, int>(u, walk));
                graphCarReverse[v].AddLast(new Tuple<int, int>(u, car));


            }
        }

        private double DjstraForCircle(int start)
        {
            distances = new double[n + 1];
            processed = new bool[n + 1];

            for (int i = 0; i < n; i++)
            {
                distances[i] = INF;
                processed[i] = false;
            }

            distances[start] = 0;

            for (int k = 1; k <= n; k++)
            {
                // Lấy min
                int a = -1;
                for (int i = 1; i <= n; i++)
                    if (processed[i] == false && (a == -1 || distances[a] > distances[i]))
                        a = i;
                if (distances[a] == INF) break;
                processed[a] = true;

                // So sánh chi phí
                foreach (var edge in adj[a])
                {
                    int b = edge.Item1;
                    double w = edge.Item2;
                    if (distances[b] > distances[a] + w)
                    {
                        distances[b] = distances[a] + w;
                    }
                }

            }
            return distances[t]; // Min cost at t ( FROM S -> T)
        }

        private void GeneratedAdjMatrix()
        {
            adj = new LinkedList<Tuple<int, double>>[n + 1];

            for (int i = 1; i <= n; i++)
            {
                adj[i] = new LinkedList<Tuple<int, double>>();
            }

            for (int i = 1; i <= n; i++)
            {
                // Lấy đường tròn i
                Tuple<int, int, int> firstCircle = rawData[i - 1];
                int xFirstCircle = firstCircle.Item1;
                int yFirstCircle = firstCircle.Item2;
                for (int j = i + 1; j <= n; j++)
                {
                    // Lấy đường tròn j
                    Tuple<int, int, int> secondCircle = rawData[j - 1];
                    int xSecondCircle = secondCircle.Item1;
                    int ySecondCircle = secondCircle.Item2;

                    //Tìm khoảng cách giữa 2 đường tròn
                    double d = Math.Sqrt(Math.Pow(xFirstCircle - xSecondCircle, 2) + Math.Pow(yFirstCircle - ySecondCircle, 2));

                    // Đồ thị vô hướng thêm vào cả 2 đỉnh danh sách kề
                    adj[i].AddLast(new Tuple<int, double>(j, d));
                    adj[j].AddLast(new Tuple<int, double>(i, d));

                }
            }
            Console.WriteLine(adj);
        }

        private void ReadMoveCircle(string fname)
        {
            string[] lines = System.IO.File.ReadAllLines(fname);
            string[] line = lines[0].Trim().Split(' '); 
            n = int.Parse(line[0]);
            s = int.Parse(line[1]);
            t = int.Parse(line[2]);

            rawData = new List<Tuple<int, int, int>>(n + 1);

            for (int i = 1; i <= n; i++)
            {
                line = lines[i].Trim().Split(' ');

                int xi = int.Parse(line[0]);
                int yi = int.Parse(line[1]);
                int ri = int.Parse(line[2]);

                rawData.Add(new Tuple<int, int, int>(xi, yi, ri));
            }
        }

        private void WriteCity(string fname, int cityIndex, int distance)
        {
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fname))
            {
                writer.WriteLine($"{cityIndex}");
                writer.WriteLine($"{distance}");
            }
        }

        private void ChooseCity()
        {
            dist = new int[n, n];
            pre = new int[n, n];

            // Initialize 
            for(int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                        dist[i, j] = 0;
                    if (a[i, j] > 0)
                    {
                        dist[i, j] = a[i, j];
                        pre[i, j] = i;
                    }
                    else
                    {
                        dist[i, j] = INF;
                        pre[i, j] = i;
                    }
                }
            }

            // Loop function to find dist
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (dist[i, j] > (dist[i, k] + dist[k, j]))
                        {
                            dist[i, j] = dist[i, k] + dist[k, j];
                            pre[i, j] = pre[i, k];
                        }
                    }
                }
            }


            // i == j cost is zero
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (i == j)
                        dist[i, j] = 0;
            Console.WriteLine();

            List<(int, int)> res = new List<(int, int)>();

     
            for (int i = 0; i < n; i++)
            {
                int _currentMax = 0;
                for (int j = 0; j < n; j++)
                {
                   if(dist[i,j] > _currentMax)
                    {
                        _currentMax = dist[i, j];
                    }
                }
                res.Add((_currentMax, i));            
            }
            (int, int) result = res.OrderBy(item => item.Item1).First();
            string fname = "ChonThanhPho.INP.txt";
            WriteCity(fname.Substring(0, fname.Length - 7) + "OUT.txt", result.Item2 + 1, result.Item1);
        }

        private void ReadMatrix2Adjlist(string fname)
        {
            string[] lines = System.IO.File.ReadAllLines(fname);
            string[] line = lines[0].Trim().Split(' ');
            n = int.Parse(line[0]);
            a = new int[n, n];
            for (int i = 1; i <= n; i++)
            {
                line = lines[i].Trim().Split(' ');
                for (int j = 0; j < line.Length; j++)
                {
                    a[i - 1, j] = int.Parse(line[j]);
                }
            }
        }

        private void WriteGoMargin(string fname, double distance)
        {
            using(System.IO.StreamWriter file = new System.IO.StreamWriter(fname))
            {
                file.Write(String.Format("{0:F0}", distance));
            }
        }

        private int Dijsktra(int xPoint, int yPoint)
        {
            InitializeResources();
            cost[xPoint, yPoint] = a[xPoint, yPoint];
            SortedSet<Tuple<int, int, int>> sortedSet = new SortedSet<Tuple<int, int, int>>();

            // Priority for min cost (to compare in Dijsktra Algorithm )
            Tuple<int, int, int> t = new Tuple<int, int, int>(cost[xPoint, yPoint], xPoint, yPoint);
            sortedSet.Add(t); // Add the first elements

            while (sortedSet.Count != 0)
            {
                Tuple<int, int, int> minValue = sortedSet.Min;

                sortedSet.Remove(minValue);

                if (visited[minValue.Item2, minValue.Item3])
                    continue;

                visited[minValue.Item2, minValue.Item3] = true;

                //Kiểm tra nếu điểm (x,y) là điểm ở biên, trả về kết quả. Kết thúc thuật toán
                if (isOutline(minValue.Item2, minValue.Item3))
                    return cost[minValue.Item2, minValue.Item3];

                //Duyệt các điểm (u,v) kề với điểm (x,y), sử dụng mảng direction
                for (int i = 0; i <= 3; i++)
                {
                    int u = minValue.Item2 + direction[i, 0];
                    int v = minValue.Item3 + direction[i, 1];

                    //Với mỗi điểm (u,v) chưa viếng thăm và có chi phí chưa tối ưu                 
                    if (!visited[u, v] && (cost[u, v] > (cost[minValue.Item2, minValue.Item3] + a[u, v])))
                    {
                        //Cập nhật lại chi phí tại điểm (u,v)
                        cost[u, v] = cost[minValue.Item2, minValue.Item3] + a[u, v];
                        //Thêm (u,v) và trong SortedSet
                        sortedSet.Add(new Tuple<int, int, int>(cost[u, v], u, v));

                    }
                }
            }
            return 0;
        }

        private bool isOutline(int a, int b)
        {
            if (a - 1 == 0 || b - 1 == 0 | a == n | b == m)
                return true;
            return false;
        }

        private void InitializeResources()
        {
            visited = new bool[n + 1, m + 1];
            cost = new int[n + 1, m + 1];
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    visited[i, j] = false;
                    cost[i, j] = INF;
                }
            }
        }

        private void ReadGoMargin(string fname)
        {
            string[]lines = System.IO.File.ReadAllLines(fname);
            string[] line = lines[0].Trim().Split(' ');
            n = int.Parse(line[0].Trim());
            m = int.Parse(line[1].Trim());
            x = int.Parse(line[2].Trim());
            y = int.Parse(line[3].Trim());
            a = new int[n + 1, m + 1];
            for (int i = 1; i <= n; i++)
            {
                line = lines[i].Trim().Split(' ');
                for (int j = 1; j <= m; j++)
                {
                    a[i, j] = int.Parse(line[j - 1]);
                }
                Console.WriteLine();
            }
        }

    }
}
