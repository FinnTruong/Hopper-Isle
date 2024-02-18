using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] tracks;

    private int lastTrackIndex = -1;

    // Start is called before the first frame update
    private void Start()
    {
        PlayMusic();
    }
    public void PlayMusic()
    {
        CancelInvoke();
        int targetIndex = Random.Range(0, tracks.Length);
        for (int i = 0; i < 1000; i++)
        {
            if (targetIndex == lastTrackIndex)
                targetIndex = Random.Range(0, tracks.Length);
        }

        AudioClip clipToPlay = tracks[targetIndex];
        AudioManager.Instance.PlayMusic(clipToPlay, 2f);
        lastTrackIndex = targetIndex;

        Invoke("PlayMusic", clipToPlay.length + 1f);

    }
}
