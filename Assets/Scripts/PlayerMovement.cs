using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSignalController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Signal Loss mechanic")]
    [Tooltip("The visual object representing the light")]
    [SerializeField] private Transform lightVisual;

    [SerializeField] private float maxSignalTime = 60f;
    [SerializeField] private float startScale = 10f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float currentSignalTimer;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSignalTimer = maxSignalTime;
        UpdateLightScale(1f);
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

        UpdateLightScale(signalPercent);

        if (currentSignalTimer <= 0)
        {
            Die();
        }
    }

    void UpdateLightScale(float percent)
    {
        if (lightVisual != null)
        {
            float currentScale = Mathf.Lerp(0f, startScale, percent);
            lightVisual.localScale = new Vector3(currentScale, currentScale, 1f);
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Signal Lost. Game Over.");
    }
}