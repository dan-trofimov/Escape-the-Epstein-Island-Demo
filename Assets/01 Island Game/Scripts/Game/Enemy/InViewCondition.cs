using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "In View", story: "Checks [FieldOfView] for the Target", category: "Conditions", id: "92600c8627cf5ff67651d12bdcbfca4b")]
public partial class InViewCondition : Condition
{
    [SerializeReference] public BlackboardVariable<FieldOfView> FieldOfView;

    private bool inView;
    public override bool IsTrue()
    {
        Debug.Log("ViewCheckAction: " + FieldOfView.Value.canSeePlayer);
        if (FieldOfView.Value.canSeePlayer == false)
        {
            inView = false;
            Debug.Log("In View" + inView);
            return false;    
        }
        else if (FieldOfView.Value.canSeePlayer == true)
        {
            inView = true;
            Debug.Log("In View" + inView);
            return true;  
        }
        else
        {
            Debug.Log("Error!");
            Debug.Log("In View" + inView);
            return false;
        }
    }


    public override void OnEnd()
    {
    }
}
