using Spine.Unity;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    private Spine.AnimationState animationState;
    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        if (skeletonAnimation == null)
        {
            Debug.LogError("SkeletonAnimation component not found on this GameObject!");
            enabled = false; // Disable the script if the component is missing
        }
    }
    void OnEnable()
    {
        IdleAnimation();
    }

    public void IdleAnimation()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
        Debug.Log("idle Animation start");
    }
    public void SadAnimation()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, "Sad", false);
        Debug.Log("Sad Animation start");
        skeletonAnimation.AnimationState.AddAnimation(0, "idle", true,3);
    }

    public void HappyAnimation()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, "Happy", false);
        Debug.Log("Happy Animation start");
        skeletonAnimation.AnimationState.AddAnimation(0, "idle", true, 3);
    }
}
