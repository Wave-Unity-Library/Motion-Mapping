#region license

/*
Copyright (c) 2013, Milosz Krajewski
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, 
are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, 
  this list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice, 
  this list of conditions and the following disclaimer in the documentation 
  and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, 
PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF 
THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

#endregion

using System.Runtime.InteropServices;

namespace SnappyPI
{
	/// <summary>P/Invoke wrapper for x86 assembly</summary>
	internal static class Snappy64
	{
		[DllImport("SnappyDL.x64.dll", EntryPoint = "snappy_compress")]
		public static extern unsafe SnappyStatus Compress(
			byte* input,
			long inputLength,
			byte* compressed,
			ref long compressedLength);

		[DllImport("SnappyDL.x64.dll", EntryPoint = "snappy_uncompress")]
		public static extern unsafe SnappyStatus Uncompress(
			byte* compressed,
			long compressedLength,
			byte* uncompressed,
			ref long uncompressedLength);

		[DllImport("SnappyDL.x64.dll", EntryPoint = "snappy_max_compressed_length")]
		public static extern long GetMaximumCompressedLength(
			long inputLength);

		[DllImport("SnappyDL.x64.dll", EntryPoint = "snappy_uncompressed_length")]
		public static extern unsafe SnappyStatus GetUncompressedLength(
			byte* compressed,
			long compressedLength,
			ref long result);

		[DllImport("SnappyDL.x64.dll", EntryPoint = "snappy_validate_compressed_buffer")]
		public static extern unsafe SnappyStatus ValidateCompressedBuffer(
			byte* compressed,
			long compressedLength);
	}
}
