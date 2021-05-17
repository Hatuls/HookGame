using UnityEngine;
using UnityEngine.UI;

public class NoteIcon : MonoBehaviour
{
    [SerializeField] bool toStart;

    [Tooltip("Go through beats per seconds or beats in seconds")]
    [SerializeField] bool BPSorBIS;
    [SerializeField] Vector3 maxScale;
    RectTransform rt;

    [SerializeField]float alphaSpeed;
    [SerializeField] int id;

  [SerializeField]  Image img;
    Color clr;
    public int GetNoteIconID => id;

  

    public bool ToStart { get; set; }
    #region MonoBehaiviour CallBacks
    private void Start()
    {
        rt = GetComponent<RectTransform>();
         clr = img.color;

    }
    private void Update()
    {
        if (ToStart)
        {
            if (rt.localScale.x > 1)
                SetAlpha();
        }
        else
        {
            if (isActiveAndEnabled)
            {
                if (rt.localScale.x >= maxScale.x)
                {
                    LeanTween.cancel(rt);
                    gameObject.SetActive(false);
                }
            }
        }
            
        
    }
    #endregion
    public void ResetScale()
    {
        if (rt != null)
            rt.localScale = Vector3.zero;
        else
            rt = GetComponent<RectTransform>();


        img.color = Color.white;
    }


    public void ScaleUp()
    {
       
       LeanTween.cancel(this.rt); 
       LeanTween.scale(this.rt, Vector3.one*2, SoundManager.Instance.BeatSpeed*2);
    }


    void SetAlpha() {
        if(ToStart)
        {
        LeanTween.alpha(rt, 0, SoundManager.Instance.BeatSpeed / 2);
            ToStart = false;
        }
    }


  
}
