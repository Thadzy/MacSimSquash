// TrajectoryUtility.cs
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryUtility : MonoBehaviour
{
    [Header("Ball & Paddle Physics")]
    public float paddleForce = 6f;          // Spring force approximation (m/s paddle tip)
    public float cor = 0.5f;                // Coefficient of Restitution (COR)
    public float paddleAngleDeg = 30f;      // Angle in degrees

    [Header("Ball Parameters")]
    public float ballMass = 0.024f;         // kg
    public float paddleMass = 0.1f;         // kg
    public float launchHeight = 0.3f;       // m

    [Header("Simulation Settings")]
    public int resolution = 60;
    public float timeStep = 0.05f;
    public Transform startPoint;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void PredictAndDraw()
    {
        Vector3 velocity = CalculateBallVelocity();
        SimulateTrajectory(startPoint.position, velocity);
    }

    public Vector3 CalculateBallVelocity()
    {
        float vPaddle = paddleForce;
        float vBall = ((1 + cor) * paddleMass * vPaddle) / (ballMass + paddleMass);

        float angleRad = paddleAngleDeg * Mathf.Deg2Rad;
        float vx = vBall * Mathf.Cos(angleRad);
        float vy = vBall * Mathf.Sin(angleRad);

        return new Vector3(vx, vy, 0f);
    }

    private void SimulateTrajectory(Vector3 startPos, Vector3 velocity)
    {
        Vector3 pos = startPos;
        Vector3 vel = velocity;
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < resolution; i++)
        {
            points.Add(pos);
            vel += Vector3.down * 9.81f * timeStep;
            pos += vel * timeStep;
            if (pos.y <= 0) break;
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}
