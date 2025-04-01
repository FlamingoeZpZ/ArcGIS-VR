using System.Collections;
using Stats;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Weapons;

namespace Character
{
    public class PlayerWarrior : MonoBehaviour, IDamagable
    {
        private static readonly int VignetteIntensity = Shader.PropertyToID("_vignetteIntensity");
        public static PlayerWarrior instance { get; private set; }
        [SerializeField] private Weapon leftWeapon;
        [SerializeField] private Weapon rightWeapon;
        [SerializeField] private CharacterStats stats;

        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;
        [SerializeField] private Transform head;

        [SerializeField] private float threshHold;
        [SerializeField] private float shootThreshHold = 0.05f;

        [Header("Transition")]
        [SerializeField] private float godmodeY;
        [SerializeField] private float transitionTime = 2;
        [SerializeField] private AnimationCurve transitionCurve;
        private Vector3 _previousLocation;
        
        private Material _onHitEffect;

        private float _currentProgress;
        private Coroutine _coroutine;
        private Coroutine _godmodeCoroutine;
        private bool _isUpMode;
        private bool _isTransitioning;
        private bool _gameStarted;

        private float enterGameTime = 3;
        private Vector3 _previousRightHandLocation;
        private Vector3 _previousLeftHandLocation;


        private float initialTime = 5;
        
        
        
        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

        }


        // Update is called once per frame
        void Update()
        {
            Vector3 currentRightHandLocation = rightHand.localPosition;
            Vector3 currentLeftHandLocation = leftHand.localPosition;

           

            Vector3 rightMotionVector = currentRightHandLocation - _previousRightHandLocation;
            Vector3 leftMotionVector = currentLeftHandLocation - _previousLeftHandLocation;
            
  
            
            //Detect if both hands are moving upwards quickly
            if (_godmodeCoroutine == null && !_isTransitioning)
            {
                initialTime -= Time.deltaTime;
                if (!_isUpMode)
                {
                    bool rightHandMovingUp = rightMotionVector.y > threshHold;

                    if(rightHandMovingUp)
                    {
                        if (initialTime > 0)
                        {
                            _godmodeCoroutine = StartCoroutine(EnterGodMode());
                           
                        }
                        initialTime = 1;
                        
                    }
                    
                    bool leftHandMovingUp = leftMotionVector.y > threshHold;

                    if(leftHandMovingUp)
                    {
                        if (initialTime > 0)
                        {
                            _godmodeCoroutine = StartCoroutine(EnterGodMode());
                           
                        }
                        initialTime = 1;
                        
                    }
                }
                else
                {
                    bool rightHandMovingUp = rightMotionVector.y < -threshHold;
                    bool leftHandMovingUp = leftMotionVector.y < -threshHold;

                    if(rightHandMovingUp)
                    {
                        if (initialTime > 0)
                        {
                            _godmodeCoroutine = StartCoroutine(ExitGodMode());
                           
                        }
                        initialTime = 1;
                        
                    }
                    if( leftHandMovingUp)
                    {
                        if (initialTime > 0)
                        {
                            _godmodeCoroutine = StartCoroutine(ExitGodMode());
                           
                        }
                        initialTime = 1;
                    }
                }
            }

            if (!_gameStarted)
            {

                if (Vector3.Dot(Vector3.up, head.forward) >= 0.9f)
                {
                    enterGameTime -= Time.deltaTime;
                    if (enterGameTime <= 0)
                    {
                        TakeDamage(0);
                        _gameStarted = true;
                        AIManager.instance.Activate();
                        leftWeapon.gameObject.SetActive(true);
                        rightWeapon.gameObject.SetActive(true);
                    }
                }
                else
                {
                    enterGameTime = 3;
                }
                
                
                //We want the head to look up, and arms to go straight down for 5 seconds.
                
                
            }
            else
            {
                bool forwardRight = rightMotionVector.z > shootThreshHold;
                bool forwardLeft = leftMotionVector.z > shootThreshHold;
                
                //Check if the delta for the left hand is some value, if it is then shoot
                if(forwardLeft) leftWeapon.Shoot();
                
                //Do the same for right
                if(forwardRight) rightWeapon.Shoot();
            }
            
            
            
            
            _previousRightHandLocation = currentRightHandLocation;
            _previousLeftHandLocation = currentLeftHandLocation;
        }

        public void TakeDamage(float damage)
        {
            Debug.Log("Play has taken damage");
            _currentProgress = 0;
        }


        private IEnumerator EnterGodMode()
        {
                                    Debug.Log("Attempting to enter godview");
                                    _isTransitioning = true;
            _previousLocation = transform.position;

            var x =SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            
            Debug.Log("I am loading chill bro");
            Vector3 start = transform.position;
            Vector3 end = transform.position + Vector3.up * godmodeY;
            yield return new WaitUntil(() => x.isDone);

            float t = 0;
            while (t < transitionTime)
            {
                t += Time.deltaTime;
                float p = transitionCurve.Evaluate(t/transitionTime);
                transform.position = Vector3.Lerp(start, end, p);
            }
            transform.position = end;
            _godmodeCoroutine = null;
            _isUpMode = true;
            _isTransitioning = false;
        }

        private IEnumerator ExitGodMode()
        {
            _isTransitioning = true;
            Debug.Log("Attempting to exit godView");

            var x =SceneManager.UnloadSceneAsync(1);
            
            Debug.Log("I am loading chill bro");
            Vector3 start = transform.position;
            Vector3 end = _previousLocation;
            yield return new WaitUntil(() => x.isDone);

            float t = 0;
            while (t < transitionTime)
            {
                t += Time.deltaTime;
                float p = transitionCurve.Evaluate(t/transitionTime);
                transform.position = Vector3.Lerp(start, end, p);
            }
            transform.position = end;

            _godmodeCoroutine = null;
            _isUpMode = false;
            _isTransitioning = false;
        }


        public void Die()
        {
            return;
        }
    }
}
