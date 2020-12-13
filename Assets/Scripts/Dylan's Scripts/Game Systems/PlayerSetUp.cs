using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetup", menuName = "ScritableObjects/PlayerSetup", order = 4)]
public class PlayerSetUp : ScriptableObject
{
    /// <summary>
    /// Dylan Loe
    /// 12-2020
    /// 
    /// References to different rig components that player script uses when scene starts
    /// 
    /// has references to both animators, controllers and avatars
    /// </summary>
    [Header("Player Animators")]
    public Animator p1Animator;
    public Animator p2Animator;
    public RuntimeAnimatorController p1Controller;
    public RuntimeAnimatorController p2Controller;
    public Avatar p1Avatar;
    public Avatar p2Avatar;
}
