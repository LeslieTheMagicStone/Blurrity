using UnityEngine;

public class CameraViewSprite : MonoBehaviour
{
    public Camera sourceCamera;
    private SpriteRenderer spriteRenderer;

    private RenderTexture renderTexture;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        sourceCamera.targetTexture = renderTexture;
    }

    void Update()
    {
        spriteRenderer.sprite = CreateSprite();
    }

    void OnDestroy()
    {
        if (renderTexture != null)
        {
            renderTexture.Release();
            renderTexture = null;
        }
    }

    private Sprite CreateSprite()
    {
        Texture2D texture = new(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        return sprite;
    }
}