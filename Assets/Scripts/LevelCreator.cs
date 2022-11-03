using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private Tower _towerTemplate;
    [SerializeField] private int _humanTowerCount;
    [SerializeField] private JumpBuffer _jumpBuffer;
    [SerializeField] private float _distanceBetweenTowerAndBuffer;
    [SerializeField] private Obstacle _obstacle;
    [SerializeField] private float _distanceBetweenTowerAndObstacle;

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        float roadLenght = _pathCreator.path.length;
        float distanceBetweenTower = roadLenght / _humanTowerCount;

        float distanceTravelled = 0;
        Vector3 towerSpawnPoint;
        Vector3 bufferSpawnPoint;
        Vector3 obstacleSpawnPoint;

        for (int i = 0; i < _humanTowerCount; i++)
        {
            distanceTravelled += distanceBetweenTower;
            towerSpawnPoint = _pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
            bufferSpawnPoint = _pathCreator.path.GetPointAtDistance(distanceTravelled - _distanceBetweenTowerAndBuffer, EndOfPathInstruction.Stop);
            obstacleSpawnPoint = _pathCreator.path.GetPointAtDistance(distanceTravelled - _distanceBetweenTowerAndObstacle, EndOfPathInstruction.Stop);

            Instantiate(_towerTemplate, towerSpawnPoint, Quaternion.identity);

            if (Random.Range(0, 2) == 0)
            {
                Instantiate(_jumpBuffer, bufferSpawnPoint, Quaternion.identity);
            }

            if (Random.Range(0, 2) == 0)
            {
                Instantiate(_obstacle, obstacleSpawnPoint, Quaternion.identity);
            }
        }
    }
}
