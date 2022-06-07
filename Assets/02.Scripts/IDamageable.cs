using UnityEngine;

public interface IDamageable
{
    void OnDamage(float damage, Vector3 hitpoint, Vector3 hitnormal);
    //       데미지 들어가는곳  충돌한 위치값이 들어가는곳   충돌한 방향
}
