using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
    }

    PlayerState _state = PlayerState.Idle;

    [SerializeField] float speed = 10.0f;
    Vector3 _destPos;

    void Start()
    {
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    void Update()
    {
        StatePattern();
    }

    public void StatePattern()
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

    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
        
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo, 100.0f, LayerMask.GetMask("Wall")))
        {
            _destPos = hitinfo.point;
            _state = PlayerState.Moving;
            Debug.Log($"Raycast Camera @ {hitinfo.collider.gameObject.tag} !");
        }
    }
    
    void UpdateDie()
    {

    }

    void UpdateMove()
    {
        Vector3 dir = _destPos - transform.position;
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("Speed", speed);
        if (dir.magnitude < 0.0001f)
            _state = PlayerState.Idle;
        else
        {
            float moveDist = Mathf.Clamp(speed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
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
            transform.position += Vector3.forward * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += Vector3.back * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += Vector3.left * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += Vector3.right * Time.deltaTime * speed;
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
