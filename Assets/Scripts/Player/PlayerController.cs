using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _jumpForce;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] SpriteRenderer _playerSprite;
    [SerializeField] Transform _groundDetectStartPosition;
    [SerializeField] float _groundDetectDistance = .7f;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] CapsuleCollider2D _collider;
    [SerializeField] float _dashSpeed;
    [SerializeField] float _dashTime;
    [SerializeField] float _waitAfterDashing;
    [SerializeField] GameObject _standing;
    [SerializeField] GameObject _ball;
    [SerializeField] float _waitToBall;
    [SerializeField] Transform _standDetectStartPosition;
    [SerializeField] float _standDetectDistance = .2f;
    [SerializeField] float _ballSpeedMultiplier = .8f;
    [SerializeField] CircleCollider2D _ballCollider;
    [SerializeField] float _coyoteTime = .3f;

    private float _coyoteCounter;
    bool _resetJumpNeeded = false;
    PlayerAnimation _playerAnimation;
    FireController _fireController;
    bool _canDoubleJump = false;
    float _dashCounter;
    PlayerEffectController _playerEffectController;
    float _dashRechargeCounter;
    float _ballCounter;
    bool _canStand;
    AbilitiesController _abilitiesController;
    bool _canInput;
    Animation _anim;
    private float _speedScale = 1;
    private float _jumpForceScale = 1;
    private bool _invertedControl = false;
    private bool _canStop = true;
    private bool _isVisible = true;
    AudioSource _audioSource;

    private void Start()
    {
        ScoreController.GetInstance()?.ResetScore();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _fireController = GetComponent<FireController>();
        _playerEffectController = GetComponent<PlayerEffectController>();
        SwitchToStanding();
        _abilitiesController = GetComponent<AbilitiesController>();
        _canInput = true;
        _canStand = true;
        _invertedControl = false;
        _canStop = true;
        _coyoteCounter = _coyoteTime;
        _isVisible = true;
        _audioSource = GetComponent<AudioSource>();
}

    private void Update()
    {
        if (!_canInput)
        {
            _rb.velocity = new Vector2(0, -50);
            //_playerAnimation.Move(0);
            //_playerAnimation.Jump(false);
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal") * (_invertedControl? -1 : 1);
        if (horizontal < 0)
        {
            Flip(false);
        }
        else if (horizontal > 0)
        {
            Flip(true);
        }

        _playerAnimation.RestoreAnimator();

        if (!Input.GetKey(KeyCode.W))
        {
            Move();
        }
        else
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }
        CheckActivateBall();
    }

    private void FixedUpdate()
    {
    }


    private void Move()
    {
        /* dash */
        _canStand = canStand();
        if (_dashRechargeCounter > 0)
        {
            _dashRechargeCounter = Mathf.Max(_dashRechargeCounter - Time.deltaTime, 0);
        }
        //else if (_abilitiesController.canDash && Input.GetButtonDown("Fire2") && _canStand)
        else if (_abilitiesController.canDash && Input.GetKeyDown(KeyCode.Mouse1) && _canStand)
                {
            _dashCounter = _dashTime;
            _dashRechargeCounter = _waitAfterDashing;
            if (_isVisible)
            {
                _playerEffectController.ShowAfterImage(_playerSprite);
            }
            SwitchToStanding();
        }

        if (_dashCounter > 0)
        {
            _dashCounter -= Time.deltaTime;
            _rb.velocity = new Vector2(_dashSpeed * transform.localScale.x, 0);
            _playerEffectController.CountDown(Time.deltaTime);
            if (_playerEffectController.GetAfterImageCounter() <= 0)
            {
                if (_isVisible)
                {
                    _playerEffectController.ShowAfterImage(_playerSprite);
                }
            }
            return;
        }
        /* dash */

        bool grounded = IsGrounded();
        Vector2 velocity = _rb.velocity;
        float horizontal = Input.GetAxisRaw("Horizontal") * (_invertedControl ? -1 : 1);
        if (_ball.activeSelf)
        {
            velocity.x = horizontal * _moveSpeed * _ballSpeedMultiplier;
        }
        else
        {
            if (horizontal != 0 && grounded)
            {
                _audioSource.enabled = true;
            }
            else
            {
                _audioSource.enabled = false;
            }
            if (_canStop)
            {
                velocity.x = horizontal * _moveSpeed * _speedScale;
            }
            else {
                if (horizontal != 0)
                {
                    _audioSource.enabled = true;
                    velocity.x = horizontal * _moveSpeed * _speedScale;
                }
                else
                {
                    _audioSource.enabled = false;
                }
            }
        }
        _playerAnimation.Move(horizontal);

        if (!grounded)
        {
            _coyoteCounter -= Time.deltaTime;
            _playerAnimation.SetCoyote(true);
        }
        else
        {
            _coyoteCounter = _coyoteTime;
            _playerAnimation.SetCoyote(false);
        }
//if (_coyoteCounter > 0)
//{
//    _canDoubleJump = false;
//        }

        // Jump
        if (( (_abilitiesController.canDoubleJump && _canDoubleJump) || (_coyoteCounter>0)) && Input.GetButtonDown("Jump"))
        {
            if (_ball.activeSelf)
            {
                velocity.y = _jumpForce * _ballSpeedMultiplier;
            }
            else
            {
                velocity.y = _jumpForce * _jumpForceScale;
            }

            _playerAnimation.Jump(!grounded); 
            //if (grounded)
            if (grounded ||  _coyoteCounter > 0)
            {
                _coyoteCounter = 0;
                //StartCoroutine("ResetJump");
                _canDoubleJump = true;
            }
            else
            {
                _playerAnimation.SetCoyote(false);
                _playerAnimation.DoubleJump();
                _canDoubleJump = false;
            }
        }

        _rb.velocity = velocity;
    }

    private void CheckActivateBall()
    {
        if (!_abilitiesController.canBecomeBall) return;
        if (!_ball.activeSelf)
        {
            if (Input.GetAxisRaw("Vertical") < -.9f)
            {
                _ballCounter = Mathf.Max(_ballCounter - Time.deltaTime, 0);
                if (_ballCounter <= 0)
                {
                    SwitchToBall();
                }
            }
            else
            {
                _ballCounter = _waitToBall;
            }

        }
        else if (Input.GetAxisRaw("Vertical") > .9f && _canStand){
            SwitchToStanding();
        }
    }

    private void Flip(bool facingRight)
    {
        //_playerSprite.flipX = !facingRight;
        //Vector3 scale = transform.localScale;
        //if (!facingRight) scale.x = System.Math.Abs(scale.x) * -1;
        //else scale.x = System.Math.Abs(scale.x);

        //transform.localScale = scale;
    }

    private bool IsGrounded()
    {
        RaycastHit2D hitLeft;
        RaycastHit2D hitMiddle;
        RaycastHit2D hitRight;
        Vector2 middle = transform.position;
        Vector2 left = transform.position;
        Vector2 right = transform.position;
        middle.y = _groundDetectStartPosition .position.y;
        left.y = _groundDetectStartPosition.position.y;
        right.y = _groundDetectStartPosition.position.y;
        left.x += _collider.size.x / 2;
        right.x -= _collider.size.x / 2f;
        hitMiddle = Physics2D.Raycast(middle, Vector2.down, _groundDetectDistance, _groundLayer);
        hitLeft = Physics2D.Raycast(left, Vector2.down, _groundDetectDistance, _groundLayer);
        hitRight = Physics2D.Raycast(right, Vector2.down, _groundDetectDistance, _groundLayer);
        Debug.DrawRay(middle, Vector2.down * _groundDetectDistance, Color.red);
        Debug.DrawRay(left, Vector2.down * _groundDetectDistance, Color.red);
        Debug.DrawRay(right, Vector2.down * _groundDetectDistance, Color.red);
        bool isGround = hitLeft.collider != null || hitMiddle.collider != null || hitRight.collider != null;
        _playerAnimation.Jump(!isGround);
        return isGround;

    }

    public bool canStand()
    {
        RaycastHit2D hitLeft;
        RaycastHit2D hitMiddle;
        RaycastHit2D hitRight;
        Vector2 middle = _standDetectStartPosition.position;
        Vector2 left = _standDetectStartPosition.position;
        Vector2 right = _standDetectStartPosition.position;
        float radius = _ballCollider.radius;
        left.x += radius;
        right.x -= radius;
        hitMiddle = Physics2D.Raycast(middle, Vector2.down, _standDetectDistance, _groundLayer);
        hitLeft = Physics2D.Raycast(left, Vector2.down, _standDetectDistance, _groundLayer);
        hitRight = Physics2D.Raycast(right, Vector2.down, _standDetectDistance, _groundLayer);
        Debug.DrawRay(middle, Vector2.down * _standDetectDistance, Color.red);
        Debug.DrawRay(left, Vector2.down * _standDetectDistance, Color.red);
        Debug.DrawRay(right, Vector2.down * _standDetectDistance, Color.red);
        bool canStand = hitLeft.collider == null && hitMiddle.collider == null && hitRight.collider == null;
        return canStand;
    }

    private void SwitchToStanding()
    {
        _standing.SetActive(true);
        _ball.SetActive(false);
        _playerAnimation.DeativateBall();
        _fireController._isStanding = true;
    }

    private void SwitchToBall()
    {
        _playerAnimation.ActivateBall();
        _standing.SetActive(false);
        _ball.SetActive(true);
        _fireController._isStanding = false;
    }

    public void DisableInput()
    {
        _fireController.enabled = false;
        _canInput = false;
    }

    public void EnableInput()
    {
        _fireController.enabled = true;
        _canInput = true;
    }

    public void FreezeSprite()
    {
        _playerAnimation.DisableAnimator();
    }

    public void UnfreezeSprite()
    {
        _playerAnimation.EnableAnimator();
    }

    public bool CanPlayerInput()
    {
        return _canInput;
    }

    public void SetSpeedScale(float scale)
    {
        _speedScale = scale;
    }
    public void SetJumpForceScale(float scale)
    {
        _jumpForceScale = scale;
    }

    public void ToggleInvertControl()
    {
        _invertedControl = !_invertedControl;
    }

    public void ToggleCanStop()
    {
        _canStop = !_canStop;
    }

    public void ToggleInvisible()
    {
        _isVisible = !_isVisible;
    }

    public bool IsVisible()
    {
        return _isVisible;
    }
}
