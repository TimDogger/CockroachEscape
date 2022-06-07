using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace CockroachCore
{
    public enum MovingState
    {
        Idle,
        Moving,
        Running
    }

    public class Cockroach : MonoBehaviour
    {
        public UnityEvent onArrived;

        public UnityEvent onBurn;

        [Range(0f, 10f)] [SerializeField] private float _speed = 10f;

        [Range(1f, 3f)] [SerializeField] private float _runningSpeedMultipler = 2f;

        [Range(1f, 10f)] [SerializeField] private float _awareDistance = 1f;

        [Range(0.001f, 1f)] [SerializeField] private float _stopThershold = .2f;

        [SerializeField] private AudioSource _audio;

        [SerializeField] private List<AudioClip> _screamSounds;

        private Vector2 _targetPos = Vector3.zero;

        private Vector2 _destination;

        private MovingState _movingState = MovingState.Idle;

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<EdgeCollider2D>())
            {
                MoveTo(_destination);
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.GetComponent<EdgeCollider2D>())
            {
                MoveTo(_destination);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.TryGetComponent(out Flashlight flashlight))
            {
                RunFrom(col.transform.position, _awareDistance, 90f);
                PlayRandomScream();
                onBurn?.Invoke();
            }
        }

        private void PlayRandomScream()
        {
            if (_audio && _screamSounds.Count > 0 && !_audio.isPlaying)
            {
                int clipIndex = Random.Range(0, _screamSounds.Count - 1);
                _audio.clip = _screamSounds[clipIndex];
                _audio.Play();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out Flashlight flashlight))
            {
                RunFrom(other.transform.position, _awareDistance, 10f);
            }
        }

        private void FixedUpdate()
        {
            if (_movingState == MovingState.Idle) return;

            UpdateMovement();
        }

        private void RunFrom(Vector2 position, float distance, float radomDirOffset = 0f)
        {
            Vector2 awareDir = ((Vector2) transform.position - position).normalized;
            awareDir = (Quaternion.Euler(0, Random.Range(-radomDirOffset, radomDirOffset), 0) * awareDir)
                .normalized; // Add random rotation offset
            Vector2 awarePos = (Vector2) transform.position + awareDir * distance;
            MoveTo(awarePos, true);
        }

        private void UpdateMovement()
        {
            Vector2 direction = (_targetPos - (Vector2) transform.position).normalized;
            float speedMultipler = _movingState == MovingState.Running ? _runningSpeedMultipler : 1f;

            Vector3 newPos = (Vector2) transform.position + (direction * _speed * speedMultipler * Time.deltaTime);
            if (Vector2.Distance(transform.position, _targetPos) < _stopThershold)
            {
                if (Vector2.Distance(transform.position, _destination) < _stopThershold)
                {
                    Stop();
                    return;
                }

                MoveTo(_destination);
            }

            newPos.z = 0;
            transform.position = newPos;

            float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, rotationZ), .5f);
        }

        private void Stop()
        {
            _movingState = MovingState.Idle;
            gameObject.SetActive(false);
            onArrived?.Invoke();
        }

        public void SetDestination(Vector2 destination)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            _destination = destination;
            MoveTo(_destination);
        }

        public void MoveTo(Vector2 position, bool shouldRun = false)
        {
            if (position != _targetPos)
            {
                _targetPos = position;
            }

            _movingState = shouldRun ? MovingState.Running : MovingState.Moving;
        }
    }
}