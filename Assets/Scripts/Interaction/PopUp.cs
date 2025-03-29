using System.Collections;
using UnityEngine;

namespace Interaction
{
    public class PopUp : MonoBehaviour
    {

        private Vector3 _initialPosition;
        [SerializeField] private bool startIsGround = true;
        [SerializeField] private Vector3 endPosition;
        [SerializeField] private float transitionDuration;
        [SerializeField] private AnimationCurve animationCurve;

        private float _currentProgress;
        private Coroutine _currentRoutine;

        private void Awake()
        {
            if (startIsGround && Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10))
            {
                _initialPosition = hit.point;
                endPosition = new Vector3(0, hit.distance, 0);
                transform.localPosition = hit.point;
            }
            else _initialPosition = transform.localPosition;
            
            
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
                transform.localPosition = Vector3.LerpUnclamped(_initialPosition, endPosition, animationCurve.Evaluate(_currentProgress/transitionDuration));
                yield return null;
            }
            _currentProgress = transitionDuration;
            transform.localPosition = endPosition;
        }
        
        private IEnumerator TransitionOut()
        {
            while (_currentProgress >= 0)
            {
                _currentProgress -= Time.deltaTime;
                transform.localPosition = Vector3.LerpUnclamped(_initialPosition, endPosition, animationCurve.Evaluate(_currentProgress/transitionDuration));
                yield return null;
            }
            _currentProgress = 0;
            transform.localPosition = _initialPosition;
        }

    }
}
