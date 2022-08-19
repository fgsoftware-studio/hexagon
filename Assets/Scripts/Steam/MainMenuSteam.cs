using FMOD.Studio;
using FMODUnity;
using GameJolt.API;
using GameJolt.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class MainMenuSteam : MonoBehaviour
{
    public TMP_Text version;
    public TMP_Text integrity;

    private EventInstance button;
    private EventInstance music;

    private void Awake()
    {
        music = RuntimeManager.CreateInstance("event:/music/music");
        button = RuntimeManager.CreateInstance("event:/ui/button");
    }

    private void Start()
    {
        var Version = Application.version;

        version.text = Version;

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
    }

    public void ButtonPlay()
    {
        music.stop(STOP_MODE.IMMEDIATE);
        button.start();

        SceneManager.LoadScene("Game");
    }

    public void ButtonAchievements()
    {
        
    }

    public void ButtonLeaderboard()
    {
        
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