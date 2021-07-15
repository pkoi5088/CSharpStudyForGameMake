# 자료구조 및 알고리즘
본 Part의 내용은 MMORPG나 RPG에서 꼭 필요한 길찾기에 대한 내용이다. 온라인 게임에서 캐릭터나 몬스터의 이동에서 길찾기 알고리즘을 한번쯤은 직접 구현해볼 것이다.  
우수법, BFS, 다익스트라, A*에 대해서 알아보고 간단한 미로프로잭트를 구현해볼 예정이다.
***
## 내용
* [Big-O 표기법](#BigO-표기법)
* [선형 자료구조](#선형-자료구조)
    + [배열](#배열)
    + [동적배열](#동적배열)
    + [연결리스트](#연결리스트)
* [미로 만들기](#미로-만들기)
    + [Binary Tree 미로 생성 알고리즘](#binary-tree-미로-생성-알고리즘)
    + [SideWinder 미로 생성 알고리즘](#sidewinder-tree-미로-생성-알고리즘)
    + [이동 및 우수법](#이동-및-우수법)
* [그래프](#그래프)
    + [DFS](#dfs)
    + [BFS](#bfs)
    + [BFS를 이용한 길찾기](#bfs-를-이용한-길찾기)
    + [다익스트라 최단 경로 알고리즘](#다익스트라-최단-경로-알고리즘)
* [우선순위 큐](#우선순위-큐)
    + [힙 이론](#힙-이론)
    + [우선순위 큐](#우선순위-큐)
* [A*알고리즘](#A알고리즘)
***
## Big-O 표기법
두 알고리즘 A,B을 비교하기 위해서?
```
    1. A가 B보다 조금 빠르다 => 모호함
    2. 실행속도 비교 => 실행 환경에 따라 다름
    3. 입력값의 범위와 개수에 따라서?
```
**BIG-O를 사용하자!**
1. 수행되는 연산의 개수를 '대략적으로' 판단한다.
2. 가장 영향력이 큰 항만 남기자
```
    O(N^2 + N + 4) = O(N^2)
```
3. BIG-O 복잡도 성능 순서
```
    O(1), O(logN), O(N), O(NlogN), O(N^2), O(2^N), O(N!)
```
***
## 선형 자료구조
자료를 순차적으로 나열한 자료구조
### 배열
```
    일정한 크기의 자료들이 순서대로 나열된 자료구조
    단점: 개수의 추가가 어렵다.
```
### 동적배열
```
    배열의 단점을 개선한 구조로 유동적으로 개수를 조절할 수 있는 자료구조(List)
    단점: 개수를 조절하는데에 비용이 과다하게 발생한다면 비효율적
```
### 연결리스트
```
    자료들을 임의의 곳에 저장하되 순서에 따라 포인터를 이용해 자료들을 서로 연결해 놓은 자료구조
    장점: 중간에 삽입, 삭제가 가능하다
    단점: N번째 자료를 조회하는데 시간이 걸린다
```
***
## 미로 만들기
길찾기 알고리즘을 단일 샘플에 따라 찾는것이 아닌 자동으로 생성된 랜덤 미로에서 잘 작동이 되는지 확인하기 위한 단계
>맵의 전체 크기는 홀수이어야 한다.
***
### Binary Tree 미로 생성 알고리즘
*Board.cs의 GenerateByBinaryTree()참조*
#### 원리
1. x,y좌표중에서 하나라도 짝수라면 해당 점을 벽으로 만든다.  
     ![BST01](https://user-images.githubusercontent.com/44914802/125721401-20e90f68-9738-4f1c-a0c8-052d064006a4.PNG)
2. 각각의 빈공간에 대해서 오른쪽 벽을 없앨지, 아래 벽을 없앨지 결정한다.  
    ![BST02](https://user-images.githubusercontent.com/44914802/125721408-1dfa3b2e-3480-4587-b4bb-916bca710a0c.PNG)
3. 외벽이 뚫렸을 경우, 다시 벽으로 만든다.
4. 도착점까지 도달을 못하는 미로일수 있음으로, 외벽을 제외한 가장 아래 줄과 가장 오른쪽 줄을 전체 빈공간으로 만든다.  
    ![BST03](https://user-images.githubusercontent.com/44914802/125721412-a85421f6-48d8-4059-8546-740ccd45611a.PNG)
#### 코드 설명
1. x가 짝수거나 y가 짝수면 해당 점을 벽으로 만든다.
```c#
            for(int y = 0; y < _size; y++)
            {
                for(int x = 0; x < _size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        _tile[y, x] = TileType.Wall;
                    else
                        _tile[y, x] = TileType.Empty;
                }
            }
```
2. 빈 공간인 점들에 대해서 오른쪽, 아래벽중 뚫을 벽을 정하고 위의 3,4번 과정을 거쳐준다.
```c#
            Random rand = new Random();
            for (int y = 0; y < _size; y++)
            {
                for (int x = 0; x < _size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;
                    //도착점일 경우 뚫지 않는다.
                    if (y == size - 2 && x == size - 2)
                        continue;
                    
                    //가장 아래 점, 가장 오른쪽 점일 경우 빈공간으로 만든다.
                    if (y == size - 2)
                    {
                        _tile[y, x + 1] = TileType.Empty;
                        continue;
                    }
                    if (x == size - 2)
                    {
                        _tile[y+1, x] = TileType.Empty;
                        continue;
                    }

                    //오른쪽과 아래쪽중 선택해 벽을 빈공간으로 만든다.
                    if (rand.Next(0, 2) == 0)
                    {
                        _tile[y, x + 1] = TileType.Empty;
                    }
                    else
                    {
                        _tile[y + 1, x] = TileType.Empty;
                    }
                }
            }
```
#### 단점
>미로의 모양이 일정한 규칙이 존재한다.
ex. 가장 아래줄이나 가장 오른쪽 줄은 항상 빈칸이다.
***
### SideWinder Tree 미로 생성 알고리즘
*Board.cs의 GenerateBySideWinder()참조*
#### 원리
1. x,y좌표중에서 하나라도 짝수라면 해당 점을 벽으로 만든다.
2. 각각의 빈공간에 대해서 오른쪽 벽을 없앨지, 아래 벽을 없앨지 결정한다.
3. 아래벽을 없앨시, 해당 점의 아래를 뚫는 것이 아니라 최근에 오른쪽으로 진행된 점들을 포함한 그룹들 중에서 아래벽을 뚫는다.  
    ![SW01](https://user-images.githubusercontent.com/44914802/125721417-7bdfb7f8-0692-4b81-8803-ea2371dccb64.PNG)
4. 외벽이 뚫렸을 경우, 다시 벽으로 만든다.
5. 도착점까지 도달을 못하는 미로일수 있음으로, 외벽을 제외한 가장 아래 줄과 가장 오른쪽 줄을 전체 빈공간으로 만든다.  
    ![SW02](https://user-images.githubusercontent.com/44914802/125721419-598de2c2-3685-48db-abf7-a221294bcb39.PNG)
#### 코드 설명
1,2,4,5번은 BinaryTree알고리즘과 동일하다.
3. 아래벽을 없앨시, 해당 점의 아래를 뚫는 것이 아니라 최근에 오른쪽으로 진행된 점들을 포함한 그룹들 중에서 아래벽을 뚫는다.
```c#
            Random rand = new Random();
            for (int y = 0; y < _size; y++)
            {
                int count = 0;
                for (int x = 0; x < _size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;
                    if (y == _size - 2 && x == _size - 2)
                        continue;

                    if (y == _size - 2)
                    {
                        _tile[y, x + 1] = TileType.Empty;
                        continue;
                    }
                    if (x == _size - 2)
                    {
                        _tile[y + 1, x] = TileType.Empty;
                        continue;
                    }

                    if (rand.Next(0, 2) == 0)
                    {
                        //오른쪽으로 뚫을 시 카운트해준다.
                        _tile[y, x + 1] = TileType.Empty;
                        count++;
                    }
                    else
                    {
                        //아래로 뚫을 시 최근에 오른쪽으로 뚫은 점들을 포함해 하나를 아래로 뚫는다.
                        int randomIndex = rand.Next(0, count);
                        _tile[y + 1, x - randomIndex * 2] = TileType.Empty;
                    }
                }
```
#### 단점
>BinaryTree 알고리즘과 마찬가지로 미로의 모양에 일정한 규칙이 존재한다.
***
## 이동 및 우수법
### 플레이어 이동
길찾기 알고리즘을 수행하는 개체(플레이어)를 Player.cs에 생성한 뒤 플레이어의 위치를 표시하기 위한 작업을 수행한다. 또한 플레이어의 위치를 수정하기위한 Update()도 작성해준다.
### 우수법
*Player.cs의 RightHand(), Update()참조*
플레이어가 현재상태에서 어떻게 이동할지에 대한 판단을 내리는 방법을 Update()에 구현해야한다. 현재 단계에서는 우수법을 Update()에 구현할 것이다.
>우수법이란?
한쪽 손을 벽에 붙이고 이동하는 방법으로 미로는 결국 면으로 이루어져 있기 때문에 언젠가는 미로에서 탈출할 수 있다는 접근법

#### 연산 순서
```
    1. 현재 위치, 바라보는 방향에서 오른쪽으로 갈 수 있는가
    2. 현재 위치, 바라보는 방향에서 전진할 수 있는가
    3. 왼쪽으로 회전
```
#### 코드 설명
1. RightHand()에서 경로들을 구해준 뒤
```c#
            while (PosY != board.DestY || PosX != board.DestX)
            {
                //보고 있는 방향에서 오른쪽으로 갈수 있는가
                if (_board.Tile[PosY + rightY[_dir], PosX + rightX[_dir]] == Board.TileType.Empty)
                {
                    //오른쪽으로 90도 회전
                    _dir = (_dir + 3) % 4;
                    //앞으로 전진
                    PosY += frontY[_dir];
                    PosX += frontX[_dir];
                    _points.Add(new Pos(PosY, PosX));
                }
                //보고 있는 방향에서 앞으로 갈수 있는가
                else if (_board.Tile[PosY + frontY[_dir], PosX + frontX[_dir]] == Board.TileType.Empty)
                {
                    //앞으로 전진
                    PosY += frontY[_dir];
                    PosX += frontX[_dir];
                    _points.Add(new Pos(PosY, PosX));
                }
                else
                {
                    //왼쪽으로 90도 회전
                    _dir = (_dir + 5) % 4;
                }
            }
```
2. Update()에서 하나씩 출력해준다.
```c#
            if (_lastIndex >= _points.Count)
                return;
            _sumTIck += deltaTick;
            if (_sumTIck >= MOVE_TICK)
            {
                _sumTIck = 0;

                PosY = _points[_lastIndex].Y;
                PosX = _points[_lastIndex].X;
                _lastIndex++;
            }
```
#### 실행 화면
![RightHand](https://user-images.githubusercontent.com/44914802/125721422-04ab7890-8ac1-41b5-bdc8-87fa8a9435aa.gif)
***
## 그래프
### DFS
>DFS(Depth First Search; 깊이 우선 탐색)
특정 노드에서 시작해 다음 분기로 넘어가기 전에 해당 분기를 완벽하게 탐색하는 방법이다.

DFS의 정의만으로 알고리즘을 이해하기는 힘들다. 다음의 연산과정과 Graph.cs의 DFS()를 보면서 이해하도록 노력해보자.
```
    1. 스택에 시작노드를 넣는다.
    2. 스택에서 가장 위에있는 노드를 꺼낸다.
    3. 인접한 이웃노드들 중 아직 방문하지 않은 노드를 스택에 넣고 넣은 노드를 방문처리한다.
    4. 모든 이웃노드들에 대한 연산이 끝날때 까지 2~4번을 반복한다.
    5. 현재 노드의 방문처리를 없앤다.
```
DFS는 주로 스택을 이용해 구현을 하는데 스택이 다루기 힘들다면 함수의 재귀를 이용해 스택과 DFS를 구현하기도 한다.
### BFS
>BFS(Breadth First Search; 너비 우선 탐색)
시작 정점을 방문한 후 시작 정점에 인접한 모든 정점들을 우선 방문하는 방법이다.

BFS는 DFS와 다르게 스택이 아닌 큐를 이용해 구현한다. 다음의 연산과정과 Graph.cs의 BFS()를 보면서 이해하도록 노력해보자.
```
    1. 큐에 시작노드를 넣는다.
    2. 큐에서 가장 앞에있는 노드를 꺼낸다.
    3. 인접한 이웃노드들 중 아직 방문하지 않은 노드를 큐에 넣고 넣은 노드를 방문처리한다.
    4. 모든 노드들을 방문할 때 까지 2~4번을 반복한다.
```
개인적으로 BFS가 DFS보다 이해하기 쉽다고 생각한다. 직접 그래프를 그려보고 하나씩 방문체크를 해보면 이해가 더 쉬울 것이다. 또한 BFS()에서는 노드의 번호를 출력하도록 했는데 다른 연산을 수행할 수도 있기 때문에 많이 어렵지는 않을 것이다.
***
### BFS를 이용한 길찾기
*Player.cs의 BFS참조*
현재 만들고 있는 미로 프로젝트에서 BFS를 이용해 길찾기 알고리즘을 만들고자 하는데 어떻게 미로를 Graph화 시킬수 있을까?
>Tile 정보를 이용해 Graph를 그려보자!

BFS에서 이동경로의 목록을 만든 후 제일 앞에있는 위치부터 하나씩 Update()에서 출력하자!
#### 코드 설명
```c#
            int[] deltaY = new int[] { -1, 0, 1, 0 };
            int[] deltaX = new int[] { 0, -1, 0, 1 };
            //방문 여부를 위한 2차원배열과 역추적을 위한 2차원 배열
            bool[,] found = new bool[_board.Size, _board.Size];
            Pos[,] parent = new Pos[_board.Size, _board.Size];

            Queue<Pos> q = new Queue<Pos>();
            q.Enqueue(new Pos(PosY, PosX));
            found[PosY, PosX] = true;
            parent[PosY, PosX] = new Pos(PosY, PosX);

            while (q.Count > 0)
            {
                Pos pos = q.Dequeue();
                int nowY = pos.Y;
                int nowX = pos.X;

                for(int i = 0; i < 4; i++)
                {
                    int nextY = nowY + deltaY[i];
                    int nextX = nowX + deltaX[i];

                    //항상 외벽은 Wall이므로 범위계산은 생략한다.
                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;
                    if (found[nextY, nextX])
                        continue;

                    q.Enqueue(new Pos(nextY, nextX));
                    found[nextY, nextX] = true;
                    parent[nextY, nextX] = new Pos(PosY, PosX);
                }
            }

            //역추적 parent의 위치가 자기 자신이 될때(시작점일 때)까지 추적한다.
            int y = _board.DestY;
            int x = _board.DestX;
            while (parent[y, x].Y != y || parent[y, x].X != x)
            {
                _points.Add(new Pos(y, x));
                Pos pos = parent[y, x];
                y = pos.Y;
                x = pos.X;
            }
            _points.Add(new Pos(y, x));
            //추적한 결과의 마지막은 시작점 이므로 뒤집어준다.
            _points.Reverse();
```
#### 실행 화면
![BFS](https://user-images.githubusercontent.com/44914802/125728207-56576f6b-8d92-42e5-921a-84104a443d7c.gif)
실행 화면을 보면 목적지까지의 경로를 탐색한 후 움직이기 때문에 우수법보다 훨씬 빠른것을 알 수 있다.
***
## 다익스트라 최단 경로 알고리즘
BFS알고리즘으로 길찾기는 진행하다보면 오른쪽으로 직진하면 목적지가 나오는데 왼쪽을 탐색하는 경우와 같이 불필요한 탐색을 진행하는 경우가 많이 발생한다. 물론 한번의 BFS의 연산이면 실행시간에 큰 의미를 찾을 수 없지만 수백, 수천, 수만번의 연산이라면 초,분 단위의 속도차이가 발생하게 된다. 또한 이동할때 비용이 든다면? 이동 비용이 서로 다르다면 어떻게 해야 할까?
>다익스트라 알고리즘을 사용하자!
#### 다익스트라 알고리즘
*Graph.cs의 Dijikstra()참조*
>한점으로가는 최단 경로는 다른 점까지의 최단경로거리 + 가중치이다.

```
    1. 시작노드의 distance는 0이다.
    2. 현재 노드에서 가장 가까이 있는 노드를 찾는다.
    3. 가장 유력한 후보의 거리와 번호를 저장한다.
    4. 저장한 다음 노드의 거리를 갱신한다.
    5. 2~4번 과정을 다음 노드가 발견이 안될때까지 반복한다.
```
#### 코드 설명
```c#
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

                //찾은 후보들에 대해서
                visited[now] = true;
                for(int next = 0; next < 6; next++)
                {
                    if (wadj[now, next] == 0)
                        continue;
                    if (visited[next])
                        continue;
                    //아직 방문하지 않았고 갱신가능 하면 갱신한다.
                    int nextDist = distance[now] = wadj[now, next];
                    if (nextDist < distance[next])
                    {
                        distance[next] = nextDist;
                    }
                }
            }
```
***
## 우선순위 큐
우선순위 큐는 주로 힙구조를 이용해 구현이 되는데 결론만 말하면 이진 트리구조에서 RootNode가 우선순위가 가장 높은 구조이다.
>힙과 우선순위큐의 이론과 관계는?
### 힙 이론
힙은 MaxHeap과 MinHeap으로 보통 표현되는데 쉽게 말하면 부모 노드가 자식노드보다 값이 큰 성질을 가지면 MaxHeap, 부모 노드가 자식노드보다 값이 작은 성질을 가지면 MinHeap이다. 이러한 성질을 항상 가지고 있는 트리의 Root는 전체 트리의 최대, 최소값임을 확신할 수 있다.
#### 구조
힙은 마지막 레벨을 제외한 모든 레벨에 노드가 채워져 있는데 이는 힙구조를 배열로 표현했을때 두 노드간의 공백이 발생하면 안된다는 것을 의미한다. 구조가 정해지면 다음과 같은 성질이 있다.
>노드의 개수를 알면 힙의 구조를 특정할 수 있다.
#### 삽입
1. 힙을 배열로 표현하는데 가장 마지막위치에 넣고자하는 원소를 삽입한다.
2. 마지막 노드에 대해서 힙의 성질이 유지되도록 해당 노드와 부모노드의 위치를 조절해준다.
#### 삭제
1. 힙의 Root를 제거한다.
2. 가장 마지막 노드를 Root로 옮긴다.
3. Root노드와 자식노드를 힙 성질을 위배하지 않도록 위치를 조절한다.
### 우선순위 큐
*PriorityQueue.cs참조*
힙 구조에서 Max와 Min이 아닌 사용자가 정의한 우선순위대로 힙구조를 구현한다면 그것이 우선순위 큐가 될 것이다. PriorityQueue.cs에서는 MaxHeap을 기반으로 구현하였다.
#### 삽입
```c#
            //맨 끝에 데이터를 삽입한다.
            _heap.Add(data);

            int now = _heap.Count - 1;
            while (now > 0)
            {
                //부모노드의 index
                int next = (now - 1) / 2;
                if (_heap[now] < _heap[next])
                    break;

                //부모와 자식을 교체
                int tmp = _heap[next];
                _heap[next] = _heap[now];
                _heap[now] = tmp;

                //위치 갱신
                now = next;
            }
```
#### 삭제
```c#
            int ret = _heap[0];

            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex];
            _heap.RemoveAt(lastIndex);
            lastIndex--;

            int now = 0;
            while (true)
            {
                int left = 2 * now + 1;
                int right = 2 * now + 2;

                int next = now;
                //왼쪽 노드와 교체
                if (left <= lastIndex && _heap[next] < _heap[left])
                    next = left;
                //오른쪽 노드와 교체
                if (right <= lastIndex && _heap[next] < _heap[right])
                    next = right;

                if (next == now)
                    break;

                //부모와 자식을 교체
                int tmp = _heap[next];
                _heap[next] = _heap[now];
                _heap[now] = tmp;

            }
            return ret;
```
***
## A*알고리즘
위에서 언급했듯이 BFS의 단점중 하나는 목적지를 옆에 두고도 다른 방향으로 탐색을 진행할 수 있다는 것이다. 이를 해결하기 위해서 탐색하는 기준에 특정한 *판단근거*를 적용해보자
>큐에 넣은 노드들 중에서 판단근거에 가장 적합한 노드먼저 탐색해보자!

### 연산순서
```
    1. 시작노드에 점수를 매기고 우선순위 큐에 넣는다. F=G+H, G는 가중치; H는 휴리스틱함수 일종의 가산점
    1-2. 점수는 목적지와 가까운 정도로 설정한다.
    2. 큐에서 우선순위가 가장 높은 노드를 꺼낸다.
    3. 더 빠른 경로가 존재하면 스킵하며 그렇지 않으면 방문처리를 한다.
    4. 목적지에 도달했다면 탐색을 종료한다.
    5. 현재 노드의 인접 노드에 대한 위치와 점수를 우선순위 큐에 넣는다.
    6. 목적지에 도달하거나 우선순위 큐가 빌때까지 2~5를 반복한다.
```
### 코드 설명
```c#
            int[] deltaY = new int[] { -1, 0, 1, 0 };
            int[] deltaX = new int[] { 0, -1, 0, 1 };
            int[] cost = new int[] { 1, 1, 1, 1 };

            //방문 여부
            bool[,] closed = new bool[_board.Size, _board.Size];
            //가는 길을 한번이라도 발견했는지
            int[,] open = new int[_board.Size, _board.Size];
            for(int iy = 0; iy < _board.Size; iy++)
            {
                for (int ix = 0; ix < _board.Size; ix++)
                {
                    open[iy, ix] = Int32.MaxValue;
                }
            }
            Pos[,] parent = new Pos[_board.Size, _board.Size];

            //우선순위 큐
            PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();
            //시작점부터
            open[PosY, PosX] = Math.Abs(_board.DestY - PosY) + Math.Abs(_board.DestX - PosX);
            pq.Push(new PQNode() { F = Math.Abs(_board.DestY - PosY) + Math.Abs(_board.DestX - PosX), G = 0, Y = PosY, X = PosX });
            parent[PosY, PosX] = new Pos(PosY, PosX);
            while (pq.Count > 0)
            {
                PQNode node = pq.Pop();

                //더 좋은 경로가 존재할 경우 스킵한다.
                if (closed[node.Y, node.X])
                    continue;

                closed[node.Y, node.X] = true;
                //목적지에 도달했으면 종료
                if (node.Y == _board.DestY && node.X == _board.DestX)
                    break;

                for (int i = 0; i < 4; i++)
                {
                    int nextY = node.Y + deltaY[i];
                    int nextX = node.X + deltaX[i];

                    //항상 외벽은 Wall이므로 범위계산은 생략한다.
                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;
                    if (closed[nextY, nextX])
                        continue;

                    //비용계산
                    int g = node.G + cost[i];
                    int h = Math.Abs(_board.DestY - nextY) + Math.Abs(_board.DestX - nextX);
                    //더 빠른 경로가 존재하면 스킵
                    if (open[nextY, nextX] < g + h)
                        continue;

                    open[nextY, nextX] = g + h;
                    pq.Push(new PQNode() { F = g + h, G = g, Y = nextY, X = nextX });
                    parent[nextY, nextX] = new Pos(node.Y, node.X);
                }
            }

            int y = _board.DestY;
            int x = _board.DestX;
            while (parent[y, x].Y != y || parent[y, x].X != x)
            {
                _points.Add(new Pos(y, x));
                Pos pos = parent[y, x];
                y = pos.Y;
                x = pos.X;
            }
            _points.Add(new Pos(y, x));
            _points.Reverse();
```
### 실행화면
![AStar](https://user-images.githubusercontent.com/44914802/125734261-8520a1d5-6dc1-4353-8cc4-a85bc3e2434b.gif)
실제 이동경로는 BFS와 동일하겠지만 내부적으로 탐색횟수를 생각하면 더 적은 움직임을 보인다.