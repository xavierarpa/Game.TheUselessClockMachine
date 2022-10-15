using System;

public sealed class ActionSuscriber<TValue>
{
    private TValue lastValue; // Si ha pasado un valor
    private Action<TValue> action = default;


    public ActionSuscriber(
        TValue initValue = default
    ){
        lastValue = initValue;
    }
    public void Subscribe(bool condition, Action<TValue> callback)
    {
        if (condition) callback.Invoke(lastValue);
        SubscribeWithoutNotify(condition, callback);
    }
    public void SubscribeWithoutNotify(bool condition, Action<TValue> callback){
        if (condition) action += callback;
        else action -= callback;
    }
    public void Invoke(TValue val)
    {
        lastValue = val;
        Invoke();
    }
    public void Invoke() => action?.Invoke(lastValue);
}
