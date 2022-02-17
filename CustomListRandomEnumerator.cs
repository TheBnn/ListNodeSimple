using System;
using System.Collections;

namespace ListNodeSimple.Enumerators
{
    public class CustomListRandomEnumerator : IEnumerator
    {
        private ListNode _head;
        private ListNode _current;
        private bool _flag;

        public CustomListRandomEnumerator(ListNode _head)
        {
            _flag = true;
            this._head = _head;
            this._current = null;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public ListNode Current
        {
            get
            {
                if (_flag) throw new InvalidOperationException();
                return _current;
            }
        }

        public bool MoveNext()
        {
            if (_flag)
            {
                _current = _head;
                _flag = false;
                return true;
            }
            if (Current.Next is null) return false;

            _current = _current.Next;
            return true;
        }

        public void Reset()
        {
            _flag = true;
            this._current = _head;
        }
    }
}
