using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

//Player가 아닌 존재가 Gun을 가지고 있을 수 있으니까 분리해놓자.
public class GunController : MonoBehaviour
{

    [Header("Holder")]
    [SerializeField] private Transform rightHandBone;
    [SerializeField] private Transform weaponHolder;
    //public Transform rightHandMount;

    [Header("Gun Data")]
    public GunData pistolData;
    public GunData rifleData;

    //private GameObject equippedGun;
    private GameObject currentGun;
    private Gun equippedGun;
    private PlayerInput playerInput;
    private Animator playerAnimator;


    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (pistolData != null)
        {
            Debug.Log("Pistol Equipped!");
            EquipGun(pistolData);
        }

    }

    //private void Update()
    //{
    //    if (playerInput.numberOnePushDown)
    //    {
    //        currentData = pistolData;
    //        currentGun = startingGun;
    //    }

    //    if (playerInput.numberTwoPushDown)
    //    {
    //        currentData = rifleData;
    //        currentGun = rifleGun;
    //    }
    //}


    //나중에 SO로 변경 예정.
    public void EquipGun(GunData gunToEquip)
    {

        Vector3 positionOffset = new Vector3(gunToEquip.posX, gunToEquip.posY, gunToEquip.posZ);
        Vector3 rotationOffset = new Vector3(gunToEquip.roatX, gunToEquip.roatY, gunToEquip.roatZ);

        currentGun = gunToEquip.prefab;

        weaponHolder.SetParent(rightHandBone, false);
        weaponHolder.localPosition = positionOffset;
        weaponHolder.localRotation = Quaternion.identity * Quaternion.Euler(rotationOffset);

        //이미 착용 중인 총이 있다면 파괴 (파괴하고 해당 좌표에 총 생성해도 괜찮을 듯..)
        if (currentGun == null)
        {
            Debug.Log("There is no Gun to Equip");
            Destroy(currentGun);
            return;
        }
   

        //Instantiate는 Object를 반환하는데 as 키워드로 Gun으로 반환 가능하다. (캐스팅이 가능한 경우 만 해당한다.)
        var gunObj = Instantiate(currentGun, weaponHolder);
        gunObj.transform.localPosition = Vector3.zero;
        gunObj.transform.localRotation = Quaternion.identity;

        //Animation override
        OverrideWeaponAnimation(gunToEquip);

        //총에따른 GunData init 시켜야 함
        //캐싱하여 최적화
        if (!gunObj.TryGetComponent<Gun>(out equippedGun))
        {
            Debug.Log("Gun Component Missing !!");
        }

        equippedGun.InitGunData(gunToEquip);
    }

    public void OverrideWeaponAnimation(GunData gunToEquip)
    {
        playerAnimator.runtimeAnimatorController = gunToEquip.playerUppderBodyAnimation;
    }

    public void Shoot()
    {
        //총을 가지고 있을 경우에 가능 (enum)
        if(equippedGun != null)
        {
            playerAnimator.SetTrigger("Shot");
            equippedGun.Shoot();
        }
    }

}
