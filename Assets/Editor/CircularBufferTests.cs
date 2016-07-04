using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

public class CircularBufferTests {
	
	[Test]
	public void EnqueueThreeFloats()
	{
		CircularBuffer queue = new CircularBuffer(5);
		queue.Enqueue(1);
		queue.Enqueue(2);
		queue.Enqueue(3);

		Assert.AreEqual(queue.Length, 3);
	}

	[Test]
	public void EnqueuingWhenFull() {
		CircularBuffer queue = new CircularBuffer(3);
		queue.Enqueue(1);
		queue.Enqueue(2);
		queue.Enqueue(3);
		queue.Enqueue(4);

		Assert.AreEqual(queue.GetTailIndex, 1);
	}

	[Test]
	public void DequeueThreeFloats() {
		CircularBuffer queue = new CircularBuffer(5);
		queue.Enqueue(1);
		queue.Enqueue(2);
		queue.Enqueue(3);

		while (!queue.IsEmpty)
		{
			queue.Dequeue();
		}

		Assert.AreEqual(queue.Length, 0);
	}

	[Test]
	public void DequeueFromEmpty() {
		CircularBuffer queue = new CircularBuffer(5);

		try {
			queue.Dequeue();
		} catch(Exception error) {
			Assert.AreEqual(error.Message, "Queue is empty.");	
		}
	}

	[Test]
	public void OverwriteQueue() {
		CircularBuffer queue = new CircularBuffer(5);
		float[] values = new float[5];
		int i = 0;

		queue.Enqueue (1);
		queue.Enqueue(2);
		queue.Enqueue(3);
		queue.Enqueue(4);
		queue.Enqueue(5);
		queue.Enqueue(6);
		queue.Enqueue(7);

		while (!queue.IsEmpty)
		{
			values[i] = queue.Dequeue();
			i++;
		}

		Assert.AreEqual(values[0], 3f);
		Assert.AreEqual(values[1], 4f);
		Assert.AreEqual(values[2], 5f);
		Assert.AreEqual(values[3], 6f);
		Assert.AreEqual(values[4], 7f);
	}
}
