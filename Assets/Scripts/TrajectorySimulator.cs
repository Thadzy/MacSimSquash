using System.Collections.Generic;

using UnityEngine;



[System.Serializable]

public class TrajectoryPoint

{

public float angleDeg;

public float displacement;

}



public class TrajectorySimulator : MonoBehaviour

{

[Header("Physics Settings")]

public float paddleVelocity = 10f;

public float cor = 0.5f;

public float massBall = 0.024f;

public float massPaddle = 0.1f;

public float gravity = 9.81f;

public float launchHeight = 0.3f;



[Header("Simulation Settings")]

public float minAngle = 1f;

public float maxAngle = 70f;

public float angleStep = 0.1f;



public List<TrajectoryPoint> trajectoryData = new();


[HideInInspector] public float lastSimulatedDisplacement;

[HideInInspector] public float lastSimulatedAngle;

[HideInInspector] public float lastSimulatedVelocity;

[HideInInspector] public float lastSimulatedTime;



public float targetAngleDeg = 0f; // Used for Map 3 angular filtering



public void Simulate()

{

trajectoryData.Clear();



float m = massBall, M = massPaddle, e = cor;

float ub = Mathf.Sqrt(2 * gravity * launchHeight);



for (float thetaDeg = minAngle; thetaDeg <= maxAngle; thetaDeg += angleStep)

{

float theta = Mathf.Deg2Rad * thetaDeg;

float sinT = Mathf.Sin(theta), cosT = Mathf.Cos(theta);



float Vp = ((M - e * m) * paddleVelocity * sinT - m * ub * cosT * (1 + e)) / (m + M);

float Vb = e * ub * cosT + e * paddleVelocity * sinT + Vp;



float tanAlpha = thetaDeg / (ub * sinT);

float alpha = Mathf.Atan(tanAlpha);

float alphaDeg = alpha * Mathf.Rad2Deg;

float beta = (alphaDeg - thetaDeg) * Mathf.Deg2Rad;



float u = Mathf.Sqrt(Vb * Vb + Mathf.Pow(ub * sinT, 2));

float uy = u * Mathf.Sin(beta);

float ux = u * Mathf.Cos(beta);



float a = -0.5f * gravity, b = uy, c = launchHeight;

float disc = b * b - 4 * a * c;

if (disc < 0) continue;



float t = Mathf.Max((-b + Mathf.Sqrt(disc)) / (2 * a), (-b - Mathf.Sqrt(disc)) / (2 * a));

float Sx = ux * t;

if (Sx < 0 || Sx > 4.6f) continue; // Max valid range



// Save last simulation info

lastSimulatedAngle = thetaDeg;

lastSimulatedDisplacement = Sx;

lastSimulatedVelocity = u;

lastSimulatedTime = t;



trajectoryData.Add(new TrajectoryPoint { angleDeg = thetaDeg, displacement = Sx });

Debug.Log($"Angle: {thetaDeg}, Sx: {Sx}, u: {u}, t: {t}");

}

}



public float GetAngleForTargetX(float targetX)

{

float best = -1f, minDiff = float.MaxValue;

foreach (var p in trajectoryData)

{

float d = Mathf.Abs(p.displacement - targetX);

if (d < minDiff)

{

best = p.angleDeg;

minDiff = d;

}

}

return best;

}



public float GetVelocityForAngle(float angleDeg)

{

float m = massBall, M = massPaddle, e = cor;

float ub = Mathf.Sqrt(2 * gravity * launchHeight);

float theta = Mathf.Deg2Rad * angleDeg;

float sinT = Mathf.Sin(theta), cosT = Mathf.Cos(theta);



float Vp = ((M - e * m) * paddleVelocity * sinT - m * ub * cosT * (1 + e)) / (m + M);

float Vb = e * ub * cosT + e * paddleVelocity * sinT + Vp;



return Mathf.Sqrt(Vb * Vb + Mathf.Pow(ub * sinT, 2));

}



public string GetZoneColor(float Sx)

{

int map = PlayerPrefs.GetInt("SelectedMap", 1);



if (map == 1)

{

if (Sx >= 0.75f && Sx < 1.13f) return "Light Blue";

if (Sx >= 1.13f && Sx < 1.51f) return "Green";

if (Sx >= 1.51f && Sx < 1.89f) return "Yellow";

if (Sx >= 1.89f && Sx < 2.27f) return "Orange";

if (Sx >= 2.27f && Sx <= 2.65f) return "Red";

}

else if (map == 2)

{

float SxMM = Sx * 1000f;

if (SxMM >= 4170 && SxMM < 4550) return "Red 2";

if (SxMM >= 3790 && SxMM < 4170) return "Orange 2";

if (SxMM >= 3410 && SxMM < 3790) return "Yellow 2";

if (SxMM >= 3030 && SxMM < 3410) return "Green 2";

if (SxMM >= 2650 && SxMM < 3030) return "Light Blue 2";

if (SxMM >= 2270 && SxMM < 2650) return "Red 1";

if (SxMM >= 1890 && SxMM < 2270) return "Orange 1";

if (SxMM >= 1510 && SxMM < 1890) return "Yellow 1";

if (SxMM >= 1130 && SxMM < 1510) return "Green 1";

if (SxMM >= 750 && SxMM < 1130) return "Light Blue 1";

}

else if (map == 3)

{

// Angle must be inside 0–12°

if (targetAngleDeg < 0 || targetAngleDeg > 12f)

return "Out of Angular Range";



if (Sx >= 0.75f && Sx < 1.13f) return "Light Blue";

if (Sx >= 1.13f && Sx < 1.51f) return "Green";

if (Sx >= 1.51f && Sx < 1.89f) return "Goal Area";

if (Sx >= 1.89f && Sx < 2.27f) return "Orange";

if (Sx >= 2.27f && Sx <= 2.65f) return "Red";

}



return "Out of Bounds";

}

}