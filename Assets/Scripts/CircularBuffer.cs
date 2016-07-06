using UnityEngine;
using System.Collections;

public class CircularBuffer {
	private float[] buffer;
	private int head;
	private int tail;
	private int bufferSize;
	private int length;

	public CircularBuffer(int bufferSize) {
		buffer = new float[bufferSize];
		this.bufferSize = bufferSize;
		head = this.bufferSize - 1;
	}

	public void Enqueue(float sample) {
		head = NextPosition(head);
		buffer[head] = sample;

		if (IsFull)
			tail = NextPosition(tail);
		else
			length++;
	}

	public float Dequeue() {
		if (IsEmpty)
			throw new UnityException("Queue is empty.");

		float dequeued = buffer[tail];
		tail = NextPosition(tail);
		length--;

		return dequeued;
	}

	private int NextPosition(int pos) {
		return (pos + 1) % bufferSize;
	}

	public float[] GetBuffer {
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
}