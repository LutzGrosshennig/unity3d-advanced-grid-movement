// Based on the code from CodX @ https://forum.unity.com/threads/reusing-an-animationcurve-through-scripting.87867/

using UnityEngine;

[CreateAssetMenu]
public class AnimationCurveAsset : ScriptableObject
{
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

    public static implicit operator AnimationCurve(AnimationCurveAsset me)
    {
        return me.curve;
    }
    public static implicit operator AnimationCurveAsset(AnimationCurve curve)
    {
        AnimationCurveAsset asset = ScriptableObject.CreateInstance<AnimationCurveAsset>();
        asset.curve = curve;
        return asset;
    }
}
