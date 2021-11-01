using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Pixelplacement;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MatrixController : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI finalMatrixTMP;
    
    [SerializeField] private Transform sceneUICanvasTransform;
    [SerializeField] private List<SnapZone> matrixSnapZones;
    // Matrices currently in the snap zones
    private Matrix4x4[] mats; 
    
    private Matrix4x4 finalMatrix;

    
    [SerializeField] private GameObject matrixPrefab;
    [SerializeField] private Transform spawnLocation;

    
    // Model vars
    
    // Make sure model being used has read/write access on its import settings
    [SerializeField] private MeshFilter manipulatedModelMeshFilter;
    
    private Mesh manipulatedMesh;
    private Vector3[] modelVerts;
    private Vector3[] orginalVerts;
    
    // Stored variables for animations (Tweens)
    [SerializeField] private Transform origin;
    private Mesh tempMesh;
    private Quaternion finalRotation;
    private Vector3 finalPosition;
    private Vector3 finalScale;
    private float animDuration = 5f;
    private float animDelay = 5f;
    
    private void Start()
    {
        manipulatedMesh = manipulatedModelMeshFilter.mesh;
        modelVerts = manipulatedMesh.vertices;
        orginalVerts = manipulatedMesh.vertices;
        mats = new Matrix4x4[3];
    }
    
    // Instantly applies the final matrix to the model
    public void ApplyMatrices()
    {
        modelVerts = manipulatedMesh.vertices;
        
        // Get the current matrices held in the snap zones
        for (int i = 0; i < mats.Length; ++i)
            mats[i] = matrixSnapZones[i].matObj.GetMatrix();

        
        
        // Calculate the final matrix
        finalMatrix = mats[0] * mats[1] * mats[2];
        
        // Caluculation the final transformations for each vertex on the model
        for (int index = 0; index < modelVerts.Length; ++index)
        {
            modelVerts[index] = finalMatrix.MultiplyPoint(modelVerts[index]);
        }
        
        // Apply the final transformations to the model
        manipulatedModelMeshFilter.mesh.vertices = modelVerts;
        manipulatedModelMeshFilter.mesh.RecalculateBounds();
        
        // Display the final matrix to the user
        //finalMatrixTMP.SetText(  finalMatrix.ToString("F2"));

    }
    
    MeshFilter CalculateMeshTransforms(MeshFilter mf, Matrix4x4 matrix)
    {
        MeshFilter tempMeshFilter = mf;

        Vector3[] tempVerts = mf.mesh.vertices;
        
        for (int index = 0; index < tempVerts.Length; ++index)
        {
            tempVerts[index] = matrix.MultiplyPoint(tempVerts[index]);
        }

        tempMeshFilter.mesh.vertices = tempVerts;
        tempMeshFilter.mesh.RecalculateBounds();
       
        return tempMeshFilter;
    }

    public void StoreAnims()
    {
        // Get the current matrices held in the snap zones
        for (int i = 0; i < mats.Length; ++i)
        {
            MatrixObject tempMatObj = matrixSnapZones[i].matObj;
            
            mats[i] = matrixSnapZones[i].matObj.GetMatrix();
            
            switch (tempMatObj.type)
            {
                case MatrixType.Scale:
                    finalScale = tempMatObj.scaleVector;
                    break;
                case MatrixType.Translate:
                    finalPosition = tempMatObj.translationVector;
                    break;
                case MatrixType.RotateX:
                case MatrixType.RotateY:
                case MatrixType.RotateZ:
                    finalRotation = Quaternion.Euler(tempMatObj.rotationVector); 
                    break;
            }

            
        }

        
    }

    
    public void CreateMatrix()
    {
        GameObject g = Instantiate(matrixPrefab, spawnLocation);
    }
    public void ResetModel()
    {
        finalMatrix = Matrix4x4.zero;
        mats[0] = Matrix4x4.zero;
        mats[1] = Matrix4x4.zero;
        mats[2] = Matrix4x4.zero;
        
        manipulatedModelMeshFilter.mesh.vertices = orginalVerts;
        manipulatedModelMeshFilter.mesh.RecalculateBounds();
        
    }

    public void ReparentUI(SelectEnterEventArgs context)
    {
        context.interactable.gameObject.transform.SetParent(sceneUICanvasTransform, true);
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
        Vector3 originalCoords = t.position;
        
        // Translate to origin
        Tween.Position(t, origin.position, animDuration, 0f);
        
        // Rotate about the origin through Z
        Tween.Rotation(t, finalRotation, animDuration, 5f);
        // Apply scale matrix anim
        Tween.LocalScale(t, finalScale, animDuration, 10f);
        // Translate back
        Tween.Position(t, originalCoords, animDuration, 15f);
        // Apply translation matrix anim
        Tween.Position(t, t.position + finalPosition, animDuration, 20f);

    }
    
    
}
