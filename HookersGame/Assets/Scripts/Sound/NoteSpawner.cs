using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoSingleton<NoteSpawner>
{
    [SerializeField] GameObject Note;
    [SerializeField] Vector2 startPosition;
    [SerializeField] float beat;
    float timer;
    public override void Init()
    {
        beat = (60f / SoundManager.GetBeat) * 2f;
    }


    private void Update()
    {
        if (timer > beat)
        {
            Debug.Log("Boom");
            timer -= beat;
        }
        timer += Time.deltaTime;
    }
}
