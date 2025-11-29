using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSignalController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("The 'Already Gone' Mechanic")]
    [Tooltip("The Transform holding your Light/Glow Sprite")]
    [SerializeField] private Transform lightVisual;

    [SerializeField] private float maxSignalTime = 60f;
    [SerializeField] private float startScale = 10f;
    [SerializeField] private float flickerStrength = 0.1f;

    private Rigidbody2D rb;
    private SpriteRenderer playerSprite;
    private SpriteRenderer lightSprite;
    private Light standardLight;
    private Light2D urpLight;

    private Vector2 moveInput;
    private float currentSignalTimer;
    private bool isDead = false;

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
        }

        UpdateSignalVisuals(1f);
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

        float signalPercent = Mathf.Clamp01(currentSignalTimer / maxSignalTime);

        float noise = Random.Range(-flickerStrength, flickerStrength) * (1 - signalPercent);
        float flickerPercent = Mathf.Clamp01(signalPercent + noise);

        UpdateSignalVisuals(flickerPercent);

        if (currentSignalTimer <= 0)
        {
            Die();
        }
    }

    void UpdateSignalVisuals(float percent)
    {
        if (lightVisual != null)
        {

            float currentScale = Mathf.Lerp(0f, startScale, percent);
            lightVisual.localScale = new Vector3(currentScale, currentScale, 1f);


            if (urpLight != null)
            {
                urpLight.intensity = percent;


            }


            if (lightSprite != null)
            {
                Color c = lightSprite.color;
                c.a = percent;
                lightSprite.color = c;
            }

            if (standardLight != null)
            {
                standardLight.intensity = percent * 2f;
                standardLight.range = currentScale;
            }
        }


        if (playerSprite != null)
        {
            playerSprite.color = Color.Lerp(Color.black, Color.white, percent);
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Signal Lost. Game Over.");
    }
}