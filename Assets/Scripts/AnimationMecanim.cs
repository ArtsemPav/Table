using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMecanim : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private string sadTriggerName = "Sad";
    [SerializeField] private string happyTriggerName = "Happy";

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found");
        }
    }
    public void SadAnimation()
    {
        animator.SetTrigger(sadTriggerName);
    }

    public void HappyAnimation()
    {
        animator.SetTrigger(happyTriggerName);
    }
}
