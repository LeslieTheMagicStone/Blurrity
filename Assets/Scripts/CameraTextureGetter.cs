using UnityEngine;

public class CameraViewSprite : MonoBehaviour
{
    public Camera sourceCamera;
    public Material copyMaterial;

    private MeshRenderer meshRenderer;
    private RenderTexture renderTexture;
    private Material material;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        sourceCamera.targetTexture = renderTexture;

        material = new Material(copyMaterial);
        meshRenderer.material = material;
        material.SetTexture("_MainTex", renderTexture);
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