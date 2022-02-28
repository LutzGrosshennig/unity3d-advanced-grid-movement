using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLight : MonoBehaviour
{
    [Header("Color Settings")]
    [SerializeField] private Color colorBright;
    [SerializeField] private Color colorDim;

    [Header("Range Settings")]
    [SerializeField] private float minLightRange;
    [SerializeField] private float maxLightRange;

    [Header("Intensity Settings")]
    [SerializeField] private float minLightIntensity;
    [SerializeField] private float maxLightIntensity;

    [Header("Animation Settings")]
    [SerializeField] private float flickerSpeed = 0.1f;

    private Light theTorch;
    private float animationTime = 0.0f;

    private float targetRange;
    private float targetIntensity;
    private Color targetColor;

    private float fromRange;
    private float fromIntensity;
    private Color fromColor;

    // Start is called before the first frame update
    void Start()
    {
        theTorch = GetComponent<Light>();
        animationTime = 0.0f;

        fromRange = minLightRange;
        fromIntensity = minLightIntensity;
        fromColor = colorBright;

        GenerateNewLightProperties();
    }

    // Update is called once per frame
    void Update()
    {
        animationTime += Time.deltaTime;

        var animationScale = 1.0f / flickerSpeed;

        if (animationTime > flickerSpeed)
        {
            CopyLightProperties();
            GenerateNewLightProperties();
            animationTime = 0.0f;

        }
        else
        {
            theTorch.intensity = Mathf.Lerp(fromIntensity, targetIntensity, animationTime);
            theTorch.range = Mathf.Lerp(fromRange, targetRange, animationTime);
            theTorch.color = Color.Lerp(fromColor, targetColor, animationTime);
        }
    }

    private void GenerateNewLightProperties()
    {
        targetIntensity = Random.Range(minLightIntensity, maxLightIntensity);
        targetRange = Random.Range(minLightRange, maxLightRange);
        targetColor = Color.Lerp(colorDim, colorBright, Random.value);
    }

    private void CopyLightProperties()
    {
        fromIntensity = targetIntensity;
        fromRange = targetRange;
        fromColor = targetColor;
    }
}
