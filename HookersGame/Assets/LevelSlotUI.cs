using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
[System.Serializable]
public class ResetScaleEvent : UnityEngine.Events.UnityEvent<int> { }

public enum LevelSlotState { Locked, Open , Completed}
public class LevelSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler,IPointerExitHandler
{
    LevelSlotState _levelSlotState;
    [SerializeField] int index;


    [SerializeField]ResetScaleEvent _resetScaleEvent;
    
    [SerializeField]
    Image _lockImage;

    [SerializeField]
    BasePart _basePart;

    [Space]

    [SerializeField]   
    CompleteIcon _completeIcon;

    [SerializeField]
    BottomPart _bottomPart;

    [SerializeField]
    ImageAndText _levelNumber;

    public void SetLevelNumberText(int index)
        => _levelNumber.Text.text = (index+1).ToString();
    public LevelSlotState LevelSlotState
    {
        get => _levelSlotState;
        set
        {
            if (_levelSlotState != value)
            {
                _levelSlotState = value;
                ActivateState();
            }

        }
    }
    private void Start()
    {
        ActivateState();
    }
    private void ActivateState()
    {
        switch (_levelSlotState)
        {
            case LevelSlotState.Locked:
                ActivateLockedState();
                break;
            case LevelSlotState.Open:
                ActivateOpenState();
                break;
            case LevelSlotState.Completed:
                ActivateCompleteState();
                break;
            default:
                break;
        }

    }
   
    private void ActivateOpenState()
    {
        UIPallett palleta = UIManager.Instance.GetPallett;
        LevelSO _level = UIManager.Instance.GetLevelData(index);

        _lockImage.gameObject.SetActive(false);


        // icon on the top right
        _completeIcon.CheckMark.SetActive(false);
        _completeIcon.OutLine.color = palleta.GetColorFrom(UIPallettColor.Sapphir);
        _completeIcon.BackGround.color = Color.white;

        //text
        _levelNumber.Image.sprite = palleta.GetSprite( UISprite.UnlockTextScoreImage);
        _levelNumber.Text.color = Color.white;

        //base
        _basePart.Background.color = Color.white;
        _basePart.Background.sprite = _level.LevelImage;

        _basePart.BackgroundOutLine.color = palleta.GetColorFrom(UIPallettColor.Sapphir);
        _basePart.Line.color = palleta.GetColorFrom(UIPallettColor.Sapphir);
        _basePart.Misgeret.color = palleta.GetColorFrom(UIPallettColor.Purple);


        //bottom Part
        _bottomPart.StatusText.Text.gameObject.SetActive(true);
        _bottomPart.TimeScore.Text.gameObject.SetActive(false);
        _bottomPart.StatusText.Text.fontSize = 22f;
        _bottomPart.StatusText.Text.text = "Lets GO!";

        _bottomPart.TimeScore.Image.sprite = palleta.GetSprite(UISprite.UnlockBackgroundScore);
        _bottomPart.StatusText.Image.sprite = palleta.GetSprite(UISprite.UnlockBackgroundFeedback);

    }

    private void ActivateLockedState()
    {
        UIPallett palleta = UIManager.Instance.GetPallett;

        _lockImage.gameObject.SetActive(true);


        // icon on the top right
        _completeIcon.CheckMark.SetActive(false);
        _completeIcon.OutLine.color = palleta.GetColorFrom(UIPallettColor.Orange);
        _completeIcon.BackGround.color = palleta.GetColorFrom(UIPallettColor.Dark_Purple);

        //text
        _levelNumber.Image.sprite = palleta.GetSprite( UISprite.lockTextScoreImage);
        _levelNumber.Text.color = palleta.GetColorFrom(UIPallettColor.Grey);

        //base
        _basePart.Background.color = palleta.GetColorFrom(UIPallettColor.Grey);
        _basePart.Background.sprite = palleta.GetSprite(UISprite.levelBackgroundDefault);
        _basePart.BackgroundOutLine.color = palleta.GetColorFrom(UIPallettColor.Orange);
        _basePart.Line.color = palleta.GetColorFrom(UIPallettColor.Orange);
        _basePart.Misgeret.color =  palleta.GetColorFrom(UIPallettColor.Orange);


        //bottom Part
        _bottomPart.StatusText.Text.gameObject.SetActive(false);
        _bottomPart.TimeScore.Text.gameObject.SetActive(false);
        _bottomPart.TimeScore.Image.sprite = palleta.GetSprite(UISprite.LockedBackgroundScore);
        _bottomPart.StatusText.Image.sprite = palleta.GetSprite(UISprite.LockedBackgroundFeedback);
   
    }

    private void ActivateCompleteState()
    {
        UIPallett palleta = UIManager.Instance.GetPallett;
        LevelSO _level = UIManager.Instance.GetLevelData(index);

        _lockImage.gameObject.SetActive(false);

        // icon on the top right
        _completeIcon.CheckMark.SetActive(true);
        _completeIcon.OutLine.color = palleta.GetColorFrom(UIPallettColor.Sapphir);
        _completeIcon.BackGround.color = Color.white;

        //text
        _levelNumber.Image.sprite = palleta.GetSprite(UISprite.UnlockTextScoreImage);
        _levelNumber.Text.color = Color.white;

        //base
        _basePart.Background.color = Color.white;
        _basePart.Background.sprite = _level.LevelImage;
       
        _basePart.BackgroundOutLine.color = palleta.GetColorFrom(UIPallettColor.Sapphir);
        _basePart.Line.color = palleta.GetColorFrom(UIPallettColor.Purple);
        _basePart.Misgeret.color = palleta.GetColorFrom(UIPallettColor.Purple);


        //bottom Part
        _bottomPart.StatusText.Text.gameObject.SetActive(true);
        _bottomPart.TimeScore.Text.gameObject.SetActive(true);

        _bottomPart.TimeScore.Text.text = _level.timeFinished.ToString();
        _bottomPart.StatusText.Text.fontSize = 15f;
        _bottomPart.StatusText.Text.text = "Completed!";

        _bottomPart.TimeScore.Image.sprite = palleta.GetSprite(UISprite.UnlockBackgroundScore);
        _bottomPart.StatusText.Image.sprite = palleta.GetSprite(UISprite.UnlockBackgroundFeedback);


    }



    bool isOverObject = false;
    public void Entrance(float time, LeanTweenType _transition)
    {
       transform.localScale = Vector3.zero;

        gameObject.SetActive(true);

        LeanTween.scale(gameObject, Vector3.one * 1.3f, time).setEase(_transition).setOnComplete(
       () => LeanTween.scale(gameObject, Vector3.one, time).setEase(_transition));
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        UIManager.Instance.LevelSelected(_levelSlotState == LevelSlotState.Locked ? -1 :index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isOverObject == false)
        {
            LeanTween.scale(gameObject,Vector3.one * 1.2f, 0.2f);
            isOverObject = true;
            _resetScaleEvent?.Invoke(index);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetScale();
    }
    public void ResetScale()
    {
            LeanTween.scale(gameObject, Vector3.one, 0.08f);


            isOverObject = false;
    }
    #region classes
    [System.Serializable]
    private class CompleteIcon
    {
        public Image BackGround;
        public Image OutLine;
        public GameObject CheckMark;
    }

    [System.Serializable]
    private class BottomPart
    {
        public ImageAndText TimeScore;
        public ImageAndText StatusText;
    }

    [System.Serializable]
    private class BasePart
    {
        public Image Background;
        public Image BackgroundOutLine;
        public Image Line;
        public Image Misgeret;
    }


    [System.Serializable]
    private class ImageAndText
    {
        public Image Image;
        public TextMeshProUGUI Text;
    }
    #endregion
}