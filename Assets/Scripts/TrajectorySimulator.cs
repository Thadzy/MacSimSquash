using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

[System.Serializable]
public class TrajectoryPoint
{
    public float angleDeg;
    public float displacement;
}

public class TrajectorySimulator : MonoBehaviour
{
    [Header("Physics Settings")]
    public float launchVelocity = 10f;  // Speed after hitting (u)
    public float launchHeight = 0.3f;  // Height above ground
    public float gravity = 9.81f;

    [Header("Simulation Settings")]
    public float minAngle = 1f;
    public float maxAngle = 89f;
    public float angleStep = 0.1f;

    [Header("Debug")]
    public bool exportCSV = true;
    public bool drawTrajectoryGraph = true;
    public LineRenderer lineRenderer;

    [Header("Output")]
    public List<TrajectoryPoint> trajectoryData = new();

    [Header("User Adjustable Parameters")]
    public float cor = 0.5f;  // Default coefficient of restitution


    void Start()
    {
        Simulate();

        if (exportCSV)
            ExportToCSV();

        if (drawTrajectoryGraph && lineRenderer != null)
            DrawGraph();
    }

    public void Simulate()
{
    trajectoryData.Clear();

    for (float angle = minAngle; angle <= maxAngle; angle += angleStep)
    {
        float rad = Mathf.Deg2Rad * angle;
        float ux = launchVelocity * Mathf.Cos(rad);
        float uy = launchVelocity * Mathf.Sin(rad);

        float a = -0.5f * gravity;
        float b = uy;
        float c = launchHeight;

        float discriminant = b * b - 4 * a * c;
        if (discriminant < 0) continue;

        float t1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
        float t2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);
        float t = Mathf.Max(t1, t2);
        float x = ux * t;

        trajectoryData.Add(new TrajectoryPoint { angleDeg = angle, displacement = x });
    }

    // if (predictor.drawTrajectoryGraph)
    // predictor.DrawGraph(); // make this public if needed

    Debug.Log($"Simulated {trajectoryData.Count} angles.");
}


    public float GetAngleForTargetX(float targetX)
    {
        float bestAngle = -1f;
        float smallestDiff = float.MaxValue;

        foreach (var point in trajectoryData)
        {
            float diff = Mathf.Abs(point.displacement - targetX);
            if (diff < smallestDiff)
            {
                smallestDiff = diff;
                bestAngle = point.angleDeg;
            }
        }

        return bestAngle;
    }

    void ExportToCSV()
    {
        string path = Application.dataPath + "/trajectory_table.csv";
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Angle (deg),Displacement (m)");

        foreach (var point in trajectoryData)
            sb.AppendLine($"{point.angleDeg},{point.displacement}");

        File.WriteAllText(path, sb.ToString());
        Debug.Log("CSV Exported to: " + path);
    }

    void DrawGraph()
    {
        if (lineRenderer == null || trajectoryData.Count == 0)
            return;

        lineRenderer.positionCount = trajectoryData.Count;

        for (int i = 0; i < trajectoryData.Count; i++)
        {
            // Draw as (x = angle, y = range)
            Vector3 pos = new Vector3(trajectoryData[i].angleDeg, trajectoryData[i].displacement, 0);
            lineRenderer.SetPosition(i, pos);
        }

        Debug.Log("LineRenderer trajectory graph generated.");
    }
}
