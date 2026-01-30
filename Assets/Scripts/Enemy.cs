using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private GameObject _visionCone;
    [SerializeField] private GameObject _bulletPre;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _firingCooldownTime;
    [SerializeField] private float _bulletLifeSpan;
    [SerializeField] private float _lookRotationOffset;
    private bool canFire = true;


    [Header("Player Detection")]
    [SerializeField] private float _detectionFovAngle;
    [SerializeField] private float _detectionRange;
    [SerializeField] private float _detectionRangeCalculationOffset;    
    [SerializeField] private LayerMask _detectionLayers;
    [SerializeField] private float _detectionCooldownTime;
    private GameObject playerObj;

    private bool onCooldown;
    private bool sentDetection;
    private bool canSeePlayer;


    

    [Header("Other")]

    public GameObject Rock;
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
        
        float offsetRange = _detectionRange + _detectionRangeCalculationOffset;
        float xScale = 2f * offsetRange * Mathf.Tan(_detectionFovAngle * 0.5f * Mathf.Deg2Rad);
        _visionCone.transform.localScale = new Vector3(xScale,offsetRange,1);

        canSeePlayer = CanSeePlayer(); 

        if (canSeePlayer) Aggrivated();
        else if (CanSeeRock()) Distracted();


        if (canSeePlayer == false && sentDetection == true && onCooldown == false)
        {
            StartCoroutine(DetectionCooldown());
        }

        if (canSeePlayer == true && sentDetection == false) 
        {
            sentDetection = true;
        
            GameManager.Instance.EnemyDetected();
        }

    }

    private IEnumerator DetectionCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(_detectionCooldownTime);
        onCooldown = false;

        if (canSeePlayer == false) 
        {
            sentDetection = false;
        }

    }

    private void Aggrivated()
    {
       
        LookAtPosition(playerObj.transform.position);
        if (canFire) FireBullet();
    }

    private void Distracted()
    {
        LookAtPosition(Rock.transform.position);
        if (canFire) FireBullet();
    }

    private bool CanSeePlayer()
    {
        Vector2 playerDirection = (playerObj.transform.position - transform.position).normalized;
        float playerAngleFromView = Vector2.Angle(transform.up, playerDirection);

        if (playerAngleFromView > _detectionFovAngle/2) return false;
        if (Vector2.Distance(playerObj.transform.position, transform.position) > _detectionRange) return false;
        if (RaycastCheckPlayer(playerDirection) == false) return false;

        return true;
    }
    private bool RaycastCheckPlayer(Vector2 playerDirection)
    {
        RaycastHit2D hit = Physics2D.Raycast(_firePoint.position, playerDirection, _detectionRange, _detectionLayers);

        return hit.collider.gameObject.CompareTag("Player");
    }


    private bool CanSeeRock()
    {
        if (Rock == null) return false;

        Vector2 rockDirection = (Rock.transform.position - transform.position).normalized;
        float rockAngleFromView = Vector2.Angle(transform.up, rockDirection);

        if (rockAngleFromView > _detectionFovAngle/2) return false;
        if (Vector2.Distance(Rock.transform.position, transform.position) > _detectionRange) return false;
        if (RaycastCheckRock(rockDirection) == false) return false;

        return true;
    }

    private bool RaycastCheckRock(Vector2 rockDirection)
    {
        RaycastHit2D hit = Physics2D.Raycast(_firePoint.position, rockDirection, _detectionRange, _detectionLayers);

        return hit.collider.gameObject.CompareTag("Rock");
    }




    private void LookAtPosition(Vector2 positionToLook)
    {
        Vector2 lookDirection = (positionToLook - (Vector2)transform.position).normalized;
        float enemyAngle = (Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg) + _lookRotationOffset;

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
            GameManager.Instance.EnemyEliminated();

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
