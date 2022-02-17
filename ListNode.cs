﻿namespace ListNodeSimple
{
    /// <summary>
    /// Структура ноды списка
    /// </summary>
    public class ListNode
    {
        public ListNode Previous;
        public ListNode Next;
        public ListNode Random; // произвольный элемент внутри списка
        public string Data;

        public ListNode(string value)
        {
            Data = value;
        }
    }
}
