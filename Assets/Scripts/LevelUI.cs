using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI totalLandedText;
    [SerializeField] private TMPro.TextMeshProUGUI perfectRow;

    private void Awake()
    {
        perfectRow.alpha = 0f;
    }

    public void UpdateHUDData(float newTotalLanded, float newPerfectRow)
    {
        totalLandedText.text = $"Total landed: {newTotalLanded}";

        if (newPerfectRow > 0f)
        {
            perfectRow.alpha = 1f;
            perfectRow.text = $"Perfect row: {newPerfectRow}";
        }
        else
        {
            perfectRow.alpha = 0f;
        }
    }
}
