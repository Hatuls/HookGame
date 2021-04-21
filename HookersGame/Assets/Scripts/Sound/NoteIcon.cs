using UnityEngine;
using UnityEngine.UI;

public class NoteIcon : MonoBehaviour
{
    [SerializeField] bool toStop;
    [SerializeField] float beatTempo;
    [SerializeField] Vector3 maxScale;
    RectTransform rt;
    Image img;
    float timer;
   

    #region MonoBehaiviour CallBacks
    private void Start()
    {
        rt = GetComponent<RectTransform>();
        img = transform.GetChild(0).GetComponent<Image>();

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
        ScaleUp();
    }
    #endregion
    public void ResetScale()
    {
        //timer = 0;
        ResetColor();
        rt.localScale = Vector3.zero;
    }
    private void PlayerAction(bool Successed)
        => ChangeColor(Successed ? Color.green : Color.red);
    private void ChangeColor(Color color)
    { img.color = color; }
    public void ResetColor()
  => ChangeColor(Color.white);
    private void ScaleUp()
    {
        if (!toStop)
        {
            //timer += Time.deltaTime;

            if (rt.localScale.magnitude < maxScale.magnitude)
            {
                rt.localScale = Vector3.Lerp(Vector3.zero, maxScale, SoundManager._currentTime / SoundManager.Instance.GetTimerMaxNote);
            }
            else
                ResetScale();
          
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
