using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player
{
    //: SceneSingleton<Player>
    /* [SerializeField] Animator animator;

     Vector2 moveVector = Vector2.zero;
     Vector2 moveVectorTarget;
     float moveSpeed = 1;
     bool isFire = false;
     bool isZoom = false;
     Vector2 mouseDeltaPos = Vector2.zero;
     float senst;//카메라 감도
     int controlWeaponIndex;

     CharacterController cc;

     public float maxHp = 100f;

     [SerializeField] GameObject tpsVCamRoot;
     [SerializeField] CinemachineVirtualCamera tpsVCam;
     [SerializeField] Transform weaponPoint;

     public Vector3 CamForward()
     {
         return tpsVCamRoot.transform.forward;
     }



     //float fireDelay = 0;
     //float delayCount = 0.1f;
     //int shell = 100;

     bool isReload = false;
     bool isActive = false;
     public bool onTank = false;
     public bool onPlay { get; private set; }


     PlayerCombatData data = new PlayerCombatData();

     public GameObject PlayerHitSound;

     // CinemachineImpulseSource impulseSource;

     private void Awake()
     {
         //combat.Init(transform, maxHp);

         //tpsVCamRoot.transform.parent = null;
         onPlay = false;

     }

     // Start is called before the first frame update
     void Start()
     {
         //Cursor.lockState = CursorLockMode.Locked;
         //tpsCmc = tpsVCam.GetComponent<CinemachineVirtualCamera>();        
         cc = GetComponent<CharacterController>();
         //impulseSource = GetComponent<CinemachineImpulseSource>();

         //SetCamType(false);
         senst = 0.5f;
     }

     // Update is called once per frame
     void Update()
     {
         if (isActive)
         {
             if (!onTank)
             {
                 MoveOrder();//이동  
                 RotateOrder();//캐릭터 및 총기 회전
             }
         }
         else if (onPlay)
         {
             DeadCamMove();
         }
     }
     private void LateUpdate()
     {
         if (isActive)
         {
             CamRotate();//카메라 회전
         }
     }
     private void MoveOrder()
     {
         moveVector = Vector2.Lerp(moveVector, moveVectorTarget * moveSpeed, Time.deltaTime * 5);

         Vector3 moveVector3 = new Vector3(moveVector.x * 0.5f, Physics.gravity.y, moveVector.y);
         //moveVector3 = this.transform.rotation * moveVector3;

         cc.Move(this.transform.rotation * moveVector3 * Time.deltaTime);

         animator.SetFloat("X_Speed", moveVector.x);
         animator.SetFloat("Y_Speed", moveVector.y);
     }
     void CamRotate()
     {
         tpsVCam.transform.localPosition = new Vector3(0, 0, -5);        

         tpsVCamRoot.transform.position = this.transform.position + new Vector3(0, 1.5f, 0);

         Vector3 camAngle = tpsVCamRoot.transform.rotation.eulerAngles;


         if (isZoom)
         {
             mouseDeltaPos *= 0.2f * senst;
         }
         else
         {
             mouseDeltaPos *= 0.4f * senst;
         }

         float x = camAngle.x - mouseDeltaPos.y;
         if (x < 180f)
         {
             x = Mathf.Clamp(x, -1f, 25f);
         }
         else
         {
             x = Mathf.Clamp(x, 345f, 361f);
         }

         tpsVCamRoot.transform.rotation = Quaternion.Euler(x, camAngle.y + mouseDeltaPos.x, camAngle.z);
         mouseDeltaPos *= 0.9f;
     }
     void DeadCamMove()
     {
         tpsVCam.transform.localPosition += new Vector3(1, 1, -1) * Time.deltaTime;
         tpsVCam.transform.LookAt(this.transform.position);
     }
     void RotateOrder()
     {
         Vector3 direction = (tpsVCam.transform.forward).normalized;

         Quaternion rotationWeapon = Quaternion.LookRotation(direction);
         rotationWeapon = Quaternion.Euler(rotationWeapon.eulerAngles.x, this.transform.rotation.eulerAngles.y, rotationWeapon.eulerAngles.z);
         //weaponPoint.rotation = Quaternion.Slerp(weaponPoint.rotation, rotationWeapon, Time.deltaTime * controlweapon.Operability() * 0.4f);

         direction = new Vector3(direction.x, 0, direction.z);

         Quaternion rotationBody = Quaternion.LookRotation(direction);
         //rotationBody = Quaternion.Euler(0, rotationBody.eulerAngles.y, 0);
         this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotationBody, Time.deltaTime * 0.4f);
     }

     public void Recoil(float recoli)
     {
         mouseDeltaPos = new Vector2(Random.Range(-recoli, recoli), Random.Range(recoli, recoli * 3)) * 0.12f;
     }

     void OnMove(InputValue inputValue)//WASD 조작
     {
         moveVectorTarget = inputValue.Get<Vector2>();//인풋 벡터 받아옴
         //moveVectorTarget = inputMovement;
         //Debug.Log(inputMovement);
     }
     void OnSprint(InputValue inputValue)
     {
         float value = inputValue.Get<float>();
         moveSpeed = (value * 4) + 1;
         //Debug.Log(isClick);
     }

     void OnAim(InputValue inputValue)
     {
         mouseDeltaPos = inputValue.Get<Vector2>();//인풋 벡터 받아옴        
     }


     public PlayerCombatData GetCombatData()
     {
         return data;
     }
 */
}

public class PlayerCombatData//참조용 데이터 클래스
{
    public float playerMaxHp;//최대 체력
    public float playerCurHp;//현재 체력
    public string controlWeaponName;//사용중인 무기 이름
    public int controlWeaponIndex;//사용중인 무기 인덱스
    public int cwMaxMag;//사용중인 무기 최대 장탄량
    public int cwCurMag;//사용중인 무기 현재 장탄량
    public int killCount;//킬수
}