using UnityEngine;
using System.Collections;

namespace WPG
{

    /// <summary>
    /// Status of sails during and after transitions
    /// </summary>
    public enum SailStatus
    {
        Furled,
        Set,
        Furling,
        Setting
    }

    /// <summary>
    /// Type of sail. This determines wind effects.
    /// </summary>
    public enum SailType
    {
        Staysail,
        Squaresail,
        Gaff
    }

    /// <summary>
    /// Used to determine which direction the wind is coming from. Based on yard position.
    /// </summary>
    public enum Side
    { 
        Port,
        Stbd,
        Bow,
        Stern
    }

    public class Ship : MonoBehaviour {

        private Sail[] allSails = { };

        [Header("Yard/Brace Properties")]
        /// <summary>
        /// The maximum angle the yards can turn in both directions. Applies to both Gaff and Square.
        /// </summary>
        [Tooltip("The maximum angle the yards can turn in both directions. Applies to both Gaff and Square.")]
        public float MaxAngle = 35f;

        /// <summary>
        /// The yard angle requested at this time.
        /// </summary>
        [Range(-35f,35f), Tooltip("The yard angle requested at this time.")]
        public float YardAngle = 0f;

        /// <summary>
        /// Speed that the yards turn at.
        /// </summary>
        [Tooltip("Speed that the yards turn at.")]
        public float YardTurnSpeed = .3f;

        
        [Space ,Header("Strength of sail deformation")]
        /// <summary>
        /// Strength of the 'wind' that deforms the sails.
        /// </summary>
        [Range(0.01f,1f), Tooltip("Strength of the 'wind' that deforms the sails.")]
        public float WindStrength = 1f;

        /// <summary>
        /// private variable of the wind strength that is used for transitioning to the target amount.
        /// </summary>
        private float _windStrength = 1f;

        /// <summary>
        /// private variable of the yard angle that is used for transitioning to the target angle
        /// </summary>
        private float _currentAngle = 0.01f;

        /// <summary>
        /// private variable that is used to determine the direction that the gaff/jibs should blow toward
        /// </summary>
        private Side _windSide = Side.Stbd;
         
        [Space(10), Header("Collections for objects to pivot")]
        /// <summary>
        /// The collection of yards that you want to turn.
        /// </summary>
        [SerializeField, Tooltip("The collection of yards that you want to turn.")]
        GameObject[] yards = null;

        /// <summary>
        /// Collection of gaff booms.
        /// </summary>
        [SerializeField, Tooltip("Collection of gaff booms.")]
        GameObject[] gaffs = null;

        /// <summary>
        /// Collection of all of the square sails.
        /// </summary>
        [SerializeField, Tooltip("Collection of all of the square sails.")]
        Sail[] SquareSails = null;

        /// <summary>
        /// Collection of the jibs/foresails.
        /// </summary>
        [SerializeField, Tooltip("Collection of the jibs/foresails.")]
        Sail[] Jibs = null;

        /// <summary>
        /// Collection of the Gaff/lateen/spankers.
        /// </summary>
        [SerializeField, Tooltip("Collection of the Gaff/lateen/spankers.")]
        Sail[] GaffSails = null;

        /// <summary>
        /// Sails to set for 'battle' condition. None and Full settings affect all sails.
        /// </summary>
        [SerializeField, Tooltip("Sails to set for 'battle' condition. None and Full settings affect all sails.")]
        Sail[] BattleSails = null;

        [Header("Guns")]
        /// <summary>
        /// Collection of the gun locators.
        /// </summary>
        [SerializeField, Tooltip("Collection of the port gun locators")]
        GameObject[] GunsPort = null;

        /// <summary>
        /// Collection of the gun stbd  locators.
        /// </summary>
        [SerializeField, Tooltip("Collection of the stbd gun locators")]
        GameObject[] GunsStbd = null;

        /// <summary>
        /// Collection of the gun bow locators.
        /// </summary>
        [SerializeField, Tooltip("Collection of the gun bow locators")]
        GameObject[] GunsBow = null;

        /// <summary>
        /// Collection of the gun stern locators.
        /// </summary>
        [SerializeField, Tooltip("Collection of the gun stern locators")]
        GameObject[] GunsStern = null;

        /// <summary>
        /// Particle to play when firing a gun.
        /// </summary>
        [SerializeField]
        GameObject gunParticle = null;

        private bool firing = false;


        [Header("Objects")]
        [SerializeField, Tooltip("Hull object that has the emissive material on it.")]
        GameObject hull = null;

        /// <summary>
        /// Light intensity for emissive texture.
        /// </summary>
        public float LightIntensity
        {
            set
            {
                value = Mathf.Clamp(value, .01f, 1f);
                Material m = hull.GetComponent<MeshRenderer>().sharedMaterial;
                Color c = m.GetColor("_EmissionColor");
                c = new Color(value,value,value,value);
                m.SetColor("_EmissionColor", c);
            }
        }

        /// <summary>
        /// Fire the guns on a specific side of the ship.
        /// </summary>
        /// <param name="_battery"></param>
        public void FireGuns(Side _battery)
        {
            if (!firing)
            {
                GameObject[] _guns = null;
                switch (_battery)
                {
                    case Side.Port:
                        _guns = GunsPort;
                        break;
                    case Side.Stbd:
                        _guns = GunsStbd;
                        break;
                    case Side.Stern:
                        _guns = GunsStern;
                        break;
                    case Side.Bow:
                        _guns = GunsBow;
                        break;
                }
                firing = true;
                StartCoroutine(FireGuns(_guns));
            }
        }

        IEnumerator FireGuns(GameObject[] _guns)
        {
            for (int i = 0; i < _guns.Length; i++)
            {
                yield return new WaitForSeconds(Random.Range(0.1f, .25f));
                _guns[i].GetComponentInChildren<ParticleSystem>().Play(); 
            }
            firing = false;
            yield return null;
        }


        void InstantiateGunParticles(GameObject[] _guns)
        {
            for (int i = 0; i < _guns.Length; i++)
            {
                GameObject p = Instantiate(gunParticle, _guns[i].transform.position, _guns[i].transform.rotation) as GameObject;
                p.transform.parent = _guns[i].transform;
            }
        }

        void Start()
        {
            LightIntensity = .01f;
            InstantiateGunParticles(GunsPort);
            InstantiateGunParticles(GunsStbd);
            InstantiateGunParticles(GunsBow);
            InstantiateGunParticles(GunsStern);

            //get all of the sails
            allSails = gameObject.GetComponentsInChildren<Sail>();
        }

        public void SetAllSails()
        {
            for (int i=0; i < allSails.Length; i++)
            {
                allSails[i].ChangeStatus(SailStatus.Set);
            }
        }

        public void FurlAllSails()
        {
            for (int i = 0; i < allSails.Length; i++)
            {
                allSails[i].ChangeStatus(SailStatus.Furled);
            }
        }

        public void SetBattleSails()
        {
            FurlAllSails();
            for (int i = 0; i < BattleSails.Length; i++)
            {
                BattleSails[i].ChangeStatus(SailStatus.Set);
            }
        }

        void Update () {

            if (YardAngle != _currentAngle)
            {
                //determine the side of the boat that the sails should be going toward for gaffs/jibs
                _windSide = (_currentAngle > 0f) ? Side.Stbd : Side.Port;

                //shift the current angle toward the requested angle. It eventually meets the requested angle and stops
                _currentAngle = Mathf.MoveTowards(_currentAngle, YardAngle, YardTurnSpeed * Time.deltaTime);

                //main yards turning to the rotation
                for (int i = 0; i < yards.Length; i++)
                {
                    yards[i].transform.rotation = Quaternion.Euler(0f, _currentAngle, 0f);
                }

                //gaffs go to the reverse rotation
                for (int i = 0; i < gaffs.Length; i++)
                {
                    gaffs[i].transform.rotation = Quaternion.Euler(0f, -_currentAngle, 0f);
                }

                //if the yard moves, it needs to adjust the sails so we are going to slightly change the windspeed to force the sails to change their scale.
                _windStrength += .00001f;
            }

            if (WindStrength != _windStrength)
            {
                //shifting the current wind toward the target strength amount
                _windStrength = Mathf.MoveTowards(_windStrength, WindStrength, 2f * Time.deltaTime);

                //Used to make the sails flatten out as they cross the centerline of the ship to keep them from 'snapping' to the other side
                float _strengthExponent = Mathf.Clamp(Mathf.Abs(_currentAngle) / MaxAngle,0.01f,1f);
                  
                //Square sails
                for (int i = 0; i < SquareSails.Length; i++)
                {
                    SquareSails[i].WindPower = Mathf.Clamp(_windStrength, 0.01f, 1f);
                }

                //Jibs
                for (int i = 0; i < Jibs.Length; i++)
                {
                    Jibs[i].WindPower = _strengthExponent * ((_windSide == Side.Stbd) ? _windStrength : -_windStrength);
                }

                //Gaff/lateen/spanker
                for (int i = 0; i < GaffSails.Length; i++)
                {
                    GaffSails[i].WindPower = _strengthExponent * ((_windSide == Side.Stbd) ? _windStrength : -_windStrength);
                }
            }
        }
    }
}

