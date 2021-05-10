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
    private void OnEnable()
    {
        SubscribeHandler(true);
    }
    private void OnDisable()
    {
        SubscribeHandler(false);
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
<<<<<<< HEAD
        if (rt != null)
            rt.localScale = Vector3.zero;
        else
            rt = GetComponent<RectTransform>();


        img.color = Color.white;
=======
       // beatTempo = 0;
        rt.localScale = Vector3.zero;
        ResetColor();
   
>>>>>>> parent of 5446647 (spawning tower better)
    }
    private void PlayerAction(bool Successed)
    {
        //if (Successed)
        //    NoteDestination.OnCorrectBeatSynced();
        //else
        //    NoteDestination.OnWrongBeatSynced();
    }
<<<<<<< HEAD

    public void ScaleUp()
    {
       
       LeanTween.cancel(this.rt); 
       LeanTween.scale(this.rt, Vector3.one*2, SoundManager.Instance.BeatSpeed*2);
=======
    private void ChangeColor(Color color)
    { img.color = color; }
    public void ResetColor()
  => ChangeColor(Color.white);
    private void ScaleUp()
    {

     //   rt.localScale = Vector3.Lerp(Vector3.zero, maxScale, beatTempo / SoundManager.GetBeatPerSecond());
      //  rt.localScale += Vector3.one * ((SoundManager._currentTime / SoundManager.Instance.GetTimerMaxNote)*NoteSpawner.GetTimerForNote) * Time.deltaTime;
        rt.localScale +=Vector3.one *(BPSorBIS?SoundManager.GetBeatAmountInSeconds(): SoundManager.GetBeatAmountInSeconds() )*Time.deltaTime;
            if (rt.localScale.magnitude>= maxScale.magnitude)
            SetAlpha();

     //   beatTempo += Time.deltaTime;
>>>>>>> parent of 5446647 (spawning tower better)
    }


    void SetAlpha() {
        if(ToStart)
        {
        LeanTween.alpha(rt, 0, SoundManager.Instance.BeatSpeed / 2);
            ToStart = false;
        }


       
        


    }
    private void SubscribeHandler(bool toSubscribeOrToUn) {
        if (toSubscribeOrToUn)
        {
            SoundManager.OnBeatPressed += PlayerAction;
        }
        else
        {
            SoundManager.OnBeatPressed -= PlayerAction;
        }
    
    }

  
}
