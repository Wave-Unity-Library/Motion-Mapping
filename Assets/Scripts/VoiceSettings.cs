using System.Linq;
using UnityEngine;

namespace Voice {
	
	public static class VoiceSettings {

		[SerializeField]
		static int frequency = 16000;

		[SerializeField]
		static int sampleSize = 640;

		[SerializeField]
		static VoiceCompression compression = VoiceCompression.Zlib;

		public static int Frequency {
			get { return frequency; }
			private set { frequency = value; }
		}

		public static VoiceCompression Compression
		{
			get { return compression; }
			private set { compression = value; }
		}

		public static VoiceCompression Decompression
		{
			get { return compression; }
			private set { compression = value; }
		}

		public static int SampleSize
		{
			get { return sampleSize; }
			private set { sampleSize = value; }
		}
	} 
}
