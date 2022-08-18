using UnityEngine;

public class FootstepSystem : MonoBehaviour
{
    [Header("Left foot settings")]
    [SerializeField] private AudioSource leftFoot;
    [SerializeField] private AudioClip leftFootClip;
    [Range(0.1f, 1.0f)]
    [SerializeField] private float leftMinimumPitch = 0.5f;

    [Header("Right foot settings")]
    [SerializeField] private AudioSource rightFoot;
    [SerializeField] private AudioClip rightFootClip;
    [Range(0.1f, 1.0f)]
    [SerializeField] private float rightMinimumPitch = 0.5f;

    [Header("turn around settings")]
    [SerializeField] private AudioClip turnClip;
    [Range(0.1f, 1.0f)]
    [SerializeField] private float turnMinimumPitch = 0.5f;



    private bool nextStepLeft = false;



    public void Step()
    {
        if (nextStepLeft)
        {
            leftFoot.pitch = Random.Range(leftMinimumPitch, 1f);
            leftFoot.PlayOneShot(leftFootClip);
            nextStepLeft = !nextStepLeft;
        }
        else
        {
            rightFoot.pitch = Random.Range(rightMinimumPitch, 1f);
            rightFoot.PlayOneShot(rightFootClip);
            nextStepLeft = !nextStepLeft;
        }
    }

    public void Turn()
    {
        leftFoot.pitch = Random.Range(turnMinimumPitch, 1f);
        rightFoot.pitch = Random.Range(turnMinimumPitch, 1f);
        leftFoot.PlayOneShot(turnClip);
        rightFoot.PlayOneShot(turnClip);
    }
}
