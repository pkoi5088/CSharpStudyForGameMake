using System;
using System.Collections.Generic;

namespace Exercise
{
    class Graph
    {
        int[,] adj = new int[6, 6]
        {
            {0,1,0,1,0,0 },
            {1,0,1,1,0,0 },
            {0,1,0,0,0,0 },
            {1,1,0,0,1,0 },
            {0,0,0,1,0,1 },
            {0,0,0,0,1,0 },
        };

        int[,] wadj = new int[6, 6]
        {
            {0,15,0,35,0,0 },
            {15,0,5,10,0,0 },
            {0,5,0,0,0,0 },
            {35,10,0,0,5,0 },
            {0,0,0,5,0,5 },
            {0,0,0,0,5,0 },
        };

        public void Dijikstra(int start)
        {
            bool[] visited = new bool[6];
            int[] distance = new int[6];
            Array.Fill(distance, Int32.MaxValue);

            distance[start] = 0;
            while (true)
            {
                //제일 좋은 후보를 찾는다.
                int closet = Int32.MaxValue;
                int now = -1;
                for(int i = 0; i < 6; i++)
                {
                    if (visited[i])
                        continue;
                    if (distance[i] == Int32.MaxValue || distance[i] >= closet)
                        continue;
                    //가장 좋은 정보로 갱신
                    closet = distance[i];
                    now = i;
                }

                //후보가 없다면 종료
                if(now==-1)
                    break;

                visited[now] = true;
                for(int next = 0; next < 6; next++)
                {
                    if (wadj[now, next] == 0)
                        continue;
                    if (visited[next])
                        continue;

                    int nextDist = distance[now] = wadj[now, next];
                    if (nextDist < distance[next])
                    {
                        distance[next] = nextDist;
                    }
                }

            }
        }

        public void DFS(int now)
        {
            bool[] visited = new bool[6];
            Console.WriteLine(now);
            visited[now] = true;

            for(int next = 0; next < 6; next++)
            {
                if (adj[now, next] == 0)
                    continue;
                if (visited[next])
                    continue;
                DFS(next);
            }
        }

        public void BFS(int start)
        {
            bool[] found = new bool[6];
            Queue<int> q = new Queue<int>();
            q.Enqueue(start);
            found[start] = true;

            while (q.Count > 0)
            {
                int now = q.Dequeue();
                Console.WriteLine(now);

                for(int next = 0; next < 6; next++)
                {
                    if (adj[now, next] == 0)
                        continue;
                    if (found[next])
                        continue;
                    q.Enqueue(next);
                    found[next] = true;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph();
            //graph.DFS(0);
            graph.Dijikstra(0);
        }
    }
}
