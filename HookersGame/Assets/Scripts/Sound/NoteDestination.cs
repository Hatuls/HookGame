using UnityEngine.UI;
using UnityEngine;


public class NoteDestination : MonoBehaviour
{
    public static NoteDestination Instance;
    static RectTransform _rt;
    [SerializeField] Image img;
    
    public void Awake()
    {
        _rt = GetComponent<RectTransform>();
        SoundManager.OnBeatPressed += OnBeat;
        Instance = this;
    }
    private void OnDisable()
    {
        SoundManager.OnBeatPressed -= OnBeat;
        
    }
    private void ChangeColor(Color color)
    => img.color = color; 
    public void ResetColor()
  => ChangeColor(Color.white);
    public void OnBeat(bool correctBeat)
        => ChangeColor(correctBeat ? Color.green : Color.red);

}
