using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private List<GameObject> _dropItems;
    [SerializeField] private GameObject _player;
    [SerializeField] private EnemyType _type;
    [SerializeField] private EnemyStats _stats; // Enemy 스택 데이터들

    private NavMeshAgent _agent;
    private Animator _animator;
  
    private StateMachine<BaseEnemy> _fsm; 
    private StateFactory<BaseEnemy> _stateFactory; // 상태들 캐싱

    // 피격용
    private SpriteRenderer _spriteRenderer;
    private bool _isHit;

    #region Getters
    public NavMeshAgent GetAgent() => _agent;
    public Animator GetAnimator() => _animator;
    public StateMachine<BaseEnemy> GetFSM() => _fsm;
    public EnemyStats Stats => _stats;
    public StateFactory<BaseEnemy> StateFactory => _stateFactory;
    // Player는 나중에 지우기
    public GameObject GetPlayer() => _player;
    public EnemyType GetEnemyType() => _type;

    // Animal 관련
    public virtual AnimalStats AnimalStats => null;
    #endregion


    protected virtual void Awake()
    {
        Init();
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
        FaceMoveDirection();
    }

    // 초기화
    private void Init()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _fsm = new StateMachine<BaseEnemy>(this);
        _stateFactory = new StateFactory<BaseEnemy>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 플레이어 범위 안에 있는지 체크
    public bool PlayerInRange()
    {
        Vector3 center = transform.position;
        float radius = _stats.DetectDistance;

        bool hit = Physics.CheckSphere(center, radius, LayerMask.GetMask("Player"));
        return hit;
    }

    // 적 피해 처리
    public void TakeDamage(int damage)
    {
        Stats.Health -= damage;
        StartCoroutine(HitColor(_spriteRenderer)); // 피격 효과
        if (Stats.Health <= 0)
        {
            if(TryGetComponent(out PoolAnimal poolAnimal))
            {
                poolAnimal.Die();
                DropItem();
            }
            _fsm.ChangeState(StateFactory.Get<DieState>());
        }
    }

    // 적 사망시 아이템 드롭 처리
    public void DropItem()
    {
        for (int i = 0; i < _dropItems.Count; i++)
        {
            //Instantiate(_dropItems[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }
    }

    // 이동 방향을 기준으로 회전 처리
    public void FaceMoveDirection()
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
        

        Vector3 direction = _agent.steeringTarget - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);

        float angle = Vector3.Angle(transform.forward, direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1f);
    }
    public void FaceTarget(Vector3 target, float turnSpeed = 5f)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.01f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);
    }
    // 피격 효과
    private IEnumerator HitColor(SpriteRenderer spriteRenderer)
    {
        _isHit = true;
        Color original = _spriteRenderer.color;
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = original;
        _isHit = false;
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

        // 실패 시 제자리 유지
        _agent.SetDestination(transform.position);
        return false;
    }
}
