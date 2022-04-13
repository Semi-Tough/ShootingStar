using System.Collections;
using UnityEngine;

class LightFade : MonoBehaviour
{
    [SerializeField] float fadeDuration = 1f;
    [SerializeField] bool delay;
    [SerializeField] float delayTime;
    [SerializeField] float startIntensity = 30f;
    [SerializeField] float finalIntensity;
    
    private WaitForSeconds waitDelayTime;
    private Light vfXLight;
    private float t;

    public LightFade()
    {
        delayTime = 0f;
        finalIntensity = 0f;
    }

    void Awake()
    {
        vfXLight = GetComponent<Light>();

        waitDelayTime = new WaitForSeconds(delayTime);
    }

    void OnEnable()
    {
        StartCoroutine(nameof(LightCoroutine));
    }

    IEnumerator LightCoroutine()
    {
        if (delay)
        {
            yield return waitDelayTime;
        }

        vfXLight.intensity = startIntensity;
        vfXLight.enabled = true;
        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            vfXLight.intensity = Mathf.Lerp(startIntensity, finalIntensity, t);

            yield return null;
        }

        vfXLight.enabled = false;
    }
}