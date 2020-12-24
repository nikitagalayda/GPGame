using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityAC : MonoBehaviour
{
    public float minPeriod = 0.5f;
    public float maxPeriod = 5f;

    private float period = 1f;
    private float lastPlayed;
    private Animator[] childAnimators;
    private Coroutine coroutine;

    void Start()
    {
        lastPlayed = Time.time;
        childAnimators = GetComponentsInChildren<Animator>(includeInactive: true);
    }

    void FixedUpdate()
    {
        if ((Time.time - lastPlayed) > period)
        {
            AnimateRandom();
            lastPlayed = Time.time;
            period = Random.Range(minPeriod, maxPeriod);
        }
    }

    private void AnimateRandom()
    {
        int rand = Random.Range(0, childAnimators.Length);
        Animator animator = childAnimators[rand];

        animator.gameObject.SetActive(true);
        StartCoroutine(WaitForAnimation(animator));
    }

    private bool AnimatorIsPlaying(Animator animator)
    {

        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
            return false;
        }
        
        if (animator.GetCurrentAnimatorStateInfo(1).fullPathHash == 0)
        {
            return true;
        }

        return false;
    }

    private IEnumerator WaitForAnimation(Animator animator)
    {
        do
        {
            yield return null;
        } while (AnimatorIsPlaying(animator));
        animator.gameObject.SetActive(false);
        yield return null;
    }

}
