using System;

public class ObservableValue<T>
{
    private T value;
    public T Value
    {
        get
        {
            return this.value;
        }
        set
        {
            var oldValue = this.value;
            var newValue = value;
            
            this.value = newValue;
            
            this.OnValueChanged?.Invoke(oldValue, newValue);
        }
    }
    public Action<T, T> OnValueChanged;
    public ObservableValue(T val)
    {
        this.value = val;
    }
}
