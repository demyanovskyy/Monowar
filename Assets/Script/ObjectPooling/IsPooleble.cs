
using Assets.Scripts.Core.ObjectPooling;
using System;
using System.Collections;
using UnityEngine;

public class IsPooleble : MonoBehaviour, IPoolable
{
    public float lifeDuration = 2f;

    private Coroutine _coroutine;

    public event Action<IPoolable> OnReturnToPool;

    public Transform Transform => transform;

    public GameObject GameObject => gameObject;

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
        OnReturnToPool?.Invoke(this);
    }

    private IEnumerator OestroyCorutine()
    {
        yield return new WaitForSeconds(lifeDuration);
        OnDestroyed();
    }



    // Start is called before the first frame update
    private void OnEnable()
    {
        _coroutine = StartCoroutine(OestroyCorutine());
    }



}
