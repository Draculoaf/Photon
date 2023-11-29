using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


//namespace Com.MyCompany.MyGame not sure why this is giving an error
[RequireComponent(typeof(InputField))]

public class PlayerNameInputField : MonoBehaviour
{
    #region private constants
    const string playerNamePrefKey = "PlayerName";
    #endregion

    #region monobehaviour callbacks
    private void Start()
    {
        string defaultName = string.Empty;
        InputField nameInput = this.GetComponent<InputField>();

        if (nameInput != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName=PlayerPrefs.GetString(playerNamePrefKey);
                nameInput.text=defaultName;
            }
        }

        PhotonNetwork.NickName = defaultName;
    }

    #endregion

    #region public methods

    public void SetPlayerName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player name is null or empty");
            return;
        }

        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString(playerNamePrefKey, value);
    }

    #endregion

}
