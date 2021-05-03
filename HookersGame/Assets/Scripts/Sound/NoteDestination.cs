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
      
        Instance = this;
    }

    private void ChangeColor(Color color)
    { img.color = color; }
    public void ResetColor()
  => ChangeColor(Color.white);

    public  void OnCorrectBeatSynced()
 => ChangeColor(Color.green);
    public  void OnWrongBeatSynced()
     => ChangeColor(Color.red);
}
