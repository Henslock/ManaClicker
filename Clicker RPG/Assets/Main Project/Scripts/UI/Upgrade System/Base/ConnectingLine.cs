using UnityEngine;

public class ConnectingLine : MonoBehaviour
{
    public GameObject node1;
    public GameObject node2;
    public LineRenderer lineRenderer;


    private bool isFadingIn;
    private bool isColorBleeding;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        EventManager.UnitEvents.OnUpgradePurchasedFromUnit += UpdateLine;
        EventManager.UnitEvents.OnResetUpgades += ResetLines;
        lineRenderer.material.SetFloat("_Alpha", 0f);
    }

    private void OnDisable()
    {
        EventManager.UnitEvents.OnUpgradePurchasedFromUnit -= UpdateLine;
        EventManager.UnitEvents.OnResetUpgades -= ResetLines;
    }

    public void InitializeLine()
    {
        gameObject.transform.position = node1.transform.position;
        if (node1 != null && node2 != null)
        {
            lineRenderer.SetPosition(1, node2.transform.position - node1.transform.position);
            lineRenderer.startColor = new Color32(8, 8, 8, 255);
            lineRenderer.endColor = new Color32(8, 8, 8, 0);
            lineRenderer.endWidth = 0.1f;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.material.SetFloat("_ColorSlidingMask", 0f);
            lineRenderer.material.SetFloat("_FlowSpeed", 0.2f);

            if (node1.TryGetComponent<IUnitUpgrade>(out IUnitUpgrade iupgrade1))
            {
                if(iupgrade1.nodeType == UpgradeNodeType.MINOR)
                {
                    lineRenderer.material.SetColor("_InwardColorFirst", new Color32(0, 175, 255, 255));
                }
                else
                {
                    lineRenderer.material.SetColor("_InwardColorFirst", new Color32(0, 255, 65, 255));
                }
            }

            if (node2.TryGetComponent<IUnitUpgrade>(out IUnitUpgrade iupgrade2))
            {
                if (iupgrade2.nodeType == UpgradeNodeType.MINOR)
                {
                    lineRenderer.material.SetColor("_InwardColorSecond", new Color32(0, 175, 255, 255));
                }
                else
                {
                    lineRenderer.material.SetColor("_InwardColorSecond", new Color32(0, 255, 65, 255));
                }
            }

            float dist = (Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1)));

            lineRenderer.textureScale = new Vector2(1.0f, 1.0f);
            lineRenderer.material.SetFloat("_RandomSeed", Random.Range(-100f, 10f));
        }
    }
    private void UpdateLine(GameObject node)
    {
        //Initial null check and fetching the upgrade interface
        IUnitUpgrade node1upgrade = null;
        IUnitUpgrade node2upgrade = null;

        if (node1.TryGetComponent<IUnitUpgrade>(out IUnitUpgrade iupgrade1))
        {
            node1upgrade = iupgrade1;
        }

        if (node2.TryGetComponent<IUnitUpgrade>(out IUnitUpgrade iupgrade2))
        {
            node2upgrade = iupgrade2;
        }

        //null check here
        if(node1upgrade == null || node2upgrade == null)
        {
            return;
        }

        //Update color of line here
        if (node == node1 || node == node2)
        {
            //If both nodes are active
            if(node1upgrade.isActive && node2upgrade.isActive && (node2upgrade.isPurchased != node1upgrade.isPurchased))
            {
                lineRenderer.startColor = new Color32(110, 110, 110, 255);
                lineRenderer.endColor = new Color32(110, 110, 110, 0);
                lineRenderer.endWidth = 0.1f;
                lineRenderer.startWidth = 0.1f;

                float dist = (Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1)));

                lineRenderer.textureScale = new Vector2(2.0f + (dist * 0.5f), 1.0f);
                lineRenderer.material.SetFloat("_SlidingMask", 0f);
                lineRenderer.material.SetFloat("_MaskAlpha", 1f);
                lineRenderer.material.SetFloat("_ColorSlidingMask", 0f);
                isFadingIn = true;
            }

            //If both nodes are purchased
            if (node1upgrade.isPurchased && node2upgrade.isPurchased)
            {
                lineRenderer.endWidth = 0.1f;
                lineRenderer.startWidth = 0.1f;

                float dist = (Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1)));

                lineRenderer.textureScale = new Vector2(2.0f + (dist * 0.5f), 1.0f);

                lineRenderer.material.SetFloat("_ColorSlidingMask", 0f);
                lineRenderer.material.SetFloat("_MaskAlpha", 1f);
                lineRenderer.material.SetFloat("_FlowSpeed", 0.55f);
                isColorBleeding = true;
            }

            //Inactive
            if(node1upgrade.isActive == false || node2upgrade.isActive == false)
            {
                lineRenderer.startColor = new Color32(8, 8, 8, 255);
                lineRenderer.endColor = new Color32(8, 8, 8, 0);
                lineRenderer.endWidth = 0.1f;
                lineRenderer.startWidth = 0.1f;
                lineRenderer.textureScale = new Vector2(1.0f, 1.0f);
                lineRenderer.material.SetFloat("_ColorSlidingMask", 0f);
                lineRenderer.material.SetFloat("_MaskAlpha", 0.8f);
                lineRenderer.material.SetFloat("_FlowSpeed", 0.2f);
            }
        }
    }

    private void Update()
    {
        if(isFadingIn)
        {
            float lerpVal = Mathf.Lerp(lineRenderer.material.GetFloat("_SlidingMask"), 1f, Time.deltaTime*5f);
            lineRenderer.material.SetFloat("_SlidingMask", lerpVal);

            if(lineRenderer.material.GetFloat("_SlidingMask") >= 0.98f)
            {
                isFadingIn = false;
            }
        }

        if(isColorBleeding)
        {
            float lerpVal = Mathf.Lerp(lineRenderer.material.GetFloat("_ColorSlidingMask"), 2f, Time.deltaTime * 1f);
            lineRenderer.material.SetFloat("_ColorSlidingMask", lerpVal);

            lineRenderer.material.SetFloat("_MaskAlpha", Mathf.Lerp(lineRenderer.material.GetFloat("_MaskAlpha"), 0.66f, Time.deltaTime*5f));

            lineRenderer.startColor = Color32.Lerp(lineRenderer.startColor, new Color32(255, 255, 255, 255), Time.deltaTime*5f);
            lineRenderer.endColor = Color32.Lerp(lineRenderer.endColor, new Color32(255, 255, 255, 0), Time.deltaTime*5f);

            float lerpVal2 = Mathf.Lerp(lineRenderer.startWidth, 0.15f, Time.deltaTime * 4f);
            lineRenderer.endWidth = lerpVal2;
            lineRenderer.startWidth = lerpVal2;

            if (lineRenderer.material.GetFloat("_ColorSlidingMask") >= 1.98f)
            {
                isColorBleeding = false;
            }

        }

        float lineFadeLerpVal = Mathf.Lerp(lineRenderer.material.GetFloat("_Alpha"), 1f, Time.deltaTime * 3f);
        lineRenderer.material.SetFloat("_Alpha", lineFadeLerpVal);
    }

    private void ResetLines()
    {
        lineRenderer.startColor = new Color32(8, 8, 8, 255);
        lineRenderer.endColor = new Color32(8, 8, 8, 0);
        lineRenderer.material.SetFloat("_ColorSlidingMask", 0);
        lineRenderer.material.SetFloat("_MaskAlpha", 0.8f);
        lineRenderer.material.SetFloat("_FlowSpeed", 0.2f);
        isColorBleeding = false;
        isFadingIn = false;
        lineRenderer.material.SetFloat("_RandomSeed", Random.Range(-10f, 10f));
    }
}
