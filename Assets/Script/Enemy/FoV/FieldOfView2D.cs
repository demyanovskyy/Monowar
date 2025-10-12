using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FieldOfViewAI : MonoBehaviour
{
    [Header("Parent")]
    [SerializeField] Enemy parent;
    [Header("Основные настройки")]
    public float viewRadius = 5f;
    [Range(0, 360)] public float viewAngle = 90f;
    public int rayCount = 80;
    public LayerMask obstacleMask;
    public LayerMask targetMask;
    public Color fovColor = new Color(1f, 1f, 0f, 0.25f);

    [Header("AI обнаружение")]
    public bool targetInSight;          // True, если цель видна
    public Transform visibleTarget;     // Ссылка на цель (например, игрок)
    public float detectionDelay = 0.2f; // Интервал обновления логики

    private Mesh mesh;
    public bool facingRight = true;
    private float detectionTimer;

    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        var mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = fovColor;
        GetComponent<MeshRenderer>().material = mat;
    }

    void LateUpdate()
    {

        facingRight = parent.GetFacingDerection();
        //facingRight = transform.eulerAngles.y < 90f || transform.eulerAngles.y > 270f;

        // Генерация визуального поля
        GenerateViewMesh();

        // Проверка цели с интервалом (чтобы не грузить CPU)
        detectionTimer -= Time.deltaTime;
        if (detectionTimer <= 0)
        {
            detectionTimer = detectionDelay;
            FindVisibleTarget();
        }
    }

    void GenerateViewMesh()
    {
        float startAngle = GetBaseAngle();
        float angleStep = viewAngle / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 2];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i <= rayCount; i++)
        {
            float angle = startAngle - angleStep * i;
            Vector3 dir = DirFromAngle(angle);
            Vector3 vertex;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewRadius, obstacleMask);
            if (hit.collider == null)
                vertex = transform.InverseTransformPoint(transform.position + dir * viewRadius);
            else
                vertex = transform.InverseTransformPoint(hit.point);

            vertices[i + 1] = vertex;

            if (i > 0)
            {
                int tri = (i - 1) * 3;
                triangles[tri] = 0;
                triangles[tri + 1] = i;
                triangles[tri + 2] = i + 1;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    void FindVisibleTarget()
    {
        targetInSight = false;
        visibleTarget = null;

        Collider2D[] targetsInRange = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        Vector2 forwardDir = facingRight ? Vector2.right : Vector2.left;

        foreach (Collider2D target in targetsInRange)
        {
            Vector2 dirToTarget = (target.transform.position - transform.position).normalized;
            float angleToTarget = Vector2.Angle(forwardDir, dirToTarget);

            if (angleToTarget < viewAngle / 2)
            {
                float distToTarget = Vector2.Distance(transform.position, target.transform.position);
                if (!Physics2D.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    targetInSight = true;
                    visibleTarget = target.transform;
                    Debug.DrawLine(transform.position, target.transform.position, Color.green);
                    return; // нашли первую видимую цель
                }
            }
        }
    }

    float GetBaseAngle()
    {
        float tempAngle;
        if (facingRight)
            tempAngle = viewAngle / 2f;
        else
        {
            tempAngle = 180f + viewAngle / 2f;
        }
        return tempAngle;

    }

    Vector3 DirFromAngle(float angleInDegrees)
    {
        float rad = angleInDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = targetInSight ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        if(visibleTarget != null)
        Debug.DrawLine(transform.position, visibleTarget.transform.position, Color.green);
    }
}