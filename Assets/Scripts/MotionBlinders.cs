using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit;
using Pixelplacement;

public class MotionBlinders : MonoBehaviour
{
    [SerializeField] private float intensity = 0.75f;
    [SerializeField] private float duration = 0.75f;
    [SerializeField] Volume  volume = null;

    private DeviceBasedContinuousMoveProvider locomotionProvider;
    private DeviceBasedContinuousTurnProvider turnProvider;
    
    private Vignette vignette = null;
    
    private void Awake()
    {
        locomotionProvider = GetComponent<DeviceBasedContinuousMoveProvider>();
        turnProvider = GetComponent<DeviceBasedContinuousTurnProvider>();
        
        if (volume.profile.TryGet(out Vignette vignette))
            this.vignette = vignette;
    }

    private void OnEnable()
    {
        locomotionProvider.beginLocomotion += FadeIn;
        locomotionProvider.endLocomotion += FadeOut;

       // turnProvider.beginLocomotion += FadeIn;
       // turnProvider.endLocomotion += FadeOut;
    }
    
    private void OnDisable()
    {
        locomotionProvider.beginLocomotion += FadeIn;
        locomotionProvider.endLocomotion += FadeOut;
        
       // turnProvider.beginLocomotion += FadeIn;
       // turnProvider.endLocomotion += FadeOut;
    }

    public void FadeIn(LocomotionSystem locomotionSystem)
    {
        Tween.Value(0, intensity, ApplyValue, duration, 0);
    }
    
    public void FadeOut(LocomotionSystem locomotionSystem)
    {
        Tween.Value(intensity, 0, ApplyValue, duration, 0);
    }

    IEnumerator Fade(float startValue, float endValue)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime <= duration)
        {
            float blend = elapsedTime / duration;
            elapsedTime += Time.deltaTime;

            float intensity = Mathf.Lerp(startValue, endValue, blend);
            ApplyValue(intensity);

            yield return null;
        }
    }

    private void ApplyValue(float f)
    {
       vignette.intensity.Override(f);
    }


}
