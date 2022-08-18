using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensityFlicker : MonoBehaviour
{
    [Header("Light intensity settings")]
    [SerializeField] float minimumIntensity = 0.5f;
    [SerializeField] float maximumIntensity = 2.5f;
    [SerializeField] float flickerDuration  = 0.35f;

    private Light lightSource;
    private float elapsedTime;

    private float timeScale;
    private float lastIntensity;
    private float targetIntensity;

    // Start is called before the first frame update
    void Start()
    {
        lightSource = GetComponent<Light>();
        ResetElapsedTime();
        timeScale = 1.0f / flickerDuration;
        lastIntensity = minimumIntensity;
        targetIntensity = maximumIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > flickerDuration)
        {
            lastIntensity = targetIntensity;
            targetIntensity = Random.Range(minimumIntensity, maximumIntensity);
            ResetElapsedTime();
            lightSource.intensity = targetIntensity;
        }
        else
        {
            lightSource.intensity = Mathf.Lerp(lastIntensity, targetIntensity, elapsedTime * timeScale);
        }
    }

    private void ResetElapsedTime()
    {
        elapsedTime = 0.0f;
    }
}
