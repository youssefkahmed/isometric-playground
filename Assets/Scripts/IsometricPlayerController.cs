using UnityEngine;

namespace IsometricPlayground
{
    public class IsometricPlayerController : MonoBehaviour
    {
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float sprintSpeed = 7f;
        [SerializeField] private float turnSpeed = 360f;

        private InputSystem_Actions _inputActions;
        private Rigidbody _rb;
        
        private Vector3 _moveInput;
        private float _currentSpeed;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _currentSpeed = walkSpeed;

            SetUpInputs();
        }

        private void Update()
        {
            Look();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void SetUpInputs()
        {
            _inputActions = new InputSystem_Actions();
            _inputActions.Enable();

            _inputActions.Player.Move.performed += ctx => ReadMoveInput(ctx.ReadValue<Vector2>());
            _inputActions.Player.Move.canceled += ctx => ReadMoveInput(ctx.ReadValue<Vector2>());
            
            _inputActions.Player.Sprint.performed += _ => ReadSprintInput(true);
            _inputActions.Player.Sprint.canceled += _ => ReadSprintInput(false);
        }

        private void ReadMoveInput(Vector2 input)
        {
            _moveInput = new Vector3(input.x, 0, input.y);
        }
        
        private void ReadSprintInput(bool isPressed)
        {
            _currentSpeed = isPressed ? sprintSpeed : walkSpeed;
        }

        private void Look()
        {
            if (_moveInput == Vector3.zero)
            {
                return;
            }
            
            Vector3 skewedInput = _moveInput.ToIsometric();
                
            Vector3 relative = transform.position + skewedInput - transform.position; 
            Quaternion targetRotation = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        
        private void Move()
        {
            _rb.MovePosition(transform.position + _currentSpeed * _moveInput.magnitude * Time.deltaTime * transform.forward);
        }
    }
}
