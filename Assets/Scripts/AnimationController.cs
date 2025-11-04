using Spine.Unity;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private string idleAnimation = "idle";
    [SerializeField] private string sadAnimation = "Sad";
    [SerializeField] private string happyAnimation = "Happy";
    [SerializeField] private float defaultMixDuration = 2f;

    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        if (skeletonAnimation == null)
        {
            Debug.LogError("SkeletonAnimation component not found on this GameObject!");
            enabled = false; // Disable the script if the component is missing
        }
        skeletonAnimation.AnimationState.Data.DefaultMix = defaultMixDuration;
    }
    void OnEnable()
    {
        IdleAnimation();
    }

    public void IdleAnimation()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, idleAnimation, true);
    }
    public void SadAnimation()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, sadAnimation, false);
        skeletonAnimation.AnimationState.AddAnimation(0, idleAnimation, true, 0f);
    }

    public void HappyAnimation()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, happyAnimation, false);
        skeletonAnimation.AnimationState.AddAnimation(0, idleAnimation, true, 0f);
    }
}
