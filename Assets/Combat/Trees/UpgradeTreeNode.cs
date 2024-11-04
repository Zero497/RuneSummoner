using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTreeNode : ScriptableObject
{
    public List<string> activeGrant;
    
    public List<string> passiveGrant;

    public List<UpgradeTreeNode> branches;
}
