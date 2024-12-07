// NOTE : This interface refers to actions for the AI Utility system.
// TODO : Should probably implement some kind of UtilitySystem controller class of sorts that handles updating the US automatically.
// TODO : Maybe make ISpecificUtilityAction interfaces for each type of utility and make each behaviour implement the interfaces so that we can move all of the
// utility systems logics to be NPC Controller independent when it comes to variables such as isFleeing and stuff like that? Or just create a GenericBehaviourController
// class which contains all of the variables we require to make the utility actions, and then inherit from that one if you want US support. Or use property setters.
public interface IUtilityAction
{
    public string Name { get; set; }
    public float Calculate(float delta);
    public void Execute(float delta);
    public void Update(float delta);
}
