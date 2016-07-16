using UnityEngine;
using System.Collections;
using Ionic.Zlib;

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
	}
}
