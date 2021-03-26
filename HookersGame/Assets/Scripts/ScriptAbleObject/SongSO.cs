using UnityEngine;

[CreateAssetMenu(fileName = "Song", menuName = "ScriptableObjects/Songs", order = 1)]
public class SongSO : ScriptableObject
{
    [SerializeField] AudioClip _song;
    [SerializeField] float beatPerMinute;
    public float GetBPM => beatPerMinute;
    public AudioClip GetSong => _song;
}