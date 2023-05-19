using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandMenuSDK
{
    public class SpawnGuideline : MonoBehaviour
    {
        [SerializeField] GameObject m_linePrefab;
        [SerializeField] float m_distance;

        private GameObject m_currentPath;

        public void Spawn(string pathName, Vector3[] pathPoints)
        {
            GameObject path = new GameObject(pathName);

            for (int i = 0; i < pathPoints.Length; i++)
            {
                pathPoints[i].y = 0;
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

                    // Calculate the direction from the current and next path point
                    Vector3 direction = Vector3.Normalize(pathPoints[i + 1] - pathPoints[i]);

                    // Instantiate a new sphere at the interpolated position
                    GameObject line = Instantiate(m_linePrefab, position, Quaternion.identity, path.transform);

                    // Set the sphere's name for easy identification
                    line.name = "Line " + ((i * numberOfSpheres) + j);

                    // Rotate the direction of the line 
                    line.transform.forward = direction;                    
                }
            }

            // Replace old path
            if (m_currentPath != null)
                Destroy(m_currentPath);
            m_currentPath = path;
        }
    }
}