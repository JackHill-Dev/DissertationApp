using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    [SerializeField] bool showController = false;
    [SerializeField] List<GameObject> controllerPrefabs;
    [SerializeField] InputDeviceCharacteristics controllerCharacteristics;
    [SerializeField] GameObject handModelPrefab;
    [SerializeField] Animator handAnimator;

    private GameObject spawnedHandModel;
    private GameObject spawnedControllerModel;
    private InputDevice targetDevice;


    // Start is called before the first frame update
    void Start()
    {
        TryInit();
    }

    void TryInit()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        
        if(devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if(prefab)
                spawnedControllerModel = Instantiate(prefab, transform);
            else
                Debug.LogError("Did not fine corresponding controller model");

            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }

    void UpdateHandAnimation()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
            handAnimator.SetFloat("Trigger", 0);



        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
            handAnimator.SetFloat("Grip", 0);


    }

    // Update is called once per frame
    void Update()
    {
        if (!targetDevice.isValid)
            TryInit();
        else
        {
            if (showController)
            {
                spawnedHandModel.SetActive(false);
                spawnedControllerModel.SetActive(true);
            }
            else
            {
                spawnedHandModel.SetActive(true);
                spawnedControllerModel.SetActive(false);
                UpdateHandAnimation();
            }
        }
    }
    
    
}
