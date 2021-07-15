using System;
using System.Collections.Generic;
using System.Text;

namespace testcs
{
    class Pos
    {
        public Pos(int y,int x) { Y = y;X = x; }
        public int X;
        public int Y;
    }

    class Player
    {
        public int PosX { get; private set; }
        public int PosY { get; private set; }
        Random _random = new Random();
        Board _board;

        enum Dir
        {
            Up = 0,
            Left = 1,
            Down = 2,
            Right = 3
        }

        int _dir = (int)Dir.Up;
        List<Pos> _points = new List<Pos>();

        public void initialize(int posY, int posX, Board board)
        {
            PosX = posX;
            PosY = posY;
            _board = board;

            int[] frontY = new int[] { -1, 0, 1, 0 };
            int[] frontX = new int[] { 0, -1, 0, 1 };
            int[] rightY = new int[] { 0, -1, 0, 1 };
            int[] rightX = new int[] { 1, 0, -1, 0 };

            _points.Add(new Pos(PosY, PosX));

            //목적지 도착 여부
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
        }

        const int MOVE_TICK = 100;
        int _sumTIck = 0;
        int _lastIndex = 0;
        public void Update(int deltaTick)
        {
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
        }
    }
}
