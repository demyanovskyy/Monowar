
using UnityEngine;

public class RotateToTargetWithProperFlipAndGizmos : MonoBehaviour
{
    [Header("Transform object")]
    public Transform transformObject;


    [Header("Target Settings")]
    public Transform target;
    public Transform parentTransform;

    [Header("Rotation Settings")]
    [SerializeField] private float startAngle = 0f;
    public float rotationSpeed = 5f;
    public float minAngle = -45f;
    public float maxAngle = 45f;

    public bool offRotate = false;
    
    private bool isRotate = true;


    [Header("Gizmo Settings")]
    public float gizmoRayLength = 2f;
    public Color currentDirColor = Color.green;
    public Color minAngleColor = Color.red;
    public Color maxAngleColor = Color.red;
    public Color arcColor = new Color(1f, 0f, 0f, 0.2f);

    




    void Start()
    {
        //startAngle = transformObject.eulerAngles.z;
    }

    public void SetIsRotate(bool r)
    {
        isRotate = r;
    }

    void Update()
    {
        if (target == null || parentTransform == null) return;

        Vector2 dir = target.position - transformObject.position;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Проверяем отражение родителя по оси Y
        bool isFlipped = Mathf.Abs(Mathf.DeltaAngle(parentTransform.eulerAngles.y, 180f)) < 1f;

        if (isFlipped)
        {
            targetAngle = 180f - targetAngle;
        }



        if (!offRotate || !isRotate)
        {
            targetAngle = 0f;
        }

        // Ограничиваем угол относительно исходного
        float relativeAngle = Mathf.DeltaAngle(startAngle, targetAngle);
        float clampedAngle = Mathf.Clamp(relativeAngle, minAngle, maxAngle);
        float finalAngle = startAngle + clampedAngle;



 

        // Целевой поворот по Z
        Quaternion targetRot = Quaternion.Euler(0f, isFlipped ? 180f : 0f, finalAngle);



        // Плавный поворот
        transformObject.rotation = Quaternion.Lerp(transformObject.rotation, targetRot, rotationSpeed * Time.deltaTime);
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
        DrawCurrentDirectionLine(transformObject.eulerAngles.z, currentDirColor, gizmoRayLength, flipped);


        // Дополнительно можно нарисовать дугу (конус)
        DrawArc(baseAngle, minAngle, maxAngle, gizmoRayLength * 0.8f, arcColor);
    }

    void DrawDirectionLine(float angleDeg, Color color, float length)
    {
        Gizmos.color = color;
        float rad = angleDeg * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
        Gizmos.DrawLine(transformObject.position, transformObject.position + dir * length);
    }

    // 🔹 Добавленная версия специально для зелёной линии
    void DrawCurrentDirectionLine(float angleDeg, Color color, float length, bool flipped)
    {
        Gizmos.color = color;
        float rad = angleDeg * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);

        if (flipped)
            dir.x = -dir.x; // только зелёную линию отражаем по X

        Gizmos.DrawLine(transformObject.position, transformObject.position + dir * length);
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
            Vector3 point = transformObject.position + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;

            if (i > 0)
                Gizmos.DrawLine(prevPoint, point);

            prevPoint = point;
        }
    }
}
