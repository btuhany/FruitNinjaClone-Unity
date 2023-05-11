using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource[] _audioSources;
    public static SoundManager Instance;
    [SerializeField] AudioClip[] _bombAudioClips;
    [SerializeField] AudioClip[] _sliceAudioClips;
    AudioClip _tempAudioClip;
    private void Awake()
    {
        Instance = this;
        _audioSources= GetComponentsInChildren<AudioSource>();
    }

    public void PlaySound(int auidoSourceIndex)
    {
        if (_audioSources[auidoSourceIndex].isPlaying) return;
            
        _audioSources[auidoSourceIndex].Play();
    }
    public void PlaySoundDelayed(int auidoSourceIndex)
    {
        StartCoroutine(PlaySoundWithDelay(auidoSourceIndex));
    }
    public void PlaySoundRandomPitch(int auidoSourceIndex, float minPitch, float maxPitch)
    {
        if (_audioSources[auidoSourceIndex].isPlaying) return;
        _audioSources[auidoSourceIndex].pitch = Random.Range(minPitch, maxPitch+0.01f);
        _audioSources[auidoSourceIndex].Play();
    }
    
    public void  PlaySoundRandomClip(int auidoSourceIndex)
    {
        if (auidoSourceIndex == 3)
        {
            _tempAudioClip = _bombAudioClips[Random.Range(0,_bombAudioClips.Length)];
        }
        _audioSources[auidoSourceIndex].PlayOneShot(_tempAudioClip);
    }
    public void PlaySliceSound()
    {
        _audioSources[7].pitch = Random.Range(0.85f, 1.2f);
        _audioSources[7].PlayOneShot(_sliceAudioClips[Random.Range(0, _sliceAudioClips.Length)]);
    }
    public void PlayBombThrowSound()
    {
      
        _audioSources[3].PlayOneShot(_bombAudioClips[Random.Range(0, _bombAudioClips.Length)]);
        _audioSources[3].volume = 0.7f;
        StopAllCoroutines();
        StartCoroutine(BombSoundEffect());
    }
    public void PlayBombSliceSound()
    {
       
        StopAllCoroutines();
        _audioSources[5].Play();
    }
    public void StopAllSounds()
    {
        for (int i = 0; i < _audioSources.Length; i++)
        {
            if (i == 5) continue;   //slice sound?
            _audioSources[i].Stop();
        }
    }
    WaitForSeconds bombDelay = new WaitForSeconds(2f);
    IEnumerator BombSoundEffect()
    {
        yield return bombDelay;
        float elapsed = 0f;
        float duration = 1f;
        while (elapsed < duration)
        {
            _audioSources[3].volume = Mathf.Lerp(_audioSources[3].volume,0, elapsed*0.03f / duration);
           // Debug.Log(Mathf.Lerp(_audioSources[index].volume, 0, t));
            elapsed += Time.deltaTime;
            yield return null;
        }
        
    }
    WaitForSeconds delay = new WaitForSeconds(0.7f);
    IEnumerator PlaySoundWithDelay(int auidoSourceIndex)
    {
        yield return delay;
        _audioSources[auidoSourceIndex].Play();
        yield return null;
    }

}
