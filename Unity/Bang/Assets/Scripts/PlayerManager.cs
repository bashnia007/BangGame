using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public GameObject DropZone;
    public GameObject PlayerArea;

    public override void OnStartClient()
    {
        base.OnStartClient();

        PlayerArea = GameObject.Find("PlayerTablet");
        DropZone = GameObject.Find("DropZone");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
