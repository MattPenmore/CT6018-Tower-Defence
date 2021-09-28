using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridMetrics
{
    //Distance from center to corners
    public const float cornerRadius = 0.1f;

    //Distance from center to edge
    public const float edgeRadius = cornerRadius * 0.866025404f;

    //Poisition of corners relative to centre going clockwise around hexagon starting at the top
    public static Vector3[] corners =
    {
        new Vector3(0f, 0f, cornerRadius),
        new Vector3(edgeRadius, 0f, 0.5f * cornerRadius),
        new Vector3(edgeRadius, 0f, -0.5f * cornerRadius),
        new Vector3(0f, 0f, -cornerRadius),
        new Vector3(-edgeRadius, 0f, -0.5f * cornerRadius),
        new Vector3(-edgeRadius, 0f, 0.5f * cornerRadius),
        //Duplicate first one to prevent out of bounds exception when creating triangles
        new Vector3(0f, 0f, cornerRadius)
    };
}
