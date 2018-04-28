using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleElectView : MonoBehaviour
{
    PositionTween pt;
    public void Folding(bool scale)
    {
        if (pt == null)
        {
            pt = gameObject.GetComponent<PositionTween>();
        }
        if (scale)
            pt.PlayForward();
        else
            pt.PlayReverse();
    }
}
