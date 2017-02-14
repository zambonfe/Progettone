﻿using UnityEngine;
using System.Collections;

public class RangedEnemyFire : MonoBehaviour
{
    public Transform weapon;
    public GameObject bulletPrefab;
    public bool isShooting;
    ReferenceManager refManager;

    RaycastHit losRayHit;

    public float speed = 10;

    bool sparo = true;

    void Start()
    {

        refManager = GameObject.FindGameObjectWithTag("Reference").GetComponent<ReferenceManager>();
    }

    public void Awake()
    {
        pool = GameObject.Find("FanteParticlePool");
        id = transform.GetSiblingIndex();
        myParticle = pool.transform.GetChild(id);
        myEffect = myParticle.GetComponentsInChildren<EffectSettings>(true)[0];
        transformTr = myParticle.GetComponentsInChildren<Transform>(true)[1];
    }

    public void Update()
    {
        this.transform.LookAt(refManager.playerObj.transform);
    }

    public IEnumerator Shooting()
    {
        isShooting = true;
        if (Physics.Linecast(weapon.transform.position, refManager.playerObj.transform.position, out losRayHit))
        {
            if (losRayHit.collider.gameObject.tag == "Player" && Vector3.Distance(refManager.playerObj.transform.position, this.transform.position) < 15)
            {
                ParticleActivator(refManager.playerObj.transform.FindChild("Head").position);
            }
        }
        yield return new WaitForSeconds(1.5f);
        isShooting = false;

        if (GetComponent<RangedEnemy>().hPoints < 0)
        {
            StopAllCoroutines();
        }
    }

    public GameObject pool;
    public Transform transformTr;
    Transform myParticle;
    int id;
    EffectSettings myEffect;

    public void ParticleActivator(Vector3 position)
    {     
        transformTr.position = position;
        myEffect.transform.position = weapon.transform.position;
        myEffect.Target = transformTr.gameObject;
        myEffect.gameObject.SetActive(true);
    }
}
