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

    public int GetNoteIconID => id;

  

    public bool ToStart { get; set; }
    #region MonoBehaiviour CallBacks
    private void Start()
    {
        rt = GetComponent<RectTransform>();
        

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
        if (!toStart)
            ScaleUp();
    }
    #endregion
    public void ResetScale()
    {
        beatTempo = 0;
        rt.localScale = Vector3.zero;
        ResetColor();
   
    }
    private void PlayerAction(bool Successed)
    {
        if (rt.localScale.magnitude > Vector3.one.magnitude)
        {

        }
      ChangeColor(Successed ? Color.green : Color.red);
    }
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

        beatTempo += Time.deltaTime;
    }

    void SetAlpha() {

        Color clr = img.color;
        img.color = new Color(clr.r, clr.g, clr.b, clr.a - alphaSpeed * Time.deltaTime);
        if (rt.localScale.magnitude > (maxScale* SoundManager.Instance.BeatSpeed).magnitude)
        {

             ToStart = false;
            gameObject.SetActive(false) ;
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
