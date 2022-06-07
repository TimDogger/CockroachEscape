using System;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.Events;

namespace CockroachCore.UI
{
    public class UI_Restart : MonoBehaviour
    {
        public UnityEvent onEnabled;
        
        [SerializeField] private Controller _controller;

        private void OnEnable()
        {
            onEnabled?.Invoke();
        }

        public void Restart()
        {
            if (!_controller)
            {
                Debug.LogError("Null controller!");
                return;
            }

            _controller.StartGame();
            gameObject.SetActive(false);
        }

        public void Exit()
        {
#if UNITY_STANDALONE
            Application.Quit();
#endif
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}