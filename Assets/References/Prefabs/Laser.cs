using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float defDistanceRay = 100;
    public Transform laserFirePoint;
    public LineRenderer m_lineRender;
    public LayerMask coisionMask;
    public GameObject startVFX;
    public GameObject endVFX;
    private List<ParticleSystem> particle = new List<ParticleSystem>();
    private bool changestatus = false;
    public bool on = true;

    private void Start()
    {
        FillLists();
    }
    private void Update()
    {
        if(on)
        {
            EnableleLaser();
            ShotLaser();
        }
        else
        {
            DisebleLaser();
        }
        
        
    }
    void ShotLaser()
    {

        RaycastHit2D _hit = Physics2D.Raycast((Vector2)laserFirePoint.position, transform.right, defDistanceRay, coisionMask);

        if (_hit)
        {
            //Debug.Log("LHit:" + _hit);
            Draw2DRay((Vector2)laserFirePoint.position, (Vector2)_hit.point);
            if (_hit.collider.tag == "Player")
            {
                //EventBus._playerIsDamaje?.Invoke(2);
                //_hit.collider.GetComponent<PlayerHealthIManager>().ApplyDamage(2f, transform.position);
            }
        }
        else
        {
            //Debug.Log("LHit:" + _hit);
            Draw2DRay((Vector2)laserFirePoint.position, (Vector2)laserFirePoint.right * defDistanceRay);

        }

  
    }

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        startVFX.transform.position = (Vector2)startPos;
        endVFX.transform.position = (Vector2)endPos;
        m_lineRender.SetPosition(0, startPos);
        m_lineRender.SetPosition(1, endPos);

    }
    void EnableleLaser()
    {
        if (!changestatus)
        {
            for (int i = 0; i < particle.Count; i++)
            {
                particle[i].Play();
            }
            m_lineRender.SetActive(true);
            on = true;
            changestatus = true;
        }

    }
    void DisebleLaser()
    {
        if (changestatus)
        {
            for (int i = 0; i < particle.Count; i++)
            {
                particle[i].Stop();
            }
            m_lineRender.SetActive(false);
            on = false;
            changestatus = false;
        }
    }

    void FillLists()
    {
        for (int i = 0; i < startVFX.transform.childCount; i++)
        {
            ParticleSystem ps = startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if(ps!=null)
            {
                particle.Add(ps);
            }
        }
        for (int i = 0; i < endVFX.transform.childCount; i++)
        {
            ParticleSystem ps = endVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
            {
                particle.Add(ps);
            }
        }
    }
}
