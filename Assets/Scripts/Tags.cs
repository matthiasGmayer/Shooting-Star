using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tags
{
    public readonly static string Hitable = "H";
    public static bool ContainsTag(this GameObject g, string tag)
    {
        return g.tag.Contains(tag);
    }
}
