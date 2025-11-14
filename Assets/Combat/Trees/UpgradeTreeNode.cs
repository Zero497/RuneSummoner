using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TreeNode", menuName = "UpgradeTreeNode")]
public class UpgradeTreeNode : ScriptableObject
{
    public List<ActiveAbility.ActiveAbilityDes> activeGrant;
    
    public List<PassiveAbility.PassiveAbilityDes> passiveGrant;

    public List<UpgradeTreeNode> branches;
}
