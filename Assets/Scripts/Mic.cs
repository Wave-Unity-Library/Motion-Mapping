//using UnityEngine;
//using System.Collections;
//
//public class Mic : MonoBehaviour {
//	private AudioSource aud;
//	int length;
//
//	void Start () {
//		aud = GetComponent<AudioSource>();
//	}
//	
//	void Update () {
////		if (Input.GetKeyDown(KeyCode.A)) StartRecording();
////		if (Input.GetKeyDown(KeyCode.S)) StopRecording();
//	}
//
//	void StartRecording() {
//		aud.clip = Microphone.Start(null, true, 1, 8000);
//		Debug.Log("Recording started.");
//	}
//
//	void StopRecording() {
//		Microphone.End(null);
//		Debug.Log("Recording ended.");
//		//float[] samples = new float[aud.clip.samples * aud.clip.channels];
//
//		//CircularBuffer buffer = new CircularBuffer(buffer length from network);
//		//TODO on receiving audio samples add to circular buffer aka samples float
//
//		//byte[] encoded = EncodeToNSpeex(aud.clip, out length);
//		//AudioClip a = DecodeFromNSpeex(encoded, length);
//
//		//GetComponent<AudioSource>().clip = a;
//		GetComponent<AudioSource>().Play();
//	}
//
//	byte[] EncodeToNSpeex(AudioClip clip, out int length) {
//		float[] samplesFloat = new float[clip.samples * clip.channels];
//		short[] samplesShort = new short[clip.samples * clip.channels];
//		int sizeOfChunk = 640 * (Mathf.FloorToInt(samplesFloat.Length / 640f) - 1);
//		short[] inputChunk = new short[sizeOfChunk];
//		byte[] encoded = new byte[sizeOfChunk];
//		NSpeex.SpeexEncoder m_wide_enc = new NSpeex.SpeexEncoder(NSpeex.BandMode.Wide);
//
//		clip.GetData(samplesFloat, 0);
//		ConvertToShort(samplesFloat, samplesShort);
//
//		for (int i = 0; i < samplesShort.Length; i++) {
//			short sample = samplesShort[i];
//			inputChunk[i] = sample;
//		}
//
//		length = 5;//m_wide_enc.Encode(inputPartChunk, 0, inputPartChunk.Length, encoded, 0, encoded.Length);
//        
//		return encoded;
//	}
//
//	AudioClip DecodeFromNSpeex(byte[] encoded, int length) {
//		short[] decoded = new short[encoded.Length];
//		float[] result = new float[encoded.Length];
//		//private NSpeex.SpeexDecoder m_wide_dec = new NSpeex.SpeexDecoder( NSpeex.BandMode.Wide );
//
////		short[] decoded = new short[ 640 ];
////		m_wide_dec.Decode( inputBytes, 0, dataLength, decoded, 0, false );
//
//		ConvertToFloat(decoded, result);
//
//		AudioClip a = AudioClip.Create(null, 20, 1, 8000, false);
//		a.SetData(result, 0);
//
//		return a;
//	}
//
//	void ConvertToShort(float[] samplesFloat, short[] samplesShort) {
//		for (int i = 0; i < samplesFloat.Length; i++) {
//			float sample = samplesFloat[i];
//			sample += 1f; // now it's in the range 0 .. 2
//			sample *= 0.5f; // now it's in the range 0 .. 1
// 
//			short val = (short) Mathf.FloorToInt(sample * short.MaxValue);
//
//			samplesShort[i] = val;
//		}
//	}
//
//	void ConvertToFloat(short[] samples, float[] result) {
//		for (int i = 0; i < samples.Length; i++) {
//			float sample = samples[i];
//			sample /= (float) short.MaxValue;
//			sample *= 2f;
//			sample -= 1f;
//
//			result[i] = sample;
//		}
//	}
//}
