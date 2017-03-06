﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//class used to link enemies to minimap icons
public class EnemyToMap
{
    public bool instantiatedOnMiniMap;
    public GameObject imageRef, enemyRef;
    public Vector3 enemyPos;

    public EnemyToMap(GameObject _enemyRef)
    {    
        enemyRef = _enemyRef;
    }
}


public class MiniMap : MonoBehaviour
{
    public GameObject iconPool;
    public List<GameObject> iconPoolList;

    public ReferenceManager refManager;

    public GameObject enemyIconPrefab;
    public Transform minimapCenter;
    public List<EnemyToMap> enemyToMapList = new List<EnemyToMap>();
    Vector3 playerPos;

	void Start ()
    {
        gameplayPrefab = GameObject.FindGameObjectWithTag("GPrefab");
        refManager = GameObject.FindGameObjectWithTag("Reference").GetComponent<ReferenceManager>();
        iconPool = GameObject.Find("IconPool");
        iconPoolList = new List<GameObject>();

        foreach (var item in iconPool.GetComponentsInChildren<Image>(true))
        {
            iconPoolList.Add(item.gameObject);
        }
	}

    int pickCounter = 0;

    GameObject PickIconFromPool()
    {
        if (pickCounter > 148)
        {
            pickCounter = 0;
        }
        else
        {
            pickCounter++;
        }
        return iconPoolList[pickCounter];
    }

    //method to create enemy minimap icon when new enemy is spawned
    public void NewEnemy(GameObject go)
    {
        EnemyToMap newGo = new EnemyToMap(go);
        newGo.imageRef = PickIconFromPool();
        playerPos = new Vector3(refManager.playerObj.transform.position.x, 0, refManager.playerObj.transform.position.z);
        enemyToMapList.Add(newGo);      

        newGo.enemyPos = new Vector3(newGo.enemyRef.transform.position.x, 0, newGo.enemyRef.transform.position.z);
        Vector3 playerToEnemy = newGo.enemyPos - playerPos;

        //if player is into minimap range
        if (playerToEnemy.magnitude < 35)
        {
            newGo.imageRef.transform.position = minimapCenter.position + playerToEnemy.magnitude * 3.35f *
                new Vector3(Mathf.Cos(Mathf.Atan2(playerToEnemy.normalized.z, playerToEnemy.normalized.x) + gameplayPrefab.transform.eulerAngles.y),
                Mathf.Sin(Mathf.Atan2(playerToEnemy.normalized.z, playerToEnemy.normalized.x) + gameplayPrefab.transform.eulerAngles.y),
                0);
        }
        //if player is outside minimap range
        else
        {
            newGo.imageRef.transform.position = minimapCenter.position + 35 * 3.35f *
                new Vector3(Mathf.Cos(Mathf.Atan2(playerToEnemy.normalized.z, playerToEnemy.normalized.x) + gameplayPrefab.transform.eulerAngles.y),
                Mathf.Sin(Mathf.Atan2(playerToEnemy.normalized.z, playerToEnemy.normalized.x) + gameplayPrefab.transform.eulerAngles.y),
                0);
        }

        newGo.imageRef.transform.SetParent(this.transform);
        newGo.instantiatedOnMiniMap = true;

        //choose color of the icon to spawn
        switch (newGo.enemyRef.GetComponent<Enemy>().enemyType)
        {
            case "fante":
                newGo.imageRef.GetComponent<Image>().color = Color.green;
                break;
            case "furia":
                newGo.imageRef.GetComponent<Image>().color = new Color(255/255, 95/255, 3/255);
                break;
            case "furiaesplosiva":
                newGo.imageRef.GetComponent<Image>().color = Color.red;
                break;
            case "predatore":
                newGo.imageRef.GetComponent<Image>().color = Color.gray;
                break;
            case "cecchino":
                newGo.imageRef.GetComponent<Image>().color = Color.blue;
                break;
            case "titano":
                newGo.imageRef.GetComponent<Image>().color = Color.yellow;
                break;
            default:
                Debug.Log("Errore minimappa");
                break;
        }
        newGo.imageRef.SetActive(true);
    }

    public GameObject gameplayPrefab;

    //method to delete enemy minimap icon when enemy is killed
    public void DeleteEnemy(GameObject go)
    {
        EnemyToMap toDestroy = enemyToMapList.Find(x => x.enemyRef == go);
        toDestroy.imageRef.transform.SetParent(iconPool.transform);
        toDestroy.imageRef.SetActive(false);
        enemyToMapList.Remove(toDestroy);       
    }

    private void Update()
    {
        MiniMapUpdate();
    }

    void MiniMapUpdate()
        {
        //player position with y normalized to zero
        playerPos = new Vector3(refManager.playerObj.transform.position.x, 0, refManager.playerObj.transform.position.z);

        foreach (var enemy in enemyToMapList)
        {
            if (enemy.enemyRef)
            {
                //enemy position with y normalized to zero
                enemy.enemyPos = new Vector3(enemy.enemyRef.transform.position.x, 0, enemy.enemyRef.transform.position.z);

                //distance vector enemy to player
                Vector3 playerToEnemy = enemy.enemyPos - playerPos;

                //if player is into minimap range
                if (playerToEnemy.magnitude < 35)
                {
                    enemy.imageRef.transform.position = minimapCenter.position + playerToEnemy.magnitude * 3.35f *
                        new Vector3(Mathf.Cos(Mathf.Atan2(playerToEnemy.normalized.z, playerToEnemy.normalized.x) + gameplayPrefab.transform.eulerAngles.y),
                        Mathf.Sin(Mathf.Atan2(playerToEnemy.normalized.z, playerToEnemy.normalized.x) + gameplayPrefab.transform.eulerAngles.y),
                        0);
                }
                //if player is outside minimap range
                else
                {
                    enemy.imageRef.transform.position = minimapCenter.position + 35 * 3.35f *
                        new Vector3(Mathf.Cos(Mathf.Atan2(playerToEnemy.normalized.z, playerToEnemy.normalized.x) + gameplayPrefab.transform.eulerAngles.y),
                        Mathf.Sin(Mathf.Atan2(playerToEnemy.normalized.z, playerToEnemy.normalized.x) + gameplayPrefab.transform.eulerAngles.y),
                        0);
                }
            }
        }
	}
}
