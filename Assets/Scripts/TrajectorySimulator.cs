using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data structure for storing trajectory simulation results
/// </summary>
[System.Serializable]
public class TrajectoryPoint
{
    public float angleDeg;      // Launch angle in degrees
    public float displacement;  // Horizontal distance traveled (Sx)
    public float height;        // Vertical position (Sy)
}

/// <summary>
/// Physics-based trajectory simulator for projectile motion
/// Calculates optimal launch angles for target distances using collision dynamics
/// </summary>
public class TrajectorySimulator : MonoBehaviour
{
    #region Physics Parameters
    [Header("Physics Settings")]
    public float paddleVelocity = 10f;    // Initial paddle velocity (up) in m/s
    public float cor = 0.5f;              // Coefficient of Restitution (e)
    public float massBall = 0.024f;       // Ball mass (m) in kg
    public float massPaddle = 0.1f;       // Paddle mass (M) in kg
    public float gravity = 9.81f;         // Gravitational acceleration (g) in m/s²
    public float launchHeight = 0.3f;     // Initial height above ground (h) in m
    #endregion

    #region Simulation Configuration
    [Header("Simulation Settings")]
    public float minAngle = 1f;     // Minimum launch angle (degrees)
    public float maxAngle = 90f;    // Maximum launch angle (degrees) - extended to 90°
    public float angleStep = 0.1f;  // Angle increment for simulation (degrees)
    #endregion

    #region Target Settings
    [Header("Target Position")]
    public float targetX = 2.65f;   // Target X position (m)
    public float targetY = 0f;      // Target Y position (m) - height above ground
    #endregion

    #region Simulation Data
    public List<TrajectoryPoint> trajectoryData = new();  // All simulation results
    #endregion

    #region Cached Results
    [HideInInspector] public float lastSimulatedDisplacement;  // Last calculated Sx
    [HideInInspector] public float lastSimulatedHeight;       // Last calculated Sy
    [HideInInspector] public float lastSimulatedAngle;        // Last calculated paddle angle
    [HideInInspector] public float lastSimulatedVelocity;     // Last calculated velocity
    [HideInInspector] public float lastSimulatedTime;        // Last calculated flight time
    [HideInInspector] public float lastMachineAngle;         // Last calculated machine angle
    #endregion

    #region Map-Specific Settings
    public float maxRangeX = 2.65f;  // Maximum range for map
    public float maxRangeY = 1f;     // Maximum height for map
    #endregion

    #region Main Simulation
    /// <summary>
    /// Run complete trajectory simulation across all angles (1-90 degrees)
    /// Uses your specified physics calculations
    /// </summary>
    public void Simulate()
    {
        trajectoryData.Clear();

        // Physics constants
        float m = massBall;
        float M = massPaddle;
        float e = cor;
        float up = paddleVelocity;
        float h = launchHeight;
        float g = gravity;
        
        // Ball velocity from drop: ub = sqrt(2gh)
        float ub = Mathf.Sqrt(2 * g * h);

        // Simulate across angle range (1-90 degrees as specified)
        for (float thetaDeg = minAngle; thetaDeg <= maxAngle; thetaDeg += angleStep)
        {
            var result = SimulateAngle(thetaDeg, m, M, e, up, ub, g, h);
            
            // Store all results (including out of bounds for analysis)
            trajectoryData.Add(new TrajectoryPoint 
            { 
                angleDeg = thetaDeg, 
                displacement = result.Sx,
                height = result.Sy
            });
        }
    }

    /// <summary>
    /// Simulate trajectory for specific launch angle using your exact formula
    /// </summary>
    private (float Sx, float Sy) SimulateAngle(float thetaDeg, float m, float M, float e, float up, float ub, float g, float h)
    {
        // Convert angle to radians
        float theta = thetaDeg * Mathf.Deg2Rad;
        float sinTheta = Mathf.Sin(theta);
        float cosTheta = Mathf.Cos(theta);

        // Step 1: Calculate paddle velocity after collision using momentum conservation
        // Vp = [(M-em)up*sin(theta) - m*ub*cos(theta)*(1+e)] / (m+M)
        float Vp = ((M - e * m) * up * sinTheta - m * ub * cosTheta * (1 + e)) / (m + M);

        // Step 2: Calculate ball velocity after collision
        // Vb = e*ub*cos(theta) + e*up*sin(theta) + Vp
        float Vb = e * ub * cosTheta + e * up * sinTheta + Vp;

        // Step 3: Find tan(alpha) = theta / (ub*sin(theta))
        float tanAlpha = thetaDeg / (ub * sinTheta);  // Using degrees as in your formula
        
        // Step 4: Find alpha in radians then convert to degrees
        float alphaRad = Mathf.Atan(tanAlpha);
        float alphaDeg = alphaRad * Mathf.Rad2Deg;

        // Step 5: Find beta = alpha - theta (in degrees, then convert to radians for calculation)
        float betaDeg = alphaDeg - thetaDeg;
        float betaRad = betaDeg * Mathf.Deg2Rad;

        // Step 6: Find speed of ball after collision using Pythagorean theorem
        // u = sqrt(Vb² + (ub*sin(theta))²)
        float u = Mathf.Sqrt(Vb * Vb + Mathf.Pow(ub * sinTheta, 2));

        // Step 7: Find velocity components using FBD
        float uy = u * Mathf.Sin(betaRad);  // y-axis component
        float ux = u * Mathf.Cos(betaRad);  // x-axis component

        // Step 8: Find time using quadratic formula
        // y = h + uy*t - (1/2)*g*t²
        // At ground level y = 0, so: -(1/2)*g*t² + uy*t + h = 0
        float a = -0.5f * g;
        float b = uy;
        float c = h;
        
        float discriminant = b * b - 4 * a * c;
        if (discriminant < 0) return (0f, 0f);  // No real solution

        // Get both solutions and use the maximum (positive) time
        float t1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
        float t2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);
        float t = Mathf.Max(t1, t2);

        // Step 9: Calculate displacement Sx = ux * t
        float Sx = ux * t;

        // Step 10: Calculate height at target X position
        // For any given X position, calculate corresponding Y
        float Sy = 0f; // Will be calculated based on target position

        // Cache simulation results
        lastSimulatedAngle = thetaDeg;
        lastSimulatedDisplacement = Sx;
        lastSimulatedVelocity = u;
        lastSimulatedTime = t;

        Debug.Log($"Angle: {thetaDeg:F1}°, Sx: {Sx:F2}m, u: {u:F2}m/s, t: {t:F2}s");
        
        return (Sx, Sy);
    }
    #endregion

    #region Optimization Functions
    /// <summary>
    /// Find best paddle angle for target X distance (10-90 degrees as specified)
    /// </summary>
    public float GetAngleForTargetX(float targetX)
    {
        float bestAngle = -1f;
        float minDifference = float.MaxValue;

        // Search through all simulated angles to find closest match
        foreach (var point in trajectoryData)
        {
            float difference = Mathf.Abs(point.displacement - targetX);
            if (difference < minDifference)
            {
                bestAngle = point.angleDeg;
                minDifference = difference;
            }
        }

        return bestAngle;
    }

    /// <summary>
    /// Calculate Sy (height) at target X position for given angle
    /// </summary>
    public float GetHeightAtTargetX(float targetX, float angleDeg)
    {
        // Recalculate trajectory for specific angle
        float m = massBall;
        float M = massPaddle;
        float e = cor;
        float up = paddleVelocity;
        float ub = Mathf.Sqrt(2 * gravity * launchHeight);
        
        float theta = angleDeg * Mathf.Deg2Rad;
        float sinTheta = Mathf.Sin(theta);
        float cosTheta = Mathf.Cos(theta);

        // Recalculate collision dynamics
        float Vp = ((M - e * m) * up * sinTheta - m * ub * cosTheta * (1 + e)) / (m + M);
        float Vb = e * ub * cosTheta + e * up * sinTheta + Vp;
        
        float tanAlpha = angleDeg / (ub * sinTheta);
        float alphaRad = Mathf.Atan(tanAlpha);
        float alphaDeg = alphaRad * Mathf.Rad2Deg;
        float betaRad = (alphaDeg - angleDeg) * Mathf.Deg2Rad;
        
        float u = Mathf.Sqrt(Vb * Vb + Mathf.Pow(ub * sinTheta, 2));
        float uy = u * Mathf.Sin(betaRad);
        float ux = u * Mathf.Cos(betaRad);

        // Calculate time to reach target X
        if (ux == 0) return 0f;
        float timeToX = targetX / ux;

        // Calculate height at that time: y = h + uy*t - (1/2)*g*t²
        float Sy = launchHeight + uy * timeToX - 0.5f * gravity * timeToX * timeToX;
        
        return Mathf.Max(0f, Sy); // Don't return negative heights
    }

    /// <summary>
    /// Calculate machine angle adjustment needed for target position (X, Y)
    /// Machine angle = 0° means normal position
    /// </summary>
    public float GetMachineAngle(float targetX, float targetY)
    {
        // Step 1: Calculate normal machine angle (when target Y = 0)
        // For max range point: arctan(maxRangeY / maxRangeX)
        float normalMachineAngle = Mathf.Atan(maxRangeY / maxRangeX) * Mathf.Rad2Deg;
        
        // Step 2: Calculate required angle for target position
        // arctan(targetY / targetX)
        float requiredAngle = Mathf.Atan(targetY / targetX) * Mathf.Rad2Deg;
        
        // Step 3: Calculate machine angle adjustment
        // Machine angle = normalMachineAngle - requiredAngle
        float machineAngle = normalMachineAngle - requiredAngle;
        
        lastMachineAngle = machineAngle;
        
        return machineAngle;
    }

    /// <summary>
    /// Get launch velocity for specific angle (for UI display)
    /// </summary>
    public float GetVelocityForAngle(float angleDeg)
    {
        // Recalculate velocity for specific angle
        float m = massBall;
        float M = massPaddle;
        float e = cor;
        float up = paddleVelocity;
        float ub = Mathf.Sqrt(2 * gravity * launchHeight);
        
        float theta = angleDeg * Mathf.Deg2Rad;
        float sinTheta = Mathf.Sin(theta);
        float cosTheta = Mathf.Cos(theta);

        float Vp = ((M - e * m) * up * sinTheta - m * ub * cosTheta * (1 + e)) / (m + M);
        float Vb = e * ub * cosTheta + e * up * sinTheta + Vp;
        
        float tanAlpha = angleDeg / (ub * sinTheta);
        float alphaRad = Mathf.Atan(tanAlpha);
        float alphaDeg = alphaRad * Mathf.Rad2Deg;
        float betaRad = (alphaDeg - angleDeg) * Mathf.Deg2Rad;
        
        return Mathf.Sqrt(Vb * Vb + Mathf.Pow(ub * sinTheta, 2));
    }
    #endregion

    #region Zone Classification
    /// <summary>
    /// Determine scoring zone based on distance and selected map
    /// </summary>
    public string GetZoneColor(float Sx)
    {
        int selectedMap = PlayerPrefs.GetInt("SelectedMap", 1);

        switch (selectedMap)
        {
            case 1:
                return GetMap1Zone(Sx);
            case 2:
                return GetMap2Zone(Sx);
            case 3:
                return GetMap3Zone(Sx);
            default:
                return "Invalid Map";
        }
    }

    /// <summary>
    /// Map 1: Standard distance-based zones
    /// </summary>
    private string GetMap1Zone(float Sx)
    {
        if (Sx >= 0.75f && Sx < 1.13f) return "Light Blue";
        if (Sx >= 1.13f && Sx < 1.51f) return "Green";
        if (Sx >= 1.51f && Sx < 1.89f) return "Yellow";
        if (Sx >= 1.89f && Sx < 2.27f) return "Orange";
        if (Sx >= 2.27f && Sx <= 2.65f) return "Red";
        
        return "Out of Bounds";
    }

    /// <summary>
    /// Map 2: Dual-zone system with millimeter precision
    /// </summary>
    private string GetMap2Zone(float Sx)
    {
        float SxMM = Sx * 1000f;  // Convert to millimeters

        // Far zones (higher distances)
        if (SxMM >= 4170 && SxMM < 4550) return "Red 2";
        if (SxMM >= 3790 && SxMM < 4170) return "Orange 2";
        if (SxMM >= 3410 && SxMM < 3790) return "Yellow 2";
        if (SxMM >= 3030 && SxMM < 3410) return "Green 2";
        if (SxMM >= 2650 && SxMM < 3030) return "Light Blue 2";

        // Near zones (lower distances)
        if (SxMM >= 2270 && SxMM < 2650) return "Red 1";
        if (SxMM >= 1890 && SxMM < 2270) return "Orange 1";
        if (SxMM >= 1510 && SxMM < 1890) return "Yellow 1";
        if (SxMM >= 1130 && SxMM < 1510) return "Green 1";
        if (SxMM >= 750 && SxMM < 1130) return "Light Blue 1";

        return "Out of Bounds";
    }

    /// <summary>
    /// Map 3: Angular constraint + special goal area
    /// </summary>
    private string GetMap3Zone(float Sx)
    {
        // Check angular constraint (machine angle should be within range)
        if (lastMachineAngle < 0 || lastMachineAngle > 12f)
            return "Out of Angular Range";

        // Standard zones with special goal area
        if (Sx >= 0.75f && Sx < 1.13f) return "Light Blue";
        if (Sx >= 1.13f && Sx < 1.51f) return "Green";
        if (Sx >= 1.51f && Sx < 1.89f) return "Goal Area";  // Special zone
        if (Sx >= 1.89f && Sx < 2.27f) return "Orange";
        if (Sx >= 2.27f && Sx <= 2.65f) return "Red";

        return "Out of Bounds";
    }
    #endregion
}