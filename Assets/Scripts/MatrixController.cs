using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MatrixController : MonoBehaviour
{
    [SerializeField] private Transform parent;
    private Matrix4x4[] mats;

    private Matrix4x4 finalMatrix;
    // Model vars
    [SerializeField] private MeshFilter meshFilter;
    private Mesh manipulatedMesh;
    private Vector3[] modelVerts;
    
    private void Start()
    {
        manipulatedMesh = meshFilter.mesh;
        modelVerts = manipulatedMesh.vertices;
        mats = new Matrix4x4[3];
        
    }

    public void ApplyMatrices()
    {
        finalMatrix = mats[0] * mats[1] * mats[2];
        
        for (int index = 0; index < modelVerts.Length; ++index)
        {
            modelVerts[index] = finalMatrix.MultiplyPoint(modelVerts[index]);
        }

        meshFilter.mesh.vertices = modelVerts;
        meshFilter.mesh.RecalculateBounds();

    }
    public void ReparentUI(SelectEnterEventArgs context)
    {
        context.interactable.gameObject.transform.SetParent(parent, true);
    }

    public void SetMatrix(int index, Matrix4x4 mat)
    {
        mats[index] = mat;
    
    }
    
    
}
