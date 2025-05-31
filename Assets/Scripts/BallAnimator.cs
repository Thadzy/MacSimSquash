using UnityEngine;

public class BallAnimator : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float arcHeight = 1.0f;
    public float duration = 1.0f;

    private float timer = 0f;
    private bool isAnimating = false;

    public void Launch(Vector3 start, Vector3 end, float arc, float time)
    {
        startPoint = start;
        endPoint = end;
        arcHeight = arc;
        duration = time;
        timer = 0f;
        isAnimating = true;
    }

    void Update()
    {
        if (!isAnimating) return;

        timer += Time.deltaTime;
        float progress = Mathf.Clamp01(timer / duration);

        // Parabolic interpolation
        Vector3 pos = Vector3.Lerp(startPoint, endPoint, progress);
        float arc = arcHeight * Mathf.Sin(Mathf.PI * progress);
        pos.y += arc;

        transform.position = pos;

        if (progress >= 1f)
            isAnimating = false;
    }
}
