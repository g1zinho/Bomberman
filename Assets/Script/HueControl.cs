using UnityEngine;

public class HueControl : MonoBehaviour
{
    // Reference to the Material using the HueShift shader
    public Material hueMaterial;

    // Exposed slider in the Inspector to control hue shift (0 to 1)
    [Range(0f, 1f)]
    public float hueShift = 0f;
    public float red = 1f;
    public float green = 1f;
    public float blue = 1f;

 void Update()
{
    if (hueMaterial != null)
    {
        hueMaterial.SetFloat("_HueShift", hueShift);
        hueMaterial.SetFloat("_RMult", red);
        hueMaterial.SetFloat("_GMult", green);
        hueMaterial.SetFloat("_BMult", blue);
    }
}
}
