using UnityEngine;

public class TorchAnimator : MonoBehaviour
{
    /// <summary>
    /// Amount of the variation
    /// </summary>
    public Vector3 Amount = new Vector3(1f, 1f, 0);

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

                //next position based on perlin noise
                var nextPos = (Mathf.PerlinNoise(time * Speed, time * Speed * 2) - 0.5f) * Amount.x * transform.right * curveValue +
                              (Mathf.PerlinNoise(time * Speed * 2, time * Speed) - 0.5f) * Amount.y * transform.up * curveValue +
                              (Mathf.PerlinNoise(time * Speed, time * Speed * 3) - 0.5f) * Amount.z * transform.forward * curveValue;

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
