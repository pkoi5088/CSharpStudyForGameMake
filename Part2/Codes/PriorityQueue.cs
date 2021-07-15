using System;
using System.Collections.Generic;

namespace Exercise
{
    class PriorityQueue<T> where T:IComparable<T>
    {
        List<T> _heap = new List<T>();
        public void Push(T data)
        {
            //맨 끝에 데이터를 삽입한다.
            _heap.Add(data);

            int now = _heap.Count - 1;
            while (now > 0)
            {
                //부모노드의 index
                int next = (now - 1) / 2;
                if (_heap[now].CompareTo(_heap[next]) < 0)
                    break;

                //부모와 자식을 교체
                T tmp = _heap[next];
                _heap[next] = _heap[now];
                _heap[now] = tmp;

                //위치 갱신
                now = next;
            }
        }

        public T Pop()
        {
            T ret = _heap[0];

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
                if (left <= lastIndex && _heap[next].CompareTo(_heap[left]) < 0)
                    next = left;
                //오른쪽 노드와 교체
                if (right <= lastIndex && _heap[next].CompareTo(_heap[right]) < 0)
                    next = right;

                if (next == now)
                    break;

                //부모와 자식을 교체
                T tmp = _heap[next];
                _heap[next] = _heap[now];
                _heap[now] = tmp;

            }
            return ret;
        }

        public int Count { get { return _heap.Count; } }
    }
}
