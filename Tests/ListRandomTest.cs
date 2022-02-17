using ListNodeSimple;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Test
{
    public class ListRandomTest
    {

        [Test]
        public void SerializeDeserialize()
        {
            var testListRandomNodes = GetTestListNode();
            var listRandomNodes = new ListRandom();

            SerializeDeserialize(testListRandomNodes, listRandomNodes);

            Assert.AreEqual(testListRandomNodes.Count, listRandomNodes.Count);

            Assert.AreEqual(listRandomNodes[1], listRandomNodes.Head.Next);
            Assert.AreEqual(null, listRandomNodes.Head.Previous);
            Assert.AreEqual(null, listRandomNodes.Head.Random);
            Assert.AreEqual("1", listRandomNodes.Head.Data);

            Assert.AreEqual(null, listRandomNodes.Tail.Next);
            Assert.AreEqual(listRandomNodes[2], listRandomNodes.Tail.Previous);
            Assert.AreEqual(null, listRandomNodes.Tail.Random);
            Assert.AreEqual("4444", listRandomNodes.Tail.Data);

            listRandomNodes.Head.Data = null;
            listRandomNodes.Tail.Data = null;
            listRandomNodes.Head.Random = listRandomNodes.Tail;
            listRandomNodes.Tail.Random = listRandomNodes.Head;

            SerializeDeserialize(listRandomNodes, testListRandomNodes);

            Assert.AreEqual(testListRandomNodes.Count, listRandomNodes.Count);

            Assert.AreEqual(testListRandomNodes[1], testListRandomNodes.Head.Next);
            Assert.AreEqual(null, testListRandomNodes.Head.Previous);
            Assert.AreEqual(testListRandomNodes[3], testListRandomNodes.Head.Random);
            Assert.AreEqual(null, testListRandomNodes.Head.Data);

            Assert.AreEqual(null, testListRandomNodes.Tail.Next);
            Assert.AreEqual(testListRandomNodes[2], testListRandomNodes.Tail.Previous);
            Assert.AreEqual(testListRandomNodes[0], testListRandomNodes.Tail.Random);
            Assert.AreEqual(null, testListRandomNodes.Tail.Data);
        }

        [Test]
        public void SerializeDeserializeManyElements()
        {
            var testListRandomNodes = new ListRandom();
            var listRandomNodes = new ListRandom();

            testListRandomNodes.Head.Data = "1";
            testListRandomNodes.Tail.Data = "10";
            for (int i = 9; i > 1; i--)
                testListRandomNodes.InsertElement(1, i.ToString());

            SerializeDeserialize(testListRandomNodes, listRandomNodes);

            Assert.AreEqual(testListRandomNodes.Count, listRandomNodes.Count);
            CollectionAssert.AreEqual(
                Enumerable.Range(1, 10).Select(x => x.ToString()).ToList(),
                listRandomNodes.GetElements());
        }

        [Test]
        public void _headExceptions()
        {
            var testListRandomNodes = new ListRandom();

            Assert.Throws<ArgumentNullException>(() => testListRandomNodes.Head = null);
            Assert.Throws<Exception>(() => testListRandomNodes.Head = new ListNode(null));
            Assert.Throws<Exception>(() => testListRandomNodes.Head = new ListNode("")
            {
                Next = new ListNode(""),
                Previous = new ListNode(""),
                Random = new ListNode("")
            });
        }

        [Test]
        public void TailExceptions()
        {
            var testListRandomNodes = new ListRandom();

            Assert.Throws<ArgumentNullException>(() => testListRandomNodes.Tail = null);
            Assert.Throws<Exception>(() => testListRandomNodes.Tail = new ListNode(null));
            Assert.Throws<Exception>(() => testListRandomNodes.Tail = new ListNode("")
            {
                Next = new ListNode(""),
                Previous = new ListNode(""),
                Random = new ListNode("")
            });
        }

        [Test]
        public void InsertElementNormal()
        {
            var testListRandomNodes = new ListRandom();
            var listRandomNodes = new ListRandom();

            testListRandomNodes.InsertElement(1, "data");
            var el = testListRandomNodes[1].Random;
            int i = 0;
            if (el == null) i = 0;
            if (el == testListRandomNodes.Head) i = 1;
            if (el == testListRandomNodes.Tail) i = 2;

            SerializeDeserialize(testListRandomNodes, listRandomNodes);

            Assert.AreEqual("data", listRandomNodes[1].Data);
            Assert.AreEqual(listRandomNodes.Head, listRandomNodes[1].Previous);
            Assert.AreEqual(listRandomNodes.Tail, listRandomNodes[1].Next);
            Assert.AreEqual(
                new ListNode[] { null, listRandomNodes.Head, listRandomNodes.Tail }[i],
                listRandomNodes[1].Random);
        }

        [Test]
        public void InsertElementArgumentOutOfRangeException()
        {
            var testListRandomNodes = new ListRandom();

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                testListRandomNodes.InsertElement(0, "data"));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                testListRandomNodes.InsertElement(2, "data"));
        }

        [Test]
        public void SerializeNormal()
        {
            ListRandom testListRandomNodes = GetTestListNode();

            using (var s = new MemoryStream())
            {
                s.Seek(0, SeekOrigin.Begin);

                testListRandomNodes.Serialize(s);

                s.Seek(0, SeekOrigin.Begin);

                byte[] buffer = new byte[s.Length];
                s.Read(buffer);

                CollectionAssert.AreEqual(GetTestSerializedData(), buffer);
            }
        }

        [Test]
        public void SerializeExceprions()
        {
            var testListRandomNodes = new ListRandom();

            Assert.Throws<ArgumentNullException>(() => testListRandomNodes.Serialize(null));

            using (var s = new MemoryStream())
            {
                s.Close();
                Assert.Throws<NotSupportedException>(() => testListRandomNodes.Serialize(s));
            }
        }

        [Test]
        public void DeserializeNormal()
        {
            using var s = new MemoryStream();
            s.Write(GetTestSerializedData());
            s.Seek(0, SeekOrigin.Begin);
            var testListRandomNodes = new ListRandom();
            testListRandomNodes.Deserialize(s);

            Assert.AreEqual(4, testListRandomNodes.Count);

            Assert.AreEqual(testListRandomNodes[1], testListRandomNodes.Head.Next);
            Assert.AreEqual(null, testListRandomNodes.Head.Previous);
            Assert.AreEqual(null, testListRandomNodes.Head.Random);
            Assert.AreEqual("1", testListRandomNodes.Head.Data);

            Assert.AreEqual(null, testListRandomNodes.Tail.Next);
            Assert.AreEqual(testListRandomNodes[2], testListRandomNodes.Tail.Previous);
            Assert.AreEqual(null, testListRandomNodes.Tail.Random);
            Assert.AreEqual("4444", testListRandomNodes.Tail.Data);

        }

        [Test]
        public void DeserializeExceptions()
        {
            var testListRandomNodes = new ListRandom();
            Assert.Throws<ArgumentNullException>(() => testListRandomNodes.Deserialize(null));

            using (var s = new MemoryStream())
            {
                s.Seek(0, SeekOrigin.Begin);
                s.Write(new byte[3], 0, 3);

                Assert.Throws<Exception>(() => testListRandomNodes.Deserialize(s));
            }


        }

        [Test]
        public void GetElementNormal()
        {
            var testListRandomNodes = new ListRandom();

            Assert.AreEqual(testListRandomNodes.Head, testListRandomNodes[0]);
            Assert.AreEqual(testListRandomNodes.Tail, testListRandomNodes[testListRandomNodes.Count - 1]);
        }

        [Test]
        public void GetElementsNormal()
        {
            var testListRandomNodes = new ListRandom();
            var listRandomNodes = new ListRandom();

            testListRandomNodes.Head.Data = "1";
            testListRandomNodes.Tail.Data = "10";
            for (int i = 9; i > 1; i--)
                testListRandomNodes.InsertElement(1, i.ToString());

            SerializeDeserialize(testListRandomNodes, listRandomNodes);

            Assert.AreEqual(testListRandomNodes.Count, listRandomNodes.Count);
            CollectionAssert.AreEqual(
                Enumerable.Range(1, 10).Select(x => x.ToString()).ToList(),
                listRandomNodes.GetElements());
        }

        private static void SerializeDeserialize(ListRandom from, ListRandom to)
        {
            using (var s = new MemoryStream())
            {
                s.Seek(0, SeekOrigin.Begin);
                from.Serialize(s);

                s.Seek(0, SeekOrigin.Begin);
                to.Deserialize(s);
            }
        }

        private static ListRandom GetTestListNode()
        {
            var testListRandomNodes = new ListRandom();
            testListRandomNodes.Head.Data = "1";
            testListRandomNodes.Tail.Data = "4444";
            testListRandomNodes.InsertElement(1, "22");
            testListRandomNodes.InsertElement(2, "333");

            foreach (var e in testListRandomNodes)
                e.Random = null;

            return testListRandomNodes;
        }

        private static byte[] GetTestSerializedData()
        {
            byte[] buffer;

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(4);

                bw.Write(-1); bw.Write(2);
                bw.Write(-1); bw.Write(1 * 2);
                bw.Write("1");

                bw.Write(3); bw.Write(-1);
                bw.Write(-1); bw.Write(4 * 2);
                bw.Write("4444");

                bw.Write(0); bw.Write(3);
                bw.Write(-1); bw.Write(2 * 2);
                bw.Write("22");

                bw.Write(2); bw.Write(1);
                bw.Write(-1); bw.Write(3 * 2);
                bw.Write("333");

                buffer = new byte[ms.Length];

                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer);
            }

            return buffer;
        }

    }
}