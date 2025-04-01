using System.Collections;
using UnityEngine;

namespace Interaction
{
    public class PopUp : MonoBehaviour
    {
        private Vector3 _initialPosition;
        [SerializeField] private bool startIsGround = true;
        [SerializeField] private Vector3 endPosition;
        [SerializeField] private float transitionDuration = 1f;
        [SerializeField] private AnimationCurve animationCurve;

        private Coroutine _currentRoutine;

        private void Awake()
        {
            if (startIsGround && Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10))
            {
                _initialPosition = hit.point;
                endPosition = _initialPosition + new Vector3(0, hit.distance, 0);
                transform.localPosition = _initialPosition;
            }
            else _initialPosition = transform.localPosition;
        }

        private void OnEnable()
        {
            StartTransition(TransitionIn());
        }

        public void Hide()
        {
            StartTransition(TransitionOut());
        }

        private void StartTransition(IEnumerator transition)
        {
            if (_currentRoutine != null)
            {
                StopCoroutine(_currentRoutine);
            }
            _currentRoutine = StartCoroutine(transition);
        }

        private IEnumerator TransitionIn()
        {
            float progress = 0f;
            while (progress < transitionDuration)
            {
                progress += Time.deltaTime;
                float t = Mathf.Clamp01(progress / transitionDuration);
                transform.localPosition = Vector3.LerpUnclamped(_initialPosition, endPosition, animationCurve.Evaluate(t));
                yield return null;
            }
            transform.localPosition = endPosition;
        }

        private IEnumerator TransitionOut()
        {
            float progress = transitionDuration;
            while (progress > 0f)
            {
                progress -= Time.deltaTime;
                float t = Mathf.Clamp01(progress / transitionDuration);
                transform.localPosition = Vector3.LerpUnclamped(endPosition, _initialPosition, animationCurve.Evaluate(t));
                yield return null;
            }
            transform.localPosition = _initialPosition;
            gameObject.SetActive(false); // Properly disables after transition
        }
    }
}
