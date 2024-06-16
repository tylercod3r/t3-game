using UnityEngine;

public class AudioController:MonoBehaviour, IAudioController
{
    [SerializeField] private AudioSource startSound;
    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource lossSound;
    [SerializeField] private AudioSource drawSound;
    [SerializeField] private AudioSource clickSound;

    public void PlayStartSound()
    {
        startSound.Play();
    }

    public void PlayClickSound()
    {
        clickSound.Play();
    }

    public void PlayWinSound()
    {
        winSound.Play();
    }

    public void PlayLossSound()
    {
        lossSound.Play();
    }

    public void PlayDrawSound()
    {
        drawSound.Play();
    }
}
