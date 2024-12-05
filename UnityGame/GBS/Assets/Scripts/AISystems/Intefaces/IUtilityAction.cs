// NOTE : This interface refers to actions for the AI Utility system.
// TODO : Should probably implement some kind of UtilitySystem controller class of sorts that handles updating the US automatically.
public interface IUtilityAction
{
    public float Calculate(float delta);
    public void Execute(float delta);
    public void Reset();
}
