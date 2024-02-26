using UnityEngine;
public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody2D;
    [SerializeField] float _moveSpeed;
    public void Shoot()
    {
        _rigidbody2D.velocity = transform.right * _moveSpeed;// * (mousePos - transform.position);
        
        Destroy(this.gameObject, 5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //Kill Enemy Here
            //collision.gameObject.GetComponent<Enemy>();
            Destroy(this.gameObject);
        }
    }

}
