using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HeartbeatSignal : MonoBehaviour
{
    public Image noiseOverlay;        
    public float beatInterval = 0.8f; 
    public float pulseStrength = 0.1f; 
    public float fadeDuration = 0.5f;  

    private float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= beatInterval)
        {
            
            StartCoroutine(Pulse());

            
            StartCoroutine(PulseAndFade());

            timer = 0;
        }
    }

    IEnumerator Pulse()
    {
        float t = 0;
        float duration = 0.15f;

        Vector3 originalScale = transform.localScale;

        while (t < duration)
        {
            t += Time.deltaTime;
            float scaleAmount = 1 + pulseStrength * (1 - t / duration);
            transform.localScale = originalScale * scaleAmount;
            yield return null;
        }

        transform.localScale = originalScale;
    }

    IEnumerator PulseAndFade()
    {
        Color c = noiseOverlay.color;
        float originalAlpha = 0f;           
        float maxAlpha = 0.35f;            

        
        float t = 0f;
        float pulseDuration = 0.1f;

        while (t < pulseDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(originalAlpha, maxAlpha, t / pulseDuration);
            noiseOverlay.color = c;
            yield return null;
        }

        
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(maxAlpha, originalAlpha, t / fadeDuration);
            noiseOverlay.color = c;
            yield return null;
        }

        
        c.a = originalAlpha;
        noiseOverlay.color = c;
    }
}
