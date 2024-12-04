using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    public float baseSpeed = 0.06f; // Normal speed
    public float sprintMultiplier = 2f; // Speed multiplier when Shift is pressed
    public Image StaminaBar;
    public float Stamina, MaxStamina;
    public float RunCost;
    public float ChargeRate;

    public GameObject bullet;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;

    private Coroutine recharge;

    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Shooting();

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Check if the Shift key is being pressed to increase speed
        float currentSpeed = baseSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && Stamina > 0)
        {
            currentSpeed *= sprintMultiplier;

            Stamina -= RunCost * Time.deltaTime;
            if (Stamina < 0) Stamina = 0;
            StaminaBar.fillAmount = Stamina / MaxStamina;

            if(Stamina == 0 && recharge == null)
            {
                recharge = StartCoroutine(RechargeStamina()); // Start stamina recharge when it's empty
            }
        }
        // If no Shift or stamina is empty, set speed to baseSpeed
        if (Stamina == 0 && !Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = baseSpeed;
        }


        //sets values to m_Movement, setting y to a zero float
        //(x,y,z)
        m_Movement.Set(horizontal, 0f, vertical);

        //ensures the movement vector always has the same magnitude. 
        m_Movement.Normalize();

        //True and Flase checking for player input for movement
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);

        //Combines the inputs, "OR" function if H or V is true, IsWalking is True
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        //first parameter is the name of the Animator Parameter that you want to set the value of, and the second is the value you want to set it to
        m_Animator.SetBool("IsWalking", isWalking);

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }
        

        //Allows the character to face where it rotates and controls speed
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);

        //calls the LookRotation method and creates a rotation looking in the direction of the given parameter
        m_Rotation = Quaternion.LookRotation(desiredForward);

        // Move the player with the adjusted speed
        transform.Translate(m_Movement * currentSpeed * Time.deltaTime, Space.World);
    }

    void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Instantiate the bullet at the player's position (slightly offset in the Y direction)
        GameObject instantiatedBullet = Instantiate(bullet, transform.position + new Vector3(0, 1, 0), Quaternion.identity);

        // Get the Rigidbody component from the bullet
        Rigidbody bulletRb = instantiatedBullet.GetComponent<Rigidbody>();

        if (bulletRb != null)
        {
            // Apply a force to the bullet in the forward direction of the player
            bulletRb.AddForce(transform.forward * 10f, ForceMode.Impulse);  // You can adjust the 10f to control the speed
        }
        }
    } 
    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);

        while(Stamina < MaxStamina)
        {
            Stamina += ChargeRate * Time.deltaTime; // Gradually recharge stamina
            if (Stamina > MaxStamina) Stamina = MaxStamina; // Ensure stamina doesn't exceed max
            StaminaBar.fillAmount = Stamina / MaxStamina; // Update the stamina bar
            yield return null; // Wait one frame
        }

        // Stop the recharge coroutine once stamina is full
        recharge = null;
    }
    void OnAnimatorMove()
    {
        //starts at new position, add movement and multiply the magnitude(direction we want to move)
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);

        m_Rigidbody.MoveRotation(m_Rotation);

    }
}
