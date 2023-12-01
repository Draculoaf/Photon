using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    #region private fields
    [SerializeField] private Text playerNameTXT;
    [SerializeField] private Slider playerHealthSlider;
    private PlayerManager target;
    float characterControllerHeight = 0f;
    Transform targetTransform;
    Renderer targetRenderer;
    CanvasGroup _canvaGroup;
    Vector3 targetPosition;
    #endregion

    #region public fields
    [SerializeField] private Vector3 screenOffset = new Vector3(0f, 3f, 0f);
    #endregion

    #region MonoBehaviour Callbacks

    private void Update()
    {
        if (playerHealthSlider != null)
        {
            playerHealthSlider.value = target.playerHealth;
        }

        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void Awake()
    {
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
    
        _canvaGroup =this.GetComponent<CanvasGroup>();

    }

    private void LateUpdate()
    {
        if (targetRenderer != null)
        {
            this._canvaGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
        }

        if (targetTransform != null)
        {
            targetPosition=targetTransform.position;
            targetPosition.y += characterControllerHeight;
            this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
        }
    }
    #endregion

    #region Public Methods

    public void SetTarget(PlayerManager _target)
    {
        if (_target == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
            return;
        }

        target = _target; //what is the point of this?? must ask
        if (playerNameTXT != null)
        {
            playerNameTXT.text = target.photonView.Owner.NickName;
        }

        targetTransform = this.target.GetComponent<Transform>();
        targetRenderer = this.target.GetComponent<Renderer>();
        CharacterController characterController = _target.GetComponent<CharacterController>();
    
        if (characterController != null )
        {
            characterControllerHeight = characterController.height;
        }
    
    }
    #endregion
}
