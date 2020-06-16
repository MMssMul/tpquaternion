// Train
using SimpleRailwaySystem;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    //Set the Max speed here
    [SerializeField] private float m_MaxSpeed = 100f;

    //Get the C# class Train unit to access it
    [SerializeField] private List<TrainUnit> m_TrainUnit = new List<TrainUnit>();

    //With this variable you can change the speed of the train
    private float m_Speed;

    [SerializeField] private RailManager m_Rail;

    //Instantiate the acceleration of the train 
    [SerializeField] private float m_Acceleration = 15f;

    //Get the distance on rail
    private float GetDistance => m_Rail.Length * m_SetRail + m_Distance;

    //Instantiate the rail set off
    [SerializeField] private float m_SetRail = 0.1f;

    //Instantiate the distance travel by the train
    private float m_Distance;


    //Instantiate the actual index
    private int m_ActualIndex;

    //Get the distance on the rail repeatly.
    private float DistanceRail(float distance)
    {
        return Mathf.Repeat(distance, m_Rail.Length);
    }

    private void Update()
    {
        //Instantiate the poistion, the normal, the tangent
        Vector3 position = Vector3.zero;
        Vector3 normal = Vector3.zero;
        Vector3 tangent = Vector3.zero;

        //initial segment
        int index = -1;

        //put axis to vertical
        float axis = Input.GetAxis("Vertical");

        //Calculate the speed of the train
        m_Speed = Mathf.Clamp(m_Speed + m_Acceleration * axis * Time.deltaTime, 0f - m_MaxSpeed, m_MaxSpeed);
        m_Distance += m_Speed * Time.deltaTime;

        //Instantiate x to evaluate the distance between the train and the wagoon
        float x = DistanceRail(GetDistance);
        m_Rail.GetPositionNormalTangent(x / m_Rail.Length, out position, out normal, out tangent, out index);

        //Loop to continue the instantiation of the MySpline method to run the different points on the rail
        for (int i = 0; i < m_TrainUnit.Count; i++)
        {
            TrainUnit trainUnit = m_TrainUnit[i];
            if (i == 0)
            {
                m_ActualIndex = index;
            }
            //look for 
            trainUnit.transform.position = position + normal * m_Rail.DistanceBetweenTerrainAndTopOfSleeper;
            trainUnit.transform.rotation = Quaternion.LookRotation(tangent, normal);

            //run method of rail manager to get the intersection in this case and the distance from the front to the rear bogie
            m_Rail.GetSphereRailIntersection(position, trainUnit.FrontWheelToRearWheel, index, -1, out position, out normal, out tangent, out index);
            trainUnit.TrainPosition(position + normal * m_Rail.DistanceBetweenTerrainAndTopOfSleeper, Quaternion.LookRotation(tangent, normal), m_Speed / m_MaxSpeed);
            //get the count of the train
            if (i < m_TrainUnit.Count - 1)
            {
                TrainUnit trainUnits = m_TrainUnit[i + 1];
                m_Rail.GetSphereRailIntersection(trainUnit.RearWheelPosition, trainUnit.RearWheelToRearConnector + trainUnits.FrontConnectorToFrontWheel, index, -1, out position, out normal, out tangent, out index);
            }
        }
    }
}
