using UnityEngine;

public static class StaticUtility 
{
    public static readonly int PlayerLayer = 1<<LayerMask.NameToLayer("Player");
    public static readonly int GroundLayer = 1<<LayerMask.NameToLayer("Default");
    public static readonly int BlockLayers = PlayerLayer | GroundLayer;
    
    public static readonly int IsMovingAnimID = Animator.StringToHash("isMoving");
    public static readonly int HurtAnimID = Animator.StringToHash("Hurt");
    public static readonly int DieAnimID = Animator.StringToHash("Die");
    public static readonly int ShootAnimID = Animator.StringToHash("Shoot");
    
}
