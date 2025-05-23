using UnityEngine;

public class SynchronizeIdleStart : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (HandUI.Instance != null)
            HandUI.Instance.OnAllButtonsDisabled.AddListener(PlayAnimation);
    }

    void PlayAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Start");
    }

    void OnDestroy()
    {
        if (HandUI.Instance != null)
            HandUI.Instance.OnAllButtonsDisabled.RemoveListener(PlayAnimation);
    }
}
