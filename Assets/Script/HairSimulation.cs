using UnityEngine;

public class HairSimulation : MonoBehaviour
{
    public GameObject head;  // Голова персонажа
    public GameObject hairSegmentPrefab;  // Префаб для сегмента волос
    public int numberOfSegments = 4;  // Количество сегментов волос

    void Start()
    {
        CreateHair();
    }

    void CreateHair()
    {
        Rigidbody2D previousSegmentRigidbody = head.GetComponent<Rigidbody2D>();  // Ригидбоди головы, к которому будут прикрепляться волосы

        // Создаем сегменты волос и привязываем их друг к другу
        for (int i = 0; i < numberOfSegments; i++)
        {
            // Создаем новый сегмент волос
            GameObject hairSegment = Instantiate(hairSegmentPrefab, head.transform.position, Quaternion.identity);
            hairSegment.name = "HairSegment_" + i;

            // Добавляем Rigidbody2D
            Rigidbody2D rb = hairSegment.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0.5f;  // Уменьшаем силу гравитации для волос, чтобы они не падали слишком быстро
            rb.linearDamping = 0.1f;  // Меньше трения
            rb.angularDamping = 0.1f;  // Меньше углового трения

            // Добавляем HingeJoint2D для соединения сегментов
            HingeJoint2D hingeJoint = hairSegment.AddComponent<HingeJoint2D>();
            hingeJoint.connectedBody = previousSegmentRigidbody;  // Подключаем сегмент к предыдущему сегменту
            hingeJoint.anchor = new Vector2(0, 0.5f);  // Точка крепления на текущем сегменте
            hingeJoint.connectedAnchor = new Vector2(0, -0.5f);  // Точка привязки на предыдущем сегменте

            // Настройка ограничений углов вращения
            JointAngleLimits2D limits = hingeJoint.limits;
            limits.min = -10f;  // Ограничение на минимальный угол
            limits.max = 10f;   // Ограничение на максимальный угол
            hingeJoint.limits = limits;

            hingeJoint.useLimits = true;  // Включаем ограничения

            //// Добавление пружины для более плавного движения
            //hingeJoint.useSpring = true;
            //JointSpring2D spring = hingeJoint.spring;
            //spring.frequency = 5f;  // Частота пружины
            //spring.dampingRatio = 0.5f;  // Амортизация пружины
            //hingeJoint.spring = spring;

            // Добавление мотора (по желанию)
            hingeJoint.useMotor = true;
            JointMotor2D motor = hingeJoint.motor;
            motor.motorSpeed = 5f;  // Скорость мотора
            motor.maxMotorTorque = 10f;  // Максимальный момент
            hingeJoint.motor = motor;

            // Обновляем `previousSegmentRigidbody` на текущий сегмент
            previousSegmentRigidbody = rb;
        }
    }
}