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

  [SerializeField]  Image img;
    Color clr;


  

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

                    gameObject.SetActive(false);
                }
            }
        }
            
        
    }
    #endregion
    public void ResetScale()
    {
        if (rt != null)
        {
            rt.localScale = Vector3.zero;
        img.color = Color.white;
        }
        else
        {
            rt = GetComponent<RectTransform>();
            ResetScale();
        }


    }


    public void ScaleUp()
    {
       LeanTween.scale(this.rt, Vector3.one*2, SoundManager.Instance.BeatSpeed*2);
    }


    void SetAlpha() {
        if(ToStart)
        {
        LeanTween.alpha(rt, 0, SoundManager.Instance.BeatSpeed /2);
            ToStart = false;
        }
    }


  
}
