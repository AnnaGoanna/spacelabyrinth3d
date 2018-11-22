using UnityEngine;

public class ControlsHelpToggle : MonoBehaviour
{
    public GameObject[] controlHelps;
    private int helpNumber = 0;
    public KeyCode key;

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            helpNumber++;

            if (controlHelps.Length < helpNumber)
            {
                helpNumber = 0;
                foreach (GameObject h in controlHelps) h.SetActive(false);
                return;
            }

            foreach (GameObject h in controlHelps)
            {
                if (controlHelps[helpNumber - 1] == h) h.SetActive(true);
                else h.SetActive(false);
            }
        }
    }
}
