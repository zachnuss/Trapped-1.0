/*
 * PlayerParticles.cs
 * 
 * Author: Christian Mullins
 * Summary: Handles functions and storing of effects, defined functions will
 *          be called from other scripts
 */
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerParticles : MonoBehaviour {
    //public particles
    public GameObject bleedParticle;
    public GameObject dustParticle;
    
    //private variables to be set
    private float dustLifeCycle = 0.9f;
    private float dustFrequency = 0.6f;
    //private helpers (dust)
    private LinkedList<GameObject> _dustList;
    private Transform _dustPar;
    private bool _isKickingDust;
    private float _dustLifeTimer;
    private float _dustKickTimer;
    //miscellaneous private helpers
    private ParticleSystem _bleedPS;
    private PlayerMovement _pMovement;

    void Start() {
        //initialize values
        _dustList = new LinkedList<GameObject>();
        _dustLifeTimer = 0f;
        _dustKickTimer = 0f;
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; ++i) {
            if (transform.GetChild(i).name == "PlayerRotate") {
                _dustPar = transform.GetChild(i);
                break;
            }
        }
        _pMovement = GetComponent<PlayerMovement>();
    }

    //set particles when necessary bools are met
    void FixedUpdate() {
        //adjust frequency with rigidbody movement
        float relativeSpeed = _getRelativeSpeed();
        float dynamicDustFreq = dustFrequency * (1f - relativeSpeed);
        float dynamicDustLife = dustLifeCycle * relativeSpeed;
        //Debug.Log("relativeSpeed: " + relativeSpeed);
        /*      DUST PARTICLES      */
        //set particles for dust and set timer
        if (_pMovement.animBottomState == PlayerMovement.playerBottomState.walking) {
            //set timers if necessary
           if (_dustKickTimer == 0f) {
               kickDustParticle();
               _dustKickTimer = Time.time + dynamicDustFreq;
           }
        }
        //dust timers
        if (Time.time > _dustLifeTimer) {
            killDustParticles();
            _dustLifeTimer = Time.time + dynamicDustLife;
        }
        if (Time.time > _dustKickTimer) {
            //reset
            _dustKickTimer = 0f;
        }

        /*      BLEED PARTICLES      */
        //set particles for blood if necessary
        if (_pMovement.pub_isBleeding) {
            startBleedParticles();
        }
        else  if (_bleedPS != null){
            killBleedParticles();
        }
    }

    //call from other script and use the timer parameter to mirror the time
    //variable for each time damage is taken
    public void startBleedParticles() {
        if (_bleedPS == null) {
            GameObject newBleedParticle = Instantiate(bleedParticle, transform);
            _bleedPS = newBleedParticle.GetComponent<ParticleSystem>();
            _bleedPS.Play();
            newBleedParticle.name = "BleedParticle";
        }
    }

    //pause particles, destroy object, and reset values
    public void killBleedParticles() {
        _bleedPS.Pause();
        Destroy(_bleedPS.gameObject);
        _bleedPS = null;
    }

    //when moving around, create dust particle at the feet of the player and
    //unparent the particle and push it into a list for the particle to be destoryed later
    public void kickDustParticle() {
        GameObject dustGO = Instantiate(dustParticle, _dustPar);
        dustGO.transform.parent = transform;
        //rotate to make sure trail is behind
        dustGO.transform.Rotate(Vector3.up, 90f);
        dustGO.transform.parent = null;
        dustGO.GetComponent<ParticleSystem>().Play();
        //push to list
        _dustList.AddLast(dustGO);
    }

    //destory next item in dustList
    public void killDustParticles() {
        int listCount = _dustList.Count;
        if (listCount > 0) {
            int loopsNeeded = Mathf.RoundToInt(listCount/3f);
            loopsNeeded = (loopsNeeded < 3) ? 1 : loopsNeeded;
            for (int i = 0; i < loopsNeeded; ++i) {
                GameObject destroying = _dustList.First.Value;
                _dustList.RemoveFirst();
                //destroying.GetComponent<ParticleSystem>().Stop();
                Destroy(destroying);
            }
            //pause the rest
            var tempNode = _dustList.First;
            int count = 0;
            while (tempNode != null && count < listCount / 2) {
                tempNode.Value.GetComponent<ParticleSystem>().Stop();
                tempNode = tempNode.Next;
                ++count;
            }
        }
    }

    //get value based on player inputs for the output speed and use to adjust
    //dust spawn frequency
    private float _getRelativeSpeed() {
        float relativeSpeed = Mathf.Abs(_pMovement.JoystickH()) 
                            + Mathf.Abs(_pMovement.JoystickV());
        return Mathf.Clamp(relativeSpeed, 0.1f, 0.7f);
    }
}