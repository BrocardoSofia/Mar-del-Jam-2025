using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Processors;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyPatrolNavMeshWithHearing : MonoBehaviour
{
    [Header("Patrol")]
    public Transform[] waypoints;
    public float patrolSpeed = 3.5f;
    public float waitTimeAtPoint = 1.5f;

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
    public AudioClip[] walkSounds;   // sonidos al patrullar
    public AudioClip[] runSounds;    // sonidos al correr
    public AudioClip attackSound;    // sonido al atacar
    public AudioSource audioSource;


    public Image fadeImage;
    public float fadeDuration = 1.5f;

    public PlayerCamera playerCamera;
    public Camera cam;
    public Transform targetPoint;
    public float duration = 1f;

    [Header("Animación (opcional)")]
    public Animator animator;
    public string paramSpeed = "Speed";
    public string paramIsRunning = "IsRunning";
    public string paramAttack = "IsAttacking";
    public string paramIsWaiting = "IsWaiting";

    // Intervalos de pasos
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

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) Debug.LogError("Falta NavMeshAgent en el enemigo.");
        agent.speed = patrolSpeed;

        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnEnable()
    {
        if (waypoints != null && waypoints.Length > 0) GoToCurrentWaypoint();
    }

    void Update()
    {
        if (playerDead) return;

        HandleFootsteps();
        UpdateAnimations(); // ← ANIMACIÓN

        if (isAttacking)
        {
            if (CheckAttackHitbox(out Collider[] hits) && Time.time - lastAttackTime >= attackCooldown)
            {
                OnDetectTargets(hits);
            }
            return;
        }

        if (CheckAttackHitbox(out Collider[] hitboxHits))
        {
            OnDetectTargets(hitboxHits);
            return;
        }

        bool heardAny = false;

        // --------- HEARING ---------
        if (NoiseSystem.Instance != null)
        {
            var heard = NoiseSystem.Instance.noises
                .Where(n => Vector3.Distance(transform.position, n.pos) <= hearingRadius)
                .ToList();

            if (heard.Count > 0)
            {
                heardAny = true;
                var nearest = heard.OrderBy(n => Vector3.Distance(transform.position, n.pos)).First();
                lastHeardPosition = nearest.pos;
                isInvestigating = true;

                agent.isStopped = false;
                agent.speed = chaseSpeed;
                agent.SetDestination(nearest.pos);
            }
        }

        // --------- INVESTIGACIÓN ---------
        if (!heardAny && isInvestigating && lastHeardPosition.HasValue)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(lastHeardPosition.Value);

            if (CheckAttackHitbox(out Collider[] hitsWhileMoving))
            {
                OnDetectTargets(hitsWhileMoving);
                return;
            }

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                TryAttack();
                lastHeardPosition = null;
                isInvestigating = false;
                agent.speed = patrolSpeed;

                GoToCurrentWaypoint();
            }
            return;
        }

        // --------- PATRULLA ---------
        agent.speed = Mathf.Lerp(agent.speed, patrolSpeed, 10f * Time.deltaTime);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;
            SafeAnimSet(paramIsWaiting, true); // ← ANIMACIÓN

            if (waitTimer >= waitTimeAtPoint)
            {
                currentIndex = (currentIndex + 1) % waypoints.Length;
                GoToCurrentWaypoint();
                waitTimer = 0f;
                SafeAnimSet(paramIsWaiting, false);
            }
        }
    }


    // ########### ANIMACIÓN principal ###########
    void UpdateAnimations()
    {
        float speed = agent.velocity.magnitude;

        SafeAnimSetFloat(paramSpeed, speed);
        SafeAnimSet(paramIsRunning, speed >= chaseSpeed * 0.7f);
    }

    void HandleFootsteps()
    {
        if (!agent.hasPath || agent.velocity.magnitude < 0.1f) return;

        float currentInterval = agent.speed >= chaseSpeed * 0.9f ? runStepInterval : walkStepInterval;
        stepTimer += Time.deltaTime;

        if (stepTimer >= currentInterval)
        {
            stepTimer = 0f;

            AudioClip[] clips = agent.speed >= chaseSpeed * 0.9f ? runSounds : walkSounds;
            if (clips != null && clips.Length > 0)
            {
                AudioClip clip = clips[Random.Range(0, clips.Length)];
                audioSource.PlayOneShot(clip);
            }
        }
    }

    void GoToCurrentWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        agent.isStopped = false;
        agent.SetDestination(waypoints[currentIndex].position);
    }

    bool CheckAttackHitbox(out Collider[] hitsFiltered)
    {
        Vector3 boxCenterWorld = transform.TransformPoint(attackBoxCenter);

        // Combina ambos LayerMask
        int mask = (attackMask | attackObject);
        if (mask == 0) mask = ~0; // fallback si ninguno está configurado

        Collider[] rawHits = Physics.OverlapBox(
            boxCenterWorld,
            attackBoxSize * 0.5f,
            transform.rotation,
            mask
        );

        hitsFiltered = rawHits
            .Where(c => c != null && c.gameObject != gameObject && !c.transform.IsChildOf(transform))
            .ToArray();

        return hitsFiltered.Length > 0;
    }

    void SafeAnimSet(string param, bool value)
    {
        if (animator == null) return;
        if (!animator.HasParameterOfType(param, AnimatorControllerParameterType.Bool)) return;
        animator.SetBool(param, value);
    }

    void SafeAnimSetFloat(string param, float value)
    {
        if (animator == null) return;
        if (!animator.HasParameterOfType(param, AnimatorControllerParameterType.Float)) return;
        animator.SetFloat(param, value);
    }

    void SafeAnimTrigger(string param)
    {
        if (animator == null) return;
        if (!animator.HasParameter(param)) return;
        animator.SetTrigger(param);
    }

    void OnDetectTargets(Collider[] hits)
    {
        if (isAttacking && Time.time - lastAttackTime < attackCooldown) return;

        // Revisar si hay Player en los hits
        bool playerHit = hits.Any(c => c != null &&
                                       (c.gameObject.layer == LayerMask.NameToLayer("Player") || c.CompareTag("Player")));

        if (playerHit)
        {
            agent.isStopped = true;
            agent.ResetPath();
            isInvestigating = false;
            lastHeardPosition = null;

            isAttacking = true;
            lastAttackTime = Time.time;
            PerformAttack(hits);

            float attackDuration = 0.6f;
            Invoke(nameof(EndAttack), attackDuration);
        }
        else
        {
            // Si no es Player, simplemente atacar y seguir
            PerformAttack(hits);
            // Reactivar patrulla
            agent.isStopped = false;
            agent.speed = patrolSpeed;
            if (waypoints != null && waypoints.Length > 0) GoToCurrentWaypoint();
        }
    }
    void EndAttack()
    {
        isAttacking = false;
        agent.isStopped = false;
        agent.speed = patrolSpeed;
        if (waypoints != null && waypoints.Length > 0) GoToCurrentWaypoint();
    }

    void TryAttack()
    {
        if (CheckAttackHitbox(out Collider[] hits))
        {
            OnDetectTargets(hits);
        }
        else
        {
            Debug.Log("TryAttack: no se encontraron objetivos al llegar.");
        }
    }

    void PerformAttack(Collider[] hits)
    {
        foreach (var c in hits)
        {
            if (c == null) continue;

            if (c.gameObject.layer == LayerMask.NameToLayer("Player") || c.CompareTag("Player"))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                if (attackSound != null && !playerDead)
                    audioSource.PlayOneShot(attackSound);

                playerDead = true;
                var movement = c.GetComponent<PlayerMovement>();
                if (movement != null) movement.enabled = false;

                playerCamera.enabled = false;

                StartCoroutine(RotateToTarget());
                StartCoroutine(FadeAndLoadScene("00.GameOver"));
            }
            else
            {
                Debug.Log("1");
                c.gameObject.layer = LayerMask.NameToLayer("Interactable");

                if (attackSound != null)
                    audioSource.PlayOneShot(attackSound);

                if (NoiseSystem.Instance != null)
                {
                    Debug.Log("2");
                    NoiseSystem.Instance.noises.Clear();
                }
            }
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


    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, hearingRadius);

        Gizmos.color = Color.red;
        Vector3 boxCenterWorld = transform.TransformPoint(attackBoxCenter);
        Gizmos.matrix = Matrix4x4.TRS(boxCenterWorld, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, attackBoxSize);
        Gizmos.matrix = Matrix4x4.identity;
    }
}

public static class AnimatorExtensions
{
    public static bool HasParameter(this Animator animator, string paramName)
    {
        if (animator == null) return false;
        return animator.parameters.Any(p => p.name == paramName);
    }

    public static bool HasParameterOfType(this Animator animator, string paramName, AnimatorControllerParameterType type)
    {
        if (animator == null) return false;
        return animator.parameters.Any(p => p.name == paramName && p.type == type);
    }
}