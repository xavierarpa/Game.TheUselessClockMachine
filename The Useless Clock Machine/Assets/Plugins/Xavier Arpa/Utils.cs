using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static float Range(this Vector2 v2) => Random.Range(v2.x, v2.y);
}
