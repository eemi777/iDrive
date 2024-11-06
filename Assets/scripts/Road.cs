using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public GameObject roadStraightPrefab;     // Prefab for a straight road segment
    public GameObject roadLeftTurnPrefab;     // Prefab for a left turn road segment
    public GameObject roadRightTurnPrefab;    // Prefab for a right turn road segment
    public Transform carTransform;            // Reference to the car's transform
    public int initialRoadLength = 5;         // Number of initial road segments
    public float segmentLength = 10f;         // Length of each road segment
    public int maxRoadSegments = 10;          // Max number of road segments at a time
    public float speed = 10f;                 // Speed of the car
    public float turnChance = 0.2f;           // Chance of a turn after a straight segment

    private Queue<GameObject> roadSegments = new Queue<GameObject>(); // Queue to track active road segments
    private Vector3 nextSpawnPosition;        // Position where the next road segment will spawn
    private Quaternion currentRotation;       // Current rotation of the road
    private int roadCount = 0;
    private int straightSegmentCounter = 3;

    void Start()
    {
        nextSpawnPosition = Vector3.zero; // Start spawning road at position (0, 0, 0)
        currentRotation = Quaternion.identity; // Start with no rotation

        // Spawn initial road segments
        for (int i = 0; i < initialRoadLength; i++)
        {
            SpawnRoadSegment();
        }
    }

    void Update()
    {
        // Move the car forward
        //carTransform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Check if we need to spawn new road segments
        if (Vector3.Distance(carTransform.position, roadSegments.Peek().transform.position) > segmentLength * 2)
        {
            SpawnRoadSegment();
            DespawnOldSegment();
        }
    }

    void SpawnRoadSegment()
    {
        GameObject newSegment;

        // Only spawn straight road segments for the first 5 segments
        if (roadCount < 5)
        {
            //Always spawn a straight road segment
            newSegment = Instantiate(roadStraightPrefab, nextSpawnPosition, currentRotation);
            straightSegmentCounter++;
        }
        else
        {
            // After the first 5 segments, allow turns based on random chance
            float randomValue = Random.Range(0f, 1f);
            if (randomValue < turnChance && straightSegmentCounter >= 3)
            {
                // Randomly choose left or right turn
                if (Random.Range(0, 2) == 0)
                {
                    newSegment = Instantiate(roadLeftTurnPrefab, nextSpawnPosition, currentRotation);
                    currentRotation *= Quaternion.Euler(0, -90, 0); // Rotate left by 90 degrees
                }
                else
                {
                    newSegment = Instantiate(roadRightTurnPrefab, nextSpawnPosition, currentRotation);
                    currentRotation *= Quaternion.Euler(0, 90, 0);  // Rotate right by 90 degrees
                }
                straightSegmentCounter = 0;
            }
            else
            {
                // Spawn a straight road segment
                newSegment = Instantiate(roadStraightPrefab, nextSpawnPosition, currentRotation);
                straightSegmentCounter++;
            }
        }

        // Add the new segment to the queue to keep track of spawned road segments
        roadSegments.Enqueue(newSegment);

        // Update the next spawn position: move forward based on current rotation and road length
        nextSpawnPosition = newSegment.transform.position + currentRotation * Vector3.forward * segmentLength;

        // Increment the road count
        roadCount++;

        // Debug the spawn position and rotation for troubleshooting
        Debug.Log("Next Spawn Position: " + nextSpawnPosition);
        Debug.Log("Current Rotation: " + currentRotation.eulerAngles);
    }

    void DespawnOldSegment()
    {
        // Destroy the oldest road segment that is behind the car
        if (roadSegments.Count > maxRoadSegments)
        {
            GameObject oldSegment = roadSegments.Dequeue();
            Destroy(oldSegment);
        }
    }
}