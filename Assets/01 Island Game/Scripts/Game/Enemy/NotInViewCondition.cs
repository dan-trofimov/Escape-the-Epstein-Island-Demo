using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Not In View", story: "Checks [FieldOfView] for Target", category: "Conditions", id: "4cc777e2433eb1b23c64e637512076ff")]
public partial class NotInViewCondition : Condition
{
    [SerializeReference] public BlackboardVariable<FieldOfView> FieldOfView;

    public override bool IsTrue()
    {
        {
            // Safety check: if the FOV component is missing, assume we can't see the player
            if (FieldOfView.Value == null)
            {
                return true;
            }

            // We return TRUE if the player is NOT in view
            // Using the boolean already calculated by your FOV script
            return !FieldOfView.Value.canSeePlayer;
        }
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
