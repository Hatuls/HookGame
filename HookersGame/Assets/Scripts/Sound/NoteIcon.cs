using UnityEngine.UI;
using System.Collections;
using UnityEngine;

public class NoteIcon : MonoBehaviour
{
    [SerializeField] bool toStop;
  [SerializeField] float beatTempo, minSuccessDelay = .95f, maxSuccessDelay = 1.05f, resetTimer = 1.2f;
    RectTransform rt;
    Image img;
    private void Start()
    {
        
        rt = GetComponent<RectTransform>();
         
        img = transform.GetChild(0).GetComponent<Image>();

        SubscribeHandler(true);
    }

    public void ResetScale()
    {
        rt.localScale = Vector3.zero;
      //  img.color -= Color.black;
    }
 
    
    public  void StopMoving()
    {
        toStop = true;
        StopCoroutine(MovingRythemGUI());
        ResetScale();
    }
    public  void StartMoving()
    {
        StopMoving();
       StartCoroutine(MovingRythemGUI());
    }


    IEnumerator MovingRythemGUI()
    {
        Color  ProceedColor;
        
      
        RectTransform childRect = transform.GetChild(0).GetComponent<RectTransform>();
       
        toStop = false;
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        while (SoundManager.Instance.GetGameStartingCondition)
        {
            if (toStop)
                break;

            NoteDestination.canBePressed = false;
            if (rt.localScale.x <= maxSuccessDelay && rt.localScale.x >= minSuccessDelay)
            {
                //Debug.Log("Time It Took To Complete 1 Beat " + Time.time);
                NoteDestination.canBePressed = true;
            }
            else if (rt.localScale.x >= resetTimer)
            {
                
                ResetScale();
            }



            beatTempo = SoundManager.GetBeatPerSecond();
            ProceedColor = new Color(0, 0, 0, (beatTempo * Time.deltaTime) / beatTempo);


            img.color += ProceedColor;
            rt.localScale += new Vector3(beatTempo, beatTempo,1)*Time.deltaTime ;
            col.size = rt.localScale * 100f;
            yield return  null;

            
            
            { 
            /*
             //   tweens[0] = LeanTween.scale(rt,Vector3.one, beatTempo).setEase(OptionOfTween);
                //tweens[1] = LeanTween.alpha(childRect, 1, beatTempo / adjustingCache).setEase(LeanTweenType.easeInExpo);
                //for (int i = 0; i < sections; i++)
                //{
                //    if (toStop)
                //    {
                //        for (int z = 0;z < tweens.Length;z++)
                //        {
                //            LeanTween.pause(tweens[z].id);
                //            LeanTween.cancel(tweens[z].uniqueId);
                //        }

                //        ResetScale();
                //        yield break;
                //    }
                //    yield return new WaitForSeconds(beatTempo / sections);
                //}

                //img.color = clr;
                //ResetScale();
            */
        }
        }
        StopMoving();

    }
    void SubscribeHandler(bool toSubscribeOrToUn) {
        if (toSubscribeOrToUn)
        {
            SoundManager.StartMusic += StartMoving;
            SoundManager.StopMusic += StopMoving;
        }
        else
        {
            SoundManager.StartMusic -= StartMoving;
            SoundManager.StopMusic -= StopMoving;
        }
    
    }
    private void OnDestroy()
    {
        SubscribeHandler(false);
    }
    private void OnDisable()
    {
        SubscribeHandler(false);
    }
}
