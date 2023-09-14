using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WelcomWordsCtr : MonoBehaviour
{
    public Image image;
    public Text text;

    // Update is called once per frame
    public void Start()
    {
        EventCenter.AddListener(EventDefine.ini, INI);

        EventCenter.AddListener(EventDefine.WelcomeWordsOn, Show);

        EventCenter.AddListener(EventDefine.WelcomeWordsOff, Hide);

        EventCenter.AddListener<string>(EventDefine.ShowImage,(s)=> { EventCenter.Broadcast(EventDefine.WelcomeWordsOff); } );

        EventCenter.AddListener<string>(EventDefine.SwitchVideo, (s) => { EventCenter.Broadcast(EventDefine.WelcomeWordsOff); });
    }
    public void INI()
    {
        text.text = ValueSheet.jsonBridge.information.WelcomeWord;
    }

    public void Show()
    {
        image.enabled = true;
        text.enabled = true;
    }

    public void Hide()
    {
        image.enabled = false;
        text.enabled = false;
    }
    public void Hide(string s)
    {
        image.enabled = false;
        text.enabled = false;
    }
}
