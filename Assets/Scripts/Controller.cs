using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

namespace CockroachCore
{
    public class Controller : MonoBehaviour
    {
        public UnityEvent onGameStarted;
        public UnityEvent onGameEnded;

        public GameObject startWaypoint;
        public GameObject stopWaypoint;
        
        public Cockroach Cockroach => _cockroach;

        [SerializeField] private Cockroach _cockroachPrefab;

        [SerializeField] private Cockroach _cockroach;

        private void Start() => StartGame();

        private void OnEnable()
        {
            if (!_cockroach)
            {
                _cockroach = Instantiate(_cockroachPrefab);
                _cockroach.transform.SetParent(transform);
                _cockroach.onArrived.AddListener(EndGame);
            }
            
            if (!(stopWaypoint && startWaypoint))
            {
                Debug.LogError($"Missing waypoints");
                return;
            }

            _cockroach.transform.position = startWaypoint.transform.position;
            _cockroach.SetDestination(stopWaypoint.transform.position);
        }

        private void OnDestroy()
        {
            Destroy(_cockroach);
        }

        public void StartGame()
        {
            enabled = true;
            onGameStarted?.Invoke();
        }

        public void EndGame()
        {
            enabled = false;
            onGameEnded?.Invoke();
        }
    }
}