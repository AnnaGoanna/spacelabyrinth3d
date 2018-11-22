using UnityEngine;

public class CreditsRoller : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.HasKey("RollCredits"))
        {
            int roll = PlayerPrefs.GetInt("RollCredits");
            if (roll == 1)
            {
                PlayerPrefs.SetInt("RollCredits", 0);
                GameObject mainMenuPanel = gameObject.FindObject("MainMenuPanel");
                GameObject aboutPanel = gameObject.FindObject("AboutPanel");
                mainMenuPanel.SetActive(false);
                aboutPanel.SetActive(true);
            }
        }
    }
}
