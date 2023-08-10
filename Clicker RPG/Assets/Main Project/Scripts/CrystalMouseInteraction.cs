using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CrystalMouseInteraction : MonoBehaviour
{
    public float rotatespeed = 5.0f;
    private Vector3 defaultScale;
    private float bonusScale;
    private float bonusScaleDelta;
    private float timeScalar;

    public float bounceSpeed = 2.0f;
    public float ElasticBounceAggression = 2.5f;
    public float ElasticBounceOvershoot = 1.2f;
    public float ElasticBounceSomething = 2.2f;

    public Material crystalMat;
    private bool showCrystalPower;
    private float targetCrystalPower = 0f;
    public float defaultCrystalPower = 0.15f;

    private float inverseLightFadeSpeedDelta = 3.0f;

    public AudioClip clickSFX;
    public AudioClip clickCritSFX;
    public AudioSource clickCritSFXSource;

    public float Berp(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);
        value = (Mathf.Sin(value * Mathf.PI * (0.2f + ElasticBounceAggression * value * value * value)) * Mathf.Pow(1f - value, ElasticBounceSomething) + value) * (1f + (ElasticBounceOvershoot * (1f - value)));
        return start + (end - start) * value;
    }

    private void Start()
    {
        showCrystalPower = true;
        defaultScale = gameObject.transform.localScale;
        targetCrystalPower = defaultCrystalPower;
    }

    void Update()
    {
        gameObject.transform.Rotate(Vector3.up, Time.deltaTime * rotatespeed);
        Vector3 targetScale = new Vector3(defaultScale.x * (1.0f + bonusScale), defaultScale.y * (1.0f + bonusScale), defaultScale.z * (1.0f + bonusScale));
        gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, targetScale, Time.deltaTime * 10f);

        inverseLightFadeSpeedDelta = Mathf.Lerp(inverseLightFadeSpeedDelta, 3.0f, Time.deltaTime);
        if (showCrystalPower)
        {
            float cp = crystalMat.GetFloat("_CrystalPower");
            float cpLerpResult = Mathf.Lerp(cp, targetCrystalPower, Time.deltaTime * inverseLightFadeSpeedDelta);
            //crystalLightData.intensity = Mathf.Lerp(crystalLightData.intensity, targetLightIntensity, Time.deltaTime * inverseLightFadeSpeedDelta);
            crystalMat.SetFloat("_CrystalPower", cpLerpResult);

        }
    }

    //Below is a bunch of mumbo jumbo to replicate a "bouncy" animation when clicking or hovering over the crystal.

    private void OnMouseEnter()
    {
        if(!CursorManager.Instance.isCursorActive) { return; }
        bonusScaleDelta = 0.1f;
    }

    private void OnMouseOver()
    {
        if (!CursorManager.Instance.isCursorActive) { return; }
        timeScalar += Time.deltaTime * bounceSpeed;
        bonusScale = Berp(0, bonusScaleDelta, timeScalar);

        targetCrystalPower = 0.2f;
        bonusScaleDelta = Mathf.Lerp(bonusScaleDelta, 0.1f, Time.deltaTime *5f);
    }

    private void OnMouseExit()
    {
        if (!CursorManager.Instance.isCursorActive) { return; }
        timeScalar = 0;
        bonusScale = 0f;

        targetCrystalPower = defaultCrystalPower;
    }

    private void OnMouseDown()
    {
        if (!CursorManager.Instance.isCursorActive) { return; }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            //If we click UI over the crystal just dont count it
            return;
        }
        //Click the crystal to gain mana!
        EventManager.GeneralEvents.OnClickCrystal?.Invoke();

        if (showCrystalPower)
        {
            float cp = crystalMat.GetFloat("_CrystalPower");
            crystalMat.SetFloat("_CrystalPower", Mathf.Max(0.15f, cp));
            cp += 0.002f;
            crystalMat.SetFloat("_CrystalPower", cp);
            crystalMat.SetFloat("_CrystalPower", Mathf.Min(0.40f, cp));
        }

        inverseLightFadeSpeedDelta = 0f;
        bonusScaleDelta = -0.05f;
    }
}
