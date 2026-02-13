using UnityEngine;

public sealed class CurrencyListView : MonoBehaviour
{
    [SerializeField] private Transform _content;
    
    public Transform Content => _content;
}