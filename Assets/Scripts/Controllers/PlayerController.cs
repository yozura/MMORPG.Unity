using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill,
    }
    
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    [SerializeField] PlayerState _state = PlayerState.Idle;
    PlayerStat _stat;
    Vector3 _destPos;
    GameObject _lockTarget;
    
    public PlayerState State
    {
        get { return _state; }
        set
        {
            _state = value;
            Animator anim = GetComponent<Animator>();
            switch(_state)
            {
                case PlayerState.Die:
                    break;
                case PlayerState.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    break;
                case PlayerState.Moving:
                    anim.CrossFade("RUN", 0.1f);
                    break;
                case PlayerState.Skill:
                    anim.CrossFade("ATTACK", 0.1f, -1, 0);
                    break;
            }
        }
    }

    void Start()
    {
        _stat = GetComponent<PlayerStat>();

        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        Managers.UI.MakeWorldSpace<UI_HPBar>(transform);
    }

    void Update()
    {
        StatePattern();
    }

    void StatePattern()
    {
        switch(State)
        {
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Moving:
                UpdateMove();
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Skill:
                UpdateSkill();
                break;

        }
    }

    bool _stopSkill = false;
    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case PlayerState.Die:
                break;
            case PlayerState.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case PlayerState.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case PlayerState.Skill:
                {
                    if (evt == Define.MouseEvent.PointerUp)
                        _stopSkill = true;
                }
                break;
        }
    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        RaycastHit hitinfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool rayCastHit = Physics.Raycast(ray, out hitinfo, 100.0f, _mask);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (rayCastHit)
                    {
                        _destPos = hitinfo.point;
                        State = PlayerState.Moving;
                        _stopSkill = false;

                        if (hitinfo.collider.gameObject.layer == (int)Define.Layer.Monster)
                            _lockTarget = hitinfo.collider.gameObject;
                        else
                            _lockTarget = null;
                    }
                }
                break;
            case Define.MouseEvent.Press:
                {
                    if (_lockTarget == null && rayCastHit)
                        _destPos = hitinfo.point;
                }
                break;
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
        }
    }
    
    void UpdateDie()
    {

    }

    void UpdateMove()
    {
        // 몬스터가 내 사정거리보다 가까우면 공격
        if(_lockTarget != null)
        {
            float distance = (_destPos - transform.position).magnitude;
            if(distance <= 1)
            {
                State = PlayerState.Skill;
                return;
            }
        }

        // 이동
        Vector3 dir = _destPos - transform.position;
        Animator anim = GetComponent<Animator>();
        if (dir.magnitude < 0.1f)
            State = PlayerState.Idle;
        else
        {
            NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            nav.Move(dir.normalized * moveDist);

            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            if(Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if(!Input.GetMouseButton(0))
                    State = PlayerState.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }

    void UpdateIdle()
    {
    }

    void UpdateSkill()
    {
        if(_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }

    }

    void OnHitEvent()
    {
        if(_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            PlayerStat myStat = gameObject.GetComponent<PlayerStat>();
            int damage = Mathf.Max(0, myStat.Attack - targetStat.Defense);
            Debug.Log(damage);
            targetStat.HP -= damage;
        }

        if(_stopSkill)
        {
            State = PlayerState.Idle;
        }
        else
        {
            State = PlayerState.Skill;
        }
    }

    void OnKeyboard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            transform.position += Vector3.forward * Time.deltaTime * _stat.MoveSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += Vector3.back * Time.deltaTime * _stat.MoveSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += Vector3.left * Time.deltaTime * _stat.MoveSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += Vector3.right * Time.deltaTime * _stat.MoveSpeed;
        }
    }

    void OriginMove()
    {
        //// Start
        //bool _moveToDest = false;

        //// Update
        //if (_moveToDest)
        //{
        //    Vector3 dir = _destPos - transform.position;
        //    Animator anim = GetComponent<Animator>();
        //    wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 10.0f * Time.deltaTime);
        //    anim.SetFloat("Wait_Run_Ratio", wait_run_ratio);
        //    anim.Play("WAIT_RUN");
        //    if (dir.magnitude < 0.0001f)
        //        _moveToDest = false;
        //    else
        //    {
        //        float moveDist = Mathf.Clamp(speed * Time.deltaTime, 0, dir.magnitude);
        //        transform.position += dir.normalized * moveDist;
        //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        //    }
        //}
        //else
        //{
        //    Animator anim = GetComponent<Animator>();
        //    wait_run_ratio = Mathf.Lerp(wait_run_ratio, 0, 10.0f * Time.deltaTime);
        //    anim.SetFloat("Wait_Run_Ratio", wait_run_ratio);
        //    anim.Play("WAIT_RUN");
        //}
    }
}
