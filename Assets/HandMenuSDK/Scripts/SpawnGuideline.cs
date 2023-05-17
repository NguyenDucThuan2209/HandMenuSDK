using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandMenuSDK
{
    public class SpawnGuideline : MonoBehaviour
    {
        [SerializeField] GameObject m_linePrefab;
        [SerializeField] float m_distance;

        public void Spawn(string pathName, Vector3[] pathPoints)
        {
            GameObject path = new GameObject(pathName);

            for (int i = 0; i < pathPoints.Length; i++)
            {                
                pathPoints[i] = new Vector3(pathPoints[i].x, pathPoints[pathPoints.Length - 1].y, pathPoints[i].z);                
            }

            for (int i = 0; i < pathPoints.Length - 1; i++)
            {
                // Calculate the distance between the current and next path points
                float segmentDistance = Vector3.Distance(pathPoints[i], pathPoints[i + 1]);

                // Calculate the number of spheres to spawn on the current segment
                int numberOfSpheres = Mathf.FloorToInt(segmentDistance / m_distance);

                // Calculate the step size for interpolation
                float step = 1f / (numberOfSpheres + 1);

                // Iterate through the number of spheres to spawn
                for (int j = 1; j <= numberOfSpheres; j++)
                {
                    // Calculate the position between the current and next path point
                    Vector3 position = Vector3.Lerp(pathPoints[i], pathPoints[i + 1], j * step);

                    // Instantiate a new sphere at the interpolated position
                    GameObject sphere = Instantiate(m_linePrefab, position, Quaternion.identity, path.transform);

                    // Set the sphere's name for easy identification
                    sphere.name = "Line " + ((i * numberOfSpheres) + j);
                }
            }
        }
    }
}