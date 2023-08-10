using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class ManaCounterUI : MonoBehaviour
{
    public TextMeshProUGUI ManaCountText;

    private void Update()
    {
        string Amnt = AbbrevationUtility.AbbreviateNumber(GameManager.Instance.manaAmount, AbbrevationUtility.FormatType.TYPE_DECIMAL);
        if (GameManager.Instance.manaAmount == 0)
            Amnt = "0";

        string[] stringSplit = Amnt.Split(char.Parse(" "));

        if (stringSplit.Length <= 1)
        {
            string finalString = string.Empty;
            string[] split = Regex.Split(Amnt, @"(\.|,)");
            for (int i = 0; i < split.Length; i++)
            {
                char check = split[i][0];
                if (check == char.Parse(",") || check == char.Parse("."))
                {
                    finalString += "<mspace=10.0>" + split[i] + "</mspace>";
                }
                else
                {
                    finalString += "<mspace=30.0>" + split[i] + "</mspace>";
                }
            }
            finalString += "\n<size=28><color=#00F1FF>MANA</color></size>";

            ManaCountText.text = finalString;
        }
        else
        {
            string parsedNumber = string.Empty;
            string[] split = Regex.Split(stringSplit[0], @"(\.|,)");

            for (int i = 0; i < split.Length; i++)
            {
                char check = split[i][0];
                if (check == char.Parse(",") || check == char.Parse("."))
                {
                    parsedNumber += "<mspace=10.0>" + split[i] + "</mspace>";
                }
                else
                {
                    parsedNumber += "<mspace=30.0>" + split[i] + "</mspace>";
                }
            }

            ManaCountText.text = parsedNumber + "<size=32>" + stringSplit[1] + "</size>" + "\n<size=28><color=#00F1FF>MANA</color></size>";
        }
    }
}
