﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Authorization.Helpers
{
    public static class RandomKey
    {
        public static string Generate(int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            if (string.IsNullOrEmpty(allowedChars))
            {
                throw new ArgumentException("allowedChars may not be empty.");
            }

            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.", byteSize));

            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                var result = new StringBuilder();
                var buf = new byte[128];
                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (var i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i]) continue;
                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }
                return result.ToString();
            }
        }
    }
}
