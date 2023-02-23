using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionBouncer : MonoBehaviour
{
    /// <summary>
    /// Amount of Shake
    /// </summary>
    public Vector3 Amount = new Vector3(1f, 1f, 0);

    /// <summary>
    /// Duration of Shake
    /// </summary>
    public float Duration = 1;

    /// <summary>
    /// Shake Speed
    /// </summary>
    public float Speed = 1;

    /// <summary>
    /// Amount over Lifetime [0,1]
    /// </summary>
    public AnimationCurve Curve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    protected float time = 0;
    protected Vector3 lastPos;
    protected Vector3 nextPos;

    private void LateUpdate()
    {
        if (time > 0)
        {
            //do something
            time -= Time.deltaTime;
            if (time > 0)
            {
                var curveValue = Curve.Evaluate(1f - time / Duration);

                var variation = (Mathf.PerlinNoise(time * Speed, time * Speed * 2) - 0.5f) * Amount.x * transform.right * curveValue +
                                (Mathf.PerlinNoise(time * Speed * 2, time * Speed) - 0.5f) * Amount.y * transform.up * curveValue +
                                (Mathf.PerlinNoise(time * Speed * 3, time * Speed) - 0.5f) * Amount.z * transform.forward * curveValue;
                //next position based on perlin noise
                nextPos = (Mathf.PerlinNoise(time * Speed, time * Speed * 2) - 0.5f) * Amount.x * transform.right * curveValue +
                          (Mathf.PerlinNoise(time * Speed * 2, time * Speed) - 0.5f) * Amount.y * transform.up * curveValue +
                          (Mathf.PerlinNoise(time * Speed, time * Speed * 3) - 0.5f) * Amount.z * transform.forward * curveValue;

                lastPos = nextPos;
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
