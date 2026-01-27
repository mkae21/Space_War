using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask collisionMask;
    public GunData gunData;
    
    private float traveled;
    private float shootingRange;
    private float speed;
    private float damage;

    //확장을 고려해서 Gun에서 이 함수 호출이 가능하도록한다.
    //GunData를 그대로 사용하는 것은 위험할 수도 있음, 필요한 값만 복사해서 들고있는것이 좋다.
    public void Init(GunData data)
    {
        traveled = 0f;
        speed = data.fireVelocity;
        shootingRange = data.shootingRange;
        damage = data.damage;
    }

    void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        traveled += moveDistance; //길이 누적

        CheckCollision(moveDistance);
        CheckBulletRange(traveled);
        transform.Translate(Vector3.forward * moveDistance);
    }


    private void CheckCollision(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        //이는 Ray가 Trigger를 어떻게 처리하는지 정하는 옵션이다. trigger도 맞았다고 할지 무시할지..
        if(Physics.Raycast(ray, out hit, moveDistance , collisionMask , QueryTriggerInteraction.Collide))
        {
            Debug.Log("Hit!");
            OnHitObject(hit);
        }
    }

    private void OnHitObject(RaycastHit hit)
    {
        Debug.Log(hit.collider.gameObject.name);
        IDamageable hitTarget = hit.collider.GetComponent<IDamageable>();

        if (hitTarget != null)
        {
            Vector3 normalVector = hit.point - transform.position;
            hitTarget.onDamage(damage,hit.point,normalVector);
            gameObject.SetActive(false); //총에 맞을 경우 끄기
        }
    }

    private void CheckBulletRange(float traveled)
    {
        if(traveled > shootingRange)
            gameObject.SetActive(false);
    }

}
