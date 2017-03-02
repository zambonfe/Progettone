﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class Player: MonoBehaviour
{
    public bool godMode = false;
    public bool isAlive = true;
    public bool tutorialMode = false;
    public bool dashTutorial = false;
    public bool tutorial = false;
    public bool stepTutorial = false;
    public bool noWeapons = false;

    public ReferenceManager refManager;
    Rigidbody playerRigidbody;
    private NavMeshAgent _navAgent;
    Tutorial tutorialElements;
    Animator anim;

    public Material occlusionMaterial;

    Vector3 movement;
    Vector3 shootDirection;

    bool primoSangue = true;
    public bool rotating;

    Ray dashRay;
    RaycastHit dashRayHit;

    float jumpHeight = 10;
    public float speed = 10f;

    bool saltoAttivo = true;
    public bool dashAttivo = true;
    public bool fireDialogue = true;

    private bool _traversingLink;

    public int maxHealth = 100;
    public int currentHealth = 100;
    public int rocketAmmo = 10;
    public float damageModifier = 1;

    public float[] costModifier = new float[4];
    public int[] baseCost = new int[4];

    public Ray occlusionRay;
    public RaycastHit[] occlusionHit;
    public Ray antiOcclusionRay;
    public RaycastHit[] antiOcclusionHit;
    public List<OccludedObject> occludedGoList = new List<OccludedObject>();
    GameObject[] allEnemies;
    Achievement achievement;
    UIController uiElements;
    AudioSource aSource;
    public AudioController aController;

    public float rx;
    public float ry;



    void Awake()
    {
        refManager = GameObject.FindGameObjectWithTag("Reference").GetComponent<ReferenceManager>();
        anim = GetComponentInChildren<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        tutorialElements = FindObjectOfType<Tutorial>();
        achievement = FindObjectOfType<Achievement>();
        uiElements = FindObjectOfType<UIController>();
        aSource = GetComponent<AudioSource>();
        aController = FindObjectOfType<AudioController>();
    }

    public void DestroyAllEnemies()
    {
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in allEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(200);
        }

        Debug.LogWarning("ci sono");
    }

    void OnTriggerStay(Collider coll)
    {
        if (coll.tag == "Health_PickUp")
        {
            //coll.gameObject.transform.GetChild(1).LookAt(Camera.main.transform);
            //coll.gameObject.transform.GetChild(1).Rotate(new Vector3(0, 180, 0));
            if (Input.GetButtonDown("Fire1"))
            {
                if (refManager.uicontroller.score >= baseCost[0] * costModifier[0] && currentHealth != maxHealth)
                {
                    aController.playSound(AudioContainer.Self.Health_PickUp);
                    refManager.uicontroller.score -= (int)(baseCost[0] * costModifier[0]);
                    costModifier[0] += 0.5f;
                    coll.gameObject.transform.GetChild(1).GetComponent<TextMesh>().text = "$" + baseCost[0] * costModifier[0];
                    refManager.uicontroller.UpdateScore();
                    currentHealth = maxHealth;
                    refManager.uicontroller.IncreaseLife();
                }
                else
                {
                    coll.gameObject.transform.GetChild(1).GetComponent<TextMesh>().color = Color.red;
                }
            }
        }

        if (coll.tag == "Ammo_PickUp")
        {
            //coll.gameObject.transform.GetChild(1).LookAt(Camera.main.transform);
            //coll.gameObject.transform.GetChild(1).Rotate(new Vector3(0, 180, 0));
            if (Input.GetButtonDown("Fire1"))
            {
          
                if (refManager.uicontroller.score >= baseCost[1] * costModifier[1] && rocketAmmo != 10)
                {
                    aController.playSound(AudioContainer.Self.Ammo_PickUp);
                    refManager.uicontroller.score -= (int)(baseCost[1] * costModifier[1]);
                    costModifier[1] += 0.5f;
                    coll.gameObject.transform.GetChild(1).GetComponent<TextMesh>().text = "$" + baseCost[1] * costModifier[1];
                    refManager.uicontroller.UpdateScore();
                    rocketAmmo += 5;
                    if (rocketAmmo > 10)
                    {
                        rocketAmmo = 10;
                    }
                    refManager.uicontroller.ammo.text = rocketAmmo.ToString();
                }
                else
                {
                    coll.gameObject.transform.GetChild(1).GetComponent<TextMesh>().color = Color.red;
                }
            }
        }

        if (coll.tag == "Weapon_PickUp")
        {
            //coll.gameObject.transform.GetChild(1).LookAt(Camera.main.transform);
            //coll.gameObject.transform.GetChild(1).Rotate(new Vector3(0, 180, 0));
            if (Input.GetButtonDown("Fire1"))
            {
                if (refManager.uicontroller.score >= baseCost[2] * costModifier[2])
                {
                    aController.playSound(AudioContainer.Self.Weapon_PickUp);
                    damageModifier += 0.25f;
                    refManager.uicontroller.UpdateWeaponUpgrade(25);
                    refManager.uicontroller.score -= (int)(baseCost[2] * costModifier[2]);
                    costModifier[2] += 0.5f;
                    coll.gameObject.transform.GetChild(1).GetComponent<TextMesh>().text = "$" + baseCost[2] * costModifier[2];
                    refManager.uicontroller.UpdateScore();
                    refManager.uicontroller.UpdateWeaponUpgrade(25);             
                }
                else
                {
                    coll.gameObject.transform.GetChild(1).GetComponent<TextMesh>().color = Color.red;
                }
            }  
        }

        if (coll.tag == "Armor_PickUp")
        {
            //coll.gameObject.transform.GetChild(1).LookAt(Camera.main.transform);
            //coll.gameObject.transform.GetChild(1).Rotate(new Vector3(0, 180, 0));
            if (Input.GetButtonDown("Fire1"))
            {
                if (refManager.uicontroller.score >= baseCost[3] * costModifier[3])
                {
                    aController.playSound(AudioContainer.Self.Armor_PickUp);
                    refManager.uicontroller.score -= (int)(baseCost[3] * costModifier[3]);
                    costModifier[2] += 0.5f;
                    coll.gameObject.transform.GetChild(1).GetComponent<TextMesh>().text = "$" + baseCost[3] * costModifier[3];
                    refManager.uicontroller.UpdateScore();
                    maxHealth += 25;
                    armorUpgrade += 25;
                    refManager.uicontroller.UpdateArmorUpgrade(25);
                    refManager.uicontroller.IncreaseLife();
                }
                else
                {
                    coll.gameObject.transform.GetChild(1).GetComponent<TextMesh>().color = Color.red;
                }
            }
        }
    }

    public int armorUpgrade = 0;

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Health_PickUp")
        {
            refManager.uicontroller.ShowPrompt();
            coll.GetComponent<PickUp>().Show();
            coll.gameObject.transform.GetChild(1).GetComponent<TextMesh>().text = "$" + baseCost[0] * costModifier[0];
        }

        if (coll.tag == "Ammo_PickUp")
        {
            refManager.uicontroller.ShowPrompt();
            coll.GetComponent<PickUp>().Show();
            coll.gameObject.transform.GetChild(1).GetComponent<TextMesh>().text = "$" + baseCost[1] * costModifier[1];
        }

        if (coll.tag == "Weapon_PickUp")
        {
            refManager.uicontroller.ShowPrompt();
            coll.GetComponent<PickUp>().Show();
            coll.gameObject.transform.GetChild(1).GetComponent<TextMesh>().text = "$" + baseCost[2] * costModifier[2];
        }

        if (coll.tag == "Armor_PickUp")
        {
            refManager.uicontroller.ShowPrompt();
            coll.GetComponent<PickUp>().Show();
            coll.gameObject.transform.GetChild(1).GetComponent<TextMesh>().text = "$" + baseCost[3] * costModifier[3];
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "Health_PickUp")
        {
            refManager.uicontroller.HidePrompt();
            coll.GetComponent<PickUp>().Hide(); 
        }

        if (coll.tag == "Ammo_PickUp")
        {
            refManager.uicontroller.HidePrompt();
            coll.GetComponent<PickUp>().Hide();
        }
        if (coll.tag == "Weapon_PickUp")
        {
            refManager.uicontroller.HidePrompt();
            coll.GetComponent<PickUp>().Hide();
        }

        if (coll.tag == "Armor_PickUp")
        {
            refManager.uicontroller.HidePrompt();
            coll.GetComponent<PickUp>().Hide();
        }
    }

    void FixedUpdate()
    {
        if (refManager.flyCamRef.endedCutScene && isAlive == true && tutorial == false)
        {

            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            rx = Input.GetAxisRaw("Horizontal_Stick");
            ry = Input.GetAxisRaw("Vertical_Stick");


            if (isDashing)
            {
                if (newRange >= 1)
                {
                    playerRigidbody.drag = 0;
                    playerRigidbody.AddForce(movement.normalized * 100 * Time.fixedDeltaTime, ForceMode.Impulse);
                    newRange--;
                }
            }
            else
            {
                newRange = dashRange;
                Move(h, v);

                //Step 1 Tutorial Movimento
                if (stepTutorial == true)
                {
                    stepTutorial = false;
                    tutorialElements.NextStep();
                }
            }


            
                if (((rx <= 0.15f && rx >= -0.15f) && (ry <= 0.15f && ry >= -0.15f)) || isDashing)
                {
                    rotating = false;
                    transform.rotation = lastRotation;
                }
                else
                {
                    rotating = true;
                    shootDirection = transform.parent.transform.right * Input.GetAxis("Horizontal_Stick") + transform.parent.transform.right * Input.GetAxis("Vertical_Stick");
                    shootDirection.x = rx * Mathf.Cos(Mathf.Deg2Rad * (transform.parent.transform.eulerAngles.y + 0)) + ry * Mathf.Sin(Mathf.Deg2Rad * (transform.parent.transform.eulerAngles.y + 0));
                    shootDirection.z = -rx * Mathf.Sin(Mathf.Deg2Rad * (transform.parent.transform.eulerAngles.y + 0)) + ry * Mathf.Cos(Mathf.Deg2Rad * (transform.parent.transform.eulerAngles.y + 0));
                    transform.rotation = Quaternion.LookRotation(shootDirection, Vector3.up);
                }
                lastRotation = transform.rotation;
            
            
        }      
    }

    public Quaternion lastRotation;

    public class OccludedObject
    {
        public Material[] matArray;
        public GameObject occludedObj;
        public MeshRenderer meshRef;
    }

    void DashTutorialMode()
    {
        //Step 2 Tutorial Dash
        if (dashTutorial == true)
        {
            dashTutorial = false;
            Debug.Log("sono qui");
            tutorialElements.NextStep();
        }
    }

    void Update()
    {
        

        if (Input.GetButtonDown("Selection"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetButtonDown("Previous Weapon"))
        {
            if (godMode == false)
            {
                godMode = true;
                refManager.uicontroller.GodModeOn();
            }

            else if (godMode == true)
            {
                godMode = false;
                refManager.uicontroller.GodModeOff();
            }

        }

        //ritorno al Menu con il Tasto Start (Fire2)
        if (Input.GetButtonDown("X"))        {
            achievement.SaveScore(uiElements.score);
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DestroyAllEnemies();
        }


        if (refManager.flyCamRef.endedCutScene)
        {
            occlusionRay = new Ray(this.transform.position, Camera.main.transform.position - this.transform.position);

            occlusionHit = Physics.RaycastAll(occlusionRay);
            foreach (var mesh in occlusionHit)
            {
                if (mesh.collider.gameObject.tag != "MainCamera" && mesh.collider.gameObject.tag != "Player" && mesh.collider.gameObject.tag != "ringhiera" && mesh.collider.tag != "Destructible" && mesh.collider.tag != "Enemy" && mesh.collider.tag != "Terrain" && mesh.collider.GetComponent<MeshRenderer>())
                {
                    if (mesh.collider.gameObject != this)
                    {
                        int counter = 0;
                        foreach (var diobubu in occludedGoList)
                        {                         
                            if (diobubu.occludedObj == mesh.collider.gameObject)
                            {
                                counter++;
                            }
                        }
                        if (counter == 0)
                        {
                            StartCoroutine(LerpAlpha(mesh.collider.gameObject, 1));

                            StartCoroutine(StillOccluding(mesh.collider.gameObject));
                        }
                        counter = 0;
                    }
                }
            }

            if (Input.GetButton("Dash") && dashAttivo == true)
            {
                
                DashTutorialMode();
                StartCoroutine(Dash());

            }
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.transform.tag == "notWalkable")
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }
    }

    public BoxCollider groundTrigger;
    public bool isGrounded = true;
    public GameObject map;

    void Move(float h, float v)
    {
        Vector3 animationVector = new Vector3();
        if (isGrounded)
        {
            movement.Set(h, 0f, v);

            movement.x = h * Mathf.Cos(Mathf.Deg2Rad * (transform.parent.transform.eulerAngles.y + 0)) + v * Mathf.Sin(Mathf.Deg2Rad * (transform.parent.transform.eulerAngles.y + 0));
            movement.z = -h * Mathf.Sin(Mathf.Deg2Rad * (transform.parent.transform.eulerAngles.y + 0)) + v * Mathf.Cos(Mathf.Deg2Rad * (transform.parent.transform.eulerAngles.y + 0));

            animationVector.x = h * Mathf.Cos(Mathf.Deg2Rad * (-transform.localEulerAngles.y)) + v * Mathf.Sin(Mathf.Deg2Rad * (-transform.localEulerAngles.y));
            animationVector.z = -h * Mathf.Sin(Mathf.Deg2Rad * (-transform.localEulerAngles.y)) + v * Mathf.Cos(Mathf.Deg2Rad * (-transform.localEulerAngles.y));
            animationVector.Normalize();

            Animating(animationVector);

            movement = movement.normalized * speed * Time.fixedDeltaTime;
            playerRigidbody.MovePosition(transform.position + movement);

        }
    }

    void Animating(Vector3 animMovement)
    {
        anim.SetFloat("Forward", animMovement.z);
        anim.SetFloat("Lateral", animMovement.x);
    }

    public float dashRange = 250;
    public float newRange;
    public bool isDashing = false;

    IEnumerator Dash()
    {
        aSource.clip = AudioContainer.Self.Dash;
        aSource.Play();

        isDashing = true;
        dashRay.origin = transform.position;
        dashRay.direction = movement.normalized;

        TrailRenderer trailRef = GetComponentInChildren<TrailRenderer>();
        dashAttivo = false;

        trailRef.enabled = true;

        float newRange;

        if (Physics.Raycast(dashRay, out dashRayHit, dashRange))
        {
            newRange = dashRayHit.distance;
        }
        else
        {
            newRange = dashRange;
        }

        trailRef.time = 0.1f;
        yield return new WaitForSeconds(0.1f);

        yield return new WaitForSeconds(0.2f);
        isDashing = false;
        playerRigidbody.drag = 5;
        trailRef.Clear();
        trailRef.enabled = false;
        yield return new WaitForSeconds(0.3f);
        playerRigidbody.drag = 1;
        yield return new WaitForSeconds(0.3f);
        dashAttivo = true;
        newRange = dashRange;
    }

    public void TakeDamage(float damageTaken)
    {
        /* if (primoSangue == true)
         {
             //primoSangue = false;
             //dialoghi.SetDialogue(1);
         }*/

        if (godMode == false)
        {
            currentHealth -= (int)damageTaken;
            refManager.uicontroller.DecrementLife((float)damageTaken / 100);
        }
        

        if (currentHealth <= 0 && isAlive == true && godMode == false)
        {
            isAlive = false;
            StartCoroutine(Die());
        }
        
    }

    IEnumerator Die()
    {
        refManager.uicontroller.GameOverOn();
        yield return null;     
    }

    

    public IEnumerator StillOccluding(GameObject go)
    {
        bool found = false;
        yield return new WaitForSeconds(0.5f);

        antiOcclusionRay = new Ray(this.transform.position, Camera.main.transform.position - this.transform.position);
        antiOcclusionHit = Physics.RaycastAll(antiOcclusionRay);

        foreach (var mesh in antiOcclusionHit)
        {
            if (mesh.collider.gameObject.tag != "Player")
            {
                if (mesh.collider.gameObject == go)
                {
                    found = true;
                    StartCoroutine(StillOccluding(go));
                }
            }
        }
        if (!found)
        {
            StartCoroutine(LerpAlpha(go, -1));
        }
    }

    public IEnumerator LerpAlpha(GameObject go, int sign)
    {
        if (go != this.gameObject && go.tag != "Enemy")
        {
            if (sign > 0)
            {
               // Debug.LogError("occluso");
                occludedGoList.Add(new OccludedObject());
                occludedGoList[occludedGoList.Count - 1].occludedObj = go;
                occludedGoList[occludedGoList.Count - 1].meshRef = go.GetComponent<MeshRenderer>();
                occludedGoList[occludedGoList.Count-1].matArray = occludedGoList[occludedGoList.Count - 1].meshRef.materials;

                for (int i = 0; i < occludedGoList[occludedGoList.Count - 1].meshRef.materials.Length; i++)
                {
                    occludedGoList[occludedGoList.Count - 1].meshRef.materials[i].SetFloat("_Mode", 2);
                    occludedGoList[occludedGoList.Count - 1].meshRef.materials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    occludedGoList[occludedGoList.Count - 1].meshRef.materials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    occludedGoList[occludedGoList.Count - 1].meshRef.materials[i].SetInt("_ZWrite", 0);
                    occludedGoList[occludedGoList.Count - 1].meshRef.materials[i].DisableKeyword("_ALPHATEST_ON");
                    occludedGoList[occludedGoList.Count - 1].meshRef.materials[i].EnableKeyword("_ALPHABLEND_ON");
                    occludedGoList[occludedGoList.Count - 1].meshRef.materials[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    occludedGoList[occludedGoList.Count - 1].meshRef.materials[i].renderQueue = 3000;
                }

                while (occludedGoList[occludedGoList.Count - 1].meshRef.material.color.a > 0.4f)
                {
                    occludedGoList[occludedGoList.Count - 1].meshRef.material.color += new Color(0, 0, 0, -3 * Time.deltaTime);
                    yield return null;
                }
            }
            else
            {
                while (occludedGoList[occludedGoList.Count - 1].meshRef.material.color.a < 1f)
                {
                    occludedGoList[occludedGoList.Count - 1].meshRef.material.color += new Color(0, 0, 0, 3 * Time.deltaTime);
                    yield return null;
                }
                for (int i = 0; i < occludedGoList[occludedGoList.Count - 1].meshRef.materials.Length; i++)
                {
                    occludedGoList[occludedGoList.Count - 1].meshRef.materials[i] = occludedGoList[occludedGoList.Count - 1].matArray[i];
                }

                occludedGoList.Remove(occludedGoList[occludedGoList.Count - 1]);
            }
        }
    }
}