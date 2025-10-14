using Assets.Scripts.Core.ObjectPooling;
using System;
using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
public class EnemyBullet : MonoBehaviour, IPoolable
{
    [SerializeField] private float damge = 1;
    [SerializeField] Rigidbody2D _rigidbody2D;
    [SerializeField] float _moveSpeed;
    [SerializeField] IsPooleble hitEffectPrefab;
    //==========for object poll==============================
    private Coroutine _coroutine;
    public event Action<IPoolable> OnReturnToPool;

    public Transform Transform => transform;

    public GameObject GameObject => gameObject;


    private IEnumerator OestroyCorutine()
    {
        yield return new WaitForSeconds(1.5f);
        OnDestroyed();
    }
    public void Reset()
    {

        OnDestroyed();
    }

    public void OnDestroyed()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        TrailRenderer t = this.GetComponentInChildren<TrailRenderer>();
        if (t != null)
        {
            t.Clear();
        }

        OnReturnToPool?.Invoke(this);

    }

    public void SetParamiter(Vector3 bTransform, Quaternion bRotate)
    {
        _coroutine = StartCoroutine(OestroyCorutine());

        _rigidbody2D.transform.position = bTransform;
        _rigidbody2D.transform.rotation = bRotate;
    }
    //===================================================================================================



    public void BulletMove()
    {
        _coroutine = StartCoroutine(OestroyCorutine());

        _rigidbody2D.linearVelocity = transform.right * _moveSpeed;

        // Destroy(this.gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        IDamageable damageableObject = collision.GetComponent<IDamageable>();

        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            // enemis
            playerStats.DamagePlayer(damge);
        }
        else
        if (damageableObject != null)
        {
            // for destructible objrcts
            damageableObject.TakeDamage(damge);
        }


        //if (hitEffectPrefab != null)
        //{
        //    GameObject hitEffect = Instantiate(hitEffectPrefab, this.transform.position, this.transform.rotation);
        //    Destroy(hitEffect, 3f);
        //}
        BulletEffect();
        OnDestroyed();
        // Destroy(this.gameObject);

    }

    public virtual void BulletEffect()
    {
        if (hitEffectPrefab != null)
        {
            IsPooleble _explode = ServiceLocator.Current.Get<LevelManager>().objectPoole.GetObject(hitEffectPrefab);
            if (_explode != null)
            {
                _explode.transform.position = transform.position;
                _explode.transform.rotation = Quaternion.identity;
                _explode.SetActive(true);
            }
        }

        OnDestroyed();

    }

}