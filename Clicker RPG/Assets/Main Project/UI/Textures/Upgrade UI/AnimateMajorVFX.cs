using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateMajorVFX : MonoBehaviour
{
    public ParticleSystemRenderer burstParticle;
    public float waveSpeed = 3.5f;
    private void Start()
    {
        burstParticle.material.SetFloat("_Scroller", 0f);
        burstParticle.material.SetFloat("_CutoffIntensity", 1.0f);
    }

    private void OnEnable()
    {
        burstParticle.material.SetFloat("_Scroller", 0f);
        burstParticle.material.SetFloat("_CutoffIntensity", 1.0f);
    }

    private void OnDisable()
    {
        burstParticle.material.SetFloat("_Scroller", 0f);
        burstParticle.material.SetFloat("_CutoffIntensity", 1.0f);
    }


    private void Update()
    {
        float lerpVal = Mathf.Lerp(burstParticle.material.GetFloat("_Scroller"), 1.2f, Time.deltaTime * waveSpeed);
        burstParticle.material.SetFloat("_Scroller", lerpVal);

        if (burstParticle.material.GetFloat("_Scroller") >= 0.8f)
        {
            float lerpVal2 = Mathf.Lerp(burstParticle.material.GetFloat("_CutoffIntensity"), 8f, Time.deltaTime);
            burstParticle.material.SetFloat("_CutoffIntensity", lerpVal2);
        }
    }
}
