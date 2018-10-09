﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FireControls : NetworkBehaviour {

    public Transform firePoint;
    public GameObject projectile;
    public float launchForce = 100;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1"))
        {
            CmdFire();
        }
	}

    [Command]
    void CmdFire()
    {
        GameObject newProjectile = GameObject.Instantiate(projectile, firePoint.position, firePoint.rotation);
        newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * launchForce);
        newProjectile.GetComponent<PuffballController>().owningPlayer = this;

        NetworkServer.Spawn(newProjectile);

    }


    [ClientRpc]
    void RpcShowHit()
    {
        if (isLocalPlayer)
        {
            print(GetComponent<PlayerScore>().playerName + " got hit!");
        }
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if (!isServer) return;

        if (collision.gameObject.tag == "Projectile")
        {
            FireControls firingPlayer = collision.gameObject.GetComponent<PuffballController>().owningPlayer;
            firingPlayer.GetComponent<PlayerScore>().Score++;
            //.GetComponent<PlayerScore>().Score++;
            RpcShowHit();
        }
    }
}
