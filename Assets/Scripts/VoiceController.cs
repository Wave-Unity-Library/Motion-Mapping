using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Ionic.Zlib;
using System;
using Voice;

public class VoiceController : NetworkBehaviour {

	AudioSource aud;
	AudioClip clip;
	CircularBuffer<float[]> cBuffer = new CircularBuffer<float[]>(5);
	bool isTransmitting = false;
	int recordFrequency;
	int lastPos = 0;
	int lastPlayed = 0;
	float[] sampleBuffer;
	float[] filtered;
	float playbackDelay = 0;
	bool isPlaying = false;
	double percentPlayed = 0;

	void Start () {
		int minFreq;
		int maxFreq;

		Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);

		recordFrequency = minFreq == 0 && maxFreq == 0 ? 44100 : 16000;
		Debug.Log (recordFrequency);

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
		
		encoded = VoiceUtils.ZlibDecompress (encoded, encoded.Length);
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
//			GetComponent<AudioSource> ().Pause ();
			isPlaying = false;
			Debug.Log ("not caught up");
		}

		if (percentPlayed >= 1) {
			percentPlayed = percentPlayed - 1;
		}
			
		Debug.Log ("Recording sent.");
		lastPlayed += samplesFloat.Length;
	}

	void StartRecording () {
		aud.clip = Microphone.Start (null, true, 1, recordFrequency);
		while (Microphone.GetPosition (null) < 0) {}
		isTransmitting = true;
	}

	void Update () {
		if (!isLocalPlayer)
			return;

		playbackDelay += Time.deltaTime;

		if (isTransmitting) {			
			int currentPos = Microphone.GetPosition (null);
			int diff = currentPos - lastPos;
			int partitionSize = recordFrequency / cBuffer.BufferLength;

			if (currentPos < lastPos) {
				diff = recordFrequency - lastPos + currentPos - 1;
			}

			if (diff >= partitionSize) {
				sampleBuffer = new float[diff * aud.clip.channels];

				aud.clip.GetData (sampleBuffer, lastPos);

				VoiceUtils.Downsample (sampleBuffer, out filtered);

				cBuffer.Enqueue (filtered);
			
				lastPos = currentPos;
			}

			if (!cBuffer.IsEmpty) {
				if (playbackDelay >= 0.05f) {
					byte[] sampleBytes = new byte[filtered.Length * 4];
			
					Buffer.BlockCopy (cBuffer.Dequeue (), 0, sampleBytes, 0, sampleBytes.Length);
					byte[] encodedWithZLib = VoiceUtils.ZlibCompress (sampleBytes, sampleBytes.Length);
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
}