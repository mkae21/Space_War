using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//Player가 아닌 존재가 Gun을 가지고 있을 수 있으니까 분리해놓자.
public class GunController : MonoBehaviour
{
    //[Header("Holder")]
    public Transform weaponHolder;
    //public Transform rightHandMount;

    [Header("Guns")]
    public Gun startingGun;
    //private Gun rifleGun;

    [Header("Data")]
    public GunData pistolData;
    public GunData rifleData;

    private Gun equippedGun;
    private Gun currentGun;
    private GunData currentData;
    private PlayerInput playerInput;

    private void Start()
    {
        if (pistolData != null && startingGun != null)
        {
            currentData = pistolData;
            EquipGun(startingGun);
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

    public void EquipGun(Gun gunToEquip)
    {
        //이미 착용 중인 총이 있다면 파괴 (파괴하고 해당 좌표에 총 생성해도 괜찮을 듯..)
        if (equippedGun != null)
            Destroy(equippedGun.gameObject);

        //Instantiate는 Object를 반환하는데 as 키워드로 Gun으로 반환 가능하다. (캐스팅이 가능한 경우 만 해당한다.)
        equippedGun = Instantiate(gunToEquip, weaponHolder.position, weaponHolder.rotation) as Gun;
        equippedGun.transform.parent = weaponHolder; //플레이어를 따라다니도록 해야한다.

        //여기서 if문으로 총에따른 GunData init 시켜야 함
        equippedGun.InitGunData(currentData);
    }

    public void Shoot()
    {
        //총을 가지고 있을 경우에 가능
        if(equippedGun != null)
            equippedGun.Shoot();
    }

}
