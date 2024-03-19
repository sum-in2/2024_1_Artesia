using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StairCollider : MonoBehaviour
{
    [SerializeField] GameObject Player;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            MapGenerator.instance.InitMap();
            Player.gameObject.GetComponent<MoveController>().MovePos();
        }
    }
}