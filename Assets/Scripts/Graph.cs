using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.UIElements;

public class Graph : MonoBehaviour
{
    [SerializeField] Transform pointPrefab;
    [SerializeField, Range(10, 100)] int resolution;
    [SerializeField, Range(2f, 100f)] float range;

    Transform[] points;
    void Awake()
    {
        points = new Transform[resolution];
        var position = Vector3.zero;
        
        float step = range / resolution;
        var scale = Vector3.one * step;

        for (int i = 0; i < points.Length; ++i)
        {
            Transform point = points[i] = Instantiate(pointPrefab);

            position.x = (i + 0.5f) * step - (range / 2f);
            point.SetParent(transform, false);
            point.localPosition = position;
            point.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i =0; i < points.Length; ++i)
        {
            Transform point = points[i];
            Vector3 position = point.localPosition;
            if(position.x > range)
            {
                position.x = (0 + 0.5f) * (range / resolution) - (range / 2f);
            }
            else
            {
                position.x += Time.deltaTime;
            }
            position.y = Mathf.Cos(Mathf.PI * position.x);
            
            point.localPosition = position;

        }
    }
}
