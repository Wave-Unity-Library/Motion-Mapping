using UnityEngine;
using System.Collections;

namespace Voice {
	public struct VoicePacket {
		public byte[] data;
		public int length;
		public VoiceCompression compression;
	}
}
