using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    public string transferMapName; //이동할 맵의 이름

    private MovingObject thePlayer; //빈껍데기
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<MovingObject>(); //thePlayer를 채워넣음
    }

    private void OnTriggerEnter2D(Collider2D collision) //istrigger 활성화 해야함
    {
        if(collision.gameObject.name == "Player") //플레이어가 닿은 경우
        {
            thePlayer.currentMapName = transferMapName; //이동할 맵의 이름을 넣음
            SceneManager.LoadScene(transferMapName);
        }
    }

}
