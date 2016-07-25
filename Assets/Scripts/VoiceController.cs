using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Ionic.Zlib;
using System;

public class VoiceController : NetworkBehaviour {

	AudioSource aud;
	AudioClip clip;
	CircularBuffer<float[]> cBuffer = new CircularBuffer<float[]>(20);
	bool isTransmitting = false;
	const int recordFrequency = 4000;
	int lastPos = 0;
	int lastPlayed = 0;
	float[] sampleBuffer;
	float playbackDelay = 0;
	bool isPlaying = false;
	double percentPlayed = 0;

	void Start () {
		aud = GetComponent<AudioSource> ();
		clip = AudioClip.Create ("test", recordFrequency, 1, recordFrequency, false);
		aud.clip = clip;
	}

	[Command]
	void CmdStopRecording (byte[] encoded) {
		RpcPlayAudio(encoded);
	}

	[ClientRpc]
	void RpcPlayAudio (byte[] encoded) {
		if (lastPlayed >= recordFrequency)
			lastPlayed -= recordFrequency;

		encoded = ZlibDecompress (encoded, encoded.Length);
		short[] samplesShort = new short[encoded.Length];
		float[] samplesFloat = new float[encoded.Length / 4];

		Buffer.BlockCopy (encoded, 0, samplesFloat, 0, encoded.Length);	

		percentPlayed = percentPlayed + ((double) samplesFloat.Length / aud.clip.samples);

		GetComponent<AudioSource> ().clip.SetData (samplesFloat, lastPlayed);

		if (!isPlaying) {
			GetComponent<AudioSource> ().Play ();
			aud.loop = true;
			isPlaying = true;
		}

		if (GetComponent<AudioSource> ().time >= percentPlayed) {
			//GetComponent<AudioSource> ().Pause ();
			isPlaying = false;
			Debug.Log ("not caught up");
		}

		if (percentPlayed >= 1) {
			percentPlayed = percentPlayed - 1;
		}

		Debug.Log ("Recording sent.");
		lastPlayed += samplesFloat.Length;
	}

	void ConvertToShort (float[] samplesFloat, short[] samplesShort) {
		for (int i = 0; i < samplesFloat.Length; i++) {
			float sample = samplesFloat[i];
			sample += 1f; // now it's in the range 0 .. 2
			sample *= 0.5f; // now it's in the range 0 .. 1

			short val = (short) Mathf.FloorToInt (sample * short.MaxValue);

			samplesShort[i] = val;
		}
	}

	void ConvertToFloat (short[] samples, float[] result) {
		for (int i = 0; i < samples.Length; i++) {
			float sample = samples[i];
			sample /= (float) short.MaxValue;
			sample *= 2f;
			sample -= 1f;

			result[i] = sample;
		}
	}

	void StartRecording () {
		aud.clip = Microphone.Start (null, true, 1, recordFrequency);
		while (Microphone.GetPosition (null) < 0) {}
		isTransmitting = true;
	}

	void OnGUI() {
		if (GUI.Button (new Rect (570, 10, 150, 100), "Click"))
			StartRecording ();
	}

	void Update () {
		if (!isLocalPlayer)
			return;

		playbackDelay += Time.deltaTime;

		if (isTransmitting) {			
			int currentPos = Microphone.GetPosition (null);
			int diff = currentPos - lastPos;

			if (currentPos < lastPos) {
				diff = recordFrequency - lastPos + currentPos - 1;
			}

			if (diff >= 200) {
				sampleBuffer = new float[diff * aud.clip.channels];
				aud.clip.GetData (sampleBuffer, lastPos);

				cBuffer.Enqueue (sampleBuffer);

				lastPos = currentPos;
			}

			if (!cBuffer.IsEmpty) {
				if (playbackDelay >= 0.05f) {
					byte[] sampleBytes = new byte[sampleBuffer.Length * 4];
					Buffer.BlockCopy (cBuffer.Dequeue(), 0, sampleBytes, 0, sampleBytes.Length);
					byte[] encodedWithZLib = ZlibCompress (sampleBytes, sampleBytes.Length);
					CmdStopRecording (encodedWithZLib);
					playbackDelay = 0;
				}
			}

		}

		if (Input.GetKeyDown (KeyCode.O)) {
			Debug.Log ("Recording started.");
			StartRecording (); 	
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			Microphone.End (null);
			Debug.Log ("Recording ended.");
			isTransmitting = false;
		}
	}

	byte[] ZlibCompress (byte[] input, int length) {
		using (var ms = new System.IO.MemoryStream ()) {
			using (var compressor = new Ionic.Zlib.ZlibStream (ms, CompressionMode.Compress, CompressionLevel.BestCompression)) {
				compressor.Write (input, 0, length);
			}

			return ms.ToArray ();
		}
	}

	byte[] ZlibDecompress (byte[] input, int length) {
		using (var ms = new System.IO.MemoryStream ()) {
			using (var compressor = new Ionic.Zlib.ZlibStream (ms, CompressionMode.Decompress, CompressionLevel.BestCompression)) {
				compressor.Write (input, 0, length);
			}

			return ms.ToArray ();
		}
	}
}