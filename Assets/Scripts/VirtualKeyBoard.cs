﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public  class VirtualKeyBoard :MonoBehaviour
{
    [DllImport("user32.dll", EntryPoint = "keybd_event")]

    public static extern void Keybd_event(

    byte bvk,//虚拟键值 ESC键对应的是27

    byte bScan,//0

    int dwFlags,//0为按下，1按住，2释放

    int dwExtraInfo//0

    );

    public void Awake()
    {
        EventCenter.AddListener(EventDefine.PressLeftArrow, LeftArrow);
        EventCenter.AddListener(EventDefine.PressRightArrow, RightArrow);
    }

    public void Update()
    {
        
    }
    public static void F5()
    {
        Keybd_event(116, 0, 1, 0);
    }

    public static void RightArrow()
    {
        Keybd_event(39, 0, 1, 0);
    }

    public static void LeftArrow()
    {
        Keybd_event(37, 0, 1, 0);
    }

}
