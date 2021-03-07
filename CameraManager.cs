using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject target; //카메라가 따라갈 대상
    public float moveSpeed; //카메라 얼마나 빠른 속도로 
    private Vector3 targetPosition; //대상의 현재 위치 값

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(target.gameObject != null) //대상이 있다면
        {
            //target의 z좌표를 가지면 카메라와 타겟이 겹쳐서 카메라에 찍히지 않음
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);

            //1초에 moveSpeed만큼 이동
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);


        }

    }
}
