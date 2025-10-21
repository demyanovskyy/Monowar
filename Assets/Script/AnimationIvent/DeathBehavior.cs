using UnityEngine;

public class DeathBehavior : StateMachineBehaviour
{



    public IsPooleble _explodeparts;
    private Transform _explodePoint;
    public float explodeTimer = 0.1f;
    private float nextexplodeTimer;

    public float _explodeRangeX = 2.0f;
    public float _explodeRangeY = .5f;

     private bool changeCollider;


    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        animator.gameObject.layer = 31;//noCollitablePlayer layer set
        _explodePoint = animator.GetComponent<Transform>().Find("ExplodePoint").transform;
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (_explodeparts != null)
        {
            nextexplodeTimer += Time.deltaTime;
            if (nextexplodeTimer / 5 >= explodeTimer)
            {
                nextexplodeTimer = 0;

                if (_explodePoint != null)
                {

                    Vector3 position = new Vector3(Random.Range(_explodePoint.transform.position.x - _explodeRangeX, _explodePoint.transform.position.x + _explodeRangeX),
                     Random.Range(_explodePoint.transform.position.y - _explodeRangeY, _explodePoint.transform.position.y + _explodeRangeY), 0);

                    IsPooleble _explode = ServiceLocator.Current.Get<LevelManager>().objectPoole.GetObject(_explodeparts);

                    _explode.transform.position = position;
                    _explode.transform.rotation = Quaternion.identity;
                    _explode.SetActive(true);

                }
                else
                {
                    Vector3 position = new Vector3(Random.Range(animator.transform.position.x - 1.0f, animator.transform.position.x + 1.0f),
                    Random.Range(animator.transform.position.y - 1.0f, animator.transform.position.y + 1.0f), 0);

                    IsPooleble _explode = ServiceLocator.Current.Get<LevelManager>().objectPoole.GetObject(_explodeparts);
                   
                    _explode.transform.position = position;
                    _explode.transform.rotation = Quaternion.identity;
                    _explode.SetActive(true);
 
                }


            }
        }
    }

}
