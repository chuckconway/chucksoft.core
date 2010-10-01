using System;

/////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2003-4 CenterSpace Software, LLC                          //
//                                                                         //
// This code is free software under the Artistic license.                  //
//                                                                         //
// CenterSpace Software                                                    //
// 260 SW Madison Avenue                                                   //
// Corvallis, Oregon, 97333                                                //
// USA                                                                     //
// http://www.centerspace.net                                              //
/////////////////////////////////////////////////////////////////////////////


/* 
   A C-program for MT19937, with initialization improved 2002/1/26.
   Coded by Takuji Nishimura and Makoto Matsumoto.

   Before using, initialize the state by using init_genrand(seed)  
   or init_by_array(init_key, key_length).

   Copyright (C) 1997 - 2002, Makoto Matsumoto and Takuji Nishimura,
   All rights reserved.                          

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions
   are met:

     1. Redistributions of source code must retain the above copyright
        notice, this list of conditions and the following disclaimer.

     2. Redistributions in binary form must reproduce the above copyright
        notice, this list of conditions and the following disclaimer in the
        documentation and/or other materials provided with the distribution.

     3. The names of its contributors may not be used to endorse or promote 
        products derived from this software without specific prior written 
        permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.


   Any feedback is very welcome.
   http://www.math.keio.ac.jp/matumoto/emt.html
   email: matumoto@math.keio.ac.jp
*/

namespace Chucksoft.Core.Cryptography
{
    /// <summary>
    /// Class MersenneTwister generates random numbers from a uniform distribution using
    /// the Mersenne Twister algorithm.
    /// </summary>
    /// <remarks>Caution: MT is for MonteCarlo, and is NOT SECURE for CRYPTOGRAPHY 
    /// as it is.</remarks>
    public class MersenneTwister
    {
        #region Constants -------------------------------------------------------

        // Period parameters.
        private const uint _lowerMask = 0x7fffffffU; // least significant r bits
        private const int _m = 397;
        private const uint _matrixA = 0x9908b0dfU; // constant vector a
        private const int _n = 624;
        private const uint _upperMask = 0x80000000U; // most significant w-r bits

        #endregion Constants

        #region Instance Variables ----------------------------------------------

        // _mag01[x] = x * _matrixA  for x=0,1
        private readonly uint[] _mag01 = {0x0U, _matrixA};

        // the array for the state vector
        private readonly uint[] _mt = new uint[_n];

        // _mti==_n+1 means _mt[_n] is not initialized
        private int _mti = _n + 1;

        #endregion Instance Variables

        #region Constructors ----------------------------------------------------

        /// <summary>
        /// Creates a random number generator using the time of day in milliseconds as
        /// the seed.
        /// </summary>
        public MersenneTwister()
        {
            InitGenrand((uint) DateTime.Now.Millisecond);
        }

        /// <summary>
        /// Creates a random number generator initialized with the given seed. 
        /// </summary>
        /// <param name="seed">The seed.</param>
        public MersenneTwister(int seed)
        {
            InitGenrand((uint) seed);
        }

        /// <summary>
        /// Creates a random number generator initialized with the given array.
        /// </summary>
        /// <param name="init">The array for initializing keys.</param>
        public MersenneTwister(int[] init)
        {
            var initArray = new uint[init.Length];
            for (int i = 0; i < init.Length; ++i)
                initArray[i] = (uint) init[i];

            InitByArray(initArray, (uint) initArray.Length);
        }

        #endregion Constructors

        #region Properties ------------------------------------------------------

        /// <summary>
        /// Gets the maximum random integer value. All random integers generated
        /// by instances of this class are less than or equal to this value. This
        /// value is <c>0x7fffffff</c> (<c>2,147,483,647</c>).
        /// </summary>
        public static int MaxRandomInt
        {
            get { return 0x7fffffff; }
        }

        #endregion Properties

        #region Member Functions ------------------------------------------------

        /// <summary>
        /// Returns a random integer greater than or equal to zero and
        /// less than or equal to <c>MaxRandomInt</c>. 
        /// </summary>
        /// <returns>The next random integer.</returns>
        public int Next()
        {
            return GenrandInt31();
        }

        /// <summary>
        /// Returns a positive random integer less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The maximum value. Must be greater than zero.</param>
        /// <returns>A positive random integer less than or equal to <c>maxValue</c>.</returns>
        public int Next(int maxValue)
        {
            return Next(0, maxValue);
        }

        /// <summary>
        /// Returns a random integer within the specified range.
        /// </summary>
        /// <param name="minValue">The lower bound.</param>
        /// <param name="maxValue">The upper bound.</param>
        /// <returns>A random integer greater than or equal to <c>minValue</c>, and less than
        /// or equal to <c>maxValue</c>.</returns>
        public int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                int tmp = maxValue;
                maxValue = minValue;
                minValue = tmp;
            }

            return (int) (Math.Floor((maxValue - minValue + 1)*GenrandReal2() + minValue));
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <returns>A single-precision floating point number greater than or equal to 0.0, 
        /// and less than 1.0.</returns>
        public float NextFloat()
        {
            return (float) GenrandReal2();
        }

        /// <summary>
        /// Returns a random number greater than or equal to zero, and either strictly
        /// less than one, or less than or equal to one, depending on the value of the
        /// given boolean parameter.
        /// </summary>
        /// <param name="includeOne">
        /// If <c>true</c>, the random number returned will be 
        /// less than or equal to one; otherwise, the random number returned will
        /// be strictly less than one.
        /// </param>
        /// <returns>
        /// If <c>includeOne</c> is <c>true</c>, this method returns a
        /// single-precision random number greater than or equal to zero, and less
        /// than or equal to one. If <c>includeOne</c> is <c>false</c>, this method
        /// returns a single-precision random number greater than or equal to zero and
        /// strictly less than one.
        /// </returns>
        public float NextFloat(bool includeOne)
        {
            if (includeOne)
            {
                return (float) GenrandReal1();
            }
            return (float) GenrandReal2();
        }

        /// <summary>
        /// Returns a random number greater than 0.0 and less than 1.0.
        /// </summary>
        /// <returns>A random number greater than 0.0 and less than 1.0.</returns>
        public float NextFloatPositive()
        {
            return (float) GenrandReal3();
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <returns>A double-precision floating point number greater than or equal to 0.0, 
        /// and less than 1.0.</returns>
        public double NextDouble()
        {
            return GenrandReal2();
        }

        /// <summary>
        /// Returns a random number greater than or equal to zero, and either strictly
        /// less than one, or less than or equal to one, depending on the value of the
        /// given boolean parameter.
        /// </summary>
        /// <param name="includeOne">
        /// If <c>true</c>, the random number returned will be 
        /// less than or equal to one; otherwise, the random number returned will
        /// be strictly less than one.
        /// </param>
        /// <returns>
        /// If <c>includeOne</c> is <c>true</c>, this method returns a
        /// single-precision random number greater than or equal to zero, and less
        /// than or equal to one. If <c>includeOne</c> is <c>false</c>, this method
        /// returns a single-precision random number greater than or equal to zero and
        /// strictly less than one.
        /// </returns>
        public double NextDouble(bool includeOne)
        {
            if (includeOne)
            {
                return GenrandReal1();
            }
            return GenrandReal2();
        }

        /// <summary>
        /// Returns a random number greater than 0.0 and less than 1.0.
        /// </summary>
        /// <returns>A random number greater than 0.0 and less than 1.0.</returns>
        public double NextDoublePositive()
        {
            return GenrandReal3();
        }

        /// <summary>
        /// Generates a random number on <c>[0,1)</c> with 53-bit resolution.
        /// </summary>
        /// <returns>A random number on <c>[0,1)</c> with 53-bit resolution</returns>
        public double Next53BitRes()
        {
            return GenrandRes53();
        }

        /// <summary>
        /// Reinitializes the random number generator using the time of day in
        /// milliseconds as the seed.
        /// </summary>
        public void Initialize()
        {
            InitGenrand((uint) DateTime.Now.Millisecond);
        }


        /// <summary>
        /// Reinitializes the random number generator with the given seed.
        /// </summary>
        /// <param name="seed">The seed.</param>
        public void Initialize(int seed)
        {
            InitGenrand((uint) seed);
        }

        /// <summary>
        /// Reinitializes the random number generator with the given array.
        /// </summary>
        /// <param name="init">The array for initializing keys.</param>
        public void Initialize(int[] init)
        {
            var initArray = new uint[init.Length];
            for (int i = 0; i < init.Length; ++i)
                initArray[i] = (uint) init[i];

            InitByArray(initArray, (uint) initArray.Length);
        }

        #region Methods ported from C -------------------------------------------

        // initializes _mt[_n] with a seed
        private void InitGenrand(uint s)
        {
            _mt[0] = s & 0xffffffffU;
            for (_mti = 1; _mti < _n; _mti++)
            {
                _mt[_mti] =
                    (uint) (1812433253U*(_mt[_mti - 1] ^ (_mt[_mti - 1] >> 30)) + _mti);
                // See Knuth TAOCP Vol2. 3rd Ed. P.106 for multiplier. 
                // In the previous versions, MSBs of the seed affect   
                // only MSBs of the array _mt[].                        
                // 2002/01/09 modified by Makoto Matsumoto             
                _mt[_mti] &= 0xffffffffU;
                // for >32 bit machines
            }
        }

        // initialize by an array with array-length
        // init_key is the array for initializing keys 
        // key_length is its length
        private void InitByArray(uint[] initKey, uint keyLength)
        {
            InitGenrand(19650218U);
            int i = 1;
            int j = 0;
            var k = (int) (_n > keyLength ? _n : keyLength);
            for (; k > 0; k--)
            {
                _mt[i] = (uint) ((_mt[i] ^ ((_mt[i - 1] ^ (_mt[i - 1] >> 30))*1664525U)) + initKey[j] + j);
                /* non linear */
                _mt[i] &= 0xffffffffU; // for WORDSIZE > 32 machines
                i++;
                j++;
                if (i >= _n)
                {
                    _mt[0] = _mt[_n - 1];
                    i = 1;
                }
                if (j >= keyLength) j = 0;
            }
            for (k = _n - 1; k > 0; k--)
            {
                _mt[i] = (uint) ((_mt[i] ^ ((_mt[i - 1] ^ (_mt[i - 1] >> 30))*1566083941U)) - i); /* non linear */
                _mt[i] &= 0xffffffffU; // for WORDSIZE > 32 machines
                i++;
                if (i >= _n)
                {
                    _mt[0] = _mt[_n - 1];
                    i = 1;
                }
            }

            _mt[0] = 0x80000000U; // MSB is 1; assuring non-zero initial array
        }

        // generates a random number on [0,0xffffffff]-interval
        private uint GenrandInt32()
        {
            uint y;
            if (_mti >= _n)
            {
                /* generate _n words at one time */
                int kk;

                if (_mti == _n + 1) /* if InitGenrand() has not been called, */
                    InitGenrand(5489U); /* a default initial seed is used */

                for (kk = 0; kk < _n - _m; kk++)
                {
                    y = (_mt[kk] & _upperMask) | (_mt[kk + 1] & _lowerMask);
                    _mt[kk] = _mt[kk + _m] ^ (y >> 1) ^ _mag01[y & 0x1U];
                }
                for (; kk < _n - 1; kk++)
                {
                    y = (_mt[kk] & _upperMask) | (_mt[kk + 1] & _lowerMask);
                    _mt[kk] = _mt[kk + (_m - _n)] ^ (y >> 1) ^ _mag01[y & 0x1U];
                }
                y = (_mt[_n - 1] & _upperMask) | (_mt[0] & _lowerMask);
                _mt[_n - 1] = _mt[_m - 1] ^ (y >> 1) ^ _mag01[y & 0x1U];

                _mti = 0;
            }

            y = _mt[_mti++];

            // Tempering
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680U;
            y ^= (y << 15) & 0xefc60000U;
            y ^= (y >> 18);

            return y;
        }

        // generates a random number on [0,0x7fffffff]-interval
        private int GenrandInt31()
        {
            return (int) (GenrandInt32() >> 1);
        }

        // generates a random number on [0,1]-real-interval
        private double GenrandReal1()
        {
            return GenrandInt32()*(1.0/4294967295.0);
            // divided by 2^32-1
        }

        // generates a random number on [0,1)-real-interval
        private double GenrandReal2()
        {
            return GenrandInt32()*(1.0/4294967296.0);
            // divided by 2^32
        }

        // generates a random number on (0,1)-real-interval
        private double GenrandReal3()
        {
            return (GenrandInt32() + 0.5)*(1.0/4294967296.0);
            // divided by 2^32
        }

        // generates a random number on [0,1) with 53-bit resolution
        private double GenrandRes53()
        {
            uint a = GenrandInt32() >> 5, b = GenrandInt32() >> 6;
            return (a*67108864.0 + b)*(1.0/9007199254740992.0);
        }

        // These real versions are due to Isaku Wada, 2002/01/09 added

        #endregion Methods ported from C

        #endregion Member Functions
    }
}