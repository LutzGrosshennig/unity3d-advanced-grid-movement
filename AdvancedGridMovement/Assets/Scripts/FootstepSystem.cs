using UnityEngine;

public class FootstepSystem : MonoBehaviour
{
    [SerializeField] private AudioSource leftFoot;
    [SerializeField] private AudioClip leftFootClip;

    [SerializeField] private AudioSource rightFoot;
    [SerializeField] private AudioClip rightFootClip;

    [SerializeField] private AudioClip turnClip;

    private bool nextStepLeft = false;

    public void Step()
    {
        if(nextStepLeft)
        {
                leftFoot.PlayOneShot(leftFootClip, Random.Range(0.2f, 0.5f));
                nextStepLeft = !nextStepLeft;
        }
        else
        {
                rightFoot.PlayOneShot(rightFootClip, Random.Range(0.2f, 0.5f));
                nextStepLeft = !nextStepLeft;
        }
    }

    public void Turn()
    {
        leftFoot.pitch = Random.Range(0.7f, 1f);
        rightFoot.pitch = Random.Range(0.7f, 1f);
        leftFoot.PlayOneShot(turnClip, Random.Range(0.1f, 0.3f));
        rightFoot.PlayOneShot(turnClip, Random.Range(0.1f, 0.3f));
    }
}
