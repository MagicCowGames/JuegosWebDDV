using UnityEngine;

public interface ISpell
{
    public Color GetSpellColor();
    public void SetSpellColor(Color color);

    public void SetSpellData(ElementQueue queue);

    public abstract void UpdateSpellColor();

}
