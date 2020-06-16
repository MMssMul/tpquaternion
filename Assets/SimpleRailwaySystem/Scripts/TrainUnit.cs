// TrainUnit
using System;
using UnityEngine;

public class TrainUnit : MonoBehaviour
{
    //Instantiation of some serialize field variable:
    [SerializeField] Transform m_Body;
    [SerializeField] Transform m_FrontBody;
    [SerializeField] Transform m_RearBody;
    [SerializeField] Transform m_FrontWheel;
    [SerializeField] Transform m_RearWheel;
    [SerializeField] Transform m_FrontConnector;
    [SerializeField] Transform m_RearConnector;
    [SerializeField] CapsuleCollider[] m_Wheels;

    //instiation of some variable
    float m_FrontConnectorToFrontWheel;

    float m_DistBetweenBogies;

    Transform m_Transform;

    float m_RearWheelToRearConnector;

    Vector3 m_PrevPosition;

    public float FrontConnectorToFrontWheel => m_FrontConnectorToFrontWheel;

    public float FrontWheelToRearWheel => m_DistBetweenBogies;

    public float RearWheelToRearConnector => m_RearWheelToRearConnector;

    public Vector3 RearConnectorPos => m_RearConnector.position;

    public Vector3 RearWheelPosition => m_RearWheel.position;

    private void Awake()
    {
        m_Transform = base.transform;
        m_PrevPosition = m_Transform.position;
        m_FrontConnectorToFrontWheel = Vector3.ProjectOnPlane(m_FrontConnector.position - m_FrontWheel.position, base.transform.up).magnitude;
        m_DistBetweenBogies = Vector3.ProjectOnPlane(m_FrontWheel.position - m_RearWheel.position, base.transform.up).magnitude;
        m_RearWheelToRearConnector = Vector3.ProjectOnPlane(m_RearWheel.position - m_RearConnector.position, base.transform.up).magnitude;
    }

    public void TrainPosition(Vector3 rearWheelPosition, Quaternion rearWheel, float speed = 0f)
    {
        m_RearWheel.position = rearWheelPosition;
        m_RearWheel.rotation = rearWheel;
        m_Body.position = m_FrontBody.position;
        m_Body.LookAt(m_RearBody);
        m_Body.rotation = Quaternion.FromToRotation(m_Body.up, Vector3.Slerp(Vector3.ProjectOnPlane(m_FrontWheel.up, m_Body.forward).normalized, Vector3.ProjectOnPlane(m_RearWheel.up, m_Body.forward).normalized, 0.5f)) * m_Body.rotation;
        m_PrevPosition = m_Transform.position;
    }
}

