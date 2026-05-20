using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    public UnityEngine.GameObject root_manager;
    [SerializeField]
    public UnityEngine.GameObject character_entity;
    [SerializeField]
    public Animator character_animator;


    public float LeftRocker_H;
    float LeftRocker_V;
    bool buttonPressed;
    float RightRocker_H;
    float RightRocker_V;

    float[] first_input = new float[2];
    float[] end_input = new float[2];

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

        StartCoroutine(Print(buttonPressed));
    }

    // Update is called once per frame
    void Update()
    {
        LeftRocker_H = Input.GetAxis("Horizontal");
        LeftRocker_V = Input.GetAxis("Vertical");
        RightRocker_H = Input.GetAxis("RightHorizontal");
        RightRocker_V = Input.GetAxis("RightVertical");

        buttonPressed = Input.GetKey(KeyCode.JoystickButton5);
        if (buttonPressed && RightRocker_H > 0.5)
            character_animator.SetBool("LeftAttack", true);
        else
            character_animator.SetBool("LeftAttack", false);

        if (buttonPressed && RightRocker_H < -0.5)
            character_animator.SetBool("RightAttack", true);
        else
            character_animator.SetBool("RightAttack", false);
    }

    IEnumerator Print(bool buttonPressed)
    {
        yield return new WaitForSeconds(1f);
        Debug.Log(RightRocker_H + " : " + RightRocker_V);
        Debug.Log(buttonPressed ? "Button is pressing" : "Button is not pressed");

    }
}


