using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class DataConverter
{
    public static Dictionary<string, object> ToDictionary(object value)
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        FieldInfo[] fields = value.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);

        for (int i = 0; i < fields.Length; i++)
        {
            var field = fields[i];
            result[field.Name] = field.GetValue(value);
        }

        return result;
    }
}
