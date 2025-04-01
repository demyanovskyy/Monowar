using System.Collections;
using UnityEngine;
public class Shoot : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _parent;
  
    [SerializeField] private GameObject MuzleFlash;
    [Range(0, 5)]
    [SerializeField] private int framToFlash = 1;
    [SerializeField] private bool flashing = false;

    
    //Animation fidback gun(arm)
    [SerializeField] private Transform _gunBody;
    [SerializeField] private float _deltaX = -0.2f;
    [SerializeField] private float _speedReturn = 2f;
    private Vector2 _gunBodyTempPos;


    private void Start()
    {
        MuzleFlash.SetActive(false);
        _gunBodyTempPos = new Vector2(_gunBody.localPosition.x, _gunBody.localPosition.y);
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
          
            if (!flashing)
            {
                StartCoroutine(FlashTime());
            }

        }
        _gunBody.localPosition = Vector2.Lerp(_gunBody.localPosition, _gunBodyTempPos, _speedReturn*Time.deltaTime);
  
    }
    

    IEnumerator FlashTime()
    {
        MuzleFlash.SetActive(true);
        int frameflash = 0;
        flashing = true;
        while (frameflash <= framToFlash)
        {
            frameflash++;
            yield return null;
        }
        MuzleFlash.SetActive(false);
        flashing = false;
    }
}