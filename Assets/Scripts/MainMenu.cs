using FMOD.Studio;
using FMODUnity;
using GameJolt.API;
using GameJolt.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class MainMenu : MonoBehaviour
{
    public TMP_Text version;
    public TMP_Text platform;
    public TMP_Text integrity;
    public GameObject playercolor;

    private EventInstance button;
    private EventInstance music;

    private void Awake()
    {
        music = RuntimeManager.CreateInstance("event:/music/music");
        button = RuntimeManager.CreateInstance("event:/ui/button");
    }

    private void Start()
    {
        var isSignedIn = GameJoltAPI.Instance.CurrentUser != null;
        var Version = Application.version;
        var Platform = Application.platform.ToString();

        version.text = Version;
        platform.text = Platform;

        switch (Application.genuine)
        {
            case true:
                integrity.text = "official";
                break;
            case false:
                integrity.text = "modded";
                break;
        }

        music.start();

        if (isSignedIn)
        {
            Debug.Log("user signed in");
            Trophies.Unlock(137920, success =>
            {
                if (success)
                    Debug.Log("trophie aquired");
                else
                    Debug.Log("user not signed");
            });
        }
    }

    public void ButtonPlay()
    {
        music.stop(STOP_MODE.IMMEDIATE);
        button.start();

        SceneManager.LoadScene("Game");
    }

    public void ButtonAchievements()
    {
        var isSignedIn = GameJoltAPI.Instance.CurrentUser != null;

        music.stop(STOP_MODE.ALLOWFADEOUT);
        button.start();
        if (isSignedIn)
            GameJoltUI.Instance.ShowTrophies();
        else if (isSignedIn == false) GameJoltUI.Instance.ShowSignIn();
    }

    public void ButtonLeaderboard()
    {
        var isSignedIn = GameJoltAPI.Instance.CurrentUser != null;

        music.stop(STOP_MODE.ALLOWFADEOUT);
        button.start();

        if (isSignedIn)
            GameJoltUI.Instance.ShowLeaderboards();
        else
            GameJoltUI.Instance.ShowSignIn();
    }

    public void ButtonSettings()
    {
        music.stop(STOP_MODE.ALLOWFADEOUT);
        button.start();
        SceneManager.LoadScene("Settings");
    }

    public void ButtonQuit()
    {
        music.stop(STOP_MODE.ALLOWFADEOUT);
        button.start();
        Application.Quit();
    }
}