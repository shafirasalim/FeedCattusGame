using System.Collections;
using UnityEngine;

namespace TopDown_Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        public enum EnemyState { Patrol, Chase }

        [Header("References")]
        public Transform player;

        [Header("Movement")]
        public float patrolSpeed = 2f;
        public float chaseSpeed = 4f;

        [Header("Detection")]
        public float chaseRange = 5f;
        public float verticalRange = 1f;

        [Header("Layers")]
        public LayerMask groundLayer;
        public LayerMask obstacleLayer;

        [Header("Raycast")]
        public float wallCheckDist = 0.2f;
        public float groundCheckDist = 1f;

        [Header("Direction")]
        public bool faceRight = false;
        public float turnDelay = 0.2f;

        [Header("Debug")]
        public bool showDebugLogs = true;

        private Rigidbody2D rb;
        private SpriteRenderer sr;
        private EnemyState state;
        private float dir = 1f;
        private bool canTurn = true;
        private bool isBlocked = false;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            state = EnemyState.Patrol;

            if (showDebugLogs)
                Debug.Log($"[ENEMY] {name} started | State: PATROL");
        }

        void Update()
        {
            float distance = Vector2.Distance(transform.position, player.position);
            CheckObstacles();

            if (state == EnemyState.Patrol)
            {
                if (CanSeePlayer(distance))
                {
                    if (showDebugLogs)
                        Debug.Log("[STATE] Patrol → Chase (Player detected!)");
                    state = EnemyState.Chase;
                }
            }
            else if (state == EnemyState.Chase)
            {
                if (isBlocked)
                {
                    if (showDebugLogs)
                        Debug.Log("[STATE] Chase → Patrol (Blocked by obstacle)");
                    state = EnemyState.Patrol;
                    return;
                }

                if (!CanSeePlayer(distance))
                {
                    if (showDebugLogs)
                        Debug.Log("[STATE] Chase → Patrol (Player lost)");
                    state = EnemyState.Patrol;
                    return;
                }

                dir = (player.position.x > transform.position.x) ? 1f : -1f;
                if ((dir > 0 && !faceRight) || (dir < 0 && faceRight)) Flip();
            }

            if (state == EnemyState.Patrol && isBlocked && canTurn)
            {
                TurnAround();
            }
        }

        void FixedUpdate()
        {
            float speed = (state == EnemyState.Chase) ? chaseSpeed : patrolSpeed;

            if (state == EnemyState.Chase && isBlocked)
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
                return;
            }

            rb.velocity = new Vector2(dir * speed, rb.velocity.y);
        }

        bool CanSeePlayer(float distance)
        {
            if (player == null || distance > chaseRange) return false;

            float verticalDistance = Mathf.Abs(player.position.y - transform.position.y);
            if (verticalDistance > verticalRange) return false;

            RaycastHit2D enemyGround = Physics2D.Raycast(transform.position, Vector2.down, 5f, groundLayer);
            RaycastHit2D playerGround = Physics2D.Raycast(player.position, Vector2.down, 5f, groundLayer);

            if (enemyGround.collider == null || playerGround.collider == null) return false;
            if (Mathf.Abs(enemyGround.point.y - playerGround.point.y) > 0.3f) return false;

            Vector2 eye = (Vector2)transform.position + new Vector2(0f, 0.4f);
            Vector2 target = (Vector2)player.position;

            RaycastHit2D obstacleHit = Physics2D.Linecast(eye, target, obstacleLayer);

            if (showDebugLogs)
                Debug.DrawLine(eye, target, obstacleHit.collider ? Color.red : Color.green, 0.1f);

            return obstacleHit.collider == null;
        }

        void CheckObstacles()
        {
            Vector2 forward = new Vector2(dir, 0);
            Vector2 wallPos = (Vector2)transform.position + forward * 0.6f;
            Vector2 edgePos = (Vector2)transform.position + forward * 0.6f + Vector2.down * 0.8f;

            bool hitWall = Physics2D.Raycast(wallPos, forward, wallCheckDist, groundLayer | obstacleLayer);
            bool hasGround = Physics2D.Raycast(edgePos, Vector2.down, groundCheckDist, groundLayer);

            isBlocked = hitWall || !hasGround;

            if (showDebugLogs)
            {
                Debug.DrawRay(wallPos, forward * wallCheckDist, hitWall ? Color.red : Color.yellow, 0.5f);
                Debug.DrawRay(edgePos, Vector2.down * groundCheckDist, hasGround ? Color.green : Color.red, 0.5f);
            }
        }

        void TurnAround()
        {
            dir *= -1f;
            Flip();
            canTurn = false;
            StartCoroutine(TurnCooldown());
        }

        IEnumerator TurnCooldown()
        {
            yield return new WaitForSeconds(turnDelay);
            canTurn = true;
        }

        void Flip()
        {
            faceRight = !faceRight;
            sr.flipX = !sr.flipX;
        }
    }
}