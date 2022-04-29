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
using Debug = UnityEngine.Debug;

public class MatrixController : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI finalMatrixTMP;
    [SerializeField]private TextMeshProUGUI finalVertexTMP;
    [SerializeField]private TextMeshProUGUI selectedVertexTMP;
 
    
    [SerializeField] private Transform sceneUICanvasTransform;
    [SerializeField] private List<SnapZone> matrixSnapZones;
    // Matrices currently in the snap zones
    private Matrix4x4[] mats; 
    
    private Matrix4x4 finalMatrix;
    [SerializeField] private TMP_Dropdown matrixDd;
    [SerializeField] private List<GameObject> matrixPrefabs;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private KeyPad keyPad;
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
    private Vector3 originalCoords;

    private Transform originalTransform;
    
    private void Start()
    {
        manipulatedMesh = manipulatedModelMeshFilter.mesh;
        modelVerts = manipulatedMesh.vertices;
        orginalVerts = manipulatedMesh.vertices;
        mats = new Matrix4x4[3];


        Matrix4x4 world = manipulatedModelMeshFilter.transform.localToWorldMatrix;
        var vert = modelVerts[1];// world.MultiplyPoint(modelVerts[1]);
        selectedVertexTMP.SetText(vert.x.ToString("F2") + "\n" + vert.y.ToString("F2") + "\n" + vert.z.ToString("F2") + "\n1");
    }
    
    // Instantly applies the final matrix to the model
    public void ApplyMatrices()
    {
        // Calculate the final matrix
        finalMatrix = CalculateFinalMatrix(); 

        // Caluculation the final transformations for each vertex on the model
        CalculateFinalVerts();

        // Apply the final transformations to the model
        manipulatedModelMeshFilter.mesh.vertices = modelVerts;
        manipulatedModelMeshFilter.mesh.RecalculateBounds();
        
        // Display the final matrix to the user
        finalMatrixTMP.SetText( finalMatrix.ToString("F2"));

        // Display the final coordiantes of the second vertex to the user
       var vert = modelVerts[1];
        
       finalVertexTMP.SetText(vert.x.ToString("F2") + "\n" + vert.y.ToString("F2") + "\n" + vert.z.ToString("F2") + "\n1");
    }

    Matrix4x4 CalculateFinalMatrix()
    {
        modelVerts = manipulatedMesh.vertices;
        
        // Get the current matrices held in the snap zones
        for (int i = 0; i < mats.Length; ++i)
            mats[i] = matrixSnapZones[i].matObj.GetMatrix();
    
        var mat = mats[0] * mats[1] * mats[2];
        Debug.Log(mat.ToString("F2"));
        return mat;
    }

    void CalculateFinalVerts()
    {
        for (int index = 0; index < modelVerts.Length; ++index)
        {
            modelVerts[index] = finalMatrix.MultiplyPoint3x4(modelVerts[index]);
        }
        
        
    }
    


    void StoreAnims()
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
                    finalRotation = Quaternion.Euler(-tempMatObj.rotationVector);
                    break;
                case MatrixType.RotateY:
                    finalRotation = Quaternion.Euler(-tempMatObj.rotationVector);
                    break;
                case MatrixType.RotateZ:
                    finalRotation = Quaternion.Euler(tempMatObj.rotationVector); 
                    break;
            }
        }
        
    }
    public void CreateMatrix()
    {
        // Create matrix object at spawn location but also make sure the UI element on it stay visible
        GameObject g = Instantiate(matrixPrefabs[matrixDd.value], spawnLocation.position, Quaternion.identity, sceneUICanvasTransform);
        // Bind the reparent function to new matrix object
        g.GetComponent<XRGrabInteractable>().selectEntered.AddListener(ReparentUI);
        // When matrix is created, bind all matrix field buttons to set active field function
        keyPad.FindAllMatrixFieldObjects();
        
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
        // Moved this function here to make it less confusing for the user 
        StoreAnims();
        // Get all the stored final transformations from the matrices
        // Calculate the final matrix
        finalMatrix = CalculateFinalMatrix(); 

        // Caluculation the final transformations for each vertex on the model
        CalculateFinalVerts();
        
        // Display the final matrix to the user
        finalMatrixTMP.SetText(  finalMatrix.ToString("F2"));
        // Display the final coordiantes of the second vertex to the user
        var vert = modelVerts[1];

        finalVertexTMP.SetText(vert.x.ToString("F2") + "\n" + vert.y.ToString("F2") + "\n" + vert.z.ToString("F2") + "\n1");
      

        // Then apply them in the right order through animtions

        Transform t = manipulatedModelMeshFilter.transform;
        originalCoords = t.position;
        
        // Translate to origin
        Tween.Position(t, origin.position, animDuration, 0f);
        // Rotate about the origin through Z
        Tween.Rotation(t, finalRotation, animDuration, 3f);
        // Apply scale matrix anim
        Tween.LocalScale(t, finalScale, animDuration, 6f);
        // Translate back
        Tween.Position(t, originalCoords, animDuration, 9f);
        // Apply translation matrix anim
        Tween.Position(t, finalPosition + t.position , animDuration, 12f);

    }

    public void ResetAnim()
    {
        Transform t = manipulatedModelMeshFilter.transform;
        Vector3 normalScale = new Vector3(1f, 1f, 1f);
        Quaternion q = Quaternion.identity;
        
        Tween.Position(t, originalCoords, 5f, 0f);
        Tween.LocalScale(t, normalScale, 5f, 5f);
        Tween.Rotation(t, q, 5f, 10f);
    }
    
    
}
