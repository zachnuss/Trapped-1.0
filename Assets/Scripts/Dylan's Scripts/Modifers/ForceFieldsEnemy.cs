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

    // Start is called before the first frame update
    void Start()
    {
        _sheild = this.gameObject;
        col = _sheild.GetComponent<Renderer>().material.color;
        //recharges slowly every second after a time where enemy is damaged
        InvokeRepeating("SheildRegen", 0f, 1f);

        
    }

    // Update is called once per frame
    void Update()
    {
        trans = (currentHealth / maxHealth) * 0.5f;
        //Debug.Log(currentHealth / maxHealth);
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            inactive = true;
        }

        if(inactive)
        {
           // _sheild.GetComponent<SphereCollider>().isTrigger = false;
          //  _sheild.GetComponent<SphereCollider>().enabled = false;
        }
        else
        {
           // _sheild.GetComponent<SphereCollider>().isTrigger = true;
           // _sheild.GetComponent<SphereCollider>().enabled = true;
        }

        col.a = trans;
        _sheild.GetComponent<Renderer>().material.color = col;

        //SheildRegen();
    }

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
