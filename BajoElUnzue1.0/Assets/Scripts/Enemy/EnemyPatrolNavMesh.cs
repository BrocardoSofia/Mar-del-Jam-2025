using System.Collections;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyPatrolNavMeshWithHearing : MonoBehaviour
{
    [Header("Patrol")]
    public Transform[] waypoints;
    public float patrolSpeed = 3.5f;
    public float waitTimeAtPoint = 1.5f;

    public PlayerMusic music;

    [Header("Hearing")]
    public float hearingRadius = 12f;
    public float chaseSpeed = 6f;

    [Header("Attack")]
    public Vector3 attackBoxCenter = new Vector3(0f, 0.5f, 1.5f);
    public Vector3 attackBoxSize = new Vector3(1.2f, 1.0f, 2.5f);
    public LayerMask attackMask;
    public LayerMask attackObject;
    public float attackCooldown = 1.0f;

    [Header("Audio")]
    public AudioClip[] walkSounds;
    public AudioClip[] runSounds;
    public AudioClip attackSound;
    public AudioSource audioSource;

    public AudioClip atackSound;
    public AudioClip ambienteSound;

    public Image fadeImage;
    public float fadeDuration = 1.5f;

    public Image atackUIImage;

    public PlayerCamera playerCamera;
    public Camera cam;
    public Transform targetPoint;
    public float duration = 1f;

    [Header("Animación")]
    public Animation animator;

    // Step sounds
    public float walkStepInterval = 0.8f;
    public float runStepInterval = 0.4f;
    private float stepTimer = 0f;

    private NavMeshAgent agent;
    private int currentIndex = 0;
    private float waitTimer = 0f;

    private Vector3? lastHeardPosition = null;
    private bool isInvestigating = false;
    private bool isAttacking = false;
    private float lastAttackTime = -999f;
    private bool playerDead = false;

    private bool forcedWatching = false;
    private int iniciaAtaque = 0;  // 0 = sonido ambiente activo, 1 = sonido ataque activo

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        agent.speed = patrolSpeed;
    }

    void OnEnable()
    {
        if (waypoints != null && waypoints.Length > 0)
            GoToCurrentWaypoint();
    }

    void Update()
    {
        if (playerDead) return;

        HandleFootsteps();
        UpdateAnimations();

        if (isAttacking)
        {
            if (CheckAttackHitbox(out Collider[] hits) && Time.time - lastAttackTime >= attackCooldown)
                OnDetectTargets(hits);

            return;
        }

        // Detect hearing
        bool heardAny = false;

        if (NoiseSystem.Instance != null)
        {
            var noises = NoiseSystem.Instance.noises
                .Where(n => Vector3.Distance(transform.position, n.pos) <= hearingRadius)
                .ToList();

            if (noises.Count > 0)
            {
                heardAny = true;

                var nearest = noises.OrderBy(n => Vector3.Distance(transform.position, n.pos)).First();
                lastHeardPosition = nearest.pos;
                isInvestigating = true;

                agent.isStopped = false;
                agent.speed = chaseSpeed;
                agent.SetDestination(nearest.pos);
            }
        }

        // Investigating / chasing
        if (isInvestigating)
        {
            if (lastHeardPosition.HasValue)
            {
                if (CheckAttackHitbox(out Collider[] hitsWhileMoving))
                {
                    OnDetectTargets(hitsWhileMoving);
                    return;
                }

                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    TryAttack();
                    isInvestigating = false;
                    NoiseSystem.Instance.RemoveAllNoice();
                    EndAttack();
                }
            }

            return;
        }

        // Patrol
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTimeAtPoint)
            {
                currentIndex = (currentIndex + 1) % waypoints.Length;
                GoToCurrentWaypoint();
                waitTimer = 0f;
            }
        }
    }

    // -------------- ANIMACIONES + MÚSICA -----------------

    void UpdateAnimations()
    {
        if (animator == null) return;

        float speed = agent.velocity.magnitude;

        // Ataque
        if (isAttacking)
        {
            animator.Play("atack_rata");
            return;
        }

        // Persecución real → isInvestigating
        if (isInvestigating)
        {
            ActivarMusicaAtaque();
            MostrarImagen();
            animator.Play("run_rata");
            return;
        }

        // Caminando
        if (speed > 0.1f)
        {
            ActivarMusicaAmbiente();
            OcultarImagen();
            animator.Play("walk_rata");
            return;
        }

        // Idle
        ActivarMusicaAmbiente();
        OcultarImagen();
        animator.Play("watch_rata");
    }

    // ----------------- MÚSICA -------------------

    void ActivarMusicaAtaque()
    {
        if (iniciaAtaque == 1) return;
        iniciaAtaque = 1;

        MusicManager.Instance.PlayMusic(atackSound);
    }

    void ActivarMusicaAmbiente()
    {
        if (iniciaAtaque == 0) return;
        iniciaAtaque = 0;

        MusicManager.Instance.PlayMusic(ambienteSound);
    }

    // ----------------- UI IMAGEN -------------------

    public void MostrarImagen()
    {
        Color c = atackUIImage.color;
        c.a = 1f;
        atackUIImage.color = c;
    }

    public void OcultarImagen()
    {
        Color c = atackUIImage.color;
        c.a = 0f;
        atackUIImage.color = c;
    }

    // ---------------- ATAQUE --------------------

    bool CheckAttackHitbox(out Collider[] hitsFiltered)
    {
        Vector3 boxCenterWorld = transform.TransformPoint(attackBoxCenter);

        int mask = (attackMask | attackObject);
        if (mask == 0) mask = ~0;

        Collider[] rawHits = Physics.OverlapBox(
            boxCenterWorld,
            attackBoxSize * 0.5f,
            transform.rotation,
            mask
        );

        hitsFiltered = rawHits
            .Where(c => c != null && c.gameObject != gameObject)
            .ToArray();

        return hitsFiltered.Length > 0;
    }

    void OnDetectTargets(Collider[] hits)
    {
        if (isAttacking && Time.time - lastAttackTime < attackCooldown) return;

        bool playerHit = hits.Any(c => c.gameObject.layer == LayerMask.NameToLayer("Player"));

        if (playerHit)
        {
            isAttacking = true;
            lastAttackTime = Time.time;

            agent.isStopped = true;
            agent.ResetPath();
            isInvestigating = false;

            PerformAttack(hits);

            Invoke(nameof(EndAttack), 0.6f);
            return;
        }

        // Golpea objetos
        PerformAttack(hits);

        // Vuelve a patrullar
        ResetToPatrol();
    }

    void PerformAttack(Collider[] hits)
    {
        foreach (var c in hits)
        {
            if (c == null) continue;

            if (c.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                if (attackSound != null && !playerDead)
                    audioSource.PlayOneShot(attackSound);

                playerDead = true;

                var movement = c.GetComponent<PlayerMovement>();
                if (movement != null) movement.enabled = false;

                playerCamera.enabled = false;

                music.reproducirFinalMalo();
                StartCoroutine(RotateToTarget());
                StartCoroutine(FadeAndLoadScene("00.GameOver"));
            }
            else
            {
                c.gameObject.layer = LayerMask.NameToLayer("Interactable");

                if (attackSound != null)
                    audioSource.PlayOneShot(attackSound);

                if (NoiseSystem.Instance != null)
                    NoiseSystem.Instance.noises.Clear();
            }
        }
    }

    void EndAttack()
    {
        isAttacking = false;
        ResetToPatrol();
    }

    void TryAttack()
    {
        //7segundos
        if (CheckAttackHitbox(out Collider[] hits))
        {
            OnDetectTargets(hits);
        }
        else
        {
            Debug.Log("TryAttack: no se encontraron objetivos al llegar.");
        }

        
    }



    // -------------- RESET GENERAL ------------------

    void ResetToPatrol()
    {
        isInvestigating = false;
        lastHeardPosition = null;

        ActivarMusicaAmbiente();
        OcultarImagen();

        agent.isStopped = false;
        agent.speed = patrolSpeed;

        if (waypoints != null && waypoints.Length > 0)
            GoToCurrentWaypoint();
    }

    // ---------------- UTILIDADES -------------------

    void GoToCurrentWaypoint()
    {
        agent.SetDestination(waypoints[currentIndex].position);
    }

    void HandleFootsteps()
    {
        if (!agent.hasPath || agent.velocity.magnitude < 0.1f) return;

        float interval = (agent.speed >= chaseSpeed * 0.9f) ? runStepInterval : walkStepInterval;

        stepTimer += Time.deltaTime;

        if (stepTimer >= interval)
        {
            stepTimer = 0f;

            AudioClip[] clips = (agent.speed >= chaseSpeed * 0.9f) ? runSounds : walkSounds;

            if (clips.Length > 0)
                audioSource.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Length)]);
        }
    }

    IEnumerator RotateToTarget()
    {
        Quaternion from = cam.transform.rotation;
        Quaternion to = Quaternion.LookRotation(targetPoint.position - cam.transform.position);

        float t = 0f;
        while (t < 1f)
        {
            cam.transform.rotation = Quaternion.Slerp(from, to, t);
            t += Time.deltaTime / duration;
            yield return null;
        }

        cam.transform.rotation = to;
    }

    IEnumerator FadeAndLoadScene(string sceneName)
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < 1f)
        {
            c.a = Mathf.Lerp(0f, 1f, t);
            fadeImage.color = c;
            t += Time.deltaTime / fadeDuration;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;

        SceneManager.LoadScene(sceneName);
    }
}
