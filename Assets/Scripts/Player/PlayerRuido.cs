using UnityEngine;

public class PlayerRuido : MonoBehaviour
{
    public bool ruido = false;
    public float noiseRadius = 8f;

    public void haceRuido(float radiusMultiplier, Vector3 position)
    {
        ruido = true;
        float radius = noiseRadius * radiusMultiplier;
        NoiseSystem.Instance.RegisterNoise(position, radius, gameObject);
    }

    public void dejaDeHacerRuido()
    {
        ruido = false;
        NoiseSystem.Instance.StopNoise(gameObject);
    }
}