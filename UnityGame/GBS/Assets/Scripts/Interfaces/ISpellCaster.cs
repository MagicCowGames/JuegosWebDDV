public interface ISpellCaster
{
    public void StartCasting();
    public void StopCasting();
    public bool GetIsCasting();
    
    public void SetElementQueue(ElementQueue queue);
    public ElementQueue GetElementQueue();
    public void ClearElementQueue();

    public void AddElements(Element[] elements);
    public Element[] GetElements(); // Most fucking worthless function tho wtf was I thinking...
    public void AddElement(Element element);
    
    public void SetForm(Form form);
    public Form GetForm();

    public void SetCastDuration(float time);
    public float GetCastDuration();

    public void HandleStartCasting() { }
    public void HandleStopCasting() { }
}
