
using UnityEngine;

public class NoteSpawner : MonoSingleton<NoteSpawner>
{
    [SerializeField] GameObject Note;
    [SerializeField] Vector2 startPosition;
    [SerializeField] float beat;

    [Range(1,4)]
    [SerializeField] int HowManySquares=1;
    NoteIcon[] NotePooling;
    float timer;
    public static int GetTimerForNote => Instance.HowManySquares;
    int index;
    private void Awake()
    {
        SoundManager.FullBeatEvent += ShowNote; 
    }
    public override void Init()
    {
        if (!isActiveAndEnabled)
            return;


        beat = SoundManager.GetBeat;

        index = 0;


        NotePooling = FindObjectsOfType<NoteIcon>();

        if (NotePooling == null)
            Debug.LogError("Didnt Find notes On scene");
        else 
        for (int i = 0; i < NotePooling.Length; i++)
                NotePooling[i].gameObject.SetActive(false);
        previousSquares = HowManySquares;
    }

    int previousSquares;
    void ShowNote()
    {
        if (previousSquares != HowManySquares)
        {
            for (int i = 0; i < NotePooling.Length; i++)
                if (NotePooling[i].gameObject.activeInHierarchy)
                NotePooling[i].gameObject.SetActive(false);

            previousSquares = HowManySquares;
        }

        index++;
        NotePooling[index % HowManySquares].gameObject.SetActive(true);
        NotePooling[index % HowManySquares].ResetScale();
        NotePooling[index % HowManySquares].ToStart = true;
        NotePooling[index % HowManySquares].ScaleUp();
        NoteDestination.Instance.ResetColor();
    }

    private void OnDisable()
    {
        SoundManager.FullBeatEvent -= ShowNote;
    }
}
