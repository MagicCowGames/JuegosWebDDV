[System.Serializable]
public class ElementPair
{
    public Element element1;
    public Element element2;

    public ElementPair(Element element1, Element element2)
    {
        if ((int)element1 > (int)element2)
        {
            this.element1 = element1;
            this.element2 = element2;
        }
        else
        {
            this.element1 = element2;
            this.element2 = element1;
        }
    }
}
