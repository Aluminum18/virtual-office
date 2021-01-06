using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// T is reference data type of list item
/// </summary>
public abstract class ScrollList<T> : MonoBehaviour
{
    [SerializeField]
    private GameObject _listItem;
    [SerializeField]
    private Transform _contentTransform;

    protected List<T> _dataList;

    protected List<GameObject> _managedItems = new List<GameObject>();

    protected void CreateListItems()
    {

        if (_dataList == null || _dataList.Count == 0)
        {
            Debug.LogError("Data list is empty", this);
            return;
        }

        int comp = _managedItems.Count - _dataList.Count;

        if (comp < 0)
        {
            CreateItems(Mathf.Abs(comp));
        }

        if (comp > 0)
        {
            RemoveItems(comp);
        }

        SetUpAllItems();

        DoOnAllItemsCreated();
    }

    protected void SetUpAllItems()
    {
        if (_dataList == null || _dataList.Count == 0)
        {
            Debug.LogError("Data list is empty", this);
            return;
        }

        for (int i = 0; i < _dataList.Count; i++)
        {
            var listItem = _managedItems[i].GetComponent<IScrollListItem<T>>();
            if (listItem == null)
            {
                Debug.LogError("There is an item that missing IScrollListItem Component");
                continue;
            }

            listItem.SetUp(_dataList[i]);
        }
    }

    protected virtual void DoOnEnable()
    {

    }

    protected virtual void DoOnAllItemsCreated()
    {

    }

    protected virtual void DoOnDisable()
    {

    }

    private void RemoveItems(int num)
    {
        for (int i = 0; i < num; i++)
        {
            int lastIdx = _managedItems.Count - 1;
            Destroy(_managedItems[lastIdx]);
            _managedItems.RemoveAt(lastIdx);
        }
    }

    private void CreateItems(int num)
    {
        for (int i = 0; i < num; i++)
        {
            var itemObj = Instantiate(_listItem, _contentTransform);
            _managedItems.Add(itemObj);
        }
    }

    private void OnEnable()
    {
        DoOnEnable();
        CreateListItems();
    }

    private void OnDisable()
    {
        DoOnDisable();
    }

}
