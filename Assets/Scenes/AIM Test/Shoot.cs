using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Shoot : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _parent;
  
    [SerializeField] private GameObject MuzleFlash;
    [SerializeField] private GameObject MuzleFlashPartical;
    
    [Range(0, 5)]
    [SerializeField] private int framToFlash = 1;
    [SerializeField] private bool flashingSprite = false;
    [SerializeField] private bool flashingParticle = false;


    //Animation fidback gun(arm)
    [SerializeField] private Transform _gunBody;
    [SerializeField] private float _deltaX = -0.2f;
    [SerializeField] private float _speedReturn = 2f;
    private Vector2 _gunBodyTempPos;

    public List<ParticleSystem> particles;

    private void Start()
    {
        
        if(flashingParticle) FillListParticle();

        MuzleFlash.SetActive(false);
        _gunBodyTempPos = new Vector2(_gunBody.localPosition.x, _gunBody.localPosition.y);
    }

    private void FillListParticle()
    {
        for(int i=0; i<MuzleFlashPartical.transform.childCount; i++)
        {
            var ps = MuzleFlashPartical.transform.GetChild(i).GetComponent<ParticleSystem>();
            if(ps!=null)
            {
                particles.Add(ps);
            }
        }
        
    }

    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            if(_parent.localScale.x > 0)
                Instantiate(_bulletPrefab, _shootPoint.position, _shootPoint.rotation).Shoot();
            else
                Instantiate(_bulletPrefab, _shootPoint.position, _shootPoint.rotation * new Quaternion(1, 1, _shootPoint.rotation.z + 180, 1)).Shoot();

            _gunBody.localPosition = new Vector2(_gunBody.localPosition.x + _deltaX, _gunBody.localPosition.y);
          
            if (!flashingSprite)
            {
                StartCoroutine(FlashTime());
            }

            if (flashingParticle)
            {
                for (int i = 0; i < particles.Count; i++)
                {
                    particles[i].Play();
                }
            }

        }
        _gunBody.localPosition = Vector2.Lerp(_gunBody.localPosition, _gunBodyTempPos, _speedReturn*Time.deltaTime);
  
    }
    

    IEnumerator FlashTime()
    {
        MuzleFlash.SetActive(true);
        int frameflash = 0;
        flashingSprite = true;
        while (frameflash <= framToFlash)
        {
            frameflash++;
            yield return null;
        }
        MuzleFlash.SetActive(false);
        flashingSprite = false;
    }
}