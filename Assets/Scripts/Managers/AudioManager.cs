using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource AudioSourceSFX;
    public List<AudioClips> AudioClipsList;


    public static AudioManager Instance;



    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }


    public void PlaySFX(string name, float delay = 0f)
    {
        AudioClip clip = null;
        var maxVolume = 1f;

        foreach (var child in AudioClipsList)
        {
            if (name == child.Name)
            {
                clip = child.AudioClip;
                maxVolume = child.MaxVolume;
            }
        }

        if (!clip)
        {
            Debug.Log("no clip found");
            return;
        }

        if (!AudioSourceSFX)
        {
            Debug.Log("no audio source found");
            return;
        }

        StartCoroutine(DoWIthDelay(() =>
                {

                    AudioSourceSFX.clip = clip;
                    AudioSourceSFX.volume = maxVolume;
                    // AudioSourceSFX.Play();
                    AudioSourceSFX.PlayOneShot(clip, maxVolume);

                }, delay));

    }





    /// <summary>
    /// Do some action with delay
    /// </summary>
    /// <param name="action"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator DoWIthDelay(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

}


[Serializable]
public class AudioClips
{
    public string Name;
    public float MaxVolume;
    public AudioClip AudioClip;
}




