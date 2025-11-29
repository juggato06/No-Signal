using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSignalController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("The 'Already Gone' Mechanic")]
    [SerializeField] private Transform lightVisual;
    [SerializeField] private float maxSignalTime = 80f;
    [SerializeField] private float startScale = 10f;
    [SerializeField] private float flickerStrength = 0.1f;
    [SerializeField] private float pulseSpeed = 3f;

    [Header("Critical State (Last 20s)")]
    [SerializeField] private float criticalThreshold = 20f;
    [SerializeField] private float criticalPulseSpeed = 10f; 
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color criticalColor = Color.red;

    [Header("UI")]
    [Tooltip("Drag your TextMeshPro UI object here to show the countdown")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Debug / Testing")]
    [Range(0f, 1f)]
    [SerializeField] private float minSignalIntensity = 0f;

    [Header("Game Over Settings")]
    [SerializeField] private string gameOverScene = "WinScene";

    private Rigidbody2D rb;
    private SpriteRenderer playerSprite;
    private SpriteRenderer lightSprite;
    private Light standardLight;
    private Light2D urpLight;

    private Vector2 moveInput;
    private float currentSignalTimer;
    private bool isDead = false;

    private float currentDebugRadius;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        currentSignalTimer = maxSignalTime;

        if (lightVisual != null)
        {
            lightSprite = lightVisual.GetComponent<SpriteRenderer>();
            standardLight = lightVisual.GetComponent<Light>();
            urpLight = lightVisual.GetComponent<Light2D>();

            Vector3 pos = lightVisual.localPosition;
            pos.z = -1f;
            lightVisual.localPosition = pos;
        }

        UpdateSignalVisuals(1f, normalColor);
    }

    void Update()
    {
        if (isDead) return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(x, y).normalized;

        HandleSignalLoss();
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = moveInput * moveSpeed;
    }

    void HandleSignalLoss()
    {
        currentSignalTimer -= Time.deltaTime;
        bool isCritical = currentSignalTimer <= criticalThreshold;

        float currentSpeed = isCritical ? criticalPulseSpeed : pulseSpeed;
        Color currentColor = isCritical ? criticalColor : normalColor;

        // UI Updates
        if (timerText != null)
        {
            float timeToDisplay = Mathf.Max(0, currentSignalTimer);
            int minutes = Mathf.FloorToInt(timeToDisplay / 60);
            int seconds = Mathf.FloorToInt(timeToDisplay % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            timerText.color = currentColor;
        }

        float signalPercent = Mathf.Clamp01(currentSignalTimer / maxSignalTime);

        float pulse = Mathf.Sin(Time.time * currentSpeed) * flickerStrength;
        float flickerPercent = Mathf.Clamp01(signalPercent + pulse);

        float finalIntensity = Mathf.Max(flickerPercent, minSignalIntensity);

        UpdateSignalVisuals(finalIntensity, currentColor);

        if (currentSignalTimer <= 0)
        {
            Die();
        }
    }

    void UpdateSignalVisuals(float percent, Color tintColor)
    {
        if (lightVisual != null)
        {
            float currentScale = Mathf.Lerp(0f, startScale, percent);

            lightVisual.localScale = new Vector3(currentScale, currentScale, 1f);
            currentDebugRadius = currentScale;

            if (urpLight != null)
            {
                urpLight.intensity = percent;
                urpLight.color = tintColor; 
            }

            if (lightSprite != null)
            {
                Color c = tintColor;
                c.a = percent; 
                lightSprite.color = c;
            }

            if (standardLight != null)
            {
                standardLight.intensity = percent * 2f;
                standardLight.range = currentScale;
                standardLight.color = tintColor;
            }
        }

        if (playerSprite != null)
        {
            playerSprite.color = Color.Lerp(Color.black, tintColor, percent);
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Signal Lost. Game Over.");

        WinScreenUI.GameWon = false;
        SceneManager.LoadScene(gameOverScene);
    }

    void OnDrawGizmos()
    {
        if (lightVisual == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(lightVisual.position, currentDebugRadius > 0 ? currentDebugRadius * 0.5f : startScale * 0.5f);
    }
}