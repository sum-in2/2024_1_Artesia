using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 TargetPos {get; private set;}
    public Vector2 Dir { get; private set;}

    void Start() {
        MovePos();
    }

    private void Update() {
        // if(input keycode ?)
        // new input manager?
        if(Dir != Vector2.zero) // 한번만 호출되게 수정해야함
            Debug.Log("SetState 호출");
    }

    void OnMove(InputValue value){
        Vector2 input = value.Get<Vector2>();
        if(input != null){
            Dir = input;
        }
    }
    
    public void MovePos(){
        transform.localPosition = MapGenerator.instance.StartPos;
    }
}