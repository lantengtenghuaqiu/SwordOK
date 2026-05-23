using System;
using System.Collections;
using System.Collections.Generic;
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

    public float LeftRocker_H;
    float LeftRocker_V;
    bool buttonPressed;
    float RightRocker_H;
    float RightRocker_V;

    float direction;

    bool startCheck = false;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.GameObject[] s = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        for (int i = 0; i < s.Length; i++)
        {
            if (s[i].name == "RootManager")
                character_entity = s[i].transform.GetChild(0).gameObject;
        }

        character_animator = character_entity.GetComponent<Animator>();
        if (character_animator == null)
            Debug.Log("null animator");
        else
            Debug.Log(character_animator.name);

        //StartCoroutine(Print());
    }

    // Update is called once per frame
    void Update()
    {
        LeftRocker_H = Input.GetAxis("Horizontal");
        LeftRocker_V = Input.GetAxis("Vertical");
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
        Debug.Log(statInfo.normalizedTime);
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
        Debug.Log(direction);

        this.firstPos = new Vector2(RightRocker_H, RightRocker_V);

        yield return new WaitForSeconds(0.1f);
        this.secondPos = new Vector2(RightRocker_H, RightRocker_V);
        Debug.Log(this.secondPos);
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
        Debug.Log(buttonPressed ? "Button is pressing" : "Button is not pressed");
        startCheck = false;
    }

    float Dot(Vector2 a, Vector2 b)
    {
        return (a.x * b.x + a.y * b.y);
    }
}


