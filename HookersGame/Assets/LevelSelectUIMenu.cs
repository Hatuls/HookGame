
using UnityEngine;

public class LevelSelectUIMenu : MonoBehaviour , IMenuHandler
{
    [SerializeField] LeanTweenType _transition = LeanTweenType.easeInOutQuint;
    [SerializeField] float _startDelay;
    [SerializeField] float _timeDifferenceBetween;
    [SerializeField] LevelSlotUI[] _levels;


    

    public void Init(ref UIPallett uIPallett)
    {
        gameObject.SetActive(true);
        

        if (SceneHandlerSO.CurrentLevel != UIManager.Instance.GetAllLevels.GetMaxLevelUnlocked)
            UpdateLevelsUI();

        TransitionEnter();
    }

    private void Start()
    {
        
        UpdateLevelsUI();
    }
    private void UpdateLevelsUI()
    {
        AllLevels levelso = UIManager.Instance.GetAllLevels;

        for (int i = 0; i < _levels.Length; i++)
        {
           
            if (i < levelso.GetMaxLevelUnlocked)
                _levels[i].LevelSlotState = LevelSlotState.Completed;
            else if (i == levelso.GetMaxLevelUnlocked)
                _levels[i].LevelSlotState = LevelSlotState.Open;
            else
                _levels[i].LevelSlotState = LevelSlotState.Locked;

            _levels[i].SetLevelNumberText(i);
        }
    }

    public void OnEnd()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < _levels.Length; i++)
        {
            _levels[i].gameObject.SetActive(false);

        }
    }

    void TransitionEnter() {
        for (int i = 0; i < _levels.Length; i++)
        {
            _levels[i].transform.localScale = Vector3.zero;
           _levels[i].gameObject.SetActive(true);
            LeanTween.scale(_levels[i].gameObject, Vector3.one, _startDelay + i * _timeDifferenceBetween).setEase(_transition);
        }
    }

    public void ResetScales(int index) {
        for (int i = 0; i < _levels.Length; i++)
        {
            if (i != index)
                _levels[i].ResetScale();
        }
    }
}
