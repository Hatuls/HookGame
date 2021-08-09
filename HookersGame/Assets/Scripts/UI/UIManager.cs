using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum UIMenus { MainMenu = 0 , SettingsMenu = 1, LevelMenu = 2,PauseMenu =3};
public class UIManager : MonoBehaviour
{
    #region Fields
    private static UIManager _instance;
    [SerializeField] UIMenus currentMenus = UIMenus.MainMenu;
    [Space]
    [SerializeField] CanvasScaler _canvas;
    [SerializeField] Image _backgroundImg;
    [SerializeField] float _opacity;
    [Space]
    [SerializeField] GameObject _innerGameMenu;
    [SerializeField] GameObject _mainMenuContainer;
    [SerializeField] UIPallett _uIPallett;
    [SerializeField] AllLevels _allLevel;

    [Space]
    [SerializeField] WinMenu _winMenu;
    [SerializeField] PauseMenu _pauseMenu;
    [SerializeField] SettingsMenu _settingsMenuGO;
    [SerializeField] LevelSelectUIMenu _levelSelectMenu;
    [SerializeField] MainMenuUI _mainMenu;


    //Vector2 mainMenuResulution = new Vector2(1920, 1080);
    //Vector2 inGameMenuResulution = new Vector2(800, 1080);

    //Ron
    private List<IMenuHandler> menuList;
    public PauseMenu PauseMenu => _pauseMenu;
    #endregion


    #region Properties

    public static UIManager Instance
    {
        get
        {
            return _instance;
        }
    }
    private UIMenus SetMenu
    {
        set
        {
            if (currentMenus != value)
            {

        
                ChangeMenus(currentMenus , value);
                 currentMenus = value;
                SetbackGround(currentMenus);
              
            }
        }
    }
    public void ResetMenu()
    {
        _pauseMenu.gameObject.SetActive(false);
        _winMenu.gameObject.SetActive(false);
        ClosePauseMenu();

        _mainMenuContainer.gameObject.SetActive(true);
        SetMenu = UIMenus.MainMenu;
        EnumToGameObject(UIMenus.LevelMenu).OnEnd();
    }
    private void SetbackGround(UIMenus currentMenus)
    {
        _backgroundImg.sprite = GetBackGroundImage(currentMenus);
        Color c = _backgroundImg.color;
        c.a = _opacity/100;
        _backgroundImg.color = c;
    }

    internal void OpenPauseMenu()
    {
  //      _canvas.referenceResolution = inGameMenuResulution;
        InnerContainer(true);
        _pauseMenu.OpenPauseMenu();
    }

    public void ClosePauseMenu()
    {
   //     _canvas.referenceResolution = mainMenuResulution;
        InnerContainer(false);
        Time.timeScale = 1;
    }

    public void OpenWinMenu()
    {
        InnerContainer(true);
       _winMenu.OpenMenu();
    }
    internal void InnerContainer(bool value)
    {
      _innerGameMenu.SetActive(value);
    }

    public ref AllLevels GetAllLevels =>ref _allLevel;
    public ref UIPallett GetPallett => ref _uIPallett;


    #endregion



    #region Monobehaiviour Callbacks
    private void Awake()
    {
        if (_instance == null)
        _instance = this;
        else if (_instance!= this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Init();

        SceneHandlerSO.HighestLevel =1;
    }
    #endregion

    #region Methods
    public void MoveToMenu(int index)
    { 
        SetMenu = (UIMenus)index; 

    
    }

    public void Init()
    {

        _backgroundImg.sprite = GetBackGroundImage(currentMenus);
        // SetMenu = UIMenus.MainMenu;
        SetMenu = UIMenus.MainMenu;

    }
    private void ChangeMenus(UIMenus currentMenus, UIMenus newMenu)
    {
        EnumToGameObject(currentMenus).OnEnd();
        EnumToGameObject(newMenu).Init(ref _uIPallett);
    }
    private IMenuHandler EnumToGameObject(UIMenus menu)
    {

        switch (menu)
        {

            case UIMenus.SettingsMenu:
                return _settingsMenuGO;

            case UIMenus.LevelMenu:
                return _levelSelectMenu;

            case UIMenus.PauseMenu:
                return _pauseMenu;

            default:
            case UIMenus.MainMenu:
                return _mainMenu;

        }
  
    }
    private Sprite GetBackGroundImage(UIMenus currentMenus)
    {
        switch (currentMenus)
        {
            case UIMenus.MainMenu:
                return _uIPallett.GetSprite(UISprite.MainMenuBackground);


    case UIMenus.SettingsMenu:
            case UIMenus.LevelMenu:
            case UIMenus.PauseMenu:
                return _uIPallett.GetSprite(UISprite.EmptyMainMenuBackground);
            

            default:
                break;
        }
        return null;
    }

    internal LevelSO GetLevelData(int index)
    {
        return _allLevel.GetLevel(index);
    }

    internal void LevelSelected(int index)
    {
        if (index < 0)
            return;

        SceneHandlerSO.LoadScene((ScenesName)(index+1));
        _mainMenuContainer.gameObject.SetActive(false);
    }



    #endregion

    public void ExitGame()
        => Application.Quit();
}

public interface IMenuHandler
{
    void Init(ref UIPallett uIPallett);
  void OnEnd();

    //GameObject GetGameObject();
}