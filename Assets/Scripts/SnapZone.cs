using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapZone : MonoBehaviour
{
    // Currently public as the matrix controller accessing this
    public MatrixObject matObj;

    private void OnTriggerStay(Collider other)
    {
        MatrixObject mO = other.GetComponent<MatrixObject>();
        
        if (mO)
        {
            matObj = mO;
        }
    }

    
}
