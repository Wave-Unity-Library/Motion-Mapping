using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using FragLabs.Audio.Codecs;
using System;
using FragLabs.Audio.Codecs.Opus;

public class AudioTransmitterHL : NetworkBehaviour {

	private AudioSource aud;
	private int encodedLength;
	private int decodedLength;

	void Start () {
		aud = GetComponent<AudioSource> ();
	}

	[Command]
	void CmdStopRecording() {
		byte[] encoded = EncodeToOpus(aud.clip, aud.clip.samples, out encodedLength);
		RpcPlayAudio(encoded, 8000, aud.clip.channels);
	}
		
	[ClientRpc]
	void RpcPlayAudio(byte[] encoded, int frequency, int channels) {
		byte[] decodedByte = DecodeFromOpus(encoded, frequency, channels);
		float[] decodedFloat = new float[decodedByte.Length / 4];

		Buffer.BlockCopy(decodedByte, 0, decodedFloat, 0, decodedByte.Length);

		AudioClip a = AudioClip.Create("test", decodedFloat.Length, 1, 8000, false);
		a.SetData(decodedFloat, 0);

		GetComponent<AudioSource>().clip = a;
		GetComponent<AudioSource>().Play();
	}

	byte[] EncodeToOpus(AudioClip clip, int samplesLength, out int encodedLength) {
		float[] samplesFloat = new float[clip.samples * clip.channels];
		byte[] samplesByte = new byte[samplesFloat.Length * 4];

		clip.GetData(samplesFloat, 0);

		Buffer.BlockCopy(samplesFloat, 0, samplesByte, 0, samplesByte.Length);

		OpusEncoder encoder = OpusEncoder.Create(8000, aud.clip.channels, Applications.Audio);

		return encoder.Encode(samplesByte, samplesByte.Length, out encodedLength);
	}

	byte[] DecodeFromOpus(byte[] encoded, int frequency, int channels) {
		OpusDecoder decoder = OpusDecoder.Create(frequency, channels);
		return decoder.Decode(encoded, encoded.Length, out decodedLength);
	}

//	short[] EncodeToNSpeex(AudioClip clip, out int length) {
//		float[] samplesFloat = new float[clip.samples * clip.channels];
//		short[] samplesShort = new short[clip.samples * clip.channels];
//		int sizeOfChunk = 640 * (int) (Mathf.FloorToInt(samplesFloat.Length / 640f) - 1);
//		short[] inputChunk = new short[sizeOfChunk];
//		byte[] encoded = new byte[sizeOfChunk];
//		NSpeex.SpeexEncoder m_wide_enc = new NSpeex.SpeexEncoder(NSpeex.BandMode.Wide);
//
//		clip.GetData(samplesFloat, 0);
//		ConvertToShort(samplesFloat, samplesShort);
//
//		for (int i = 0; i < sizeOfChunk; i++) {
//			short sample = samplesShort[i];
//			inputChunk[i] = sample;
//		}
//
//		length = m_wide_enc.Encode(inputChunk, 0, inputChunk.Length, encoded, 0, encoded.Length);
//
//		return inputChunk;
//	}
//
//	AudioClip DecodeFromNSpeex(short[] encoded, int length) {
//		float[] result = new float[encoded.Length];
//		short[] decoded = new short[encoded.Length];
//		NSpeex.SpeexDecoder m_wide_dec = new NSpeex.SpeexDecoder(NSpeex.BandMode.Wide);
//	
//		//yo = m_wide_dec.Decode(encoded, 0, encoded.Length, decoded, 0, false);
//
//		ConvertToFloat(encoded, result);
//
//		AudioClip a = AudioClip.Create("test", result.Length, 1, 12000, false);
//		a.SetData(result, 0);
//
//		return a;
//	}

	void ConvertToShort(float[] samplesFloat, short[] samplesShort) {
		for (int i = 0; i < samplesFloat.Length; i++) {
			float sample = samplesFloat[i];
			sample += 1f; // now it's in the range 0 .. 2
			sample *= 0.5f; // now it's in the range 0 .. 1

			short val = (short) Mathf.FloorToInt(sample * short.MaxValue);

			samplesShort[i] = val;
		}
	}

	void ConvertToFloat(short[] samples, float[] result) {
		for (int i = 0; i < samples.Length; i++) {
			float sample = samples[i];
			sample /= (float) short.MaxValue;
			sample *= 2f;
			sample -= 1f;

			result[i] = sample;
		}
	}

	void StartRecording() {
		aud.clip = Microphone.Start(null, true, 1, 12000);
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			Debug.Log ("Recording started.");
			StartRecording (); 	
		}

		if (Input.GetKeyDown (KeyCode.S)) {
			Microphone.End (null);
			Debug.Log ("Recording ended.");
			CmdStopRecording ();
		}
	}
}