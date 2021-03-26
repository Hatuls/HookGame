using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{

    [SerializeField] SongSO[] songs;
    SongSO currentSong;

    public override void Init()
    {
       // currentSong = songs[0];
    }
    // Start is called before the first frame update
    public static float GetBeatPerSecond()
        => SoundManager.Instance.currentSong.GetBPM / 60f;


}
