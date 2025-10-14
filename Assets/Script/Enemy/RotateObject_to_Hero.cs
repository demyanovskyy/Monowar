//using UnityEngine;

//public class RotateObject_to_Hero : MonoBehaviour
//{
//    [SerializeField] Enemy parent;
//    [SerializeField] public bool isRotate = true;  //ìîæåò âðàùàòñÿ 
//    [SerializeField] private float speed = 5f;//ñêîðîñòü ïîâîðîòà
//    [SerializeField] private float minAngle = -40; // îãðàíè÷åíèå ïî óãëàì
//    [SerializeField] private float maxAngle = 40;
//    [SerializeField] private float deltaFace = 0f;//ñìåùåíèå ãîëîâû
//    [SerializeField] private bool offAngleZero = false;
//    [SerializeField] private Transform targetRotate;//Target point

//    private Vector3 angeMin, angeMax;
//    private float invert = 1;
//    private float angle;
//    private float radius = 20f;
//    private float deltaAngle = -90;





//    public void OnEnable()
//    {
//        if (targetRotate == null)
//        {

//            // targetRotate = EventBus._playerShotPoint?.Invoke(); //GameObject.FindGameObjectWithTag("PlayerShootPoint").transform; //Player.instance.transform;

//        }

//    }

//    public void ChekPlayer()
//    {
//        if (targetRotate == null)
//        {
//            // targetRotate = EventBus._playerShotPoint?.Invoke(); //Player.instance.transform;

//        }
//    }
//    private void Update()
//    {
//        ChekPlayer();

//        if (parent.GetFacingDerection())
//            invert = 1;
//        else
//            invert = -1;

//        if (isRotate)
//        {
//            Vector3 rotateTarget = targetRotate.position;

//            Vector2 direction = new Vector3(rotateTarget.x, rotateTarget.y + deltaFace, 10.0f) - transform.position;
//            angle = Mathf.Atan2(direction.y, direction.x * invert) * Mathf.Rad2Deg;
//            angle = Mathf.Clamp(angle, minAngle, maxAngle);

//            Quaternion rotation = Quaternion.AngleAxis(angle * invert, Vector3.forward);
//            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);

//        }

//        if (!offAngleZero && !isRotate)
//        {
//            if (targetRotate != null)
//            {
//                if (targetRotate.position.x < transform.position.x && parent.GetFacingDerection())
//                {
//                    angle = 0;
//                }
//                else if (targetRotate.position.x > transform.position.x && !parent.GetFacingDerection())
//                {
//                    angle = 0;
//                }
//            }
//            else
//                angle = 0;

//            Quaternion rotation = Quaternion.AngleAxis(angle * invert, Vector3.forward);
//            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
//        }

//    }

//    private void OnDrawGizmos()
//    {

//        if (invert > 0)
//        {
//            angeMin = DerectionFromAngle(0, -deltaAngle - minAngle);
//            angeMax = DerectionFromAngle(0, -deltaAngle - maxAngle);
//        }
//        else
//        {
//            angeMin = DerectionFromAngle(180, -deltaAngle + minAngle);
//            angeMax = DerectionFromAngle(180, -deltaAngle + maxAngle);
//        }



//        Gizmos.color = Color.magenta;
//        Gizmos.DrawLine(transform.position, transform.position + angeMin * radius);
//        Gizmos.color = Color.blue;
//        Gizmos.DrawLine(transform.position, transform.position + angeMax * radius);


//    }


//    public static Vector2 DerectionFromAngle(float eulerY, float angleInDegrees)
//    {
//        angleInDegrees += eulerY;

//        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
//    }


//}


using UnityEngine;

[ExecuteAlways]
public class RotateToTargetWithProperFlipAndGizmos : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    public Transform parentTransform;

    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;
    public float minAngle = -45f;
    public float maxAngle = 45f;

    [Header("Gizmo Settings")]
    public float gizmoRayLength = 2f;
    public Color currentDirColor = Color.green;
    public Color minAngleColor = Color.red;
    public Color maxAngleColor = Color.red;
    public Color arcColor = new Color(1f, 0f, 0f, 0.2f);

    private float startAngle;

    void Start()
    {
        startAngle = transform.eulerAngles.z;
    }

    void Update()
    {
        if (target == null || parentTransform == null) return;

        Vector2 dir = target.position - transform.position;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Проверяем отражение родителя по оси Y
        bool isFlipped = Mathf.Abs(Mathf.DeltaAngle(parentTransform.eulerAngles.y, 180f)) < 1f;

        if (isFlipped)
        {
            targetAngle = 180f - targetAngle;
        }

        // Ограничиваем угол относительно исходного
        float relativeAngle = Mathf.DeltaAngle(startAngle, targetAngle);
        float clampedAngle = Mathf.Clamp(relativeAngle, minAngle, maxAngle);
        float finalAngle = startAngle + clampedAngle;

        // Целевой поворот по Z
        Quaternion targetRot = Quaternion.Euler(0f, isFlipped ? 180f : 0f, finalAngle);

        // Плавный поворот
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }
 
    void OnDrawGizmos()
    {
        if (parentTransform == null) return;

        bool flipped = Mathf.Abs(Mathf.DeltaAngle(parentTransform.eulerAngles.y, 180f)) < 1f ||
                       Mathf.Abs(Mathf.DeltaAngle(parentTransform.eulerAngles.y, -180f)) < 1f;

        // Базовый угол, учитывая отражение
        float baseAngle = flipped ? 180f - startAngle : startAngle;

        // Отрисовка диапазона
        DrawDirectionLine(baseAngle + minAngle, minAngleColor, gizmoRayLength * 0.8f);
        DrawDirectionLine(baseAngle + maxAngle, maxAngleColor, gizmoRayLength * 0.8f);
        //DrawDirectionLine(transform.eulerAngles.z, currentDirColor, gizmoRayLength);
        // ✅ Зелёная линия с учётом отражения
        DrawCurrentDirectionLine(transform.eulerAngles.z, currentDirColor, gizmoRayLength, flipped);


        // Дополнительно можно нарисовать дугу (конус)
        DrawArc(baseAngle, minAngle, maxAngle, gizmoRayLength * 0.8f, arcColor);
    }

    void DrawDirectionLine(float angleDeg, Color color, float length)
    {
        Gizmos.color = color;
        float rad = angleDeg * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
        Gizmos.DrawLine(transform.position, transform.position + dir * length);
    }

    // 🔹 Добавленная версия специально для зелёной линии
    void DrawCurrentDirectionLine(float angleDeg, Color color, float length, bool flipped)
    {
        Gizmos.color = color;
        float rad = angleDeg * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);

        if (flipped)
            dir.x = -dir.x; // только зелёную линию отражаем по X

        Gizmos.DrawLine(transform.position, transform.position + dir * length);
    }
    void DrawArc(float baseAngle, float min, float max, float radius, Color color)
    {
        Gizmos.color = color;
        int segments = 20;
        Vector3 prevPoint = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;
            float angle = baseAngle + Mathf.Lerp(min, max, t);
            float rad = angle * Mathf.Deg2Rad;
            Vector3 point = transform.position + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;

            if (i > 0)
                Gizmos.DrawLine(prevPoint, point);

            prevPoint = point;
        }
    }
}
