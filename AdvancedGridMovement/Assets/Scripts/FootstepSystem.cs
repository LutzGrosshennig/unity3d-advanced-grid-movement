using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSystem : MonoBehaviour
{
    [SerializeField] private AudioSource leftFoot;
    [SerializeField] private AudioClip leftFootClip;

    [SerializeField] private AudioSource rightFoot;
    [SerializeField] private AudioClip rightFootClip;

    private bool nextStepLeft = false;

    public void Step()
    {
        if(nextStepLeft)
        {
                leftFoot.PlayOneShot(leftFootClip);
                nextStepLeft = !nextStepLeft;
        }
        else
        {
                rightFoot.PlayOneShot(rightFootClip);
                nextStepLeft = !nextStepLeft;
        }
    }

    public void Turn()
    {

    }
}
