using UnityEngine;

public class SkinManager : MonoBehaviour
{
    private readonly string[] _textures =
    {
        "Textures/Sci-Fi Texture Pack 1/Materials/Texture_4",
        "Textures/UltimateFreeTextures/Textures/Materials/Stone24",
        "Textures/UltimateFreeTextures/Textures/Materials/organic40"
    };

    private readonly string _defaultTexture =
        "Textures/Metal Floor (Rust Low)/Material/Metal_floor (Rust Low)";

    public Material GetLevelMaterial(int levelNumber, bool useDefault = false)
    {
        string textureName;
        if (useDefault) textureName = _defaultTexture;
        else textureName = _textures[levelNumber];

        return (Material) Resources.Load(textureName, typeof(Material));
    }
}
