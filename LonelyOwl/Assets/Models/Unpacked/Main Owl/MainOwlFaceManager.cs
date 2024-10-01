using UnityEngine;

public class MainOwlFaceManager : MonoBehaviour
{
    public Texture2D neutral;
    public Texture2D blink;
    public Texture2D happy;
    public Material faceMaterial;

    public bool isHappy = false;

    private float blinkFrequency = 2.0f;
    private float blinkDuration = 0.2f;
    private float currentBlinkTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var TexToSet = neutral;
        if (isHappy)
        {
            TexToSet = happy;
        }
        else if(blinkFrequency <= currentBlinkTime)
        {
            TexToSet = blink;

            if(currentBlinkTime - blinkFrequency > blinkDuration)
            {
                currentBlinkTime = 0f;
            }
        }

        faceMaterial.mainTexture = TexToSet;

        currentBlinkTime += Time.deltaTime;
    }
}
