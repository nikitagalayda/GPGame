using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ProjectileGenerator : MonoBehaviourPunCallbacks
{
    public GameObject bulletPrefab;
    public void GenerateTheBullet(Vector3 currentMouseVector, Vector3 spawnPosition, float preRunningDistance){
        Vector2 shootDirection = (Vector2)Vector3.Normalize((Vector2)currentMouseVector - (Vector2)spawnPosition);                  
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, (Vector2)spawnPosition + preRunningDistance * shootDirection, Quaternion.identity);
        var pjc = bullet.GetComponent<ProjectileController>();
        if(pjc != null){
            pjc.parentObject = this.gameObject;
            pjc.tg = shootDirection;            
        }
    }
}
