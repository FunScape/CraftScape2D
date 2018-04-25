using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CSNetworkManager : NetworkManager {

    new string networkAddress { get { return "129.123.129.57"; } }

    void Awake()
    {
        base.networkAddress = this.networkAddress;
    }

}
