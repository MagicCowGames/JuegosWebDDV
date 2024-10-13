using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementQueue
{
    #region Variables

    public Element[] Elements { get; private set; } // list of elements
    public int Count { get; private set; } // number of elements within the list
    public int Slots { get; private set; } // number of element slots (actual length of the array)

    #endregion

    #region Constructor

    public ElementQueue(int numSlots = 5)
    {
        this.Slots = numSlots;
        this.Elements = new Element[this.Slots];
        for (int i = 0; i < this.Slots; ++i)
            this.Elements[i] = Element.None;
        this.Count = 0;
    }

    #endregion

    #region PublicMethods

    public void Clear()
    {
        for (int i = 0; i < this.Slots; ++i)
            this.Elements[i] = Element.None;
        this.Count = 0;
    }

    public void Add(Element element)
    {

    }

    #endregion

    #region PrivateMethods

    private void Remove(int i)
    {
        // Can't remove the element if it's out of bounds.
        if (i < 0 || i >= this.Count)
            return;

        // Remove the element.
        // We only need to decrease the count by one, because we're going to shift the elements back and then set the last to None.
        // In short, we don't need to set Elements[i] to None yet.
        this.Count -= 1;

        // Shift elements to the left, leaving the None to the right most possible slot
        for (int j = i; j < this.Count - 1; ++j)
            this.Elements[j] = this.Elements[j + 1];

        // Set the last element to None
        this.Elements[this.Count] = Element.None;
    }

    #endregion
}
