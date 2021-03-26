using UnityEngine.UI;
using System.Collections;
using UnityEngine;

public class NoteIcon : MonoBehaviour
{
    [SerializeField] bool toStop;

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
        yield break;
        float beatTempo, minSuccessDelay =.8f,maxSuccessDelay = 1.1f, resetTimer = 1.2f;
        RectTransform childRect = transform.GetChild(0).GetComponent<RectTransform>();
       
        toStop = false;
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        while (SoundManager.Instance.GetGameStartingCondition)
        {
            if (toStop)
                break;

          
            // check if it supressed the 
            if (rt.localScale.magnitude > resetTimer)
            {
         //       Debug.Log("rt.localScale.magnitude " + rt.localScale.magnitude
         //+ "\n Time It Took  " + Time.time);
                ResetScale();
            }



            beatTempo = SoundManager.GetBeatPerSecond();
            ProceedColor = new Color(0, 0, 0, (beatTempo * Time.deltaTime) / beatTempo);


            img.color += ProceedColor;
            rt.localScale += new Vector3(beatTempo, beatTempo)/100f;
            //col.size = Vector2.MoveTowards(rt.localScale,new Vector2(100f,100f),beatTempo * Time.deltaTime);
            yield return  Time.deltaTime;

            
            
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
