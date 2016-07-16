using UnityEngine;
using System.Collections;
using Ionic.Zlib;
using NSpeex;

namespace Voice {
	
	public static class VoiceUtils {

		public static void ConvertToShort (this float[] samplesFloat, short[] samplesShort) {
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

		public static void ConverToFloat (this short[] samplesShort, float[] samplesFloat) {
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

		static NSpeex.SpeexEncoder nspeexEnc = new NSpeex.SpeexEncoder(NSpeex.BandMode.Wide);
		static NSpeex.SpeexDecoder nspeexDec = new NSpeex.SpeexDecoder(NSpeex.BandMode.Wide);

		public static byte[] NSpeexCompress (float[] samplesFloat, out int length) {
			short[] samplesShort = new short[samplesFloat.Length];
			byte[] encoded = new byte[samplesFloat.Length];

			samplesFloat.ConvertToShort(samplesShort);
			length = nspeexEnc.Encode(samplesShort, 0, samplesShort.Length, encoded, 0, encoded.Length);

			return encoded;
		}

		public static float[] NSpeexDecompress (byte[] samplesByte, int dataLength) {
			short[] samplesShort = new short[samplesByte.Length];
			float[] decoded = new float[samplesByte.Length];

			nspeexDec.Decode(samplesByte, 0, dataLength, samplesShort, 0, false);
			samplesShort.ConverToFloat (decoded);

			return decoded;
		}
			
		public static byte[] ZlibCompress (byte[] input, int length) {
			using (var ms = new System.IO.MemoryStream ()) {
				using (var compressor = new Ionic.Zlib.ZlibStream (ms, CompressionMode.Compress, CompressionLevel.BestCompression)) {
					compressor.Write (input, 0, length);
				}

				return ms.ToArray ();
			}
		}

		public static byte[] ZlibDecompress (byte[] input, int length) {
			using (var ms = new System.IO.MemoryStream ()) {
				using (var compressor = new Ionic.Zlib.ZlibStream (ms, CompressionMode.Decompress, CompressionLevel.BestCompression)) {
					compressor.Write (input, 0, length);
				}

				return ms.ToArray ();
			}
		}

		public static float[] Downsample(float[] samplesFloat, out float[] filtered) {
			filtered = new float[samplesFloat.Length / 4];

			for (int i = 3, index = 0; i < samplesFloat.Length; i += 4, index++) {
				filtered[index] = samplesFloat[i];
			}

			return filtered;
		}
	}
}
