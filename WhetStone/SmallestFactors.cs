﻿using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Looping;
using WhetStone.SystemExtensions;

namespace NumberStone
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class smallestFactor
    {
        /// <overloads>Get the smallest prime factor of a number.</overloads>
        /// <summary>
        /// Get the smallest prime factor of an <see cref="int"/>.
        /// </summary>
        /// <param name="value">The number to factorize.</param>
        /// <param name="start">The smallest prime to check or <see langword="null"/> to check all of them.</param>
        /// <returns>The smallest prime number after <paramref name="start"/> that divides <paramref name="value"/>, or <paramref name="value"/> if none found.</returns>
        public static int SmallestFactor(this int value, int? start = null)
        {
            value.ThrowIfAbsurd(nameof(value),false,false);
            start.ThrowIfAbsurd(nameof(start));
            if (value%2 == 0)
                return 2;
            // ReSharper disable once RedundantExplicitArraySize
            int[] val = new int[4999]
                #region array
            {
                3, 5, 7, 3, 11, 13, 3, 17, 19, 3, 23, 5, 3, 29, 31, 3, 5, 37, 3, 41, 43, 3, 47, 7, 3, 53, 5, 3, 59, 61, 3, 5, 67, 3, 71, 73, 3, 7, 79,
                3, 83, 5, 3, 89, 7, 3, 5, 97, 3, 101, 103, 3, 107, 109, 3, 113, 5, 3, 7, 11, 3, 5, 127, 3, 131, 7, 3, 137, 139, 3, 11, 5, 3, 149, 151,
                3, 5, 157, 3, 7, 163, 3, 167, 13, 3, 173, 5, 3, 179, 181, 3, 5, 11, 3, 191, 193, 3, 197, 199, 3, 7, 5, 3, 11, 211, 3, 5, 7, 3, 13, 223,
                3, 227, 229, 3, 233, 5, 3, 239, 241, 3, 5, 13, 3, 251, 11, 3, 257, 7, 3, 263, 5, 3, 269, 271, 3, 5, 277, 3, 281, 283, 3, 7, 17, 3, 293,
                5, 3, 13, 7, 3, 5, 307, 3, 311, 313, 3, 317, 11, 3, 17, 5, 3, 7, 331, 3, 5, 337, 3, 11, 7, 3, 347, 349, 3, 353, 5, 3, 359, 19, 3, 5,
                367, 3, 7, 373, 3, 13, 379, 3, 383, 5, 3, 389, 17, 3, 5, 397, 3, 401, 13, 3, 11, 409, 3, 7, 5, 3, 419, 421, 3, 5, 7, 3, 431, 433, 3,
                19, 439, 3, 443, 5, 3, 449, 11, 3, 5, 457, 3, 461, 463, 3, 467, 7, 3, 11, 5, 3, 479, 13, 3, 5, 487, 3, 491, 17, 3, 7, 499, 3, 503, 5,
                3, 509, 7, 3, 5, 11, 3, 521, 523, 3, 17, 23, 3, 13, 5, 3, 7, 541, 3, 5, 547, 3, 19, 7, 3, 557, 13, 3, 563, 5, 3, 569, 571, 3, 5, 577,
                3, 7, 11, 3, 587, 19, 3, 593, 5, 3, 599, 601, 3, 5, 607, 3, 13, 613, 3, 617, 619, 3, 7, 5, 3, 17, 631, 3, 5, 7, 3, 641, 643, 3, 647,
                11, 3, 653, 5, 3, 659, 661, 3, 5, 23, 3, 11, 673, 3, 677, 7, 3, 683, 5, 3, 13, 691, 3, 5, 17, 3, 701, 19, 3, 7, 709, 3, 23, 5, 3, 719,
                7, 3, 5, 727, 3, 17, 733, 3, 11, 739, 3, 743, 5, 3, 7, 751, 3, 5, 757, 3, 761, 7, 3, 13, 769, 3, 773, 5, 3, 19, 11, 3, 5, 787, 3, 7,
                13, 3, 797, 17, 3, 11, 5, 3, 809, 811, 3, 5, 19, 3, 821, 823, 3, 827, 829, 3, 7, 5, 3, 839, 29, 3, 5, 7, 3, 23, 853, 3, 857, 859, 3,
                863, 5, 3, 11, 13, 3, 5, 877, 3, 881, 883, 3, 887, 7, 3, 19, 5, 3, 29, 17, 3, 5, 907, 3, 911, 11, 3, 7, 919, 3, 13, 5, 3, 929, 7, 3, 5,
                937, 3, 941, 23, 3, 947, 13, 3, 953, 5, 3, 7, 31, 3, 5, 967, 3, 971, 7, 3, 977, 11, 3, 983, 5, 3, 23, 991, 3, 5, 997, 3, 7, 17, 3, 19,
                1009, 3, 1013, 5, 3, 1019, 1021, 3, 5, 13, 3, 1031, 1033, 3, 17, 1039, 3, 7, 5, 3, 1049, 1051, 3, 5, 7, 3, 1061, 1063, 3, 11, 1069, 3,
                29, 5, 3, 13, 23, 3, 5, 1087, 3, 1091, 1093, 3, 1097, 7, 3, 1103, 5, 3, 1109, 11, 3, 5, 1117, 3, 19, 1123, 3, 7, 1129, 3, 11, 5, 3, 17,
                7, 3, 5, 31, 3, 1151, 1153, 3, 13, 19, 3, 1163, 5, 3, 7, 1171, 3, 5, 11, 3, 1181, 7, 3, 1187, 29, 3, 1193, 5, 3, 11, 1201, 3, 5, 17, 3,
                7, 1213, 3, 1217, 23, 3, 1223, 5, 3, 1229, 1231, 3, 5, 1237, 3, 17, 11, 3, 29, 1249, 3, 7, 5, 3, 1259, 13, 3, 5, 7, 3, 31, 19, 3, 1277,
                1279, 3, 1283, 5, 3, 1289, 1291, 3, 5, 1297, 3, 1301, 1303, 3, 1307, 7, 3, 13, 5, 3, 1319, 1321, 3, 5, 1327, 3, 11, 31, 3, 7, 13, 3,
                17, 5, 3, 19, 7, 3, 5, 23, 3, 1361, 29, 3, 1367, 37, 3, 1373, 5, 3, 7, 1381, 3, 5, 19, 3, 13, 7, 3, 11, 1399, 3, 23, 5, 3, 1409, 17, 3,
                5, 13, 3, 7, 1423, 3, 1427, 1429, 3, 1433, 5, 3, 1439, 11, 3, 5, 1447, 3, 1451, 1453, 3, 31, 1459, 3, 7, 5, 3, 13, 1471, 3, 5, 7, 3,
                1481, 1483, 3, 1487, 1489, 3, 1493, 5, 3, 1499, 19, 3, 5, 11, 3, 1511, 17, 3, 37, 7, 3, 1523, 5, 3, 11, 1531, 3, 5, 29, 3, 23, 1543, 3,
                7, 1549, 3, 1553, 5, 3, 1559, 7, 3, 5, 1567, 3, 1571, 11, 3, 19, 1579, 3, 1583, 5, 3, 7, 37, 3, 5, 1597, 3, 1601, 7, 3, 1607, 1609, 3,
                1613, 5, 3, 1619, 1621, 3, 5, 1627, 3, 7, 23, 3, 1637, 11, 3, 31, 5, 3, 17, 13, 3, 5, 1657, 3, 11, 1663, 3, 1667, 1669, 3, 7, 5, 3, 23,
                41, 3, 5, 7, 3, 19, 1693, 3, 1697, 1699, 3, 13, 5, 3, 1709, 29, 3, 5, 17, 3, 1721, 1723, 3, 11, 7, 3, 1733, 5, 3, 37, 1741, 3, 5, 1747,
                3, 17, 1753, 3, 7, 1759, 3, 41, 5, 3, 29, 7, 3, 5, 1777, 3, 13, 1783, 3, 1787, 1789, 3, 11, 5, 3, 7, 1801, 3, 5, 13, 3, 1811, 7, 3, 23,
                17, 3, 1823, 5, 3, 31, 1831, 3, 5, 11, 3, 7, 19, 3, 1847, 43, 3, 17, 5, 3, 11, 1861, 3, 5, 1867, 3, 1871, 1873, 3, 1877, 1879, 3, 7, 5,
                3, 1889, 31, 3, 5, 7, 3, 1901, 11, 3, 1907, 23, 3, 1913, 5, 3, 19, 17, 3, 5, 41, 3, 1931, 1933, 3, 13, 7, 3, 29, 5, 3, 1949, 1951, 3,
                5, 19, 3, 37, 13, 3, 7, 11, 3, 1973, 5, 3, 1979, 7, 3, 5, 1987, 3, 11, 1993, 3, 1997, 1999, 3, 2003, 5, 3, 7, 2011, 3, 5, 2017, 3, 43,
                7, 3, 2027, 2029, 3, 19, 5, 3, 2039, 13, 3, 5, 23, 3, 7, 2053, 3, 11, 29, 3, 2063, 5, 3, 2069, 19, 3, 5, 31, 3, 2081, 2083, 3, 2087,
                2089, 3, 7, 5, 3, 2099, 11, 3, 5, 7, 3, 2111, 2113, 3, 29, 13, 3, 11, 5, 3, 2129, 2131, 3, 5, 2137, 3, 2141, 2143, 3, 19, 7, 3, 2153,
                5, 3, 17, 2161, 3, 5, 11, 3, 13, 41, 3, 7, 2179, 3, 37, 5, 3, 11, 7, 3, 5, 13, 3, 31, 2203, 3, 2207, 47, 3, 2213, 5, 3, 7, 2221, 3, 5,
                17, 3, 23, 7, 3, 2237, 2239, 3, 2243, 5, 3, 13, 2251, 3, 5, 37, 3, 7, 31, 3, 2267, 2269, 3, 2273, 5, 3, 43, 2281, 3, 5, 2287, 3, 29,
                2293, 3, 2297, 11, 3, 7, 5, 3, 2309, 2311, 3, 5, 7, 3, 11, 23, 3, 13, 17, 3, 2333, 5, 3, 2339, 2341, 3, 5, 2347, 3, 2351, 13, 3, 2357,
                7, 3, 17, 5, 3, 23, 2371, 3, 5, 2377, 3, 2381, 2383, 3, 7, 2389, 3, 2393, 5, 3, 2399, 7, 3, 5, 29, 3, 2411, 19, 3, 2417, 41, 3, 2423,
                5, 3, 7, 11, 3, 5, 2437, 3, 2441, 7, 3, 2447, 31, 3, 11, 5, 3, 2459, 23, 3, 5, 2467, 3, 7, 2473, 3, 2477, 37, 3, 13, 5, 3, 19, 47, 3,
                5, 11, 3, 41, 2503, 3, 23, 13, 3, 7, 5, 3, 11, 2521, 3, 5, 7, 3, 2531, 17, 3, 43, 2539, 3, 2543, 5, 3, 2549, 2551, 3, 5, 2557, 3, 13,
                11, 3, 17, 7, 3, 31, 5, 3, 2579, 29, 3, 5, 13, 3, 2591, 2593, 3, 7, 23, 3, 19, 5, 3, 2609, 7, 3, 5, 2617, 3, 2621, 43, 3, 37, 11, 3,
                2633, 5, 3, 7, 19, 3, 5, 2647, 3, 11, 7, 3, 2657, 2659, 3, 2663, 5, 3, 17, 2671, 3, 5, 2677, 3, 7, 2683, 3, 2687, 2689, 3, 2693, 5, 3,
                2699, 37, 3, 5, 2707, 3, 2711, 2713, 3, 11, 2719, 3, 7, 5, 3, 2729, 2731, 3, 5, 7, 3, 2741, 13, 3, 41, 2749, 3, 2753, 5, 3, 31, 11, 3,
                5, 2767, 3, 17, 47, 3, 2777, 7, 3, 11, 5, 3, 2789, 2791, 3, 5, 2797, 3, 2801, 2803, 3, 7, 53, 3, 29, 5, 3, 2819, 7, 3, 5, 11, 3, 19,
                2833, 3, 2837, 17, 3, 2843, 5, 3, 7, 2851, 3, 5, 2857, 3, 2861, 7, 3, 47, 19, 3, 13, 5, 3, 2879, 43, 3, 5, 2887, 3, 7, 11, 3, 2897, 13,
                3, 2903, 5, 3, 2909, 41, 3, 5, 2917, 3, 23, 37, 3, 2927, 29, 3, 7, 5, 3, 2939, 17, 3, 5, 7, 3, 13, 2953, 3, 2957, 11, 3, 2963, 5, 3,
                2969, 2971, 3, 5, 13, 3, 11, 19, 3, 29, 7, 3, 41, 5, 3, 2999, 3001, 3, 5, 31, 3, 3011, 23, 3, 7, 3019, 3, 3023, 5, 3, 13, 7, 3, 5,
                3037, 3, 3041, 17, 3, 11, 3049, 3, 43, 5, 3, 7, 3061, 3, 5, 3067, 3, 37, 7, 3, 17, 3079, 3, 3083, 5, 3, 3089, 11, 3, 5, 19, 3, 7, 29,
                3, 13, 3109, 3, 11, 5, 3, 3119, 3121, 3, 5, 53, 3, 31, 13, 3, 3137, 43, 3, 7, 5, 3, 47, 23, 3, 5, 7, 3, 29, 3163, 3, 3167, 3169, 3, 19,
                5, 3, 11, 3181, 3, 5, 3187, 3, 3191, 31, 3, 23, 7, 3, 3203, 5, 3, 3209, 13, 3, 5, 3217, 3, 3221, 11, 3, 7, 3229, 3, 53, 5, 3, 41, 7, 3,
                5, 17, 3, 3251, 3253, 3, 3257, 3259, 3, 13, 5, 3, 7, 3271, 3, 5, 29, 3, 17, 7, 3, 19, 11, 3, 37, 5, 3, 3299, 3301, 3, 5, 3307, 3, 7,
                3313, 3, 31, 3319, 3, 3323, 5, 3, 3329, 3331, 3, 5, 47, 3, 13, 3343, 3, 3347, 17, 3, 7, 5, 3, 3359, 3361, 3, 5, 7, 3, 3371, 3373, 3,
                11, 31, 3, 17, 5, 3, 3389, 3391, 3, 5, 43, 3, 19, 41, 3, 3407, 7, 3, 3413, 5, 3, 13, 11, 3, 5, 23, 3, 47, 3433, 3, 7, 19, 3, 11, 5, 3,
                3449, 7, 3, 5, 3457, 3, 3461, 3463, 3, 3467, 3469, 3, 23, 5, 3, 7, 59, 3, 5, 11, 3, 3491, 7, 3, 13, 3499, 3, 31, 5, 3, 11, 3511, 3, 5,
                3517, 3, 7, 13, 3, 3527, 3529, 3, 3533, 5, 3, 3539, 3541, 3, 5, 3547, 3, 53, 11, 3, 3557, 3559, 3, 7, 5, 3, 43, 3571, 3, 5, 7, 3, 3581,
                3583, 3, 17, 37, 3, 3593, 5, 3, 59, 13, 3, 5, 3607, 3, 23, 3613, 3, 3617, 7, 3, 3623, 5, 3, 19, 3631, 3, 5, 3637, 3, 11, 3643, 3, 7,
                41, 3, 13, 5, 3, 3659, 7, 3, 5, 19, 3, 3671, 3673, 3, 3677, 13, 3, 29, 5, 3, 7, 3691, 3, 5, 3697, 3, 3701, 7, 3, 11, 3709, 3, 47, 5, 3,
                3719, 61, 3, 5, 3727, 3, 7, 3733, 3, 37, 3739, 3, 19, 5, 3, 23, 11, 3, 5, 13, 3, 3761, 53, 3, 3767, 3769, 3, 7, 5, 3, 3779, 19, 3, 5,
                7, 3, 17, 3793, 3, 3797, 29, 3, 3803, 5, 3, 13, 37, 3, 5, 11, 3, 3821, 3823, 3, 43, 7, 3, 3833, 5, 3, 11, 23, 3, 5, 3847, 3, 3851,
                3853, 3, 7, 17, 3, 3863, 5, 3, 53, 7, 3, 5, 3877, 3, 3881, 11, 3, 13, 3889, 3, 17, 5, 3, 7, 47, 3, 5, 3907, 3, 3911, 7, 3, 3917, 3919,
                3, 3923, 5, 3, 3929, 3931, 3, 5, 31, 3, 7, 3943, 3, 3947, 11, 3, 59, 5, 3, 37, 17, 3, 5, 3967, 3, 11, 29, 3, 41, 23, 3, 7, 5, 3, 3989,
                13, 3, 5, 7, 3, 4001, 4003, 3, 4007, 19, 3, 4013, 5, 3, 4019, 4021, 3, 5, 4027, 3, 29, 37, 3, 11, 7, 3, 13, 5, 3, 4049, 4051, 3, 5,
                4057, 3, 31, 17, 3, 7, 13, 3, 4073, 5, 3, 4079, 7, 3, 5, 61, 3, 4091, 4093, 3, 17, 4099, 3, 11, 5, 3, 7, 4111, 3, 5, 23, 3, 13, 7, 3,
                4127, 4129, 3, 4133, 5, 3, 4139, 41, 3, 5, 11, 3, 7, 4153, 3, 4157, 4159, 3, 23, 5, 3, 11, 43, 3, 5, 4177, 3, 37, 47, 3, 53, 59, 3, 7,
                5, 3, 13, 4201, 3, 5, 7, 3, 4211, 11, 3, 4217, 4219, 3, 41, 5, 3, 4229, 4231, 3, 5, 19, 3, 4241, 4243, 3, 31, 7, 3, 4253, 5, 3, 4259,
                4261, 3, 5, 17, 3, 4271, 4273, 3, 7, 11, 3, 4283, 5, 3, 4289, 7, 3, 5, 4297, 3, 11, 13, 3, 59, 31, 3, 19, 5, 3, 7, 29, 3, 5, 4327, 3,
                61, 7, 3, 4337, 4339, 3, 43, 5, 3, 4349, 19, 3, 5, 4357, 3, 7, 4363, 3, 11, 17, 3, 4373, 5, 3, 29, 13, 3, 5, 41, 3, 4391, 23, 3, 4397,
                53, 3, 7, 5, 3, 4409, 11, 3, 5, 7, 3, 4421, 4423, 3, 19, 43, 3, 11, 5, 3, 23, 4441, 3, 5, 4447, 3, 4451, 61, 3, 4457, 7, 3, 4463, 5, 3,
                41, 17, 3, 5, 11, 3, 4481, 4483, 3, 7, 67, 3, 4493, 5, 3, 11, 7, 3, 5, 4507, 3, 13, 4513, 3, 4517, 4519, 3, 4523, 5, 3, 7, 23, 3, 5,
                13, 3, 19, 7, 3, 4547, 4549, 3, 29, 5, 3, 47, 4561, 3, 5, 4567, 3, 7, 17, 3, 23, 19, 3, 4583, 5, 3, 13, 4591, 3, 5, 4597, 3, 43, 4603,
                3, 17, 11, 3, 7, 5, 3, 31, 4621, 3, 5, 7, 3, 11, 41, 3, 4637, 4639, 3, 4643, 5, 3, 4649, 4651, 3, 5, 4657, 3, 59, 4663, 3, 13, 7, 3,
                4673, 5, 3, 4679, 31, 3, 5, 43, 3, 4691, 13, 3, 7, 37, 3, 4703, 5, 3, 17, 7, 3, 5, 53, 3, 4721, 4723, 3, 29, 4729, 3, 4733, 5, 3, 7,
                11, 3, 5, 47, 3, 4751, 7, 3, 67, 4759, 3, 11, 5, 3, 19, 13, 3, 5, 17, 3, 7, 4783, 3, 4787, 4789, 3, 4793, 5, 3, 4799, 4801, 3, 5, 11,
                3, 17, 4813, 3, 4817, 61, 3, 7, 5, 3, 11, 4831, 3, 5, 7, 3, 47, 29, 3, 37, 13, 3, 23, 5, 3, 43, 4861, 3, 5, 31, 3, 4871, 11, 3, 4877,
                7, 3, 19, 5, 3, 4889, 67, 3, 5, 59, 3, 13, 4903, 3, 7, 4909, 3, 17, 5, 3, 4919, 7, 3, 5, 13, 3, 4931, 4933, 3, 4937, 11, 3, 4943, 5, 3,
                7, 4951, 3, 5, 4957, 3, 11, 7, 3, 4967, 4969, 3, 4973, 5, 3, 13, 17, 3, 5, 4987, 3, 7, 4993, 3, 19, 4999, 3, 5003, 5, 3, 5009, 5011, 3,
                5, 29, 3, 5021, 5023, 3, 11, 47, 3, 7, 5, 3, 5039, 71, 3, 5, 7, 3, 5051, 31, 3, 13, 5059, 3, 61, 5, 3, 37, 11, 3, 5, 5077, 3, 5081, 13,
                3, 5087, 7, 3, 11, 5, 3, 5099, 5101, 3, 5, 5107, 3, 19, 5113, 3, 7, 5119, 3, 47, 5, 3, 23, 7, 3, 5, 11, 3, 53, 37, 3, 5147, 19, 3,
                5153, 5, 3, 7, 13, 3, 5, 5167, 3, 5171, 7, 3, 31, 5179, 3, 71, 5, 3, 5189, 29, 3, 5, 5197, 3, 7, 11, 3, 41, 5209, 3, 13, 5, 3, 17, 23,
                3, 5, 5227, 3, 5231, 5233, 3, 5237, 13, 3, 7, 5, 3, 29, 59, 3, 5, 7, 3, 5261, 19, 3, 23, 11, 3, 5273, 5, 3, 5279, 5281, 3, 5, 17, 3,
                11, 67, 3, 5297, 7, 3, 5303, 5, 3, 5309, 47, 3, 5, 13, 3, 17, 5323, 3, 7, 73, 3, 5333, 5, 3, 19, 7, 3, 5, 5347, 3, 5351, 53, 3, 11, 23,
                3, 31, 5, 3, 7, 41, 3, 5, 19, 3, 5381, 7, 3, 5387, 17, 3, 5393, 5, 3, 5399, 11, 3, 5, 5407, 3, 7, 5413, 3, 5417, 5419, 3, 11, 5, 3, 61,
                5431, 3, 5, 5437, 3, 5441, 5443, 3, 13, 5449, 3, 7, 5, 3, 53, 43, 3, 5, 7, 3, 5471, 13, 3, 5477, 5479, 3, 5483, 5, 3, 11, 17, 3, 5, 23,
                3, 5501, 5503, 3, 5507, 7, 3, 37, 5, 3, 5519, 5521, 3, 5, 5527, 3, 5531, 11, 3, 7, 29, 3, 23, 5, 3, 31, 7, 3, 5, 5557, 3, 67, 5563, 3,
                19, 5569, 3, 5573, 5, 3, 7, 5581, 3, 5, 37, 3, 5591, 7, 3, 29, 11, 3, 13, 5, 3, 71, 31, 3, 5, 41, 3, 7, 5623, 3, 17, 13, 3, 43, 5, 3,
                5639, 5641, 3, 5, 5647, 3, 5651, 5653, 3, 5657, 5659, 3, 7, 5, 3, 5669, 53, 3, 5, 7, 3, 13, 5683, 3, 11, 5689, 3, 5693, 5, 3, 41, 5701,
                3, 5, 13, 3, 5711, 29, 3, 5717, 7, 3, 59, 5, 3, 17, 11, 3, 5, 5737, 3, 5741, 5743, 3, 7, 5749, 3, 11, 5, 3, 13, 7, 3, 5, 73, 3, 29, 23,
                3, 53, 5779, 3, 5783, 5, 3, 7, 5791, 3, 5, 11, 3, 5801, 7, 3, 5807, 37, 3, 5813, 5, 3, 11, 5821, 3, 5, 5827, 3, 7, 19, 3, 13, 5839, 3,
                5843, 5, 3, 5849, 5851, 3, 5, 5857, 3, 5861, 11, 3, 5867, 5869, 3, 7, 5, 3, 5879, 5881, 3, 5, 7, 3, 43, 71, 3, 5897, 17, 3, 5903, 5, 3,
                19, 23, 3, 5, 61, 3, 31, 5923, 3, 5927, 7, 3, 17, 5, 3, 5939, 13, 3, 5, 19, 3, 11, 5953, 3, 7, 59, 3, 67, 5, 3, 47, 7, 3, 5, 43, 3,
                5981, 31, 3, 5987, 53, 3, 13, 5, 3, 7, 17, 3, 5, 6007, 3, 6011, 7, 3, 11, 13, 3, 19, 5, 3, 6029, 37, 3, 5, 6037, 3, 7, 6043, 3, 6047,
                23, 3, 6053, 5, 3, 73, 11, 3, 5, 6067, 3, 13, 6073, 3, 59, 6079, 3, 7, 5, 3, 6089, 6091, 3, 5, 7, 3, 6101, 17, 3, 31, 41, 3, 6113, 5,
                3, 29, 6121, 3, 5, 11, 3, 6131, 6133, 3, 17, 7, 3, 6143, 5, 3, 11, 6151, 3, 5, 47, 3, 61, 6163, 3, 7, 31, 3, 6173, 5, 3, 37, 7, 3, 5,
                23, 3, 41, 11, 3, 6197, 6199, 3, 6203, 5, 3, 7, 6211, 3, 5, 6217, 3, 6221, 7, 3, 13, 6229, 3, 23, 5, 3, 17, 79, 3, 5, 6247, 3, 7, 13,
                3, 6257, 11, 3, 6263, 5, 3, 6269, 6271, 3, 5, 6277, 3, 11, 61, 3, 6287, 19, 3, 7, 5, 3, 6299, 6301, 3, 5, 7, 3, 6311, 59, 3, 6317, 71,
                3, 6323, 5, 3, 6329, 13, 3, 5, 6337, 3, 17, 6343, 3, 11, 7, 3, 6353, 5, 3, 6359, 6361, 3, 5, 6367, 3, 23, 6373, 3, 7, 6379, 3, 13, 5,
                3, 6389, 7, 3, 5, 6397, 3, 37, 19, 3, 43, 13, 3, 11, 5, 3, 7, 6421, 3, 5, 6427, 3, 59, 7, 3, 41, 47, 3, 17, 5, 3, 6449, 6451, 3, 5, 11,
                3, 7, 23, 3, 29, 6469, 3, 6473, 5, 3, 11, 6481, 3, 5, 13, 3, 6491, 43, 3, 73, 67, 3, 7, 5, 3, 23, 17, 3, 5, 7, 3, 6521, 11, 3, 61,
                6529, 3, 47, 5, 3, 13, 31, 3, 5, 6547, 3, 6551, 6553, 3, 79, 7, 3, 6563, 5, 3, 6569, 6571, 3, 5, 6577, 3, 6581, 29, 3, 7, 11, 3, 19, 5,
                3, 6599, 7, 3, 5, 6607, 3, 11, 17, 3, 13, 6619, 3, 37, 5, 3, 7, 19, 3, 5, 6637, 3, 29, 7, 3, 17, 61, 3, 6653, 5, 3, 6659, 6661, 3, 5,
                59, 3, 7, 6673, 3, 11, 6679, 3, 41, 5, 3, 6689, 6691, 3, 5, 37, 3, 6701, 6703, 3, 19, 6709, 3, 7, 5, 3, 6719, 11, 3, 5, 7, 3, 53, 6733,
                3, 6737, 23, 3, 11, 5, 3, 17, 43, 3, 5, 29, 3, 6761, 6763, 3, 67, 7, 3, 13, 5, 3, 6779, 6781, 3, 5, 11, 3, 6791, 6793, 3, 7, 13, 3,
                6803, 5, 3, 11, 7, 3, 5, 17, 3, 19, 6823, 3, 6827, 6829, 3, 6833, 5, 3, 7, 6841, 3, 5, 41, 3, 13, 7, 3, 6857, 19, 3, 6863, 5, 3, 6869,
                6871, 3, 5, 13, 3, 7, 6883, 3, 71, 83, 3, 61, 5, 3, 6899, 67, 3, 5, 6907, 3, 6911, 31, 3, 6917, 11, 3, 7, 5, 3, 13, 29, 3, 5, 7, 3, 11,
                53, 3, 6947, 6949, 3, 17, 5, 3, 6959, 6961, 3, 5, 6967, 3, 6971, 19, 3, 6977, 7, 3, 6983, 5, 3, 29, 6991, 3, 5, 6997, 3, 7001, 47, 3,
                7, 43, 3, 7013, 5, 3, 7019, 7, 3, 5, 7027, 3, 79, 13, 3, 31, 7039, 3, 7043, 5, 3, 7, 11, 3, 5, 7057, 3, 23, 7, 3, 37, 7069, 3, 11, 5,
                3, 7079, 73, 3, 5, 19, 3, 7, 41, 3, 47, 31, 3, 7103, 5, 3, 7109, 13, 3, 5, 11, 3, 7121, 17, 3, 7127, 7129, 3, 7, 5, 3, 11, 37, 3, 5, 7,
                3, 7151, 23, 3, 17, 7159, 3, 13, 5, 3, 67, 71, 3, 5, 7177, 3, 43, 11, 3, 7187, 7, 3, 7193, 5, 3, 23, 19, 3, 5, 7207, 3, 7211, 7213, 3,
                7, 7219, 3, 31, 5, 3, 7229, 7, 3, 5, 7237, 3, 13, 7243, 3, 7247, 11, 3, 7253, 5, 3, 7, 53, 3, 5, 13, 3, 11, 7, 3, 19, 29, 3, 7283, 5,
                3, 37, 23, 3, 5, 7297, 3, 7, 67, 3, 7307, 7309, 3, 71, 5, 3, 13, 7321, 3, 5, 17, 3, 7331, 7333, 3, 11, 41, 3, 7, 5, 3, 7349, 7351, 3,
                5, 7, 3, 17, 37, 3, 53, 7369, 3, 73, 5, 3, 47, 11, 3, 5, 83, 3, 19, 7393, 3, 13, 7, 3, 11, 5, 3, 31, 7411, 3, 5, 7417, 3, 41, 13, 3, 7,
                17, 3, 7433, 5, 3, 43, 7, 3, 5, 11, 3, 7451, 29, 3, 7457, 7459, 3, 17, 5, 3, 7, 31, 3, 5, 7477, 3, 7481, 7, 3, 7487, 7489, 3, 59, 5, 3,
                7499, 13, 3, 5, 7507, 3, 7, 11, 3, 7517, 73, 3, 7523, 5, 3, 7529, 17, 3, 5, 7537, 3, 7541, 19, 3, 7547, 7549, 3, 7, 5, 3, 7559, 7561,
                3, 5, 7, 3, 67, 7573, 3, 7577, 11, 3, 7583, 5, 3, 7589, 7591, 3, 5, 71, 3, 11, 7603, 3, 7607, 7, 3, 23, 5, 3, 19, 7621, 3, 5, 29, 3,
                13, 17, 3, 7, 7639, 3, 7643, 5, 3, 7649, 7, 3, 5, 13, 3, 47, 79, 3, 11, 7669, 3, 7673, 5, 3, 7, 7681, 3, 5, 7687, 3, 7691, 7, 3, 43,
                7699, 3, 7703, 5, 3, 13, 11, 3, 5, 7717, 3, 7, 7723, 3, 7727, 59, 3, 11, 5, 3, 71, 7741, 3, 5, 61, 3, 23, 7753, 3, 7757, 7759, 3, 7, 5,
                3, 17, 19, 3, 5, 7, 3, 31, 43, 3, 13, 7789, 3, 7793, 5, 3, 11, 29, 3, 5, 37, 3, 73, 13, 3, 7817, 7, 3, 7823, 5, 3, 7829, 41, 3, 5, 17,
                3, 7841, 11, 3, 7, 47, 3, 7853, 5, 3, 29, 7, 3, 5, 7867, 3, 17, 7873, 3, 7877, 7879, 3, 7883, 5, 3, 7, 13, 3, 5, 53, 3, 7901, 7, 3,
                7907, 11, 3, 41, 5, 3, 7919, 89, 3, 5, 7927, 3, 7, 7933, 3, 7937, 17, 3, 13, 5, 3, 7949, 7951, 3, 5, 73, 3, 19, 7963, 3, 31, 13, 3, 7,
                5, 3, 79, 23, 3, 5, 7, 3, 61, 7993, 3, 11, 19, 3, 53, 5, 3, 8009, 8011, 3, 5, 8017, 3, 13, 71, 3, 23, 7, 3, 29, 5, 3, 8039, 11, 3, 5,
                13, 3, 83, 8053, 3, 7, 8059, 3, 11, 5, 3, 8069, 7, 3, 5, 41, 3, 8081, 59, 3, 8087, 8089, 3, 8093, 5, 3, 7, 8101, 3, 5, 11, 3, 8111, 7,
                3, 8117, 23, 3, 8123, 5, 3, 11, 47, 3, 5, 79, 3, 7, 17, 3, 8147, 29, 3, 31, 5, 3, 41, 8161, 3, 5, 8167, 3, 8171, 11, 3, 13, 8179, 3, 7,
                5, 3, 19, 8191, 3, 5, 7, 3, 59, 13, 3, 29, 8209, 3, 43, 5, 3, 8219, 8221, 3, 5, 19, 3, 8231, 8233, 3, 8237, 7, 3, 8243, 5, 3, 73, 37,
                3, 5, 23, 3, 11, 8263, 3, 7, 8269, 3, 8273, 5, 3, 17, 7, 3, 5, 8287, 3, 8291, 8293, 3, 8297, 43, 3, 19, 5, 3, 7, 8311, 3, 5, 8317, 3,
                53, 7, 3, 11, 8329, 3, 13, 5, 3, 31, 19, 3, 5, 17, 3, 7, 8353, 3, 61, 13, 3, 8363, 5, 3, 8369, 11, 3, 5, 8377, 3, 17, 83, 3, 8387,
                8389, 3, 7, 5, 3, 37, 31, 3, 5, 7, 3, 13, 47, 3, 19, 8419, 3, 8423, 5, 3, 8429, 8431, 3, 5, 11, 3, 23, 8443, 3, 8447, 7, 3, 79, 5, 3,
                11, 8461, 3, 5, 8467, 3, 43, 37, 3, 7, 61, 3, 17, 5, 3, 13, 7, 3, 5, 29, 3, 8501, 11, 3, 47, 67, 3, 8513, 5, 3, 7, 8521, 3, 5, 8527, 3,
                19, 7, 3, 8537, 8539, 3, 8543, 5, 3, 83, 17, 3, 5, 43, 3, 7, 8563, 3, 13, 11, 3, 8573, 5, 3, 23, 8581, 3, 5, 31, 3, 11, 13, 3, 8597,
                8599, 3, 7, 5, 3, 8609, 79, 3, 5, 7, 3, 37, 8623, 3, 8627, 8629, 3, 89, 5, 3, 53, 8641, 3, 5, 8647, 3, 41, 17, 3, 11, 7, 3, 8663, 5, 3,
                8669, 13, 3, 5, 8677, 3, 8681, 19, 3, 7, 8689, 3, 8693, 5, 3, 8699, 7, 3, 5, 8707, 3, 31, 8713, 3, 23, 8719, 3, 11, 5, 3, 7, 8731, 3,
                5, 8737, 3, 8741, 7, 3, 8747, 13, 3, 8753, 5, 3, 19, 8761, 3, 5, 11, 3, 7, 31, 3, 67, 8779, 3, 8783, 5, 3, 11, 59, 3, 5, 19, 3, 13,
                8803, 3, 8807, 23, 3, 7, 5, 3, 8819, 8821, 3, 5, 7, 3, 8831, 11, 3, 8837, 8839, 3, 37, 5, 3, 8849, 53, 3, 5, 17, 3, 8861, 8863, 3,
                8867, 7, 3, 19, 5, 3, 13, 83, 3, 5, 8887, 3, 17, 8893, 3, 7, 11, 3, 29, 5, 3, 59, 7, 3, 5, 37, 3, 11, 8923, 3, 79, 8929, 3, 8933, 5, 3,
                7, 8941, 3, 5, 23, 3, 8951, 7, 3, 13, 17, 3, 8963, 5, 3, 8969, 8971, 3, 5, 47, 3, 7, 13, 3, 11, 89, 3, 17, 5, 3, 8999, 9001, 3, 5,
                9007, 3, 9011, 9013, 3, 71, 29, 3, 7, 5, 3, 9029, 11, 3, 5, 7, 3, 9041, 9043, 3, 83, 9049, 3, 11, 5, 3, 9059, 13, 3, 5, 9067, 3, 47,
                43, 3, 29, 7, 3, 31, 5, 3, 61, 9091, 3, 5, 11, 3, 19, 9103, 3, 7, 9109, 3, 13, 5, 3, 11, 7, 3, 5, 9127, 3, 23, 9133, 3, 9137, 13, 3,
                41, 5, 3, 7, 9151, 3, 5, 9157, 3, 9161, 7, 3, 89, 53, 3, 9173, 5, 3, 67, 9181, 3, 5, 9187, 3, 7, 29, 3, 17, 9199, 3, 9203, 5, 3, 9209,
                61, 3, 5, 13, 3, 9221, 23, 3, 9227, 11, 3, 7, 5, 3, 9239, 9241, 3, 5, 7, 3, 11, 19, 3, 9257, 47, 3, 59, 5, 3, 13, 73, 3, 5, 9277, 3,
                9281, 9283, 3, 37, 7, 3, 9293, 5, 3, 17, 71, 3, 5, 41, 3, 9311, 67, 3, 7, 9319, 3, 9323, 5, 3, 19, 7, 3, 5, 9337, 3, 9341, 9343, 3, 13,
                9349, 3, 47, 5, 3, 7, 11, 3, 5, 17, 3, 9371, 7, 3, 9377, 83, 3, 11, 5, 3, 41, 9391, 3, 5, 9397, 3, 7, 9403, 3, 23, 97, 3, 9413, 5, 3,
                9419, 9421, 3, 5, 11, 3, 9431, 9433, 3, 9437, 9439, 3, 7, 5, 3, 11, 13, 3, 5, 7, 3, 9461, 9463, 3, 9467, 17, 3, 9473, 5, 3, 9479, 19,
                3, 5, 53, 3, 9491, 11, 3, 9497, 7, 3, 13, 5, 3, 37, 9511, 3, 5, 31, 3, 9521, 89, 3, 7, 13, 3, 9533, 5, 3, 9539, 7, 3, 5, 9547, 3, 9551,
                41, 3, 19, 11, 3, 73, 5, 3, 7, 17, 3, 5, 61, 3, 11, 7, 3, 9587, 43, 3, 53, 5, 3, 29, 9601, 3, 5, 13, 3, 7, 9613, 3, 59, 9619, 3, 9623,
                5, 3, 9629, 9631, 3, 5, 23, 3, 31, 9643, 3, 11, 9649, 3, 7, 5, 3, 13, 9661, 3, 5, 7, 3, 19, 17, 3, 9677, 9679, 3, 23, 5, 3, 9689, 11,
                3, 5, 9697, 3, 89, 31, 3, 17, 7, 3, 11, 5, 3, 9719, 9721, 3, 5, 71, 3, 37, 9733, 3, 7, 9739, 3, 9743, 5, 3, 9749, 7, 3, 5, 11, 3, 43,
                13, 3, 9767, 9769, 3, 29, 5, 3, 7, 9781, 3, 5, 9787, 3, 9791, 7, 3, 97, 41, 3, 9803, 5, 3, 17, 9811, 3, 5, 9817, 3, 7, 11, 3, 31, 9829,
                3, 9833, 5, 3, 9839, 13, 3, 5, 43, 3, 9851, 59, 3, 9857, 9859, 3, 7, 5, 3, 71, 9871, 3, 5, 7, 3, 41, 9883, 3, 9887, 11, 3, 13, 5, 3,
                19, 9901, 3, 5, 9907, 3, 11, 23, 3, 47, 7, 3, 9923, 5, 3, 9929, 9931, 3, 5, 19, 3, 9941, 61, 3, 7, 9949, 3, 37, 5, 3, 23, 7, 3, 5,
                9967, 3, 13, 9973, 3, 11, 17, 3, 67, 5, 3, 7, 97, 3, 5, 13, 3
            };
            #endregion
            if (value < val.Length*2 + 2)
                return val[(value-3)/2];
            bool? pbl = value.IsPrimeByList();
            if (pbl ?? false)
            {
                return value;
            }
            IEnumerable<int> primesToCheck = primes.Primes(Math.Sqrt(value).Floor() + 1).Skip(1);
            if (start.HasValue)
            {
                var aslist = primesToCheck.AsList();
                primesToCheck = aslist.Skip(aslist.BinarySearchStartBias(a=>a>=start, binarySearch.BooleanBinSearchStyle.GetFirstTrue));
            }
            foreach (int prime in primesToCheck)
            {
                while (value % prime == 0)
                {
                    return prime;
                }
            }
            return value;
        }
        /// <summary>
        /// Get the smallest prime factor of an <see cref="long"/>.
        /// </summary>
        /// <param name="value">The number to factorize.</param>
        /// <param name="start">The smallest prime to check or <see langword="null"/> to check all of them.</param>
        /// <returns>The smallest prime number after <paramref name="start"/> that divides <paramref name="value"/>, or <paramref name="value"/> if none found.</returns>
        public static long SmallestFactor(this long value, long? start = null)
        {
            value.ThrowIfAbsurd(nameof(value), false, false);
            start.ThrowIfAbsurd(nameof(start));
            if (value % 2 == 0)
                return 2;
            if (value < int.MaxValue)
                return SmallestFactor((int)value, (int?)start);
            IEnumerable<int> primesToCheck = primes.Primes(Math.Sqrt(value).Floor() + 1).Skip(1);
            if (start.HasValue)
            {
                var aslist = primesToCheck.AsList();
                primesToCheck = aslist.Skip(aslist.BinarySearch(a => a >= start, binarySearch.BooleanBinSearchStyle.GetFirstTrue));
            }
            foreach (int prime in primesToCheck)
            {
                while (value % prime == 0)
                {
                    return prime;
                }
            }
            return value;
        }
    }
}
