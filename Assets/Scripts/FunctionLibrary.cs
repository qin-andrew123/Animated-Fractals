using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary
{
    public delegate Vector3 Function(float u, float v, float t);

    public enum FunctionName { Wave, MultiWave, Ripple , Sphere, Torus };

    public static Function[] functions = { Wave, MultiWave, Ripple , Sphere, Torus }; 
    public static Function GetFunction(FunctionName name)
    {
        return functions[(int)name];
    }

    public static FunctionName GetNextFunctionName(FunctionName name)
    {
        if ((int)name < functions.Length - 1)
        {
            return name + 1;
        }
        return 0;
    }

    public static FunctionName GetRandomFunctionNameOtherThan(FunctionName name)
    {
        var choice = (FunctionName)Random.Range(1, functions.Length);
        return choice == name ? 0 : choice;
    }

    public static Vector3 Morph(float u, float v, float t, Function from, Function to, float progress)
    {
        return Vector3.LerpUnclamped(from(u, v, t), to(u, v, t), SmoothStep(0f, 1f, progress));
    }

    public static Vector3 Wave(float u, float v, float t)
    {
        Vector3 vec;
        vec.x = u;
        vec.y = Sin(PI * (u + v + t));
        vec.z = v;

        return vec;
    }

    public static Vector3 MultiWave(float u, float v, float t)
    {
        Vector3 vec;
        vec.x = u;
        vec.y = Sin(PI * (u + v + t));
        vec.y += 0.5f * Sin(2f * PI * (v + t));
        vec.y += Sin(PI * (u + v + 0.25f * t));
        vec.y *= (1f / 2.5f);
        vec.z = v;
        return vec;
    }

    public static Vector3 Ripple(float u, float v, float t)
    {
        float d = Sqrt(u * u + v * v);
        Vector3 vec;
        vec.x = u;
        vec.y = Sin(PI * (4f * d - t));
        vec.y /= 1f + 10f * d;
        vec.z = v;

        return vec;
    }

    public static Vector3 Sphere(float u, float v, float t)
    {
        float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t));
        float s = r * Cos(0.5f * PI * v);
        Vector3 vec;
        vec.x = s* Sin(PI * u);
        vec.y = r * Sin(PI * 0.5f * v);
        vec.z = s * Cos(PI * u);

        return vec;
    }

    public static Vector3 Torus(float u, float v, float t)
    {
        float r1 = 0.15f + 0.05f * Cos(PI * (8f * u + 4f * v + 2f * t));
        float r2 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t)); ;
        float s = r2 + r1 * Cos(PI * v);
        Vector3 vec;
        vec.x = s * Sin(PI * u);
        vec.y = r1 * Sin(PI * v);
        vec.z = s * Cos(PI * u);

        return vec;
    }
}
