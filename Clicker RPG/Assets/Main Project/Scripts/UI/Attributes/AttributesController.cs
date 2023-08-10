using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class AttributesController : MonoBehaviour
{
    //Stat text
    public TextMeshProUGUI strText;
    public TextMeshProUGUI intText;
    public TextMeshProUGUI agiText;
    public TextMeshProUGUI chrText;
    public TextMeshProUGUI pwrText;

    public Button strButton;
    public Button intButton;
    public Button agiButton;
    public Button chrButton;
    public Button pwrButton;

    private List<Button> attributeButtons = new List<Button>();

    private PlayerStats plrStats;
    private void Start()
    {
        attributeButtons = new List<Button>() { strButton, intButton, agiButton, chrButton, pwrButton };

        plrStats = GameManager.Instance.player.GetComponent<PlayerStats>();
        strText.text = "0";
        intText.text = "0";
        agiText.text = "0";
        chrText.text = "0";
        pwrText.text = "0";

        strButton.onClick.AddListener(() => IncreaseAttribute(plrStats.Strength));
        intButton.onClick.AddListener(() => IncreaseAttribute(plrStats.Intelligence));
        agiButton.onClick.AddListener(() => IncreaseAttribute(plrStats.Agility));
        chrButton.onClick.AddListener(() => IncreaseAttribute(plrStats.Charisma));
        pwrButton.onClick.AddListener(() => IncreaseAttribute(plrStats.Power));
    }

    private void Update()
    {
        strText.text = plrStats.Strength.value.ToString();
        intText.text = plrStats.Intelligence.value.ToString();
        agiText.text = plrStats.Agility.value.ToString();
        chrText.text = plrStats.Charisma.value.ToString();
        pwrText.text = plrStats.Power.value.ToString();

        if(plrStats.currentAttributePoints <= 0)
        {
            foreach(Button btn in attributeButtons)
            {
                btn.interactable = false;
            }
        }
        else
        {
            foreach (Button btn in attributeButtons)
            {
                btn.interactable = true;
            }
        }
    }

    public void IncreaseAttribute(Stat _stat)
    {
        if (plrStats.currentAttributePoints > 0)
        {
            _stat.baseValue += 1;
            plrStats.currentAttributePoints--;
        }
        else
            return;
    }
}
