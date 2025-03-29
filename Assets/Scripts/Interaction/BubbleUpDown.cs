using System.Collections;
using UnityEngine;

namespace Interaction
{
    public class BubbleUpDown : MonoBehaviour
    {

        [SerializeField] private Vector3 initialScale;
        [SerializeField] private Vector3 endScale;
        [SerializeField] private float transitionDuration;
        [SerializeField] private AnimationCurve animationCurve;

        private float _currentProgress;
        private Coroutine _currentRoutine;

        private void Awake()
        {
            transform.localScale = initialScale;
        }

        private void OnEnable()
        {
            if (_currentRoutine != null)
            {
                StopCoroutine(_currentRoutine);   
            }
            _currentRoutine = StartCoroutine(TransitionIn());
        }

        private void OnDisable()
        {
            if (_currentRoutine != null)
            {
                StopCoroutine(_currentRoutine);   
            }
            _currentRoutine = StartCoroutine(TransitionOut());
        }

        private IEnumerator TransitionIn()
        {
            while (_currentProgress <= transitionDuration)
            {
                _currentProgress += Time.deltaTime;
                transform.localScale = Vector3.LerpUnclamped(initialScale, endScale, animationCurve.Evaluate(_currentProgress/transitionDuration));
                yield return null;
            }
            _currentProgress = transitionDuration;
            transform.localScale = endScale;
        }
        
        private IEnumerator TransitionOut()
        {
            while (_currentProgress >= 0)
            {
                _currentProgress -= Time.deltaTime;
                transform.localScale = Vector3.LerpUnclamped(initialScale, endScale, animationCurve.Evaluate(_currentProgress/transitionDuration));
                yield return null;
            }
            _currentProgress = 0;
            transform.localScale = initialScale;
        }

    }
}
