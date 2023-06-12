using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.UIElements;
using static FunctionLibrary;

public class Graph : MonoBehaviour
{
    public enum TransitionMode { Cycle, Random }
    [SerializeField] Transform pointPrefab;
    [SerializeField, Range(10, 200)] int resolution;
    [SerializeField] FunctionLibrary.FunctionName function;
    [SerializeField, Min(0f)] float functionDuration = 1f;
    [SerializeField, Min(0f)] float transitionDuration = 1f;
    [SerializeField] TransitionMode transitionMode;

    Transform[] points;
    float duration;
    bool transitioning;
    FunctionLibrary.FunctionName transitionFunction;

    void Awake()
    {
        points = new Transform[resolution * resolution];
        
        float step = 2f / resolution;
        var scale = Vector3.one * step;

        for (int i = 0, x = 0, z = 0; i < points.Length; ++i, ++x)
        {
            Transform point = points[i] = Instantiate(pointPrefab);

            point.SetParent(transform, false);
            point.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        duration += Time.deltaTime;
        if (transitioning) 
        {
            if (duration >= transitionDuration)
            {
                duration -= transitionDuration;
                transitioning = false;
            }
        }
        else if (duration >= functionDuration)
        {
            duration -= functionDuration;
            transitioning = true;
            transitionFunction = function;
            PickNextFunction();
        }
        if(transitioning)
        {
            UpdateFunctionTransition();
        }
        else
        {
            UpdateFunction();
        }
    }
    private void PickNextFunction()
    {
        function = transitionMode == TransitionMode.Cycle ?
            FunctionLibrary.GetNextFunctionName(function) :
            FunctionLibrary.GetRandomFunctionNameOtherThan(function);
    }
    private void UpdateFunction()
    {
        FunctionLibrary.Function fxn = FunctionLibrary.GetFunction(function);
        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; ++i, ++x)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            Transform point = points[i];
            Vector3 position = point.localPosition;

            float u = (x + 0.5f) * step - 1f;

            points[i].localPosition = fxn(u, v, time);
        }
    }

    private void UpdateFunctionTransition()
    {
        FunctionLibrary.Function from = FunctionLibrary.GetFunction(transitionFunction);
        FunctionLibrary.Function to = FunctionLibrary.GetFunction(function);
        float progress = duration / transitionDuration;
        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; ++i, ++x)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            Transform point = points[i];
            Vector3 position = point.localPosition;

            float u = (x + 0.5f) * step - 1f;

            points[i].localPosition = FunctionLibrary.Morph(u, v, time, from, to, progress);
        }
    }
}
