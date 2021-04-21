
using UnityEngine;

public partial class SoundManager : MonoSingleton<SoundManager>
{

    [SerializeField] SongSO[] songs;
    static SongSO currentSong;
    [SerializeField] bool gameStarted;

    //public bool isCorrectTiming
    //     => NoteDestination.canBePressed
    //     || CheatMenu.Instance.GetUnLimitedCompressor;


   [SerializeField] AudioSource _audioSource;
    NoteDestination noteDestination;


    public delegate void SoundEvent();
    public static event SoundEvent StartMusic;
    public static event SoundEvent StopMusic;
    public static float GetBeat => currentSong.GetBPM;
    public bool GetGameStartingCondition => gameStarted;


    public override void Init()
    {
        _audioSource = GetComponent<AudioSource>();
        noteDestination = FindObjectOfType<NoteDestination>();
        currentSong = songs[0];
        AdjustAudioProfile(audioProfile);
        notes = FindObjectsOfType<NoteIcon>();
    }
    // Start is called before the first frame update
    public static float GetBeatPerSecond()
        => Instance._beatInterval;
    private void Start()
    {
     
        currentSong = songs[0];
        // StartMusic?.Invoke();
     
    }
  

    public void StartPlayingSong()
    {
        StartMusic?.Invoke();
        return;
        if (!_audioSource.isPlaying)
        {

            _audioSource.Play();
            StartMusic?.Invoke();
        }
        else
        {
            _audioSource.Stop();
            StopMusic?.Invoke();
        }
    }
}



