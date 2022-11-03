using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTower : MonoBehaviour
{
    [SerializeField] private Human _startHuman;
    [SerializeField] private Transform _distanceChecker;
    [SerializeField] private float _fixationMaxDistance;
    [SerializeField] private BoxCollider _checkCollider;

    private List<Human> _humans;

    public event UnityAction<int> HumanAdded;

    private void Start()
    {
        _humans = new List<Human>();
        Vector3 spawnPoint = transform.position;
        _humans.Add(Instantiate(_startHuman, spawnPoint, Quaternion.identity, transform));
        _humans[0].Run();
        HumanAdded?.Invoke(_humans.Count);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.TryGetComponent(out Human human))
        {
            Tower collisionTower = human.GetComponentInParent<Tower>();

            if (collisionTower != null)
            {
                List<Human> collectedHumans = collisionTower.CollectHuman(_distanceChecker, _fixationMaxDistance);

                if (collectedHumans != null)
                {
                    _humans[0].StopRun();
                    _humans[0].Waving();

                    for (int i = collectedHumans.Count - 1; i >= 0; i--)
                    {
                        Human insertHuman = collectedHumans[i];
                        collectedHumans[i].Waving();
                        InsertHuman(insertHuman);
                        DisplaceCheckers(insertHuman, true);
                    }

                    HumanAdded?.Invoke(_humans.Count);
                    _humans[0].StopWaving();
                    _humans[0].Run();
                }

                collisionTower.Break();
            }
        }
    }

    private void InsertHuman(Human collectedHuman)
    {
        _humans.Insert(0, collectedHuman);
        SetHumanPosition(collectedHuman);
    }

    private void SetHumanPosition(Human human)
    {
        human.transform.parent = transform;
        human.transform.localPosition = new Vector3(0, human.transform.localPosition.y, 0);
        human.transform.localRotation = Quaternion.identity;
    }

    private void DisplaceCheckers(Human human, bool isCollecting)
    {
        float displaceScale;

        if (isCollecting)
            displaceScale = 1.45f;
        else
            displaceScale = -1.45f;

        Vector3 distanceCheckerNewPosition = _distanceChecker.position;
        distanceCheckerNewPosition.y -= human.transform.localScale.y * displaceScale;
        _distanceChecker.position = distanceCheckerNewPosition;
        _checkCollider.center = _distanceChecker.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Obstacle obstacle))
        {
            if (_humans.Count > obstacle.DamageToTower)
            {
                for (int i = 0; i < obstacle.DamageToTower; i++)
                {
                    Destroy(_humans[0].gameObject);
                    _humans.RemoveAt(0);
                    DisplaceCheckers(_humans[0], false);
                }

                _humans[0].StopWaving();
                _humans[0].Run();
            }
            else
            {
                Debug.Log("You Lose!");
            }
        }
    }
}
