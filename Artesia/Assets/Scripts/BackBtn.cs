using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBtn : MonoBehaviour
{
        public void OnBackButton(){
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
