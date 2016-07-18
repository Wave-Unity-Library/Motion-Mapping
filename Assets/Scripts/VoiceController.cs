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

		float[] decoded = VoiceUtils.Decompress (encoded);
	
		percentPlayed = percentPlayed + ((double) decoded.Length / aud.clip.samples);

		GetComponent<AudioSource> ().clip.SetData (decoded, lastPlayed);

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
		lastPlayed += decoded.Length;
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
					byte[] encoded = new byte[filtered.Length * 4];

					encoded = VoiceUtils.Compress (cBuffer.Dequeue ());

					CmdStopRecording (encoded);
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