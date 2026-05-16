using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI totalLandedText;
    [SerializeField] private TMPro.TextMeshProUGUI perfectRowText;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;

    private void Awake()
    {
        perfectRowText.alpha = 0f;
    }

    public void UpdateHUDData(int newTotalLanded, int newPerfectRow, int newScore)
    {
        totalLandedText.text = $"Total landed: {newTotalLanded}";
        scoreText.text = $"Score: {newScore}";

        if (newPerfectRow > 0f)
        {
            perfectRowText.alpha = 1f;
            perfectRowText.text = $"Perfect row: {newPerfectRow}";
        }
        else
        {
            perfectRowText.alpha = 0f;
        }
    }
}
