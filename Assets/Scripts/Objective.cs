using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
    [SerializeField] private TMP_Text objectiveText;

    public void SetObjectiveText(string objective)
    {
        objectiveText.text = objective;
    }

    public void ToggleObjectiveDialogue(bool state)
    {
        objectiveText.gameObject.SetActive(state);
    }
}


