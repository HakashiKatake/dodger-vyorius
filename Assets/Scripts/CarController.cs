using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;

    [SerializeField] private float motorForce, breakForce, maxSteerAngle;
    [SerializeField] private float boostMultiplier = 15f;

    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    public bool isPlayerOne;

    public Light[] BackLights;

    private bool isBoosting = false;

    public int Lifes = 5;
    private int currentLifes;
    public float TimeToRecover = 5;
    private float Timer_Recover;
    private bool Recovring = false;

    public TMP_Text LifesTxt;

    private bool Crashed = false;

    // Flip cooldown variables
    public float flipCooldown = 2f; // Time between flip actions
    private float lastFlipTime = -10f; // Track the last time the car was flipped

    private void Start()
    {
        currentLifes = Lifes;
        LifesTxt.text = $"Lifes : {currentLifes} / {Lifes}";
    }

    private void FixedUpdate()
    {
        if (!Crashed)
        {
            GetInput();
            HandleMotor();
            HandleSteering();
        }        
        UpdateWheels();
        UpdateLights();
        HandleRecover();
    }

    private void GetInput()
    {
        if (isPlayerOne)
        {
            // player 1 inputs WASD      Space : Break       R : Flip Car
            horizontalInput = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
            verticalInput = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
            isBreaking = Input.GetKey(KeyCode.Space);
            isBoosting = Input.GetKey(KeyCode.LeftShift);
            if (Input.GetKeyDown(KeyCode.R) && Time.time - lastFlipTime > flipCooldown)
            {
                FlipCar();
            }
        }
        else
        {
            // player 2 inputs Arrows     RightControl : Break      RightShift : Flip Car
            horizontalInput = Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
            verticalInput = Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
            isBreaking = Input.GetKey(KeyCode.RightControl);
            isBoosting = Input.GetKey(KeyCode.RightShift);
            if (Input.GetKeyDown(KeyCode.RightShift) && Time.time - lastFlipTime > flipCooldown)
            {
                FlipCar();
            }
        }
    }

    private void HandleMotor()
    {
        float currentMotorForce = isBoosting ? motorForce * boostMultiplier : motorForce;

        if (verticalInput > 0)
        {
            currentMotorForce += 200f;
        }
        frontLeftWheelCollider.motorTorque = verticalInput * currentMotorForce;
        frontRightWheelCollider.motorTorque = verticalInput * currentMotorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void UpdateLights()
    {
        bool shouldActivateBrakeLights = isBreaking || verticalInput < 0;

        foreach (Light light in BackLights)
        {
            light.enabled = shouldActivateBrakeLights;
        }
    }

    private void HandleRecover()
    {
        if (Recovring)
        {
            Timer_Recover += Time.deltaTime;
            if (Timer_Recover >= TimeToRecover)
            {
                Timer_Recover = 0;
                Recovring = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("obs"))
        {
            if (!Recovring)
            {
                //Crash
                currentLifes--;
                LifesTxt.text = $"Lifes : {currentLifes} / {Lifes}";
                Recovring = true;
                if (currentLifes == 0)
                {                    
                    Crashed = true; // player can't control the car no more
                    //loose
                }
            }
        }
    }

    private void FlipCar()
    {
        // Flip the car in case it got stuck or flipped over
        lastFlipTime = Time.time;
        Vector3 currentPosition = transform.localPosition;
        transform.localPosition = new Vector3(currentPosition.x, currentPosition.y + 2, currentPosition.z);
        transform.localRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);       
    }
}
