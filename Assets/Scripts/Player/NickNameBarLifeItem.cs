using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NickNameBarLifeItem : MonoBehaviour
{
    private const float Y_OFFSET = 1.75f;

    private Transform _owner;

    private TextMeshProUGUI _nameText;

    [SerializeField] Image _lifeBarImage;


    // Start is called before the first frame update
    public void SetOwner(NetworkPlayer owner)
    {
        _owner = owner.gameObject.transform;
        //owner.SendMyLiFeHandler().OnLifeUpdate += UpdateLifeBar;
        _nameText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateNickName(string newNickName)
    {
        _nameText.text = newNickName;
    }

    public void UpdateLifeBar(float amount)
    {
        _lifeBarImage.fillAmount = amount;
    }

    public void UpdatePosition()
    {
        transform.position = _owner.position + Vector3.up * Y_OFFSET;
    }
}
