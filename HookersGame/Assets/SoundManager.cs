
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{

    [SerializeField] SongSO[] songs;
  static  SongSO currentSong;
    [SerializeField] bool gameStarted;

   public bool isCorrectTiming
        => noteDestination.canBePressed
        || CheatMenu.Instance.GetUnLimitedCompressor;

    
    AudioSource _audioSource;
    NoteDestination noteDestination;


    public delegate void SoundEvent();
    public static event SoundEvent StartMusic;
    public static event SoundEvent StopMusic;

    public bool GetGameStartingCondition => gameStarted;
    public override void Init()
    {
        _audioSource = GetComponent<AudioSource>();
        noteDestination = FindObjectOfType<NoteDestination>();
        currentSong = songs[0];
    }
    // Start is called before the first frame update
    public static float GetBeatPerSecond()
        => currentSong.GetBPM / 60f;
    private void Start()
    {
        currentSong = songs[0];
        StartMusic?.Invoke();
    }
    public void StartPlayingSong() {
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
