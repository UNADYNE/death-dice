using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class US_PlayerConnectionController : NetworkBehaviour
{
    public GameObject player;

    public Vector3 playerSpawnLocation;

    void Start()
    {
        if (!isLocalPlayer)
            return;
        playerSpawnLocation = new Vector3(0, .5f, 0);
        player = Instantiate(player, playerSpawnLocation, Quaternion.identity);
        NetworkServer.SpawnWithClientAuthority(player, connectionToClient);
        player.GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
    }

}
