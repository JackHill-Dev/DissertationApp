using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatrixModel : MonoBehaviour
{
    
    [SerializeField] private Material transparentMat;
    [SerializeField] private Material opaqueMat;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ToggleTrasparency(bool bTransparentEnabled)
    {
        if (bTransparentEnabled)
            // enable transparent matrial on model
            this.GetComponent<Renderer>().material = transparentMat;
        else
            // enable opaque material on model
            this.GetComponent<Renderer>().material = opaqueMat;
        
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }
    
}
