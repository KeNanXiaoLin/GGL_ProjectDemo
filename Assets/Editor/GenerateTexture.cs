using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateTexture : EditorWindow
{
    private int textureSize = 256;
    private Color textureColor = Color.white;
    private Texture2D generatedTexture;

    [MenuItem("Tools/Generate Texture")]
    public static void ShowWindow()
    {
        GetWindow<GenerateTexture>("Generate Texture");
    }

    private void OnGUI()
    {
        GUILayout.Label("Texture Settings", EditorStyles.boldLabel);

        // Texture size input
        textureSize = EditorGUILayout.IntField("Texture Size", textureSize);
        textureSize = Mathf.Clamp(textureSize, 16, 4096);

        // Color picker
        textureColor = EditorGUILayout.ColorField("Texture Color", textureColor);

        GUILayout.Space(10);

        // Generate button
        if (GUILayout.Button("Generate Texture"))
        {
            GenerateNewTexture();
        }

        // Save button (only enabled if texture is generated)
        GUI.enabled = generatedTexture != null;
        if (GUILayout.Button("Save Texture"))
        {
            SaveTexture();
        }
        GUI.enabled = true;

        GUILayout.Space(10);

        // Preview area
        if (generatedTexture != null)
        {
            GUILayout.Label("Texture Preview");
            GUILayout.Box(generatedTexture, GUILayout.Width(200), GUILayout.Height(200));
        }
    }

    private void GenerateNewTexture()
    {
        // Create new texture
        generatedTexture = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);
        generatedTexture.name = "GeneratedTexture";

        // Fill texture with color
        for (int x = 0; x < textureSize; x++)
        {
            for (int y = 0; y < textureSize; y++)
            {
                generatedTexture.SetPixel(x, y, textureColor);
            }
        }

        // Apply changes
        generatedTexture.Apply();

        Debug.Log("Texture generated: " + textureSize + "x" + textureSize + ", Color: " + textureColor);
    }

    private void SaveTexture()
    {
        if (generatedTexture == null) return;

        // Get path to save
        string path = EditorUtility.SaveFilePanelInProject("Save Texture", "GeneratedTexture", "png", "Please enter a file name");

        if (!string.IsNullOrEmpty(path))
        {
            // Encode texture to PNG
            byte[] bytes = generatedTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(path, bytes);

            // Import the texture back into Unity
            AssetDatabase.ImportAsset(path);
            Debug.Log("Texture saved to: " + path);
        }
    }
}