using UnityEngine;
using System.Linq;

public static class AnimationSafePlay
{
    public static bool SafePlay(this Animator animator, string clipName)
    {
        if (animator == null) return false;

        var clips = animator.runtimeAnimatorController.animationClips;
        if (!clips.Any(c => c.name == clipName))
        {
            Debug.LogWarning($"Animator no tiene el clip: {clipName}");
            return false;
        }

        animator.Play(clipName);
        return true;
    }
}
