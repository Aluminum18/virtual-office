using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public static class TextUtils
{
    public static string xorString(string s1, string s2)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < s1.Length; i++)
        {
            sb.Append((char)(s1[i] ^ s2[i % s2.Length]));
        }
        return sb.ToString();
    }
}
