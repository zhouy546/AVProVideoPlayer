using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendUDPOpenDoor : MonoBehaviour
{

    public static SendUDPOpenDoor instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendOpenDoor()
    {
        SendUPDData.instance.udp_Send("open", "192.168.1.11", 29010);
        Debug.Log("开门");
    }
}
