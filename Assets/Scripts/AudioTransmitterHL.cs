using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class AudioTransmitterHL : NetworkBehaviour {

	private AudioSource aud;
	private int length;

	void Start () {
		aud = GetComponent<AudioSource> ();
	}

	[Command]
	void CmdStopRecording() {
		short[] encoded = EncodeToNSpeex(aud.clip, out length);
		RpcPlayAudio(encoded, length);
	}
		
	[ClientRpc]
	void RpcPlayAudio(short[] encoded, int length) {
		AudioClip a = DecodeFromNSpeex(encoded, length);
		GetComponent<AudioSource> ().clip = a;
		GetComponent<AudioSource> ().Play ();
	}

	byte[] EncodeToNSpeex(AudioClip clip, out int length) {
		float[] samplesFloat = new float[clip.samples * clip.channels];
		short[] samplesShort = new short[clip.samples * clip.channels];
		int sizeOfChunk = 640 * (int) (Mathf.FloorToInt(samplesFloat.Length / 640f) - 1);
		short[] inputChunk = new short[sizeOfChunk];
		byte[] encoded = new byte[sizeOfChunk];
		NSpeex.SpeexEncoder m_wide_enc = new NSpeex.SpeexEncoder(NSpeex.BandMode.Wide);

		clip.GetData(samplesFloat, 0);
		ConvertToShort(samplesFloat, samplesShort);

		for (int i = 0; i < sizeOfChunk; i++) {
			short sample = samplesShort[i];
			inputChunk[i] = sample;
		}

		length = m_wide_enc.Encode(inputChunk, 0, inputChunk.Length, encoded, 0, encoded.Length);

		return encoded;
	}

	AudioClip DecodeFromNSpeex(byte[] encoded, int length) {
		float[] result = new float[encoded.Length];
		short[] decoded = new short[encoded.Length];
		NSpeex.SpeexDecoder m_wide_dec = new NSpeex.SpeexDecoder(NSpeex.BandMode.Wide);
	
		yo = m_wide_dec.Decode(encoded, 0, encoded.Length, decoded, 0, false);

		ConvertToFloat(decoded, result);

		AudioClip a = AudioClip.Create("test", result.Length, 1, 12000, false);
		a.SetData(result, 0);

		return a;
	}

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