using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerMovement movement;
    public SFXManager soundMaker;
    public Transform rayOrigin;
    public float rayDistance = 1.5f;
    public LayerMask groundMask;

    [Header("Superficies")]
    public LayerMask cementMask;
    public LayerMask woodMask;
    public LayerMask tileMask;

    [Header("Sonidos por superficie")]
    public AudioClip walkCement;
    public AudioClip runCement;
    public AudioClip crouchCement;

    public AudioClip walkWood;
    public AudioClip runWood;
    public AudioClip crouchWood;

    public AudioClip walkTile;
    public AudioClip runTile;
    public AudioClip crouchTile;

    private MovementMode previousMode = MovementMode.Idle;
    private SurfaceType previousSurface = SurfaceType.Cement;

    private void Awake()
    {
        if (movement == null) movement = GetComponent<PlayerMovement>();
        if (rayOrigin == null) rayOrigin = transform;
    }

    private void Update()
    {

        if (movement != null)
        {
            MovementMode currentMode = CalculateMode(movement);
            SurfaceType currentSurface = DetectSurface();

            if (currentMode != previousMode || currentSurface != previousSurface)
            {
                SwitchAudio(previousMode, currentMode, previousSurface, currentSurface);
                previousMode = currentMode;
                previousSurface = currentSurface;
            }
        }
            
    }

    private MovementMode CalculateMode(PlayerMovement m)
    {
        if (!m.IsMoving) return MovementMode.Idle;
        if (m.IsCrouching) return MovementMode.CrouchWalk;
        return m.IsRunning ? MovementMode.Run : MovementMode.Walk;
    }

    private SurfaceType DetectSurface()
    {
        Vector3 origin = rayOrigin.position + Vector3.up * 0.1f;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayDistance, groundMask, QueryTriggerInteraction.Ignore))
        {
            int layer = hit.collider.gameObject.layer;

            if (IsInMask(layer, cementMask)) return SurfaceType.Cement;
            if (IsInMask(layer, woodMask)) return SurfaceType.Wood;
            if (IsInMask(layer, tileMask)) return SurfaceType.Tile;
        }

        return SurfaceType.Cement; // fallback
    }

    private bool IsInMask(int layer, LayerMask mask)
    {
        return (mask.value & (1 << layer)) != 0;
    }

    private void SwitchAudio(MovementMode fromMode, MovementMode toMode, SurfaceType fromSurface, SurfaceType toSurface)
    {
        soundMaker.playerStopSoundLoop();

        if (toMode == MovementMode.Idle) return;

        AudioClip clip = GetClipFor(toMode, toSurface);
        if (clip != null)
        {
            soundMaker.playerSoundLoopRandomStart(clip);
        }
        else
        {
            Debug.LogWarning($"Clip faltante para modo {toMode} y superficie {toSurface}", this);
        }
    }

    private AudioClip GetClipFor(MovementMode mode, SurfaceType surface)
    {
        switch (surface)
        {
            case SurfaceType.Cement:
                switch (mode)
                {
                    case MovementMode.Walk: return walkCement;
                    case MovementMode.Run: return runCement;
                    case MovementMode.CrouchWalk: return crouchCement;
                }
                break;

            case SurfaceType.Wood:
                switch (mode)
                {
                    case MovementMode.Walk: return walkWood;
                    case MovementMode.Run: return runWood;
                    case MovementMode.CrouchWalk: return crouchWood;
                }
                break;

            case SurfaceType.Tile:
                switch (mode)
                {
                    case MovementMode.Walk: return walkTile;
                    case MovementMode.Run: return runTile;
                    case MovementMode.CrouchWalk: return crouchTile;
                }
                break;
        }
        return null;
    }

    private enum MovementMode { Idle, Walk, Run, CrouchWalk }
    private enum SurfaceType { Cement, Wood, Tile }
}