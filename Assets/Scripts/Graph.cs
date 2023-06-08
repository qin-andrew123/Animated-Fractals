using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.UIElements;

public class Graph : MonoBehaviour
{
    [SerializeField] Transform pointPrefab;
    [SerializeField, Range(10, 100)] int resolution;
    [SerializeField] FunctionLibrary.FunctionName function;

    Transform[] points;
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
        float time = Time.time;

        FunctionLibrary.Function fxn = FunctionLibrary.GetFunction(function);
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
}
