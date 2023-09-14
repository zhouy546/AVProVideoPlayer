using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class MediaCtr : MonoBehaviour
{
    public MediaPlayer mediaPlayer;

    public VideoItem currentVideoInfo;

    public static MediaCtr instance;

    public DisplayUGUI displayUGUI;

    public Coroutine coroutine;
    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        EventCenter.AddListener(EventDefine.ini, INI);


    }

    // Update is called once per frame
    void Update()
    {
    }

    void INI() {
        EventCenter.AddListener<string>(EventDefine.SwitchVideo, playVideo);

        EventCenter.AddListener(EventDefine.volumedown, volumedown);

        EventCenter.AddListener(EventDefine.volumeup, volumeup);

        EventCenter.AddListener(EventDefine.pause, pause);

        EventCenter.AddListener(EventDefine.fastforward, fastforward);

        EventCenter.AddListener(EventDefine.fastreverse, fastreverse);

        EventCenter.AddListener(EventDefine.mute, mute);

        EventCenter.AddListener<string>(EventDefine.ShowImage, onImageShow);


        mediaPlayer.Events.AddListener(listening);

        EventCenter.Broadcast(EventDefine.SwitchVideo, getScreenProtectUDP());


        EventCenter.AddListener(EventDefine.popUpPPT, SetmuteTrue);

        EventCenter.AddListener(EventDefine.popUpVideo, SetmuteFalse);

        EventCenter.AddListener(EventDefine.popUpVideo, () =>{ playVideo(getScreenProtectUDP()); });
    }

    private void onImageShow(string s)
    {
        mediaPlayer.Stop();
        displayUGUI.color = new Color(1, 1, 1, 0);
    }

    public string getScreenProtectUDP()
    {
        foreach (var item in ValueSheet.jsonBridge.information.video)
        {
            if (item.iscreenprotect)
            {
                return item.udp;
            }
        }

        return "2001";
    }

    public void fastforward()
    {
        float targetMillSeconds = mediaPlayer.Control.GetCurrentTimeMs() + 15000;

        targetMillSeconds =Mathf.Clamp(targetMillSeconds, 0, mediaPlayer.Info.GetDurationMs());

        mediaPlayer.Control.Seek(targetMillSeconds);
    }

    public void fastreverse()
    {
        float targetMillSeconds = mediaPlayer.Control.GetCurrentTimeMs() - 15000;

        targetMillSeconds = Mathf.Clamp(targetMillSeconds, 0, mediaPlayer.Info.GetDurationMs());

        mediaPlayer.Control.Seek(targetMillSeconds);
    }


    public void TurnLightOn()
    {
        StartCoroutine(turnLightsOnIEn());


    }

    private IEnumerator turnLightsOnIEn()
    {
        foreach (var item in ValueSheet.jsonBridge.information.lightOn)
        {
            Debug.Log("开灯指令" + item);
            yield return new WaitForSeconds(0.1f);
            SendUPDData.instance.udp_Send(item, ValueSheet.jsonBridge.information.LightServer, ValueSheet.jsonBridge.information.lightServerPort);

        }
        Debug.Log("开灯");
    }


    public void TurnLightOff()
    {
        StartCoroutine(turnLightsOffIEn());
    }

    private IEnumerator turnLightsOffIEn() {
        foreach (var item in ValueSheet.jsonBridge.information.lightOff)
        {
            yield return new WaitForSeconds(0.1f);
            SendUPDData.instance.udp_Send(item, ValueSheet.jsonBridge.information.LightServer, ValueSheet.jsonBridge.information.lightServerPort);

            Debug.Log("关灯指令" + item);
        }
        Debug.Log("影片开始关灯");
    }

    public void playVideo(string udp)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        displayUGUI.color = new Color(1, 1, 1,1);

        string url = ValueSheet.udpVideoInfo_KP[udp].url;
        bool isloop = ValueSheet.udpVideoInfo_KP[udp].loop;

        mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, url, false);

        currentVideoInfo = ValueSheet.udpVideoInfo_KP[udp];
        mediaPlayer.Control.SetLooping(isloop);
        mediaPlayer.Play();

    }

    public IEnumerator OnVideFinished()
    {
        //SendUPDData.instance.udp_Send(ValueSheet.jsonBridge.information.image[0].udp, "127.0.0.1", 29010);

        yield return new WaitForSeconds(0.1f);

        SendUPDData.instance.udp_Send(getScreenProtectUDP(), "127.0.0.1", 29010);
    }

    public void listening(MediaPlayer mediaPlayer, MediaPlayerEvent.EventType et, ErrorCode errorCode)
    {
        if (et == MediaPlayerEvent.EventType.Started)
        {
            if (currentVideoInfo.iscreenprotect==true)
            {
                TurnLightOn();
            }
            else
            {
                TurnLightOff();
            }

        }else if(et == MediaPlayerEvent.EventType.FinishedPlaying)
        {
            if (currentVideoInfo.iscreenprotect==false)
            {

                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
              
                coroutine =StartCoroutine(OnVideFinished());
                
                Debug.Log("影片结束开灯");
            }

        }
    }

    public void volumeup() {
        Debug.Log("VolumeUp");

        float volume = mediaPlayer.Control.GetVolume();

        volume = Mathf.Clamp01(volume + 0.1f);

        mediaPlayer.Control.SetVolume(volume);
    }

    public void volumedown() {
        Debug.Log("VolumeDown");

        float volume = mediaPlayer.Control.GetVolume();

        volume = Mathf.Clamp01(volume - 0.1f);

        mediaPlayer.Control.SetVolume(volume);
    }

    public void pause()
    {
        if (mediaPlayer.Control.IsPaused())
        {
            mediaPlayer.Control.Play();
        }
        else
        {
            mediaPlayer.Control.Pause();
        }
    }

    private void SetmuteTrue()
    {
        mediaPlayer.Control.MuteAudio(true);
    }

    private void SetmuteFalse()
    {
        mediaPlayer.Control.MuteAudio(false);
    }

    public void mute()
    {
        if (mediaPlayer.Control.IsMuted())
        {
            mediaPlayer.Control.MuteAudio(false);
        }
        else
        {
            mediaPlayer.Control.MuteAudio(true);
        }
    }

}
