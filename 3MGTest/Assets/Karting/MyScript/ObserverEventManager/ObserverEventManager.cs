using UnityEngine.Events;
public static class ObserverEventManager
{
    /*
     Observer
Also known as: Event-Subscriber, Listener 
     */
      
    public static event UnityAction OnCoinUpdate;
    public static void RaiseCoinUpdate() => OnCoinUpdate?.Invoke();

    //----
    public static event UnityAction OnEventUpdate;
    public static void RaiseOnEventUpdate() => OnEventUpdate?.Invoke();

}