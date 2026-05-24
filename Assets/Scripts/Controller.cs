using System.Collections;
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

    public float currentAngle = 0f;   // 当前角度
    public float targetAngle = 0f;   // 目标角度
    public float rotateSpeed = 0.08f; // 旋转速度
    public float threshold = 0.1f;    // 到达阈值

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.GameObject[] s = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        this.currentDirection = this.transform.forward;
    }
    private float ClampAngle(float angle)
    {
        angle = Mathf.Repeat(angle, 360f);
        if (angle < 0)
            angle += 360f;
        return angle;
    }
    void CalculateAnagle()
    {
        if (Mathf.Abs(targetAngle - currentAngle) >= threshold)
        {
            float diff = targetAngle - currentAngle;

            diff = Mathf.Repeat(diff + 180f, 360f) - 180f;

            if (Mathf.Abs(diff) < threshold)
            {
                currentAngle = targetAngle;
            }
            else
            {
                currentAngle += diff * rotateSpeed;
            }


            currentAngle = Mathf.Repeat(currentAngle, 360f);


            //UnityEngine.Debug.Log($"当前角度: {currentAngle:F2}  |  目标角度: {targetAngle}");
        }
    }

    // Update is called once per frame
    void Update()
    {

        //CalculateAnagle();
        //RightRocker_H = Input.GetAxis("RightHorizontal");
        //RightRocker_V = Input.GetAxis("RightVertical");
        //this.currentDirection = this.transform.forward.normalized;

        ////if (RightRocker_V == 0.0f && RightRocker_H == 0.0f)
        ////{
        ////    RightRocker_V = -1.0f;
        ////}
        //    this.rightController = new Vector3(RightRocker_H, 0.0f, -RightRocker_V).normalized;

        //float dot = Vector3.Dot(new Vector3(0.0f, 0.0f, 1.0f), this.rightController);
        //dot = Mathf.Clamp(dot, -1f, 1f); ;

        ////float angle = 0.0f;
        //if (RightRocker_H > 0.0f)
        //{
        //    targetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        //}
        //else
        //{
        //    targetAngle = (Mathf.PI * 2 - Mathf.Acos(dot)) * Mathf.Rad2Deg;
        //}

        //this.transform.rotation = Quaternion.Euler(0, currentAngle, 0);

        RightRocker_H = Input.GetAxis("RightHorizontal");
        RightRocker_V = Input.GetAxis("RightVertical");

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
        if (!buttonPressed)
        {
            float deadZone = 0.1f;

            if (Mathf.Abs(RightRocker_H) > deadZone || Mathf.Abs(RightRocker_V) > deadZone)
            {
                this.rightController = new Vector3(RightRocker_H, 0.0f, -RightRocker_V).normalized;

                float dot = Vector3.Dot(Vector3.forward, this.rightController);
                dot = Mathf.Clamp(dot, -1f, 1f);

                if (RightRocker_H > 0.0f)
                    targetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;
                else
                    targetAngle = (Mathf.PI * 2 - Mathf.Acos(dot)) * Mathf.Rad2Deg;
            }

            CalculateAnagle();

            this.transform.rotation = Quaternion.Euler(0, currentAngle, 0);

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


