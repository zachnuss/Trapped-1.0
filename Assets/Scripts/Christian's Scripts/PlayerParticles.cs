/*
 * PlayerParticles.cs
 * 
 * Author: Christian Mullins
 * Summary: Handles functions and storing of effects, defined functions will
 *          be called from other scripts
 */
using UnityEngine;

public class PlayerParticles : MonoBehaviour {
    //particles
    public GameObject bleedParticle;
    public GameObject dustParticle;

    //private helper vars
    private ParticleSystem _dustPS;
    private ParticleSystem _bleedPS;
    private PlayerMovement _pMovement;

    //get dust particle system and create in-game object
    void Start() {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; ++i) {
            if (transform.GetChild(i).name == "PlayerRotate") {
                GameObject dustGO = Instantiate(dustParticle,
                                                transform.GetChild(i));
                //set parent
                dustGO.transform.parent = transform;
                _dustPS = dustGO.GetComponent<ParticleSystem>();
                _dustPS.Pause();
                break;
            }
        }
        _pMovement = GetComponent(PlayerMovement);
    }

    //set particles when necessary bools are met
    void FixedUpdate() {
        //set particles for dust
        if (_pMovement.animBottomState == playerBottomState.walking) {
            kickDustParticles();
        }
        else {
            killDustParticles();
        }
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
            newBleedParticle.name = "BleedParticles";
        }
    }

    //pause particles, destroy object, and reset values
    public void killBleedParticles() {
        _bleedPS.Pause();
        Destroy(_bleedPS.gameObject);
        _bleedPS = null;
    }

    //when moving around, kick up dust particles at the foot of the player
    public void kickDustParticles() {
        if (!_dustPS.isPlaying) {
            _dustPS.Play();
        }
    }

    //pause particles, destroy object, and reset values
    public void killDustParticles() {
        _dustPS.Pause();
    }
}