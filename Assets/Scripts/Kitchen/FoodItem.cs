using System.Collections;
using UnityEngine;
using DG.Tweening; // DoTween

public class FoodItem : MonoBehaviour
{
    public enum FoodType { Ingredient, Mug }
    public FoodType foodType;

    public Sprite rawSprite;
    public Sprite cookedSprite;
    public float cookTime;
    public float shakeDuration = 0.2f; 

    private Vector3 originalScale;

    private SpriteRenderer spriteRenderer;
    private bool isCooking = false;
    private Tween shakeTween;
    private bool onCookingSurface = false;

    public bool isCooked = false;

    void Start()
    {
        originalScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = rawSprite;
    }

    void OnTriggerEnter(Collider other)
    {
        if ((foodType == FoodType.Ingredient && other.CompareTag("Grill")) ||
            (foodType == FoodType.Mug && other.CompareTag("Keg")))
        {
            onCookingSurface = true;
            if (!isCooking && !isCooked) 
                StartCoroutine(CookFood());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((foodType == FoodType.Ingredient && other.CompareTag("Grill")) ||
            (foodType == FoodType.Mug && other.CompareTag("Keg")))
        {
            onCookingSurface = false;
        }
    }

    IEnumerator CookFood()
    {
        isCooking = true;

        if (!isCooked)
        {
            shakeTween = transform.DOScale(originalScale * 1.05f, 0.2f)
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetEase(Ease.InOutSine);
        }

        float timer = 0f;
        while (timer < cookTime && onCookingSurface)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        shakeTween.Kill(); // Stop animation
        transform.DOScale(originalScale, 0.2f); // Reset scale

        if (onCookingSurface)
        {
            spriteRenderer.sprite = cookedSprite;
            isCooked = true;
        }
        isCooking = false;
    }
}
