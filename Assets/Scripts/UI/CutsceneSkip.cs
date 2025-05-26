using UnityEngine;

public class CutsceneSkip : MonoBehaviour
{
    [Header("Animators")]
    public Animator cameraAnimator;
    public Animator canvasAnimator;
    public Animator characterAAnimator;
    public Animator characterBAnimator;

    [Header("Animation Clip Info")]
    public float targetSkipTime = 28f;
    public float fullAnimationDuration = 30f;

    [Header("Menu")]
    public GameObject startMenu;

    private bool hasSkipped = false;

    void Update()
    {
        if (!hasSkipped && Input.GetKeyDown(KeyCode.Space))
        {
            SkipTo25Seconds();
        }
    }

    void SkipTo25Seconds()
    {
        hasSkipped = true;

        float normalizedTime = targetSkipTime / fullAnimationDuration;

        SkipAnimatorToTime(cameraAnimator, normalizedTime);
        SkipAnimatorToTime(canvasAnimator, normalizedTime);
        SkipAnimatorToTime(characterAAnimator, normalizedTime);
        SkipAnimatorToTime(characterBAnimator, normalizedTime);
    }

    void SkipAnimatorToTime(Animator animator, float normalizedTime)
    {
        if (animator == null) return;

        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        animator.Play(currentState.fullPathHash, 0, normalizedTime);
        animator.Update(0);
    }

}
