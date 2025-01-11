using System;
using UnityEngine;

//modified from this page:
//https://coderwall.com/p/wfy-fa/show-a-popup-field-to-serialize-string-or-integer-values-from-a-list-of-choices-in-unity3d

public class WritableOptionsAttribute : PropertyAttribute {
    public delegate string[] GetOptionsList();

    public WritableOptionsAttribute(string[] list) {
        List = list;
    }

    public WritableOptionsAttribute(Type type, string methodName) {
        var method = type.GetMethod(methodName);
        if (method != null)
            List = method.Invoke(null, null) as string[];
        else
            Debug.LogError($"Could not find method {methodName} for {type}.");
    }

    public string[] List { get; private set; }
}