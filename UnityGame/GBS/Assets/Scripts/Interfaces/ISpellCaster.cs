public interface ISpellCaster
{
    public void StartCasting();
    public void StopCasting();
    public bool GetIsCasting();
    
    public void SetElementQueue(ElementQueue queue);
    public ElementQueue GetElementQueue();

    public void SetElements(Element[] elements);
    public Element[] GetElements();
    public void AddElement(Element element);
    
    public void SetForm(Form form);
    public Form GetForm();

    public void SetCastDuration(float time);
    public float GetCastDuration();

    // These 2 could be done using events rather than virtual methods, but I've already used this idea so whatever, fuck it.
    protected virtual void HandleStartCasting() { }
    protected virtual void HandleStopCasting() { }
}
