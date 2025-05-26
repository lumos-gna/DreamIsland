using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private List<GameObject> _dropItems;

    private NavMeshAgent _agent;
    private Animator _animator;
    private GameObject _player;

    private EnemyStats _stats; // Enemy 스택 데이터들
    private StateMachine<BaseEnemy> _fsm; 
    private StateFactory<BaseEnemy> _stateFactory; // 상태들 캐싱

    #region Getters
    public NavMeshAgent GetAgent() => _agent;
    public Animator GetAnimator() => _animator;
    public StateMachine<BaseEnemy> GetFSM() => _fsm;
    public EnemyStats Stats => _stats;
    public StateFactory<BaseEnemy> StateFactory => _stateFactory;
    public Transform GetPlayerTransform() => _player.transform;
    public Transform GetEnemyTransform() => this.transform;
    public float GetAttackPower() => _stats.AttackPower;
    #endregion


    protected virtual void Awake()
    {
        Init();
    }

    protected virtual void Start()
    {
        _fsm.ChangeState(StateFactory.Get<MoveState>());
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
        _animator = GetComponent<Animator>();
        _stats = new EnemyStats();
        _fsm = new StateMachine<BaseEnemy>(this);
        _stateFactory = new StateFactory<BaseEnemy>();
    }

    // 플레이어 범위 안에 있는지 체크
    public bool PlayerInRange()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        return distance < _stats.DetectDistance;
    }

    // 적 피해 처리
    public void TakePhysicalDamage(int damage)
    {
        Stats.Health -= damage;
        if (Stats.Health <= 0)
        {
            _fsm.ChangeState(StateFactory.Get<MoveState>());
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

    public void FaceMoveDirection()
    {
        if (_agent == null || _agent.desiredVelocity.sqrMagnitude < 0.01f) return;

        Vector3 moveDirection = _agent.desiredVelocity;
        moveDirection.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        targetRotation *= Quaternion.Euler(0f, -90f, 0f); // 플레이어 바라보게 방향 보정
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }
}
