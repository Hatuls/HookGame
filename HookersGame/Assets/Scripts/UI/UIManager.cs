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
    [SerializeField] Image _backgroundImg;
    [SerializeField] float _opacity;
    [Space]
    [SerializeField] UIPallett _uIPallett;
    [SerializeField] AllLevels _allLevel;

    [Space]
    [SerializeField] PauseMenu _pauseMenu;
    [SerializeField] SettingsMenu _settingsMenuGO;
    [SerializeField] LevelSelectUIMenu _levelSelectMenu;
    [SerializeField] MainMenuUI _mainMenu;


    //Ron
    private List<IMenuHandler> menuList;

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

    private void SetbackGround(UIMenus currentMenus)
    {
        _backgroundImg.sprite = GetBackGroundImage(currentMenus);
        Color c = _backgroundImg.color;
        c.a = _opacity/100;
        _backgroundImg.color = c;
    }

    public ref AllLevels GetAllLevels =>ref _allLevel;
    public ref UIPallett GetPallett => ref _uIPallett;


    #endregion



    #region Monobehaiviour Callbacks
    private void Awake()
    {
        _instance = this;

    }

    private void Start()
    {
        Init();
        SceneHandlerSO.CurrentLevel= 0;
        SceneHandlerSO.HighestLevel =1;
    }
    #endregion

    #region Methods
    public void MoveToMenu(int index) => SetMenu = (UIMenus)index;
    public void Init()
    {
        //SetMenu = UIMenus.MainMenu;
        MenuByScene(SceneHandlerSO.CurrentLevel);
        _backgroundImg.sprite = GetBackGroundImage(currentMenus);
       // SetMenu = UIMenus.MainMenu;

        //Ron
        //InitMenuList();

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
    }

    //Ron
    //private void InitMenuList()
    //{
    //    menuList.Add(_mainMenu);
    //    menuList.Add(_pauseMenu);
    //    menuList.Add(_levelSelectMenu);
    //    menuList.Add(_settingsMenuGO);
    //}
     
    public void MenuByScene(int index)
    {
        if (index == 0)
        {
            //foreach(IMenuHandler found in menuList)
            //{
            //    found.GetGameObject().SetActive(false);
            //}

            SetMenu = UIMenus.MainMenu;

        }
        else { SetMenu = UIMenus.PauseMenu; }


    }

    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion
}

public interface IMenuHandler
{
    void Init(ref UIPallett uIPallett);
  void OnEnd();

    //GameObject GetGameObject();
}