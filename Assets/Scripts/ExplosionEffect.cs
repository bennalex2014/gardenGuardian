using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [Header("Animation Settings")]
    public float expandDuration = 0.3f;  // How long to expand
    public float maxScale = 3f;          // Final size multiplier
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float timer = 0f;
    private Vector3 startScale;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Start small
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float progress = timer / expandDuration;

        if (progress <= 1f)
        {
            // Scale up
            float curveValue = scaleCurve.Evaluate(progress);
            transform.localScale = startScale * maxScale * curveValue;

            // Fade out
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = 1f - progress; // Fade from 1 to 0
                spriteRenderer.color = color;
            }
        }
    }
}