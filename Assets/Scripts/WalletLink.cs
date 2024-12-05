using UnityEngine;
using UnityEngine.EventSystems;

public class WalletLink : MonoBehaviour
{
    public GameObject connectPanel;
    private string playerId = "player123"; 

    void Start()
    {
        if (connectPanel != null)
        {
            AddEventTriggerListener(connectPanel, OnConnectPanelClick);
        }
        else
        {
            Debug.LogError("Connect Panel is not assigned in the inspector!");
        }
    }

    void OnConnectPanelClick()
    {
        
        string url = $"http://localhost:3000/playerId={playerId}";

       
        Application.OpenURL(url);
    }

    private void AddEventTriggerListener(GameObject target, UnityEngine.Events.UnityAction action)
    {
        EventTrigger trigger = target.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = target.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        entry.callback.AddListener((eventData) => action());
        trigger.triggers.Add(entry);
    }
}
