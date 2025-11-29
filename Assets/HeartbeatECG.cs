using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HeartbeatECG : MonoBehaviour
{
    [Header("UI Components")]
    public Image noiseOverlay;       

    [Header("ECG Settings")]
    public AnimationCurve ekgCurve;  
    public float ekgSpeed = 1.5f;    
    public float maxAlpha = 0.35f;   
    public float minAlpha = 0f;      

    [Header("Optional Pulse")]
    public Transform pulseObject;    
    public float pulseStrength = 0.1f; 
    public float pulseDuration = 0.15f;

    private float timeCounter = 0f;

    void Update()
    {
        
        timeCounter += Time.deltaTime * ekgSpeed;

        
        float curveValue = ekgCurve.Evaluate(timeCounter % ekgCurve.keys[ekgCurve.length - 1].time);
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, curveValue);

        Color c = noiseOverlay.color;
        c.a = alpha;
        noiseOverlay.color = c;

        
        if (pulseObject != null && curveValue > 0.8f) 
        {
            StartCoroutine(Pulse(pulseObject));
        }
    }

    IEnumerator Pulse(Transform target)
    {
        Vector3 originalScale = target.localScale;
        float t = 0f;

        while (t < pulseDuration)
        {
            t += Time.deltaTime;
            float scaleAmount = 1 + pulseStrength * (1 - t / pulseDuration);
            target.localScale = originalScale * scaleAmount;
            yield return null;
        }

        target.localScale = originalScale;
    }
}
