﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject doorHinge1;
    public GameObject doorHinge2;
    public GameObject door1;
    public GameObject door2;
    public Image healthBar;
    public GameObject healthToken;
    public GameObject invincibilityToken;
    public Light roomLight;
    public GameObject weapon;
    public GameObject projectile;
    public GameObject enemy;
    public GameObject slider;
    private bool invincible;
    private bool toRight = true;
    private bool dark = true;
    private int collided = 0;
    private int moveSpeed = 4;
    private int minDist = 3;

     public void SetHealthBarValue(float value)
    {
        healthBar.fillAmount = value;
        if(healthBar.fillAmount < 0.2f)
        {
            SetHealthBarColor(Color.red);
        }
        else if(healthBar.fillAmount < 0.4f)
        {
            SetHealthBarColor(Color.yellow);
        }
        else
        {
            SetHealthBarColor(Color.green);
        }
    }

    public float GetHealthBarValue()
    {
        return healthBar.fillAmount;
    }
 
    /// <summary>
    /// Sets the health bar color
    /// </summary>
    /// <param name="healthColor">Color </param>
    public void SetHealthBarColor(Color healthColor)
    {
        healthBar.color = healthColor;
    }

    void Start()
    {
        this.SetHealthBarValue(1.0f);
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    void OnCollisionEnter(Collision col)
    {   
        List<string> firstFourLevels = new List<string>();
        firstFourLevels.Add("Scene 1");
        firstFourLevels.Add("Scene 2");
        firstFourLevels.Add("Scene 3");
        firstFourLevels.Add("Scene 4");
        if(firstFourLevels.Contains(SceneManager.GetActiveScene().name))
        {
            if (col.gameObject.name == "Floor 2")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        if (col.gameObject.name == "Health token")
        {
            this.SetHealthBarValue(1.0f);
            healthToken.SetActive(false);
            weapon.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (col.gameObject.name == "Invincibility token")
        {
            invincible = true;
            weapon.GetComponent<Renderer>().material.color = Color.yellow;
            invincibilityToken.SetActive(false);
            StartCoroutine(SetFalse());
        }
        
        if(SceneManager.GetActiveScene().name == "Scene 5")
        {
            if (col.gameObject.name == "Door 1" | col.gameObject.name == "Door 2")
            {
                collided = 1;
            }

            /*if (col.gameObject.name == "Floor 2")
            {
                GetComponent<PlayerInteraction>().enabled = false;
            }*/
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.name == "Health token")
            {
                weapon.GetComponent<Renderer>().material.color = Color.grey;
            }
        if(SceneManager.GetActiveScene().name == "Scene 5")
        {
            if (col.gameObject.name == "Door 1" | col.gameObject.name == "Door 2")
            {
                collided = 2;
            }
        }
    }

    IEnumerator SetFalse()
    {
        yield return new WaitForSeconds(10); //wait 10 seconds
        invincible = false;
        weapon.GetComponent<Renderer>().material.color = Color.grey;
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Scene 2")
        {
            if (toRight)
            {
                enemy.transform.Translate(-Time.deltaTime, 0, 0, Space.World);
                if (enemy.transform.position.x <= -7.91)
                {
                    toRight = false;
                }
            }
            else
            {
                enemy.transform.Translate(Time.deltaTime, 0, 0, Space.World);
                if (enemy.transform.position.x >= -2.26)
                {
                    toRight = true;
                }
            }
            if(Vector3.Distance(enemy.transform.position,this.transform.position) <= minDist)
            {
                    this.SetHealthBarValue(this.GetHealthBarValue() - 0.003f);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Scene 3")
        {
            if(Vector3.Distance(enemy.transform.position,door1.transform.position) >= 4)
            {
                enemy.transform.LookAt(this.transform);
                if(Vector3.Distance(enemy.transform.position,this.transform.position) > minDist)
                {
                    enemy.transform.position += enemy.transform.forward*moveSpeed*Time.deltaTime;
                }
            }
            if(Vector3.Distance(enemy.transform.position,this.transform.position) <= minDist & !invincible)
            {
                this.SetHealthBarValue(this.GetHealthBarValue() - 0.003f);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Scene 4")
        {
            if (this.transform.position.y <= 20)
            {
                this.SetHealthBarValue(this.GetHealthBarValue() - 1.0f);
            }
        }
        else if(SceneManager.GetActiveScene().name == "Scene 5")
        {
            healthToken.transform.Rotate(1,0,0);
            invincibilityToken.transform.Rotate(1,0,0);

            if (collided == 1 & doorHinge1.transform.rotation.eulerAngles.y <= 28)
            {
                doorHinge1.transform.Rotate(0,1.5f,0);
                door1.GetComponent<Renderer>().material.color = Color.green;
                doorHinge2.transform.Rotate(0,-1.5f,0);
                door2.GetComponent<Renderer>().material.color = Color.green;
            }
            if (collided == 2 & doorHinge1.transform.rotation.eulerAngles.y >= 28)
            {
                doorHinge1.transform.Rotate(0,-24,0);
                door1.GetComponent<Renderer>().material.color = Color.blue;
                doorHinge2.transform.Rotate(0,24,0);
                door2.GetComponent<Renderer>().material.color = Color.blue;
            }
            if(doorHinge1.transform.rotation.eulerAngles.y <= 4)
            {
                collided = 0;
                door1.GetComponent<Renderer>().material.color = Color.blue;
                door2.GetComponent<Renderer>().material.color = Color.blue;
            }
            if(roomLight.intensity >= 5.89f)
            {
                if (dark)
                {
                    weapon.SetActive(true);
                    projectile.SetActive(true);
                    slider.SetActive(true);
                }
                dark = false;
                enemy.transform.LookAt(this.transform);
                if(Vector3.Distance(enemy.transform.position,this.transform.position) > minDist)
                {
                    enemy.transform.position += enemy.transform.forward*moveSpeed*Time.deltaTime;
                }
                if(Vector3.Distance(enemy.transform.position,this.transform.position) <= minDist & !invincible)
                {
                    this.SetHealthBarValue(this.GetHealthBarValue() - 0.003f);
                }
            }
        }
        
    }
}