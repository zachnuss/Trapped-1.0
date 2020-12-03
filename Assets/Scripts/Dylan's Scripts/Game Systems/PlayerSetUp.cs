using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetup", menuName = "ScritableObjects/PlayerSetup", order = 4)]
public class PlayerSetUp : ScriptableObject
{
    [Header("Player Animators")]
    public Animator p1Animator;
    public Animator p2Animator;
    public RuntimeAnimatorController p1Controller;
    public RuntimeAnimatorController p2Controller;
    public Avatar p1Avatar;
    public Avatar p2Avatar;
}
