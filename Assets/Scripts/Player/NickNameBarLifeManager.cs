using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NickNameBarLifeManager : MonoBehaviour
{
    public static NickNameBarLifeManager Instance;

    private List<NickNameBarLifeItem> _allItems = new List<NickNameBarLifeItem>();

    [SerializeField] NickNameBarLifeItem _itemPrefab;

    private void Awake()
    {
        if(Instance == null) 
        {      
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public NickNameBarLifeItem CreateNewItem(NetworkPlayer local)
    {
        var newItem = Instantiate(_itemPrefab, transform);
        _allItems.Add(newItem);

        newItem.SetOwner(local);

        local.OnPlayerDespawned += () =>
        {
            _allItems.Remove(newItem);
            Destroy(newItem.gameObject);
        };

        return newItem;
    }

    private void LateUpdate()
    {
        foreach (var item in _allItems) item.UpdatePosition();
    }
}
