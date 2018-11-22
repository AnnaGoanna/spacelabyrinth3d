using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    public GameObject menu;
    public KeyCode key;

    private void Update()
    {
        if (Input.GetKeyDown(key)) menu.SetActive(!menu.activeSelf);
    }
}
