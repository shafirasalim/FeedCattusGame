using UnityEngine;
namespace TopDown_Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        public enum EnemyState { Patrol, Chase }

        [Header("References")]
        public Transform player;

        [Header("Speed Settings")]
        public float patrolSpeed = 2f;
        public float chaseSpeed = 4f;

        [Header("Detection")]
        public float chaseRange = 6f;
        public float patrolChangeTime = 3f;

        [Header("Rotation")]
        public float rotationSpeed = 250f;

        private Rigidbody2D rb;
        private Vector2 moveDirection;
        private float patrolTimer;
        private EnemyState currentState;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            currentState = EnemyState.Patrol;
            ChooseRandomDirection();
        }

        void Update()
        {
            float distance = Vector2.Distance(transform.position, player.position);

            switch (currentState)
            {
                case EnemyState.Patrol:
                    PatrolState(distance);
                    break;
                case EnemyState.Chase:
                    ChaseState(distance);
                    break;
            }
            RotateToMovement();
        }

        void FixedUpdate()
        {
            float currentSpeed = currentState == EnemyState.Patrol ? patrolSpeed : chaseSpeed;

            Vector2 newVelocity = new Vector2(moveDirection.x * currentSpeed, rb.velocity.y);
            rb.velocity = newVelocity;

            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        }

        void PatrolState(float distance)
        {
            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolChangeTime)
            {
                ChooseRandomDirection();
                patrolTimer = 0f;
            }

            if (distance <= chaseRange)
            {
                currentState = EnemyState.Chase;
            }
        }

        void ChaseState(float distance)
        {
            moveDirection = (player.position - transform.position).normalized;

            if (distance > chaseRange)
            {
                currentState = EnemyState.Patrol;
            }
        }

        void ChooseRandomDirection()
        {
            float randomX = Random.Range(-1f, 1f);
            float randomY = Random.Range(-1f, 1f);
            moveDirection = new Vector2(randomX, randomY).normalized;
        }

        void RotateToMovement()
        {
            if (moveDirection != Vector2.zero)
            {
                float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                targetAngle += 90f;

                Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (currentState == EnemyState.Patrol)
            {
                ChooseRandomDirection();
            }
        }
    }
}