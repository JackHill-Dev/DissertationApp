using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class LocomotionController : MonoBehaviour
{
    [SerializeField] XRController rightTeleportRay;
    [SerializeField] XRController leftTeleportRay;
    [SerializeField] InputHelpers.Button teleportActivationBtn;
    [SerializeField] float activationThreshold= 0.1f;

    [SerializeField] XRRayInteractor leftInteractorRay;
    [SerializeField] XRRayInteractor rightInteractorRay;

    private CharacterController characterController;
    private GameObject XRCamera;
    
    public bool enableRightTeleport { get; set; } = true;
    public bool enableLeftTeleport { get; set; } = true;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        XRCamera = GetComponent<XRRig>().cameraGameObject;
    }

    private void Start()
    {
        PostionCharController();
    }

    // Update is called once per frame
    void Update()
    {
        HandleTeleportation();

        PostionCharController();

    }
    
    // Code from this tutorial: https://www.youtube.com/watch?v=6N__0jeg6k0
    private void PostionCharController()
    {
        float headheight = Mathf.Clamp(XRCamera.transform.localPosition.y, 1, 2);
        characterController.height = headheight;
        
        Vector3 newCenter = Vector3.zero;
        newCenter.y = characterController.height / 2;
        newCenter.y += characterController.skinWidth;

        newCenter.x = XRCamera.transform.localPosition.x;
        newCenter.z = XRCamera.transform.localPosition.z;

        characterController.center = newCenter;
        
        
    }

    void HandleTeleportation()
    {
        Vector3 pos = new Vector3();
        Vector3 norm = new Vector3();
        int index = 0;
        bool validTarget = false;
        
        if(rightTeleportRay)
        {
            bool isRightHovering = rightInteractorRay.TryGetHitInfo(out pos, out norm, out index, out validTarget);
            rightTeleportRay.gameObject.SetActive(enableRightTeleport && CheckIfActivated(rightTeleportRay) && !isRightHovering);
        }
        
        if(leftTeleportRay)
        {
            bool isLeftHovering = leftInteractorRay.TryGetHitInfo(out pos, out norm, out index, out validTarget);
            leftTeleportRay.gameObject.SetActive(enableLeftTeleport && CheckIfActivated(leftTeleportRay) && !isLeftHovering) ;
            
            
        }
    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationBtn, out bool isActivated, activationThreshold);
        return isActivated;

    }
    
}
