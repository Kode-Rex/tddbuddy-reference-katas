using System.Collections.Generic;

namespace LinkedListKata;

public class LinkedList<T>
{
    private sealed class Node
    {
        public T Value;
        public Node? Next;

        public Node(T value)
        {
            Value = value;
        }
    }

    private Node? _head;
    private int _count;

    public int Size() => _count;

    public void Append(T value)
    {
        var node = new Node(value);
        if (_head is null)
        {
            _head = node;
        }
        else
        {
            var current = _head;
            while (current.Next is not null) current = current.Next;
            current.Next = node;
        }
        _count++;
    }

    public void Prepend(T value)
    {
        _head = new Node(value) { Next = _head };
        _count++;
    }

    public T Get(int index)
    {
        return NodeAt(index).Value;
    }

    public T Remove(int index)
    {
        if (index < 0 || index >= _count) throw OutOfRange(index);
        Node removed;
        if (index == 0)
        {
            removed = _head!;
            _head = removed.Next;
        }
        else
        {
            var previous = NodeAt(index - 1);
            removed = previous.Next!;
            previous.Next = removed.Next;
        }
        _count--;
        return removed.Value;
    }

    public void InsertAt(int index, T value)
    {
        if (index < 0 || index > _count) throw OutOfRange(index);
        if (index == 0)
        {
            Prepend(value);
            return;
        }
        if (index == _count)
        {
            Append(value);
            return;
        }
        var previous = NodeAt(index - 1);
        previous.Next = new Node(value) { Next = previous.Next };
        _count++;
    }

    public bool Contains(T value) => IndexOf(value) >= 0;

    public int IndexOf(T value)
    {
        var comparer = EqualityComparer<T>.Default;
        var i = 0;
        for (var current = _head; current is not null; current = current.Next, i++)
        {
            if (comparer.Equals(current.Value, value)) return i;
        }
        return -1;
    }

    public T[] ToArray()
    {
        var result = new T[_count];
        var i = 0;
        for (var current = _head; current is not null; current = current.Next, i++)
        {
            result[i] = current.Value;
        }
        return result;
    }

    private Node NodeAt(int index)
    {
        if (index < 0 || index >= _count) throw OutOfRange(index);
        var current = _head!;
        for (var i = 0; i < index; i++) current = current.Next!;
        return current;
    }

    private static ArgumentOutOfRangeException OutOfRange(int index)
        => new(nameof(index), $"index out of range: {index}");
}
