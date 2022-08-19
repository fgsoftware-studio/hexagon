using System;
using System.Linq;
using Steamworks;
using TMPro;
using UnityEngine;

public class SteamInit : MonoBehaviour
{
    public TMP_Text user;
    private void Start()
    {
        try
        {
            SteamClient.Init(480);
        }
        catch (Exception e)
        {
            Debug.Log("error");
        }

        user.text = SteamClient.Name;
        Debug.Log(SteamClient.Name);
        SteamApps.InstallDlc(480);
    }

    private void Update()
    {
        SteamClient.RunCallbacks();
    }
}