using UnityEngine;

public class RandomIdleStart : MonoBehaviour
{
    private string idleAnimationStateName = "Idle";
    private float animationLength = 1f;

    void Start()
    {
        Animator animator = GetComponent<Animator>();
        float randomStartTime = Random.Range(0f, animationLength);
        animator.Play(idleAnimationStateName, 0, randomStartTime / animationLength);
    }
}
