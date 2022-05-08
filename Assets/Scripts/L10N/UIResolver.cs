using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResolver : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<LangResolver>().ResolveTexts();
    }
}