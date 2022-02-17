using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ListNodeSimple
{
    public class ListRandom : IEnumerable, IListSerialize
    {
        private const int minStreamLength = (sizeof(int) * 4) * 2 + sizeof(int);

        private List<ListNode> Store;

        private ListNode _head;
        public ListNode Head
        {
            get
            {
                return _head;
            }
            set
            {
                if (value is null) throw new ArgumentNullException();
                if (Store.Contains(value.Next))
                {
                    int i = Store.IndexOf(value.Next);
                    if (Store[i].Previous == value)
                    {
                        int indx = Store.IndexOf(_head);
                        Store[indx] = value;
                        _head = Store[indx];
                        return;
                    }
                }
                throw new Exception("Incorrect _head");
            }
        }

        private ListNode _tail;
        public ListNode Tail
        {
            get
            {
                return _tail;
            }
            set
            {
                if (value is null) throw new ArgumentNullException();
                if (Store.Contains(value.Previous))
                {
                    int i = Store.IndexOf(value.Previous);
                    if (Store[i].Next == value)
                    {
                        int indx = Store.IndexOf(_tail);
                        Store[indx] = value;
                        _head = Store[indx];
                        return;
                    }
                }
                throw new Exception("Incorrect tail");

            }
        }

        public int Count
        {
            get
            {
                return Store.Count;
            }
            private set
            {

            }
        }
        public ListNode this[int index]
        {
            get
            {
                int c = 0;
                foreach (var e in this)
                {
                    if (c == index) return e;
                    c++;
                }
                throw new Exception("get element exception");
            }
            private set { }

        }

        private Random rnd;

        public ListRandom()
        {
            Init();
        }

        private void Init()
        {
            Store = new List<ListNode>();
            rnd = new Random();

            Store.Add(new ListNode(null));
            Store.Add(new ListNode(null));

            _head = Store[0];
            _tail = Store[1];

            _head.Next = _tail;
            _head.Random = rnd.Next(2) == 0 ? null : _tail;
            _head.Previous = null;

            _tail.Next = null;
            _tail.Random = rnd.Next(2) == 0 ? null : _head;
            _tail.Previous = _head;

        }

        public void InsertElement(int index, string value)
        {
            if (index <= 0 || index >= Store.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            ListNode Prev = null;
            ListNode Next = null;
            ListNode node = new ListNode(value);

            var c = 0;

            foreach (var listNode in this)
            {
                if (index == c + 1)
                {
                    Prev = listNode;
                }

                if (index != c)
                {
                    c++;
                }
                else
                {
                    Next = listNode;
                    break;
                }
            }

            Next.Previous = node;
            Prev.Next = node;
            node.Previous = Prev;
            node.Next = Next;

            int i = rnd.Next(Count - 1) - 1;
            node.Random = (i < 0) ? null : Store[i];

            Store.Add(node);
        }

        public void Serialize(Stream s)
        {
            if (s is null)
            {
                throw new ArgumentNullException("s");
            }

            if (!s.CanWrite)
            {
                throw new NotSupportedException("s is unwritable");
            }

            var binaryWriter = new BinaryWriter(s);

            binaryWriter.Write(Count);

            foreach (var currentNode in Store)
            {
                int prev = (currentNode.Previous is null) ? -1 : Store.IndexOf(currentNode.Previous);
                int next = (currentNode.Next is null) ? -1 : Store.IndexOf(currentNode.Next);
                int random = (currentNode.Random is null) ? -1 : Store.IndexOf(currentNode.Random);
                int datalength = (currentNode.Data is null) ? -1 : currentNode.Data.Length;

                binaryWriter.Write(prev);
                binaryWriter.Write(next);
                binaryWriter.Write(random);
                binaryWriter.Write(datalength * sizeof(char));
                if (currentNode.Data != null) binaryWriter.Write(currentNode.Data);
            }
        }

        public void Deserialize(Stream s)
        {
            if (s is null)
            {
                throw new ArgumentNullException("s");
            }

            if (!s.CanRead)
            {
                throw new NotSupportedException("s is unreadable");
            }

            if (s.Length < minStreamLength)
            {
                throw new Exception("Stream is incorrect");
            }

            var indexes = new List<TempNode>();

            var binaryReader = new BinaryReader(s);

            var count = binaryReader.ReadInt32();

            try
            {
                for (int i = 0; i < count; i++)
                {
                    var prev = binaryReader.ReadInt32();
                    var next = binaryReader.ReadInt32();
                    var random = binaryReader.ReadInt32();
                    var datalength = binaryReader.ReadInt32();
                    var data = String.Empty;

                    if (datalength >= 0) data = binaryReader.ReadString();
                    indexes.Add(new TempNode()
                    {
                        Node = new ListNode(data),
                        Next = next,
                        Previous = prev,
                        Random = random
                    });
                }
            }
            catch
            {
                throw new Exception("Stream is corrupt");
            }

            Init();

            for (int i = 0; i < indexes.Count; i++)
            {
                FillNode(indexes, i);
            }

        }

        private void FillNode(List<TempNode> indexes, int i)
        {
            var tempNode = indexes[i];
            var node = tempNode.Node;

            switch (tempNode.Previous)
            {
                case -1:
                    node.Previous = null;
                    Store[0] = node; _head = Store[0];
                    break;
                default:
                    node.Previous = indexes[tempNode.Previous].Node;
                    break;
            }

            switch (tempNode.Random)
            {
                case -1:
                    node.Random = null;
                    break;
                default:
                    node.Random = indexes[tempNode.Random].Node;
                    break;
            }

            switch (tempNode.Next)
            {
                case -1:
                    node.Next = null;
                    Store[1] = node; _tail = Store[1];
                    break;
                default:
                    node.Next = indexes[tempNode.Next].Node;
                    break;
            }

            if (node.Previous != null && node.Next != null)
            {
                Store.Add(node);
            }
        }

        public List<string> GetElements()
        {
            List<string> o = new List<string>();
            foreach (var e in this)
            {
                o.Add(e.Data);
            }
            return o;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public Enumerators.CustomListRandomEnumerator GetEnumerator()
        {
            return new Enumerators.CustomListRandomEnumerator(_head);
        }
    }
}
