using System.Collections;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;


public class Controller : MonoBehaviour
{
    [SerializeField]
    public UnityEngine.GameObject root_manager;
    [SerializeField]
    public UnityEngine.GameObject character_entity;
    [SerializeField]
    public Animator character_animator;

    public Vector2 firstPos;
    public Vector2 secondPos;

    float LeftRocker_H;
    float LeftRocker_V;
    bool buttonPressed;
    float RightRocker_H;
    float RightRocker_V;

    float direction;

    bool startCheck = false;

    Vector3 currentDirection;
    Vector3 rightController;
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.GameObject[] s = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        this.currentDirection = this.transform.forward;
        //for (int i = 0; i < s.Length; i++)
        //{
        //    if (s[i].name == "RootManager")
        //        character_entity = s[i].transform.GetChild(0).gameObject;
        //}

        //character_animator = character_entity.GetComponent<Animator>();
        //if (character_animator == null)
        //    Debug.Log("null animator");
        //else
        //    Debug.Log(character_animator.name);

        //StartCoroutine(Print());
    }

    // Update is called once per frame
    void Update()
    {
        RightRocker_H = Input.GetAxis("RightHorizontal");
        RightRocker_V = Input.GetAxis("RightVertical");
        this.currentDirection = this.transform.forward.normalized;

        if (RightRocker_V == 0.0f && RightRocker_H ==0.0f)
        {
            RightRocker_V = -1.0f;
        }
        this.rightController = new Vector3(RightRocker_H, 0.0f, -RightRocker_V).normalized;

        float dot = Vector3.Dot(new Vector3(0.0f, 0.0f, 1.0f), this.rightController);
        dot = Mathf.Clamp(dot, -1f, 1f); ;

        float angle = 0.0f;
        if (RightRocker_H > 0.0f)
        {
            angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        }
        else
        {
            angle = (Mathf.PI * 2 - Mathf.Acos(dot)) * Mathf.Rad2Deg;
        }

        //Mathf.Lerp()
        this.transform.rotation = Quaternion.Euler(0, angle, 0);
        UnityEngine.Debug.Log(this.currentDirection + " : " + rightController + " : " + dot + " : " + angle);

        buttonPressed = Input.GetKey(KeyCode.JoystickButton5);
        if (buttonPressed && !startCheck)
        {
            startCheck = true;
            StartCoroutine(Print());

        }
        if (startCheck == false)
        {
            StopCoroutine(Print());
        }
        AnimatorStateInfo statInfo = character_animator.GetCurrentAnimatorStateInfo(0);

        if (statInfo.normalizedTime >= 0.9f)
        {
            character_animator.SetBool("RightAttack", false);
            character_animator.SetBool("LeftAttack", false);
            if (statInfo.IsName("Attack_01"))
            {
            }
            //if (statInfo.IsName("Attack_03"))
            character_animator.Play("Idle");
        }

    }

    IEnumerator Print()
    {
        UnityEngine.Debug.Log(direction);

        this.firstPos = new Vector2(RightRocker_H, RightRocker_V);

        yield return new WaitForSeconds(0.1f);
        this.secondPos = new Vector2(RightRocker_H, RightRocker_V);
        UnityEngine.Debug.Log(this.secondPos);
        direction = Dot(this.secondPos - this.firstPos, Vector2.right);

        if (direction > 0.0f)
        {
            character_animator.SetBool("LeftAttack", true);
        }
        if (direction < 0.0f)
        {
            character_animator.SetBool("RightAttack", true);
        }



        direction = 0.0f;
        this.firstPos = Vector2.zero;
        this.secondPos = Vector2.zero;
        UnityEngine.Debug.Log(buttonPressed ? "Button is pressing" : "Button is not pressed");
        startCheck = false;
    }

    float Dot(Vector2 a, Vector2 b)
    {
        return (a.x * b.x + a.y * b.y);
    }
}


