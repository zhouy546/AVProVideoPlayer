using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using System.IO;

using LitJson;

using UnityEngine.UI;

public class ReadJson : MonoBehaviour {


    public static ReadJson instance;

  //  public  Ntext ntext;

    private string jsonString;


    public void Start()
    {
        StartCoroutine(initialization());
    }

    public IEnumerator initialization() {
        if (instance == null)
        {

            instance = this;

        }

     yield return   StartCoroutine(readJson());
    }

    IEnumerator readJson() {
        string spath = Application.streamingAssetsPath + "/information.json";

        Debug.Log(spath);

        WWW www = new WWW(spath);

        yield return www;

        jsonString = System.Text.Encoding.UTF8.GetString(www.bytes);


       ValueSheet.jsonBridge = JsonMapper.ToObject<Root>(jsonString.ToString());

        Debug.Log(ValueSheet.jsonBridge.information.ProgramName);

        Debug.Log(ValueSheet.jsonBridge.information.video[0].url);


        foreach (var item in ValueSheet.jsonBridge.information.video)
        {
            ValueSheet.udpVideoInfo_KP.Add(item.udp, item);
        }

        foreach (var item in ValueSheet.jsonBridge.information.image)
        {
            ValueSheet.udpImageInfo_kp.Add(item.udp, item);
        }

        EventCenter.Broadcast(EventDefine.ini);
    }
}
