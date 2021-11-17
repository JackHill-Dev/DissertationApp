using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public enum MatrixType
{
    Scale,
    Translate,
    RotateX,
    RotateY,
    RotateZ
    
}
public class MatrixObject : MonoBehaviour
{
    public MatrixType type;
    
    [SerializeField] private List<TextMeshProUGUI> dropdownText;
    //[SerializeField] private List<Dropdown> _dropdowns;
    private float[] matValues;


    public Vector3 scaleVector;
    public Vector3 translationVector;
    public Vector3 rotationVector;
    
    private Matrix4x4 mat;
    // Start is called before the first frame update
    void Start()
    {
        mat = new Matrix4x4();
        matValues = new float[4];
        
        // Initialise vectors
        scaleVector = Vector3.zero;
        translationVector = Vector3.zero;
        rotationVector = Vector3.zero;
        
       // GenerateOptions();
    }

    private void GetMatrixValues()
    {
        for (int i = 0; i < dropdownText.Count; i++)
        {
            matValues[i] = float.Parse(dropdownText[i].text);
        }

        switch (type)
        {
            case MatrixType.RotateX:
                matValues[0] = Mathf.Cos(matValues[0] * Mathf.Deg2Rad);
                matValues[1] = Mathf.Sin(matValues[1] * Mathf.Deg2Rad);
                matValues[2] = -Mathf.Sin(matValues[2] * Mathf.Deg2Rad);
                matValues[3] = Mathf.Cos(matValues[3] * Mathf.Deg2Rad);
                break;
            case MatrixType.RotateY:
            case MatrixType.RotateZ:
                matValues[0] = Mathf.Cos(matValues[0] * Mathf.Deg2Rad);
                matValues[1] = -Mathf.Sin(matValues[1] * Mathf.Deg2Rad);
                matValues[2] = Mathf.Sin(matValues[2] * Mathf.Deg2Rad);
                matValues[3] = Mathf.Cos(matValues[3] * Mathf.Deg2Rad);
                break;
          
                
            default: break;
                
        }
    }
    
    public Matrix4x4 GetMatrix()
    {
        GetMatrixValues();
        
        switch (type)
        {
            // m-x-y: x represents the row index y represents the value position on that row
            case MatrixType.RotateX:
                mat.m00 = 1; 
                mat.m11 = matValues[0];
                mat.m12 = matValues[1];
                mat.m21 = matValues[2];
                mat.m22 = matValues[3];
                
                rotationVector = new Vector3(float.Parse( dropdownText[0].text), 0, 0);
                break;
            case MatrixType.RotateY:
                mat.m00 = matValues[0];
                mat.m02 = matValues[1];
                mat.m20 = matValues[2];
                mat.m22 = matValues[3];
                mat.m11 = 1;
                
                rotationVector = new Vector3(0, float.Parse( dropdownText[0].text),  0);
                break;
            
            case MatrixType.RotateZ:
                mat.m00 = matValues[0];
                mat.m01 = matValues[1];
                mat.m10 = matValues[2];
                mat.m11 = matValues[3];
                mat.m22 = 1;

                rotationVector = new Vector3(0, 0,  float.Parse( dropdownText[0].text));
                break;
            
            case MatrixType.Scale:
                mat.SetColumn(0, new Vector4(matValues[0],0,0,0));
                mat.SetColumn(1, new Vector4(0,matValues[1],0,0));
                mat.SetColumn(2, new Vector4(0,0,matValues[2],0));
                scaleVector = new Vector3(matValues[0], matValues[1], matValues[2]);
                break;
            
            case MatrixType.Translate:
                mat.SetColumn(3, new Vector4(matValues[0], matValues[1], matValues[2],1));
                mat.m00 = 1;
                mat.m11 = 1;
                mat.m22 = 1;
                translationVector = new Vector3(matValues[0], matValues[1], matValues[2]);

                break;
        }
        
        // set bottom left value of matrix to 1
        mat.m33 = 1;
        
        return mat;
    }

    
}
