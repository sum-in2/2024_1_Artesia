using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour, IState<PlayerController>
{
    private PlayerController m_playerController;
    public float speed = 0.2f;
    //public Vector2 targetPos { get; set;}
    Vector3 dir;
    RaycastHit2D hit;
    float elapsedTime;

    public void OperateEnter(PlayerController sender){
        if(!m_playerController)
            m_playerController = sender;
        elapsedTime = 0;
    }
    public void OperateUpdate(PlayerController sender){
        return;
    }
    public void OperateExit(PlayerController sender){

    }
}

/*public float speed = 0.2f;
    Vector2 OriPos;
    Vector2 targetPos;
    public bool playerTurn_move = false;

    private bool isMoving = false;

    void Start() {
        MovePos();
    }

    public void MovePos(){
        transform.localPosition = MapGenerator.instance.StartPos;
    }

    void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        float dirY = Input.GetAxisRaw("Vertical");

        Debug.DrawRay(transform.position, new Vector3(dirX,dirY,0), Color.black, 0.3f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(dirX,dirY,0), 1, LayerMask.GetMask("Tile"));

        if ((dirX != 0 || dirY != 0) && !isMoving && !hit && !playerTurn_move)
        {
            playerTurn_move = true;
            StartCoroutine( PlayerMove());
        }
    }
    
    private IEnumerator PlayerMove()
    {
        isMoving = true;

        float elapsedTime = 0;

        OriPos = new Vector3((int)transform.position.x, (int)transform.position.y,0);
        targetPos = OriPos + new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        while(elapsedTime < speed){
            transform.position = Vector2.Lerp(OriPos, targetPos, elapsedTime / speed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        this.gameObject.transform.position = targetPos;
        isMoving = false;
    }
    */