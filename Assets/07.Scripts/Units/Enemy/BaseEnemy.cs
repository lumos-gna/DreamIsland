using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Attack,
    Flee
}

public class BaseEnemy : MonoBehaviour, IPoolableEnemy
{
    [SerializeField] private List<ItemData> _dropItems;
    [SerializeField] private EnemyType _type;
    [SerializeField] private EnemyStats _stats; // Enemy 스텟 데이터들
    [SerializeField] private ParticleSystem _footParticle;

    private NavMeshAgent _agent;
    private Animator _animator;
    private GameObject _player;
    private StateMachine<BaseEnemy> _fsm; 
    private StateFactory<BaseEnemy> _stateFactory; // 상태들 캐싱


    // 피격용
    private EnemyHealth _enemyHealth;
    public event Action<IPoolableEnemy> OnRespawn;

    #region Getters
    public NavMeshAgent GetAgent() => _agent;
    public Transform GetPlayerTransform() => _player.transform;
    public Animator GetAnimator() => _animator;
    public StateMachine<BaseEnemy> GetFSM() => _fsm;
    public EnemyStats Stats => _stats;
    public StateFactory<BaseEnemy> StateFactory => _stateFactory;
    // Player는 나중에 지우기
    public GameObject GetPlayer() => _player;
    public EnemyType GetEnemyType() => _type;
    public EnemyHealth GetEnemyHealth() => _enemyHealth;

    // 적 관련
    public virtual FleeEnemyStats FleeEnemyStats => null;
    public virtual AttackEnemyStats AttackEnemyStats => null;
    #endregion


    protected virtual void Awake()
    {
        Init();
        //_player = PlayerManager.Instance._Player.gameObject;
        if (_player == null)
        {
            var found = GameObject.FindWithTag("Player");
            if (found != null)
                _player = found;
        }
    }

    protected virtual void Start()
    {
        if (_type == EnemyType.Attack)
            _fsm.ChangeState(StateFactory.Get<MoveState>());
        else
            _fsm.ChangeState(StateFactory.Get<IdleState>());
    }

    protected virtual void Update()
    {
        _fsm.Update();
        FaceDirection();
    }

    // 초기화
    private void Init()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _fsm = new StateMachine<BaseEnemy>(this);
        _stateFactory = new StateFactory<BaseEnemy>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    //스폰 혹은 리스폰 직후 FSM을 최초 진입 상태로 되돌림
    public void ResetState()
    {
        if (_type == EnemyType.Attack)
            _fsm.ChangeState(StateFactory.Get<MoveState>());
        else
            _fsm.ChangeState(StateFactory.Get<IdleState>());
    }


    // 플레이어 범위 안에 있는지 체크
    public bool PlayerInRange()
    {
        Vector3 center = transform.position;
        float radius = _stats.DetectDistance;

        Collider[] hits = Physics.OverlapSphere(center, radius, LayerMask.GetMask("Player"));
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player")) // 태그로 확정
                return true;
        }

        return false;
    }

    // 적 피해 처리
    public void TakeDamage(int damage)
    {
        _enemyHealth.ApplyDamage(damage);
        
    }

    // 적 사망시 아이템 드롭 처리
    public void DropItem()
    {
        for (int i = 0; i < _dropItems.Count; i++)
        {
            //Instantiate(_dropItems[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }
    }

    // 적이 플레이어 바라보게기 회전 처리
    public void FaceDirection()
    {
        if (_agent == null || !_agent.hasPath || _agent.velocity.sqrMagnitude < 0.01f)
        {
            // 이동 중이 아니면 공격 상태일 수 있으니 플레이어 바라보게
            if (GetFSM().CurrentState is EnemyAttackState)
            {
                FaceTarget(GetPlayer().transform.position);
            }
            
            return;
        }

        FaceTarget(_agent.steeringTarget, 1f);
    }
    public void FaceTarget(Vector3 target, float turnSpeed = 5f)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.01f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);
    }

    // NavMesh에서 유요한 위치인지 체크 및 경로 설정
    public bool TrySetDestination(Vector3 target)
    {
        if (_agent == null || !_agent.isOnNavMesh)
            return false;

        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            NavMeshPath path = new NavMeshPath();
            if (_agent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                _agent.SetDestination(hit.position);
                return true;
            }
        }

        // 실패시 주변 탐색 무작위로 주변 탐색
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * 1.5f;
            Vector3 candidate = transform.position + randomOffset;
            if (NavMesh.SamplePosition(candidate, out NavMeshHit nearbyHit, 1.5f, NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();
                if (_agent.CalculatePath(nearbyHit.position, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    _agent.SetDestination(nearbyHit.position);
                    return true;
                }
            }
        }

        // 그래도 안 되면 이동하지 않음
        return false;
    }

    public void OnSpawn() => gameObject.SetActive(true);

    public void OnDespawn() => gameObject.SetActive(false);

    public void Die()
    {
        QuestCheck();
        DropItem();
        OnDespawn();
        OnRespawn?.Invoke(this);
    }


    public void PlayFootParticle()
    {
        if (_footParticle != null && !_footParticle.isPlaying)
        {
            _footParticle.Play();
        }
    }

    public void StopFootParticle()
    {
        if (_footParticle != null && _footParticle.isPlaying)
            _footParticle.Stop();
    }

    private void QuestCheck()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            string controllerName = animator.runtimeAnimatorController.name;
            QuestManager.Instance.QuestPlusCount(controllerName);
        }

        
    }

}
