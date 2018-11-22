using UnityEngine;

public class CustomLevelData : MonoBehaviour
{
    private static CustomLevelData instance;

    private string customLevelName = null;

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
    }

    public void SetCustomLevelName(string name)
    {
        customLevelName = name;
    }

    public string GetCustomLevelName()
    {
        return customLevelName;
    }
}
