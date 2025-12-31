using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource seSource;

    [SerializeField] AudioClip warningClip;
    [SerializeField] AudioClip lookWarningClip;
    [SerializeField] AudioClip clearClip;
    [SerializeField] AudioClip stageopenClip;

    public void PlayWarning()
    {
        seSource.PlayOneShot(warningClip);
    }

    public void PlayLookWarning()
    {
        seSource.PlayOneShot(lookWarningClip);
    }

    public void PlayClear()
    {
        seSource.PlayOneShot(clearClip);
    }

    public void PlayStageOpen()
    {
        seSource.PlayOneShot(stageopenClip);
    }
}