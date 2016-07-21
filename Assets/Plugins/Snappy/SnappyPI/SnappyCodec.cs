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

using System;

namespace SnappyPI
{
	/// <summary>Snappy block interface.</summary>
	public static class SnappyCodec
	{
		#region private implementation

		/// <summary>Ensures the operation succeeded. Throws exception otherwise.</summary>
		/// <param name="status">The status.</param>
		private static void Ensure(SnappyStatus status)
		{
			if (status != SnappyStatus.OK)
				throw new InvalidOperationException(string.Format("Snappy error code: {0}", status));
		}

		#endregion

		#region public interface

		/// <summary>Gets the maximum compressed length for given input length.</summary>
		/// <param name="inputLength">The input length.</param>
		/// <returns>Maximum compressed length.</returns>
		public static int GetMaximumCompressedLength(int inputLength)
		{
			if (IntPtr.Size == 4)
			{
				return Snappy32.GetMaximumCompressedLength(inputLength);
			}
			else
			{
				// checked - exception here is better than just random errors later
				return checked((int)Snappy64.GetMaximumCompressedLength(inputLength));
			}
		}

		/// <summary>Gets the uncompressed length.</summary>
		/// <param name="compressed">The compressed buffer.</param>
		/// <param name="compressedLength">Length of the compressed buffer.</param>
		/// <param name="uncompressedLength">Returns uncompressed length.</param>
		/// <returns>Status.</returns>
		public static unsafe SnappyStatus GetUncompressedLength(
			byte* compressed, int compressedLength, ref int uncompressedLength)
		{
			if (IntPtr.Size == 4)
			{
				return Snappy32.GetUncompressedLength(compressed, compressedLength, ref uncompressedLength);
			}
			else
			{
				long l = uncompressedLength;
				var result = Snappy64.GetUncompressedLength(compressed, compressedLength, ref l);
				// checked - exception here is better than just random errors later
				uncompressedLength = checked((int)l);
				return result;
			}
		}

		/// <summary>Compresses the specified input.</summary>
		/// <param name="input">The input.</param>
		/// <param name="inputLength">Length of the input.</param>
		/// <param name="output">The output.</param>
		/// <param name="outputLength">Length of the output.</param>
		/// <returns>Status.</returns>
		public static unsafe SnappyStatus Compress(
			byte* input, int inputLength, byte* output, ref int outputLength)
		{
			if (IntPtr.Size == 4)
			{
				return Snappy32.Compress(input, inputLength, output, ref outputLength);
			}
			else
			{
				long l = outputLength;
				var result = Snappy64.Compress(input, inputLength, output, ref l);
				// checked - exception here is better than just random errors later
				outputLength = checked((int)l);
				return result;
			}
		}

		/// <summary>Uncompresses the specified input.</summary>
		/// <param name="input">The input.</param>
		/// <param name="inputLength">Length of the input.</param>
		/// <param name="output">The output.</param>
		/// <param name="outputLength">Length of the output.</param>
		/// <returns></returns>
		public static unsafe SnappyStatus Uncompress(
			byte* input, int inputLength, byte* output, ref int outputLength)
		{
			if (IntPtr.Size == 4)
			{
				return Snappy32.Uncompress(input, inputLength, output, ref outputLength);
			}
			else
			{
				long l = outputLength;
				var result = Snappy64.Uncompress(input, inputLength, output, ref l);
				// checked - exception here is better than just random errors later
				outputLength = checked((int)l);
				return result;
			}
		}

		/// <summary>Compresses the specified input.</summary>
		/// <param name="input">The input.</param>
		/// <param name="inputOffset">The input offset.</param>
		/// <param name="inputLength">Length of the input.</param>
		/// <returns>Compressed block of bytes.</returns>
		public static unsafe byte[] Compress(byte[] input, int inputOffset, int inputLength)
		{
			var output_length = GetMaximumCompressedLength(inputLength);
			var output = new byte[output_length];

			fixed (byte* i = &input[inputOffset])
			fixed (byte* o = &output[0])
			{
				Ensure(Compress(i, inputLength, o, ref output_length));
				if (output_length < output.Length)
				{
					var temp = new byte[output_length];
					Buffer.BlockCopy(output, 0, temp, 0, output_length);
					return temp;
				}
				else
				{
					return output;
				}
			}
		}

		/// <summary>Uncompresses the specified compressed block of bytes.</summary>
		/// <param name="compressed">The compressed block of bytes.</param>
		/// <param name="compressedOffset">The offset.</param>
		/// <param name="compressedLength">Length of input.</param>
		/// <returns></returns>
		public static unsafe byte[] Uncompress(byte[] compressed, int compressedOffset, int compressedLength)
		{
			fixed (byte* i = &compressed[compressedOffset])
			{
				int uncompressedLength = 0;
				Ensure(GetUncompressedLength(i, compressedLength, ref uncompressedLength));
				var output = new byte[uncompressedLength];
				fixed (byte* o = &output[0])
				{
					Ensure(Uncompress(i, compressedLength, o, ref uncompressedLength));
				}
				return output;
			}
		}

		#endregion
	}
}