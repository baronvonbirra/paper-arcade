using System.Collections;
using UnityEngine;

namespace PaperDollArcade
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private void Awake()
        {
            if (animator == null) animator = GetComponent<Animator>();
        }

        public void PlayAnimation(string animationType)
        {
            if (animator != null)
            {
                animator.SetTrigger(animationType);
            }
            else
            {
                // Simple scale bounce animation if no Animator component attached
                StartCoroutine(BounceAnimationCoroutine());
            }
        }

        private IEnumerator BounceAnimationCoroutine()
        {
            Vector3 originalScale = transform.localScale;
            Vector3 targetScale = originalScale * 1.1f;
            float duration = 0.15f;

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                transform.localScale = Vector3.Lerp(originalScale, targetScale, t / duration);
                yield return null;
            }

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                transform.localScale = Vector3.Lerp(targetScale, originalScale, t / duration);
                yield return null;
            }

            transform.localScale = originalScale;
        }
    }
}
