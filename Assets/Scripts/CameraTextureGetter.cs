using UnityEngine;

public class GetCameraTexture : MonoBehaviour
{
    public Camera sourceCamera;
    public Material targetMaterial;
    public string texturePropertyName = "_MainTex";

    private RenderTexture renderTexture;

    void Start()
    {
        Debug.Log(sourceCamera.targetTexture);
        renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        sourceCamera.targetTexture = renderTexture;
    }

    void Update()
    {
        targetMaterial.SetTexture(texturePropertyName, renderTexture);
    }

    void OnDestroy()
    {
        if (renderTexture != null)
        {
            renderTexture.Release();
            renderTexture = null;
        }
    }
}