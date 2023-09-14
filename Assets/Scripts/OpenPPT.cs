using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using yunqiLibrary;


public class OpenPPT : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    static int offset = 2;
    const uint SWP_SHOWWINDOW = 0x0040;
    const int GWL_STYLE = -16;  //边框用的
    const int WS_BORDER = 1;
    const int WS_POPUP = 0x800000;
    public bool isPPtIni = false;

    public Text ValueSheetisPPTPlayingText;
    public Text hWndText;
    private void Awake()
    {
        EventCenter.AddListener(EventDefine.popUpPPT, popupppt);
        EventCenter.AddListener(EventDefine.popUpVideo, popupVideo);

        EventCenter.AddListener(EventDefine.ini, ini);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void ini()
    {
        SendUPDData.instance.udp_Send(MediaCtr.instance.getScreenProtectUDP(), "127.0.0.1", 29010);


        StartCoroutine(iniPPT());

    }



    // Update is called once per frame
    void Update()
    {
        if (isPPtIni)
        {
            if (ValueSheet.isPPTPlaying)
            {
                SwitchToThisWindow(ValueSheet.PPThandler, true);
               // UnityEngine.Debug.Log("rUNNING");
            }
            else
            {
                IntPtr hWnd = FindWindow(null, ValueSheet.jsonBridge.information.ProgramName);
                hWndText.text = hWnd.ToString();
               SwitchToThisWindow(hWnd, true);    // 激活，显示在最前 
            }
        }
        ValueSheetisPPTPlayingText.text = ValueSheet.isPPTPlaying.ToString();
       
    }

    private void popupppt()
    {
        ValueSheet.isPPTPlaying = true;
    }

    private void popupVideo()
    {
        UnityEngine.Debug.Log("I am running");

        ValueSheet.isPPTPlaying = false;
        //StartCoroutine(clickToFront());
    }
    public IEnumerator iniPPT()
    {
        yield return new WaitForSeconds(10F);
        System.Diagnostics.Process.Start(ValueSheet.jsonBridge.information.PPTURL);
        yield return new WaitForSeconds(10F);


        IntPtr hWnd = FindWindow(null, ValueSheet.jsonBridge.information.PPTEditorhandler);
        SwitchToThisWindow(hWnd, true);    // 激活，显示在最前 
        yield return new WaitForSeconds(0.2f);

        //开始放映幻灯片
        VirtualKeyBoard.F5();
        yield return new WaitForSeconds(3f);
        //找到放映PPT句柄

        ValueSheet.PPThandler = GetForegroundWindow();

        bool result = SetWindowPos(ValueSheet.PPThandler, 0, ValueSheet.jsonBridge.information.xpos, ValueSheet.jsonBridge.information.ypos, ValueSheet.jsonBridge.information.screenwidth + offset, ValueSheet.jsonBridge.information.screenheight + offset, SWP_SHOWWINDOW);       //设置屏幕大小和位置
        SwitchToThisWindow(ValueSheet.PPThandler, true);

        yield return new WaitForSeconds(1f);
        //开始播放PPT
        ValueSheet.isPPTPlaying = true;
        isPPtIni = true;

        yield return new WaitForSeconds(1f);
        SendUPDData.instance.udp_Send(MediaCtr.instance.getScreenProtectUDP(), "127.0.0.1", 29010);

        EventCenter.Broadcast(EventDefine.popUpVideo);
    }


}
