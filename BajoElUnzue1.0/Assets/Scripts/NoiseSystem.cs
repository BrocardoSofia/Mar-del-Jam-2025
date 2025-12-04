using System.Collections.Generic;
using UnityEngine;

public class NoiseSystem : MonoBehaviour
{
    public static NoiseSystem Instance { get; private set; }

    public class NoiseEvent
    {
        public Vector3 pos;
        public float radius;
        public float time;
        public GameObject source;
        public bool isActive;
        public bool isFinal;
    }

    public List<NoiseEvent> noises = new List<NoiseEvent>();
    public float noiseLifetime = 8f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void RegisterNoise(Vector3 pos, float radius, GameObject source = null)
    {
        if (source != null)
        {
            var existing = noises.Find(n => n.source == source);
            if (existing != null)
            {
                existing.pos = pos;
                existing.radius = radius;
                existing.time = Time.time;
                existing.isActive = true;
                existing.isFinal = false;
                return;
            }
        }

        noises.Add(new NoiseEvent
        {
            pos = pos,
            radius = radius,
            time = Time.time,
            source = source,
            isActive = true,
            isFinal = false
        });
    }

    public void StopNoise(GameObject source)
    {
        if (source == null) return;
        var existing = noises.Find(n => n.source == source);
        if (existing != null)
        {
            existing.isActive = false;
            existing.isFinal = true;
            existing.time = Time.time;
        }
    }

    public void ConsumeNoiseAtPosition(Vector3 pos, float threshold = 0.5f)
    {
        noises.RemoveAll(n => Vector3.Distance(n.pos, pos) <= threshold);
    }

    void Update()
    {
        noises.RemoveAll(n => (Time.time - n.time) > noiseLifetime);
    }
}