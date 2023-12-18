using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AudioManagerController : MonoBehaviour
{
    public AudioClip[] clips;

    [Range(0,1)]
    public float range3DAudio;

    public GameObject prefabAudioSource;

    public void PlayAudio(string audioName, GameObject parent, bool loop = false)
    {
        AudioSource audioSource = Instantiate(prefabAudioSource, parent.transform).GetComponent<AudioSource>();

        audioSource.clip = GetAudioClipByName(audioName);
        audioSource.loop = loop;

        audioSource.Play();

        StartCoroutine("DestroyAudioSource", audioSource);
    }

    private AudioClip GetAudioClipByName(string name)
    {
        foreach (var clip in clips)
        {
            if (clip.name.Equals(name)) return clip;
        }

        return null;
    }

    private IEnumerator DestroyAudioSource(AudioSource audioSource)
    {
        while (true)
        {
            if (!audioSource.isPlaying)
            {
                Destroy(audioSource.gameObject);
                yield break;
            }

            yield return null;
        }
    }
}
