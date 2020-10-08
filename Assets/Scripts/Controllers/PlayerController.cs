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

    enum CursorType
    {
        None,
        Attack,
        Hand,
    }

    PlayerState _state = PlayerState.Idle;
    PlayerStat _stat;
    Vector3 _destPos;

    Texture2D _attackIcon;
    Texture2D _handIcon;


    CursorType _cursorType = CursorType.None;

    void Start()
    {
        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        _handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");
        _stat = GetComponent<PlayerStat>();

        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    void Update()
    {
        UpdateMouseCursor();
        StatePattern();
    }

    void UpdateMouseCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo, 100.0f, _mask))
        {
            if (hitinfo.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                if(_cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);
                    _cursorType = CursorType.Attack;
                }
            }
            else
            {
                if(_cursorType != CursorType.Hand)
                {
                    Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto);
                    _cursorType = CursorType.Hand;
                }
            }
        }
    }

    void StatePattern()
    {
        switch(_state)
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
        }
    }

    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
        
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo, 100.0f, _mask))
        {
            _destPos = hitinfo.point;
            _state = PlayerState.Moving;

            if(hitinfo.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                Debug.Log("Monster Click");
            }
            else
            {
                Debug.Log("Ground Click");
            }
        }
    }
    
    void UpdateDie()
    {

    }

    void UpdateMove()
    {
        Vector3 dir = _destPos - transform.position;
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("Speed", _stat.MoveSpeed);
        if (dir.magnitude < 0.1f)
            _state = PlayerState.Idle;
        else
        {
            NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            nav.Move(dir.normalized * moveDist);

            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            if(Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                _state = PlayerState.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }

    void UpdateIdle()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("Speed", 0);
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
