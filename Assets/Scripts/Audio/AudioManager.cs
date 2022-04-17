/****************************************************
    文件：AudioManager.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/16 20:34:55
    功能：音频管理器
*****************************************************/

using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] private AudioSource sfxPlayer;

    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;

    public void PlaySfx(AudioData audioData)
    {
        sfxPlayer.PlayOneShot(audioData.AudioClip, audioData.Volume);
    }


    public void PlayRandomPitch(AudioData audioData)
    {
        sfxPlayer.pitch = Random.Range(minPitch, maxPitch);
        PlaySfx(audioData);
    }

    public void PlayRandomPitch(AudioData[] audioData)
    {
        PlayRandomPitch(audioData[Random.Range(0, audioData.Length)]);
    }
}