using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoSingleton<NoteSpawner>
{
    [SerializeField] GameObject Note;
    [SerializeField] Vector2 startPosition;
    [SerializeField] float beat;

    [Range(1,3)]
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
    
    }


    void ShowNote()
    {
        NotePooling[index % HowManySquares].gameObject.SetActive(true);
        NotePooling[index % HowManySquares].ResetScale();
        NotePooling[index % HowManySquares].ToStart = true;
        index++;
    }

    private void OnDisable()
    {
        SoundManager.FullBeatEvent -= ShowNote;
    }
}
