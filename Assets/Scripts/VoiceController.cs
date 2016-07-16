﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Ionic.Zlib;
using System;
using Voice;

public class VoiceController : NetworkBehaviour {

	private AudioSource aud;
	private AudioClip clip;
	private CircularBuffer<float[]> cBuffer = new CircularBuffer<float[]>(20);
	private bool isTransmitting = false;
	private const int recordFrequency = 5000;
	private int lastPos = 0;
	private int lastPlayed = 0;
	private float[] sampleBuffer;
	private float playbackDelay = 0;
	private bool isPlaying = false;
	private bool shouldPlay = false;
	private double percentPlayed = 0;
	private float lastTime = 0;

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
		if (lastPlayed >= 5000)
			lastPlayed -= 5000;
		
		encoded = VoiceUtils.ZlibDecompress (encoded, encoded.Length);
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

			if (currentPos < lastPos) {
				diff = recordFrequency - lastPos + currentPos - 1;
			}

			if (diff >= 250) {
				sampleBuffer = new float[diff * aud.clip.channels];
				aud.clip.GetData (sampleBuffer, lastPos);

				cBuffer.Enqueue (sampleBuffer);
			
				lastPos = currentPos;
			}

			if (!cBuffer.IsEmpty) {
				if (playbackDelay >= 0.05f) {
					byte[] sampleBytes = new byte[sampleBuffer.Length * 4];
			
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