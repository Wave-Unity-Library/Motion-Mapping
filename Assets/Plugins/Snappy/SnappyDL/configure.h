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

#ifndef CONFIGURE_H
#define CONFIGURE_H

// THIS FILE IS (AND SHOULD BE) FORCE-INCLUDE

typedef unsigned __int32 __uint32;
typedef unsigned __int64 __uint64;

#define SNAPPYDL_API __declspec(dllexport)

#define __i386__

#ifdef _M_X64
    #define __x86_64__
#endif

#define HAVE_BUILTIN_CTZ
#include <intrin.h>

static __uint32 __inline __builtin_ctz(__uint32 x)
{
   unsigned long r = 0;
   return _BitScanForward(&r, x) ? r : 32;
}

static __uint32 __inline __builtin_clz(__uint32 x)
{
	unsigned long r = 0;
	return _BitScanReverse(&r, x) ? 31 - r : 32;
}

#ifdef _M_X64

// in 64-bit version this is available
static __uint64 __inline __builtin_ctzll(__uint64 x)
{
	unsigned long r = 0;
	return _BitScanForward64(&r, x) ? r : 64;
}

#else

// but in 32-bit version we need to emulate
static __uint64 __inline __builtin_ctzll(__uint64 x)
{
	if ((__uint32)x == 0)
	{
		return 32 + __builtin_ctz((__uint32)(x >> 32));
	}
	else 
	{
	    return __builtin_ctz((__uint32)x);
	}
}

#endif

#endif