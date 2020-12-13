using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// - has health that recharges after certain amount of time they dont take damage
/// - while active they must be destroyed before player can attack enemy
/// - will not impact enemy collisions, only reads player bullets (everything else is ignored)
/// </summary>
public class ForceFieldsEnemy : MonoBehaviour
{
    public float maxHealth = 60;
    public float currentHealth = 60;
    public bool inactive = false;
    public bool ableToRecharge = false;

    //as health is lower, sheild oppacity is lower
    GameObject _sheild;
    public float trans;
    public Color col;
    public float sheildRegenTick = 5;

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Sets the initial variables and starts the regen function
    /// </summary>
    void Start()
    {
        _sheild = this.gameObject;
        col = _sheild.GetComponent<Renderer>().material.color;
        //recharges slowly every second after a time where enemy is damaged
        InvokeRepeating("SheildRegen", 0f, 1f);

    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// sets alpha of material based on its health (every frame)
    /// </summary>
    void Update()
    {
        trans = (currentHealth / maxHealth) * 0.5f;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            this.GetComponent<MeshRenderer>().enabled = false;
            inactive = true;
        }
        else
            this.GetComponent<MeshRenderer>().enabled = true;

        col.a = trans;
        _sheild.GetComponent<Renderer>().material.color = col;

    }
    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// regen function that regens health based on if there is health to update and if amount of time has passed
    /// </summary>
    void SheildRegen()
    {
        if(ableToRecharge && inactive)
        {
            if(currentHealth < maxHealth)
            {
                currentHealth += sheildRegenTick;
            }
        }
    }

}
