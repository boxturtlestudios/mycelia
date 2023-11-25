using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace BoxTurtleStudios.Utilities
{
    public static class MathB
    {
        //Better modulo
        public static int Mod (int k, int n) {  return ((k %= n) < 0) ? k+n : k;  }
    }

    public class ObjectPrinter
    {
        public static void PrintProperties(object obj)
        {
            Type type = obj.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            foreach (FieldInfo field in fields)
            {
                string fieldName = field.Name;
                object fieldValue = field.GetValue(obj);
                UnityEngine.Debug.Log($"{fieldName}: {fieldValue}");
            }
        }
    }
}

