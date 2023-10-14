using UnityEngine;

public class TorchAnimator : MonoBehaviour
{
    /// <summary>
    /// Amount of the variation
    /// </summary>
    public Vector3 Amount = new(1f, 1f, 0);

    /// <summary>
    /// Duration of variation
    /// </summary>
    public float Duration = 1;

    /// <summary>
    /// Speed of the variation
    /// </summary>
    public float Speed = 1;

    /// <summary>
    /// Fade in/out
    /// </summary>
    public AnimationCurve Curve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    protected float time = 0;

    private void LateUpdate()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;

            if (time > 0)
            {
                var curveValue = Curve.Evaluate(1f - time / Duration);

                var timeSpeed = time * Speed;

                //next position based on perlin noise
                var nextPos = (Mathf.PerlinNoise(timeSpeed, timeSpeed * 2) - 0.5f) * Amount.x * curveValue * transform.right +
                              (Mathf.PerlinNoise(timeSpeed * 2, timeSpeed) - 0.5f) * Amount.y * curveValue * transform.up +
                              (Mathf.PerlinNoise(timeSpeed, timeSpeed * 3) - 0.5f) * Amount.z * curveValue * transform.forward;

                transform.localPosition = nextPos;
            }
            else
            {
            }
        }
        else
        {
            time = Duration;
        }
    }
}
