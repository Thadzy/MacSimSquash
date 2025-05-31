// TrajectorySimulator.cs
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
    public float paddleVelocity = 10f;   // u_p (user input)
    public float cor = 0.5f;             // e (user input)
    public float massBall = 0.024f;      // m
    public float massPaddle = 0.1f;      // M
    public float gravity = 9.81f;
    public float launchHeight = 0.3f;    // h

    [Header("Simulation Settings")]
    public float minAngle = 1f;
    public float maxAngle = 70f;
    public float angleStep = 0.1f;

    [Header("Debug")]
    public bool exportCSV = false;
    public bool drawTrajectoryGraph = false;
    public LineRenderer lineRenderer;

    [Header("Output")]
    public List<TrajectoryPoint> trajectoryData = new();

    public void Simulate()
    {
        trajectoryData.Clear();
        float m = massBall;
        float M = massPaddle;
        float e = cor;
        float ub = Mathf.Sqrt(2 * gravity * launchHeight); // impact velocity

        for (float thetaDeg = minAngle; thetaDeg <= maxAngle; thetaDeg += angleStep)
        {
            float theta = Mathf.Deg2Rad * thetaDeg;
            float sinT = Mathf.Sin(theta);
            float cosT = Mathf.Cos(theta);

            // Step 1: V_p
            float numerator = (M - e * m) * paddleVelocity * sinT - m * ub * cosT * (1 + e);
            float denominator = m + M;
            float Vp = numerator / denominator;

            // Step 2: V_b
            float Vb = e * ub * cosT + e * paddleVelocity * sinT + Vp;

            // Step 3–5: alpha
            float tanAlpha = thetaDeg / (ub * sinT);
            float alpha = Mathf.Atan(tanAlpha);
            float alphaDeg = alpha * Mathf.Rad2Deg;

            // Step 6: beta
            float beta = (alphaDeg - thetaDeg) * Mathf.Deg2Rad;

            // Step 7: final u (speed after hit)
            float u = Mathf.Sqrt(Vb * Vb + Mathf.Pow(ub * sinT, 2));

            // Step 8–9: components
            float uy = u * Mathf.Sin(beta);
            float ux = u * Mathf.Cos(beta);

            // Step 10: quadratic solve for time
            float a = -0.5f * gravity;
            float b = uy;
            float c = launchHeight;
            float discriminant = b * b - 4 * a * c;
            if (discriminant < 0) continue;

            float t1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
            float t2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);
            float t = Mathf.Max(t1, t2);

            // Step 11: Sx
            float Sx = ux * t;
            trajectoryData.Add(new TrajectoryPoint { angleDeg = thetaDeg, displacement = Sx });
        }

        Debug.Log($"Simulated {trajectoryData.Count} angles with COR and velocity.");

        if (exportCSV)
            ExportToCSV();

        if (drawTrajectoryGraph && lineRenderer != null)
            DrawGraph();
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
        StringBuilder sb = new();
        sb.AppendLine("Angle (deg),Displacement (m)");

        foreach (var point in trajectoryData)
            sb.AppendLine($"{point.angleDeg},{point.displacement}");

        File.WriteAllText(path, sb.ToString());
        Debug.Log("📁 CSV Exported to: " + path);
    }

    void DrawGraph()
    {
        if (lineRenderer == null || trajectoryData.Count == 0)
            return;

        lineRenderer.positionCount = trajectoryData.Count;
        for (int i = 0; i < trajectoryData.Count; i++)
        {
            Vector3 pos = new Vector3(trajectoryData[i].angleDeg, trajectoryData[i].displacement, 0);
            lineRenderer.SetPosition(i, pos);
        }

        Debug.Log("LineRenderer trajectory graph generated.");
    }
}
