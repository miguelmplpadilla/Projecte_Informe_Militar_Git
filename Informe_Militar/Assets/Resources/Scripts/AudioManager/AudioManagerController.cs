using System.Threading.Tasks;
using UnityEngine;

public class AudioManagerController : MonoBehaviour
{
    public static void PlaySfx(string audioName, GameObject parent, bool loop = false, float pitch = 1, float volume = 1)
    {
        PlayAudio(GetSfxByName(audioName), parent, loop, pitch, volume);
    }

    private static void PlayAudio(AudioClip audioClip, GameObject parent, bool loop, float pitch, float volume)
    {
        if (audioClip == null)
        {
            Debug.Log("Audio "+ audioClip.name+ " no encontrado");
            return;
        }

        GameObject prefabAudioSource = UnityEngine.Resources.Load<GameObject>("Prefabs/Audio/AudioSource");

        Debug.Log(prefabAudioSource);

        AudioSource audioSource = Instantiate(prefabAudioSource, parent.transform).GetComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.loop = loop;

        audioSource.Play();

        audioSource.pitch = pitch;
        audioSource.volume = volume;

        DestroyAudioSource(audioSource);
    }

    private static AudioClip GetSfxByName(string name)
    {
        AudioClip[] audios = UnityEngine.Resources.LoadAll<AudioClip>("Audios/SFX");

        foreach (var audio in audios)
            if (audio.name.Equals(name)) return audio;

        return null;
    }

    private static async void DestroyAudioSource(AudioSource audioSource)
    {
        while (true)
        {
            if (!audioSource.isPlaying)
            {
                Destroy(audioSource.gameObject);
                return;
            }

            await Task.Delay(100);
        }
    }
}
