using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;


public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    #region private fields

    [SerializeField] private GameObject beams;
    private bool isFiring;

    #endregion

    #region public fields

    public float playerHealth = 1f;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    #endregion

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isFiring);
            stream.SendNext(playerHealth);
        }
        else
        {
            this.playerHealth = (float)stream.ReceiveNext();
            this.isFiring = (bool)stream.ReceiveNext();
        }


    }

    #endregion

    #region monobehaviour callbacks

    void Awake()
    {
        if (beams == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
        }
        else
        {
            beams.SetActive(false);
        }

        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        CameraWork _camWork=this.gameObject.GetComponent<CameraWork>();

        if (_camWork != null)
        {
            if (photonView.IsMine)
            {
                _camWork.OnStartFollowing();
            }
        } else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.\", this");
        }

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {

        if (beams!= null && isFiring!=beams.activeInHierarchy) 
        {
            beams.SetActive(isFiring);
        }

        if (photonView.IsMine) 
        { 
            ProcessInput();

            if (playerHealth <= 0f)
            {
                GameManager.Instance.LeaveRoom();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) { return; }

        if (!other.name.Contains("Beam")) {  return; }

        playerHealth -= .1f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine) { return; }

        if (!other.name.Contains("Beam")) { return; }

        playerHealth -= .1f * Time.deltaTime;
    }

    #endregion

    #region custom methods

    private void ProcessInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!isFiring)
            {
                isFiring = true;
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (isFiring)
            {
                isFiring = false;
            }
        }
    }

    #region methodsto to do with keeping players on the arena

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        this.CalledOnLevelWasLoaded(scene.buildIndex);
    }


    void OnLevelWasLoaded(int level)
    {
        this.CalledOnLevelWasLoaded(level);
    }

    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #endregion
}

#endregion

