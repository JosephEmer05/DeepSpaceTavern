using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    private string hexColor = "#FF0000";

    private List<Material> materials = new List<Material>();
    private Color color;

    private void Awake()
    {
        if (!ColorUtility.TryParseHtmlString(hexColor, out color))
        {
            Debug.LogError("Invalid hex color format! Defaulting to white.");
            color = Color.white;
        }

        Renderer[] foundRenderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in foundRenderers)
        {
            materials.AddRange(renderer.materials);
        }
    }

    public void ToggleHighlight(bool val)
    {
        if (val)
        {
            foreach (var material in materials)
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", color);
            }
        }
        else
        {
            foreach (var material in materials)
            {
                material.DisableKeyword("_EMISSION");
            }
        }
    }
}
