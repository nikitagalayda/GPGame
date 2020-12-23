using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityAC : MonoBehaviour
{
    public float period = 5f;

    private float lastPlayed;
    private Animator[] childAnimators;
    private GameObject lastObj;

    void Start()
    {
        lastPlayed = Time.time;
        childAnimators = GetComponentsInChildren<Animator>(includeInactive: true);
        lastObj = childAnimators[0].gameObject;
    }

    void FixedUpdate()
    {
        if ((Time.time - lastPlayed) > period)
        {
            lastObj.SetActive(false);
            AnimateRandom();
            lastPlayed = Time.time;
            period = Random.Range(0.5f, 5f);
        }
    }

    private void AnimateRandom()
    {
        int rand = Random.Range(0, childAnimators.Length);
        Animator animator = childAnimators[rand];

        animator.gameObject.SetActive(true);
        lastObj = animator.gameObject;
        animator.Play("electricity", -1);
        // WaitForAnimation(animator);
    }

    // private bool AnimatorIsPlaying(Animator animator)
    // {
    //     if (animator.GetCurrentAnimatorStateInfo(-1).IsName("electricity"))
    //     {
    //         return true;
    //     }
    //     return false;
    // }

    // private IEnumerator WaitForAnimation(Animator animator)
    // {
    //     do
    //     {
    //         yield return null;
    //     } while (AnimatorIsPlaying(animator));
    // }
}
