using UnityEngine;
using System.Collections;
namespace WPG
{
    public class Sail : MonoBehaviour {

        [Header("Sail")]
        [SerializeField]
        SailType Type = SailType.Squaresail;

        [Header("Meshes")]
        [Tooltip("Mesh used to change from full to furled. Is swapped for furled mesh when at furled scale")]
        public GameObject SailFull = null;
        public GameObject SailFurled = null;

        [Header("Status of sail")]
        public SailStatus Status = SailStatus.Set;

        [Header("Scales and Speed")]
        public Vector3 FullScale = Vector3.one;
        public Vector3 FurledScale = new Vector3(1f, 1f, 0f);
        public float TransitionTime = 10f;
        private float transitionTime = 10f;

        public float WindPower = 1f;
        private float windPower = 1f;

        //public Vector3 target = Vector3.one;

        [Range(0f, 1f)]
        public float currentStatus = 1f;

        void Start()
        {
            SailFull.SetActive(true);
            SailFurled.SetActive(false);
            ChangeStatus(SailStatus.Set);
        }

        public void ChangeStatus(SailStatus _status)
        {
            StopAllCoroutines();
            StartCoroutine(changeStatus(_status));
        }
        
        void Update()
        {
            if (windPower != WindPower && Status != SailStatus.Furled)
            {
                updateWindScale();
                windPower = WindPower;
            }
            
            if (currentStatus > 0)
            {
                SailFull.SetActive(true);
                SailFurled.SetActive(false);
                SailFull.transform.localScale = Vector3.Lerp(FurledScale, FullScale, currentStatus);
                updateWindScale(currentStatus);
            } else
            {
                SailFull.SetActive(false);
                SailFurled.SetActive(true);
            }

        }

        void updateWindScale(float scale = 1f)
        {
            if (Type == SailType.Squaresail)
            {
                SailFull.transform.localScale = new Vector3(SailFull.transform.localScale.x, SailFull.transform.localScale.y, scale * WindPower);
            }
            else if (Type == SailType.Gaff)
            {
                SailFull.transform.localScale = new Vector3(scale * WindPower, SailFull.transform.localScale.y, SailFull.transform.localScale.z);
            }
            else if (Type == SailType.Staysail)
            {
                SailFull.transform.localScale = new Vector3(scale * WindPower, SailFull.transform.localScale.y, SailFull.transform.localScale.z);
            }
        }

        IEnumerator changeStatus(SailStatus _status)
        {
            if (_status != Status)
            {
                transitionTime = TransitionTime * currentStatus;
                if (_status == SailStatus.Set) //we want to count up
                {
                    Status = SailStatus.Setting;
                    while (transitionTime < TransitionTime)
                    {
                        currentStatus = transitionTime / TransitionTime;
                        transitionTime += Time.deltaTime;
                        yield return null;
                    }
                    currentStatus = 1;
                }

                if (_status == SailStatus.Furled) //we want to count down
                {
                    Status = SailStatus.Setting;
                    while (transitionTime > 0)
                    {
                        currentStatus = transitionTime / TransitionTime;
                        transitionTime -= Time.deltaTime;
                        yield return null;
                    }
                    currentStatus = 0;
                }

                Status = _status;
            }
        }
    }
}

