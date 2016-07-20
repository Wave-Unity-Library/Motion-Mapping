using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CircularBuffer<T> {
	T[] buffer;
	int head;
	int tail;
	int bufferSize;
	int length;

	public CircularBuffer(int bufferSize) {
		buffer = new T[bufferSize];
		this.bufferSize = bufferSize;
		head = this.bufferSize - 1;
	}

	public void Enqueue(T sample) {
		head = NextPosition(head);
		buffer[head] = sample;

		if (IsFull)
			tail = NextPosition(tail);
		else
			length++;
	}

	public T Dequeue() {
		if (IsEmpty)
			throw new UnityException("Queue is empty.");

		T dequeued = buffer[tail];
		tail = NextPosition(tail);
		length--;

		return dequeued;
	}

	int NextPosition(int pos) {
		return (pos + 1) % bufferSize;
	}

	public T[] GetBuffer {
		get { return buffer; }
	}

	public int GetHeadIndex {
		get { return head; }
	}

	public int GetTailIndex {
		get { return tail; }
	}

	public bool IsEmpty {
		get { return length == 0; }
	}

	public bool IsFull {
		get { return length == bufferSize; }
	}

	public int Length {
		get { return length; }
	}

	public int BufferLength {
		get { return bufferSize; }
	}
}