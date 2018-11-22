using UnityEngine;

public class CustomLevelLauncher : MonoBehaviour
{
    public LoadSceneOnClick sceneLoader;

    private CustomLevelData _customLevelData;
    

    void Start ()
    {
        _customLevelData = FindObjectOfType<CustomLevelData>();
    }
	
    public void DisplayLevelPicker()
    {
        ModalFilePicker.InstantiateModalDialog("OpenFileDialog", "Choose a file to open", "Open", false, OpenLevel);
    }

    private void OpenLevel(string name)
    {
        _customLevelData.SetCustomLevelName(name);
        sceneLoader.LoadByIndex(1);
    }
}
