using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapZone : MonoBehaviour
{
    //[SerializeField] private MatrixController matController;
    public MatrixObject matObj;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    

    private void OnTriggerStay(Collider other)
    {
        MatrixObject mO = other.GetComponent<MatrixObject>();
        
        if (mO)
        {
            matObj = mO;
        }
    }

    
}
