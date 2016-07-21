using UnityEngine;
using System.Collections;

namespace Voice {
	
	public enum VoiceCompression : byte {
		Zlib,
		Opus,
		NSpeex,
		Snappy
	} 
}