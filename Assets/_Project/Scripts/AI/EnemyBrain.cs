using System.Collections;
using HuyetNguyetDaiNgan.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace HuyetNguyetDaiNgan.AI
{
    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack,
        Return,
        Dead
    }

    /// <summary>
    /// Trí tuệ nhân tạo (AI) cơ bản cho quái vật / Sói Bậc 1 bằng FSM và NavMeshAgent.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyBrain : MonoBehaviour
    {
        [Header("AI State")]
        [SerializeField] private EnemyState currentState = EnemyState.Patrol;

        [Header("Detection & Combat Radii")]
        [Tooltip("Bán kính phát hiện người chơi (m)")]
        [SerializeField] private float detectRadius = 9.0f;

        [Tooltip("Tầm tấn công cận chiến (m)")]
        [SerializeField] private float attackRange = 1.8f;

        [Tooltip("Khoảng cách người chơi chạy thoát thì quái ngừng đuổi (m)")]
        [SerializeField] private float losePlayerDistance = 16.0f;

        [Tooltip("Bán kính đi tuần tra quanh vị trí ban đầu (m)")]
        [SerializeField] private float patrolRadius = 6.0f;

        [Header("Combat Stats")]
        [SerializeField] private float attackDamage = 12.0f;
        [SerializeField] private float attackCooldown = 1.5f;

        [Header("Speeds")]
        [SerializeField] private float patrolSpeed = 2.5f;
        [SerializeField] private float chaseSpeed = 5.5f;

        // References
        private NavMeshAgent _agent;
        private HealthComponent _health;
        private Transform _playerTransform;
        private IDamageable _playerDamageable;
        private Vector3 _spawnOrigin;
        private float _lastAttackTime = -99f;
        private float _patrolTimer;

        public EnemyState CurrentState => currentState;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _health = GetComponent<HealthComponent>();
            _spawnOrigin = transform.position;
        }

        private void OnEnable()
        {
            if (_health != null)
            {
                _health.OnDied += HandleDeath;
            }
        }

        private void OnDisable()
        {
            if (_health != null)
            {
                _health.OnDied -= HandleDeath;
            }
        }

        private void Start()
        {
            FindPlayer();
            SetState(EnemyState.Patrol);
        }

        private void FindPlayer()
        {
            var playerObj = GameObject.FindWithTag("Player");
            if (playerObj == null)
            {
                var controller = Object.FindAnyObjectByType<Player.PlayerMovementController>();
                if (controller != null) playerObj = controller.gameObject;
            }

            if (playerObj != null)
            {
                _playerTransform = playerObj.transform;
                _playerDamageable = playerObj.GetComponent<IDamageable>();
            }
        }

        private void Update()
        {
            if (currentState == EnemyState.Dead) return;

            if (_playerTransform == null)
            {
                FindPlayer();
                if (_playerTransform == null) return;
            }

            float distToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

            switch (currentState)
            {
                case EnemyState.Patrol:
                    UpdatePatrol(distToPlayer);
                    break;

                case EnemyState.Chase:
                    UpdateChase(distToPlayer);
                    break;

                case EnemyState.Attack:
                    UpdateAttack(distToPlayer);
                    break;

                case EnemyState.Return:
                    UpdateReturn(distToPlayer);
                    break;
            }
        }

        private void UpdatePatrol(float distToPlayer)
        {
            if (distToPlayer <= detectRadius && _playerDamageable != null && !_playerDamageable.IsDead)
            {
                SetState(EnemyState.Chase);
                return;
            }

            _patrolTimer -= Time.deltaTime;
            if (_patrolTimer <= 0f || (_agent.hasPath && _agent.remainingDistance < 0.5f))
            {
                _patrolTimer = Random.Range(3f, 6f);
                Vector3 randomPoint = _spawnOrigin + Random.insideUnitSphere * patrolRadius;
                randomPoint.y = _spawnOrigin.y;

                if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
                {
                    _agent.SetDestination(hit.position);
                }
            }
        }

        private void UpdateChase(float distToPlayer)
        {
            if (_playerDamageable != null && _playerDamageable.IsDead)
            {
                SetState(EnemyState.Return);
                return;
            }

            if (distToPlayer > losePlayerDistance)
            {
                SetState(EnemyState.Return);
                return;
            }

            if (distToPlayer <= attackRange)
            {
                SetState(EnemyState.Attack);
                return;
            }

            _agent.SetDestination(_playerTransform.position);
        }

        private void UpdateAttack(float distToPlayer)
        {
            if (_playerDamageable != null && _playerDamageable.IsDead)
            {
                SetState(EnemyState.Return);
                return;
            }

            if (distToPlayer > attackRange * 1.25f)
            {
                SetState(EnemyState.Chase);
                return;
            }

            // Xoay mặt về phía người chơi
            Vector3 lookDir = (_playerTransform.position - transform.position);
            lookDir.y = 0f;
            if (lookDir.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), 10f * Time.deltaTime);
            }

            // Ra đòn khi hết thời gian hồi
            if (Time.time >= _lastAttackTime + attackCooldown)
            {
                _lastAttackTime = Time.time;
                if (_playerDamageable != null && !_playerDamageable.IsDead)
                {
                    DamageInfo attackInfo = new DamageInfo(attackDamage, DamageType.Physical, gameObject, _playerTransform.position);
                    _playerDamageable.TakeDamage(attackInfo);
                }
            }
        }

        private void UpdateReturn(float distToPlayer)
        {
            if (distToPlayer <= detectRadius && _playerDamageable != null && !_playerDamageable.IsDead)
            {
                SetState(EnemyState.Chase);
                return;
            }

            if (!_agent.hasPath || _agent.remainingDistance < 0.5f)
            {
                SetState(EnemyState.Patrol);
            }
        }

        private void SetState(EnemyState newState)
        {
            currentState = newState;
            if (!_agent.isActiveAndEnabled || !_agent.isOnNavMesh) return;

            switch (newState)
            {
                case EnemyState.Patrol:
                    _agent.speed = patrolSpeed;
                    _agent.isStopped = false;
                    break;

                case EnemyState.Chase:
                    _agent.speed = chaseSpeed;
                    _agent.isStopped = false;
                    break;

                case EnemyState.Attack:
                    _agent.isStopped = true;
                    _agent.ResetPath();
                    break;

                case EnemyState.Return:
                    _agent.speed = patrolSpeed;
                    _agent.isStopped = false;
                    _agent.SetDestination(_spawnOrigin);
                    break;
            }
        }

        private void HandleDeath()
        {
            SetState(EnemyState.Dead);
            if (_agent != null && _agent.isOnNavMesh)
            {
                _agent.isStopped = true;
                _agent.enabled = false;
            }

            var col = GetComponent<Collider>();
            if (col != null) col.enabled = false;

            StartCoroutine(DeathDisappearRoutine());
        }

        private IEnumerator DeathDisappearRoutine()
        {
            // Chìm nhẹ xuống đất trước khi biến mất
            float timer = 0f;
            Vector3 startPos = transform.position;
            while (timer < 2.5f)
            {
                timer += Time.deltaTime;
                transform.position = startPos + Vector3.down * (timer * 0.4f);
                yield return null;
            }

            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}

