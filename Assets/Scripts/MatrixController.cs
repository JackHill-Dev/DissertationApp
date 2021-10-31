using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using Pixelplacement;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MatrixController : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI finalMatrixTMP;
    
    [SerializeField] private Transform parent;
    [SerializeField] private List<SnapZone> matrixSnapZones;
    private Matrix4x4[] mats; // Matrices currently in the snap zones
    private Matrix4x4 finalMatrix;

    private List<GameObject> vertices;

    [SerializeField] private GameObject matrixPrefab;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private Transform origin;
    // Model vars
    
    // Make sure model being used has read/write access on its import settings
    [SerializeField] private MeshFilter manipulatedModelMeshFilter;
    
    private Mesh manipulatedMesh;
    private Vector3[] modelVerts;
    private Vector3[] orginalVerts;
    
    private void Start()
    {
        manipulatedMesh = manipulatedModelMeshFilter.mesh;
        modelVerts = manipulatedMesh.vertices;
        orginalVerts = manipulatedMesh.vertices;
        mats = new Matrix4x4[3];
    }
    public void ApplyMatrices()
    {
        modelVerts = manipulatedMesh.vertices;
        // Get the current matrices held in the snap zones
        for (int i = 0; i < mats.Length; ++i)
            mats[i] = matrixSnapZones[i].matObj.GetMatrix();
        
        finalMatrix = mats[0] * mats[1] * mats[2];
        
        
        for (int index = 0; index < modelVerts.Length; ++index)
        {
            modelVerts[index] = finalMatrix.MultiplyPoint(modelVerts[index]);
        }

        manipulatedModelMeshFilter.mesh.vertices = modelVerts;
        manipulatedModelMeshFilter.mesh.RecalculateBounds();

        //finalMatrixTMP.SetText(  finalMatrix.ToString("F2"));

    }

    public void CreateMatrix()
    {
        Instantiate(matrixPrefab, spawnLocation);
    }
    public void ResetModel()
    {
        finalMatrix = Matrix4x4.zero;
        mats[0] = Matrix4x4.zero;
        mats[1] = Matrix4x4.zero;
        mats[2] = Matrix4x4.zero;
        
        manipulatedModelMeshFilter.mesh.vertices = orginalVerts;
        manipulatedModelMeshFilter.mesh.RecalculateBounds();
       // modelVerts = orginalVerts;
    }

    public void ReparentUI(SelectEnterEventArgs context)
    {
        context.interactable.gameObject.transform.SetParent(parent, true);
    }

    public void SetMatrix(int index, Matrix4x4 mat)
    {
        mats[index] = mat;
    
    }

    public void AnimateTransformations()
    {
        // Get all the stored final transformations from the matrices
        // Then apply them in the right order through animtions
        
        Transform t = manipulatedModelMeshFilter.transform;

        Vector3 rotation = new Vector3(0, 0, 30);
        
        Quaternion q = Quaternion.Euler(rotation);
        
        // Translate to origin
        Tween.Position(t, origin.position, 5f, 0f);
        // Rotate about the origin through Z
        Tween.Rotation(t, q, 5f, 5f);
        // Translate back
        Tween.Position(t, new Vector3(6, 8, 0), 5f, 10f);

    }
    
    
}
