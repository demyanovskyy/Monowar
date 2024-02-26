using UnityEngine;
public class AimGun : MonoBehaviour
{
    [SerializeField] Transform _arm;
    float _offset = -90;
    Vector3 mousePos;
    public float _flipdistance;

    void Update()
    {

        Aim1();
       
    }
  
    public void Aim1()
    {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 perendicular = _arm.position - mousePos;

            Quaternion val = Quaternion.LookRotation(Vector3.forward, perendicular);
            val *= Quaternion.Euler(0, 0, _offset);
           // Debug.Log("X:" + Mathf.Abs(perendicular.x));
            if (Mathf.Abs(perendicular.x) > _flipdistance)
                _arm.rotation = val;

    }



    protected virtual void OnDrawGizmos()
    {
         Gizmos.DrawLine(_arm.position, mousePos);
        Gizmos.DrawWireSphere(transform.position, _flipdistance);
 
    }
}
