using System;

[Serializable]
public class TimedEffect<T>
{
    public T type;
    public int duration;

    public TimedEffect(T type, int duration)
    {
        this.type = type;
        this.duration = duration;
    }
}
