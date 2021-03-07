using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public string currentMapName; //tranferMap 스크립트에 있는 transferMapName변수의 값을 저장

    private BoxCollider2D boxCollider;
    public LayerMask layerMask; //어떤 레이어와 충돌인지

    public float speed; //캐릭터 기본 이동 속도
    private Vector3 vector; //캐릭터 위치

    public float runSpeed; //달릴때 속도
    private float applyRunSpeed; //실제 달릴때 적용되는 속도
    private bool applyRunFlag = false;

    public int walkCount; //픽셀단위로 움직이기 위해
    private int currentWalkCount; //픽셀단위로 움직이기 위해

    private bool canMove = true;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject); //오브젝트를 파괴하지 말라
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>(); //오브젝트에 붙어있는 애니메이터 컴포넌트를 얻기 위해 GetComponert 사용
    }

    IEnumerator MoveCorutine() //기다리는거?
    {
        while(Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        { //코루틴 호출시 한번만 실행
            if (Input.GetKey(KeyCode.LeftShift))
            {
                applyRunSpeed = runSpeed;
                applyRunFlag = true;
            }
            else
            {
                applyRunSpeed = 0;
                applyRunFlag = false;
            }


            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z); //입력된 값을 저장

            if (vector.x != 0) //x가 입력되면 y의 값은 0으로 설정 (키 두개 입력시 이동방향과 모션이 일치하게 하기 위해)
                vector.y = 0;

            animator.SetFloat("DirX", vector.x); //입력받은 키 값을 vector로 받았고 그것을 DirX로 전달하여 애니메이터에서 해당 값을 동작
            animator.SetFloat("DirY", vector.y);

            RaycastHit2D hit; //A지점에서 B지점으로 레이저를 쏘았을 때 방해물이 없으면 Null리턴

            Vector2 start = transform.position; //A지점, 캐릭터 현재 위치값
            Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount); //B지점, 이동하고자 하는 위치값
            //우방향키 입력시 vector.x는 1, speed는 2.4, walkCount는 20 => 오른쪽으로 48픽셀 이동

            boxCollider.enabled = false; //플레이어 자신과 충돌하는것을 막기 위해 잠시 박스콜라이더 끔
            hit = Physics2D.Linecast(start, end, layerMask);
            boxCollider.enabled = true;

            if (hit.transform != null) //가려는 곳이 벽이면 이동하지 않음
                break;

            animator.SetBool("Walking", true);

            while (currentWalkCount < walkCount)
            {
                if (vector.x != 0)
                {
                    transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0);
                }
                else if (vector.y != 0)
                {
                    transform.Translate(0, vector.y * (speed + applyRunSpeed), 0);
                }
                if (applyRunFlag) //쉬프트를 누르면 두칸씩 가는 것을 막기 위해
                {
                    currentWalkCount++; //한번 더 증가
                }
                currentWalkCount++;
                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0; //초기화해서 다시 반복문을 돌 수 있게         
        }
        animator.SetBool("Walking", false); //다시 서있는 동작으로
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) //방향키가 입력되면
            {
                canMove = false; //방향키 한번 입력에 coroutine이 한번만 실행되도록
                StartCoroutine(MoveCorutine());
            }
        }
        
    }
}
