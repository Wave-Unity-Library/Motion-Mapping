using UnityEngine;
using Ionic.Zlib;
using NSpeex;
using System;
using System.Collections;
using FragLabs.Audio.Codecs;
using FragLabs.Audio.Codecs.Opus;
using SnappyPI;

namespace Voice {
	
	public static class VoiceUtils {

		static int length;

		public static void ConvertToShort(this float[] samplesFloat, short[] samplesShort) {
			if (samplesShort.Length < samplesFloat.Length) {
				throw new System.ArgumentException("in's length: " + samplesFloat.Length + " out's length: " + samplesShort.Length);
			}

			for (int i = 0; i < samplesFloat.Length; i++) {
				float sample = samplesFloat[i];
				sample += 1f; // now it's in the range 0 .. 2
				sample *= 0.5f; // now it's in the range 0 .. 1

				short val = (short) Mathf.FloorToInt (sample * short.MaxValue);

				samplesShort[i] = val;
			}
		}

		public static void ConverToFloat(this short[] samplesShort, float[] samplesFloat) {
			if (samplesFloat.Length < samplesShort.Length) {
				throw new System.ArgumentException("in's length: " + samplesShort.Length + " out's length: " + samplesFloat.Length);
			}

			for (int i = 0; i < samplesShort.Length; i++) {
				float sample = samplesShort[i];
				sample /= (float) short.MaxValue;
				sample *= 2f;
				sample -= 1f;

				samplesFloat[i] = sample;
			}
		}

		public static float[] ConvertToFloat(byte[] samplesByte) {
			float[] samplesFloat = new float[samplesByte.Length / 4];

			Buffer.BlockCopy (samplesByte, 0, samplesFloat, 0, samplesByte.Length);

			return samplesFloat;
		}

		public static byte[] ConvertToByte(float[] samplesFloat) {
			byte[] samplesByte = new byte[samplesFloat.Length * 4];

			Buffer.BlockCopy (samplesFloat, 0, samplesByte, 0, samplesByte.Length);

			return samplesByte;
		}

		static NSpeex.SpeexEncoder nspeexEnc = new NSpeex.SpeexEncoder(NSpeex.BandMode.Narrow);
		static NSpeex.SpeexDecoder nspeexDec = new NSpeex.SpeexDecoder(NSpeex.BandMode.Narrow);

		public static byte[] NSpeexCompress(float[] samplesFloat, out int length) {
			int sizeOfChunk = 320 * (Mathf.FloorToInt (samplesFloat.Length / 320f) - 1);
			short[] samplesShort = new short[samplesFloat.Length];
			short[] samplesShortForNSpeex = new short[sizeOfChunk];
			byte[] encoded = new byte[sizeOfChunk];

			samplesFloat.ConvertToShort(samplesShort);

			for (int i = 0; i < sizeOfChunk; i++) {
				samplesShortForNSpeex [i] = samplesShort [i];
			}

			length = nspeexEnc.Encode(samplesShortForNSpeex, 0, samplesShortForNSpeex.Length, encoded, 0, encoded.Length);

			return encoded;
		}

		public static float[] NSpeexDecompress(byte[] samplesByte, int dataLength) {
			short[] samplesShort = new short[samplesByte.Length];
			float[] decoded = new float[samplesByte.Length];

			nspeexDec.Decode(samplesByte, 0, dataLength, samplesShort, 0, false);
			samplesShort.ConverToFloat (decoded);

			return decoded;
		}
			
		public static byte[] ZlibCompress(byte[] input, int length) {
			using (var ms = new System.IO.MemoryStream ()) {
				using (var compressor = new Ionic.Zlib.ZlibStream (ms, CompressionMode.Compress, CompressionLevel.BestCompression)) {
					compressor.Write (input, 0, length);
				}

				return ms.ToArray ();
			}
		}

		public static byte[] ZlibDecompress(byte[] input, int length) {
			using (var ms = new System.IO.MemoryStream ()) {
				using (var compressor = new Ionic.Zlib.ZlibStream (ms, CompressionMode.Decompress, CompressionLevel.BestCompression)) {
					compressor.Write (input, 0, length);
				}

				return ms.ToArray ();
			}
		}

//		static OpusEncoder opusEnc = OpusEncoder.Create (VoiceSettings.Frequency, 1, Applications.Audio);
//		static OpusDecoder opusDec = OpusDecoder.Create (VoiceSettings.Frequency, 1);

//		public static byte[] OpusCompress(byte[] samplesByte, int length, out int encodedLength) {
//			return opusEnc.Encode (samplesByte, length, out encodedLength);
//		}
//
//		public static byte[] OpusDecompress(byte[] samplesByte, int length, out int decodedLength) {
//			return opusDec.Decode (samplesByte, length, out decodedLength);
//		}

		public static void Downsample(float[] samplesFloat, out float[] filtered) {
			filtered = new float[samplesFloat.Length / 2];
			int length = (2 / samplesFloat.Length) * samplesFloat.Length;

			for (int i = 0, index = 0; i < length; i += 2, index++) {
				float sum = 0;

				for (int j = i; j < i + 2; j++) {
					sum += samplesFloat [j];
				}

				sum /= 2;
				filtered [index] = sum;
			}
		}

		public static VoicePacket Compress(float[] samples) {
			VoicePacket packet = new VoicePacket ();
			packet.compression = VoiceSettings.Compression;

			switch(packet.compression) {
				case VoiceCompression.Zlib: {
					byte[] encoded = ConvertToByte (samples);

					packet.data = ZlibCompress (encoded, encoded.Length);
				}
				break;

//				case VoiceCompression.Opus: {
//					int length;
//					
//					byte[] encoded = ConvertToByte (samples);
//					
//					byte[] encodedWithOpus = OpusCompress (encoded, encoded.Length, out length);
//
//					return encodedWithOpus;
//				}
//				break;

				case VoiceCompression.NSpeex: {
					packet.data = NSpeexCompress (samples, out length);
				}
				break;

				case VoiceCompression.Snappy: {
					byte[] encoded = ConvertToByte (samples);
				
					packet.data = SnappyCodec.Compress (encoded, 0, encoded.Length);
				}
				break;
			}

			return packet;
		}

		public static float[] Decompress(VoicePacket packet) {

			switch(VoiceSettings.Decompression) {
				case VoiceCompression.Zlib: {

					packet.data = ZlibDecompress (packet.data, packet.data.Length);
					
					float[] decoded = ConvertToFloat (packet.data);

					return decoded;
				} 
				break;

//				case VoiceCompression.Opus: {
//
//					int length;
//						
//					encoded = OpusDecompress (encoded, encoded.Length, out length);
//
//					float[] decoded = ConvertToFloat (encoded);
//
//					return decoded;
//				}
//				break;

				case VoiceCompression.NSpeex: {
					float[] decoded = NSpeexDecompress (packet.data, length);

					return decoded;
				}
				break;

				case VoiceCompression.Snappy: {
					byte[] decodedFromSnappy = SnappyCodec.Uncompress (packet.data, 0, packet.data.Length);

					float[] decoded = ConvertToFloat (decodedFromSnappy);

					return decoded;
				}
				break;
			}

			return new float[0];
		}
	}
}