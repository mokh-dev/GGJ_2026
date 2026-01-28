using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private GameObject _bulletPre;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _firingCooldownTime;
    [SerializeField] private float _bulletLifeSpan;
    [SerializeField] private float _lookRotationOffset;
    private bool canFire = true;


    [Header("Player Detection")]
    [SerializeField] private float _fovDetectionAngle;
    [SerializeField] private float _fovDetectionRange;
    private GameObject playerObj;
    

    [Header("Other")]
    private Rigidbody2D rb;
    private Camera cam;


    


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        cam = Camera.main;
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        if (CanSeePlayer()) Aggrivated(); 

    }

    private void Aggrivated()
    {
        Debug.Log("angry");
        PointEnemyToPlayer();
        if (canFire) FireBullet();
    }

    private bool CanSeePlayer()
    {
        Vector2 playerDirection = (playerObj.transform.position - transform.position).normalized;
        float playerAngleFromView = Vector2.Angle(transform.up, playerDirection);

        if (playerAngleFromView > _fovDetectionAngle/2) return false;
        if (Vector2.Distance(playerObj.transform.position, transform.position) > _fovDetectionRange) return false;

        return true;
    }

    private void PointEnemyToPlayer()
    {
        Vector2 playerDirection = (playerObj.transform.position - transform.position).normalized;
        float enemyAngle = (Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg) + _lookRotationOffset;

        rb.SetRotation(enemyAngle);
    }

    private void FireBullet()
    {
        Quaternion enemyRotation = Quaternion.AngleAxis(transform.eulerAngles.z, Vector3.forward);
        GameObject bullet = Instantiate(_bulletPre, _firePoint.position, enemyRotation);
        Destroy(bullet, _bulletLifeSpan);

        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.AddForce(transform.up * _bulletSpeed, ForceMode2D.Impulse);
        

        canFire = false;
        StartCoroutine(FiringCooldown());        
    }

    private IEnumerator FiringCooldown()
    {
        yield return new WaitForSeconds(_firingCooldownTime);
        canFire = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
            Debug.Log("enemy hit");
        }
    }
}
