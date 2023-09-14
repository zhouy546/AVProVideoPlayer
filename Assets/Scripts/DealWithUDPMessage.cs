
//*********************❤*********************
// 
// 文件名（File Name）：	DealWithUDPMessage.cs
// 
// 作者（Author）：			LoveNeon
// 
// 创建时间（CreateTime）：	Don't Care
// 
// 说明（Description）：	接受到消息之后会传给我，然后我进行处理
// 
//*********************❤*********************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

public class DealWithUDPMessage : MonoBehaviour {


    public static DealWithUDPMessage instance;
    // public GameObject wellMesh;
    private string dataTest;

    //private static bool isInScreenProtect=true;


    //public LogoWellCtr logoWellCtr;
    //private bool enterTrigger, exitTrigger;
    /// <summary>
    /// 消息处理
    /// </summary>
    /// <param name="_data"></param>
    public void MessageManage(string _data)
    {
        if (_data != "")
        {

            dataTest = _data;

            Debug.Log(dataTest);
            if (ValueSheet.isPPTPlaying)
            {
                if (dataTest == ValueSheet.jsonBridge.information.pptNextUDP)
                {
                    EventCenter.Broadcast(EventDefine.PressRightArrow);
                }
                else if (dataTest == ValueSheet.jsonBridge.information.pptPerviousUDP)
                {
                    EventCenter.Broadcast(EventDefine.PressLeftArrow);
                }
                else if (dataTest == ValueSheet.jsonBridge.information.popupPPTUDP)
                {
                    EventCenter.Broadcast(EventDefine.popUpVideo);
                }
            }
            else
            {
                if (ValueSheet.udpVideoInfo_KP.ContainsKey(dataTest))
                {
                    //EventCenter.Broadcast(EventDefine.popUpVideo);
                    EventCenter.Broadcast(EventDefine.SwitchVideo, dataTest);
                }
                else if (ValueSheet.jsonBridge.information.WelcomWordUDP==dataTest)
                {
                    EventCenter.Broadcast(EventDefine.WelcomeWordsOn);
                }

                else if (ValueSheet.udpImageInfo_kp.ContainsKey(dataTest))
                {
                    //EventCenter.Broadcast(EventDefine.popUpVideo);
                    EventCenter.Broadcast(EventDefine.ShowImage, dataTest);
                }

                else if (ValueSheet.jsonBridge.information.volumedown == dataTest)
                {
                    EventCenter.Broadcast(EventDefine.volumedown);
                }
                else if (ValueSheet.jsonBridge.information.volumeup == dataTest)
                {
                    EventCenter.Broadcast(EventDefine.volumeup);
                }
                else if (ValueSheet.jsonBridge.information.pause == dataTest)
                {
                    EventCenter.Broadcast(EventDefine.pause);
                }
                else if (ValueSheet.jsonBridge.information.fastforward == dataTest)
                {
                    EventCenter.Broadcast(EventDefine.fastforward);
                }
                else if (ValueSheet.jsonBridge.information.fastreverse == dataTest)
                {
                    EventCenter.Broadcast(EventDefine.fastreverse);
                }
                else if (ValueSheet.jsonBridge.information.mute == dataTest)
                {
                    EventCenter.Broadcast(EventDefine.mute);
                }
                else if (dataTest == ValueSheet.jsonBridge.information.popupPPTUDP)
                {
                    EventCenter.Broadcast(EventDefine.popUpPPT);
                }
            }
          
            

        }

    }



    private void Awake()
    {

    }

    public IEnumerator Initialization() {
        if (instance == null)
        {
            instance = this;
        }
        yield return new  WaitForSeconds(0.01f);
    }

    public void Start()
    {

    }


    private void Update()
    {


        //Debug.Log("数据：" + dataTest);  
    }

}
