using TMPro;
using UnityEngine;

public class OptionButtonContainer : MonoBehaviour
{
    [SerializeField] private TMP_Text buttonText;

    public void SetText(string optionText)
    {
        buttonText.text = optionText;
    }
}
