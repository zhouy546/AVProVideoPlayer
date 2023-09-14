using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ValueSheet 
{

    public static Root jsonBridge;

    public static Dictionary<string, VideoItem> udpVideoInfo_KP = new Dictionary<string, VideoItem>();
    public static Dictionary<string, imageItem> udpImageInfo_kp = new Dictionary<string, imageItem>();

    public static bool isPPTPlaying;

    public static IntPtr PPThandler;




}


public class VideoItem
{

    public string udp;

    public bool loop; 

    public string url;

    public bool iscreenprotect; 
}

public class imageItem
{

    public string udp;

    public string url;

}

public class Information
{
    public List<VideoItem> video = new List<VideoItem>();

    public List<imageItem> image = new List<imageItem>();

    public string PPTURL; //= "D:\\pptTest.pptx";

    public string PPTEditorhandler; //= "pptTest.pptx - PowerPoint";

    public string mute;

    public string pause;

    public string fastforward;

    public string fastreverse;

    public string volumeup;

    public string volumedown;

    public string pptNextUDP;

    public string pptPerviousUDP;

    public string popupPPTUDP;

    public string WelcomeWord;

    public string WelcomWordUDP;

    public int xpos;

    public int ypos;

    public int screenwidth;

    public int screenheight; 

    public string ProgramName;

    public string LightServer;

    public int lightServerPort;

    public List<string> lightOn = new List<string>();

    public List<string> lightOff = new List<string>();
}

public class Root
{
    public Information information;
}