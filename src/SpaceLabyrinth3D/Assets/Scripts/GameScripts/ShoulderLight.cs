using UnityEngine;

namespace GameScripts
{
    public class ShoulderLight : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetButtonDown("ToggleLight"))
            {
                GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
            }
        }
    }
}
