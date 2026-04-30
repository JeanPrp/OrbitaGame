using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerShipController : MonoBehaviour
{
    [Header("Joystick")]
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private float joystickSensitivity = 1.25f;

    [Header("Velocidade")]
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float acceleration = 24f;
    [SerializeField] private float brakeAcceleration = 20f;
    [SerializeField] private float idleDamping = 5f;

    [Header("Visual")]
    [SerializeField] private Transform shipVisual;
    [SerializeField] private float rotateSpeed = 16f;

    [Header("Limites na tela")]
    [SerializeField] private float horizontalScreenPadding = 0.35f;
    [SerializeField] private float bottomMargin = 0.8f;
    [SerializeField] private float topMargin = 1.0f;
    [SerializeField] private bool useRelativeViewportMargins = true;
    [SerializeField, Range(0f, 0.45f)] private float horizontalViewportPadding = 0.08f;
    [SerializeField, Range(0f, 0.45f)] private float bottomViewportMargin = 0.08f;
    [SerializeField, Range(0f, 0.45f)] private float topViewportMargin = 0.10f;

    private Rigidbody2D rb;
    private Camera mainCam;

    private Vector2 inputVector;
    private bool hasInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;

        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Update()
    {
        ReadInput();
        RotateShipVisual();
    }

    private void FixedUpdate()
    {
        Vector2 currentVelocity = rb.linearVelocity;
        Vector2 targetVelocity = Vector2.zero;

        if (hasInput)
        {
            targetVelocity = inputVector * maxSpeed;

            float accel = Vector2.Dot(currentVelocity, targetVelocity) < 0f
                ? brakeAcceleration
                : acceleration;

            rb.linearVelocity = Vector2.MoveTowards(
                currentVelocity,
                targetVelocity,
                accel * Time.fixedDeltaTime
            );
        }
        else
        {
            rb.linearVelocity = Vector2.MoveTowards(
                currentVelocity,
                Vector2.zero,
                idleDamping * Time.fixedDeltaTime
            );
        }

        ClampInsideCamera();
    }

    private void ReadInput()
    {
        inputVector = Vector2.zero;
        hasInput = false;

        if (joystick != null)
        {
            Vector2 joy = new Vector2(joystick.Horizontal, joystick.Vertical);

            if (joy.sqrMagnitude > 0.0001f)
            {
                joy *= joystickSensitivity;
                joy = Vector2.ClampMagnitude(joy, 1f);

                inputVector = joy;
                hasInput = true;
                return;
            }
        }

        Vector2 keyDir = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        if (keyDir.sqrMagnitude > 0.01f)
        {
            inputVector = Vector2.ClampMagnitude(keyDir, 1f);
            hasInput = true;
        }
    }

    private void RotateShipVisual()
    {
        if (shipVisual == null) return;

        Vector2 velocity = rb.linearVelocity;
        if (velocity.sqrMagnitude < 0.01f) return;

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        shipVisual.rotation = Quaternion.Lerp(
            shipVisual.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );
    }

    private void ClampInsideCamera()
    {
        float camHeight = mainCam.orthographicSize;
        float camWidth = camHeight * mainCam.aspect;
        Vector3 camPos = mainCam.transform.position;

        Vector2 pos = rb.position;
        Vector2 vel = rb.linearVelocity;

        float minX;
        float maxX;
        float minY;
        float maxY;

        if (useRelativeViewportMargins)
        {
            minX = camPos.x - camWidth + (camWidth * 2f * horizontalViewportPadding);
            maxX = camPos.x + camWidth - (camWidth * 2f * horizontalViewportPadding);
            minY = camPos.y - camHeight + (camHeight * 2f * bottomViewportMargin);
            maxY = camPos.y + camHeight - (camHeight * 2f * topViewportMargin);
        }
        else
        {
            minX = camPos.x - camWidth + horizontalScreenPadding;
            maxX = camPos.x + camWidth - horizontalScreenPadding;
            minY = camPos.y - camHeight + bottomMargin;
            maxY = camPos.y + camHeight - topMargin;
        }

        if (pos.x < minX)
        {
            pos.x = minX;
            vel.x = Mathf.Max(0f, vel.x);
        }
        else if (pos.x > maxX)
        {
            pos.x = maxX;
            vel.x = Mathf.Min(0f, vel.x);
        }

        if (pos.y < minY)
        {
            pos.y = minY;
            vel.y = Mathf.Max(0f, vel.y);
        }
        else if (pos.y > maxY)
        {
            pos.y = maxY;
            vel.y = Mathf.Min(0f, vel.y);
        }

        rb.position = pos;
        rb.linearVelocity = vel;
    }
}