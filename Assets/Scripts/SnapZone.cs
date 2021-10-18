using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapZone : MonoBehaviour
{
    [SerializeField] private MatrixController matController;

    [SerializeField] private int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(!matController) return;
    }

    private void OnTriggerEnter(Collider other)
    {
        MatrixObject mO = other.GetComponent<MatrixObject>();
        if (mO)
        {
            matController.SetMatrix(index, mO.GetMatrix());
        }
    }
}
