using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip m_sndCoins;

    public AudioSource[] m_AudioSources;
    int m_CurAudio = 0;

    public void Awake()
    {
        GameManager.SoundManager = this;
    }

    public void PlaySound(AudioClip clip)
    {
        if (m_CurAudio == m_AudioSources.Length)
        {
            m_CurAudio = 0;
        }

        m_AudioSources[m_CurAudio].clip = clip;
        m_AudioSources[m_CurAudio].pitch = Random.Range(0.9f,1.0f);
        m_AudioSources[m_CurAudio].Play();

        m_CurAudio++;
    }

    public void PlayCoinSound()
    {
        PlaySound(m_sndCoins);
    }
}
