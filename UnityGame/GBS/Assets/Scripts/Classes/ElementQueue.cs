using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementQueue
{
    #region Variables

    // Primary variables
    public Element[] Elements { get; private set; } // list of elements
    public int Count { get; private set; } // number of elements within the list
    public int Slots { get; private set; } // number of element slots (actual length of the array)

    // Secondary variables
    public int[] ElementsCounts { get; private set; } // holds a count of the number of elements of a given type that is stored within the queue. Can be used to check what spell type to generate out of the current element queue.

    #endregion

    #region Constructor

    public ElementQueue(int numSlots = 5)
    {
        this.Slots = numSlots;
        this.Elements = new Element[this.Slots];
        for (int i = 0; i < this.Slots; ++i)
            this.Elements[i] = Element.None;
        this.Count = 0;

        this.ElementsCounts = new int[(int)Element.COUNT];
        for (int i = 0; i < this.ElementsCounts.Length; ++i)
            this.ElementsCounts[i] = 0;
    }

    #endregion

    #region PublicMethods

    public void Clear()
    {
        for (int i = 0; i < this.Slots; ++i)
            this.Elements[i] = Element.None;
        this.Count = 0;

        for (int i = 0; i < this.ElementsCounts.Length; ++i)
            this.ElementsCounts[i] = 0;
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

        // Remove the element (only need to reduce counts)
        // We only need to decrease the count by one, because we're going to shift the elements back and then set the last to None.
        // In short, we don't need to set Elements[i] to None yet.
        this.Count -= 1; // Reduce the total element count.
        this.ElementsCounts[(int)this.Elements[i]] -= 1; // Reduce the count for this specific element.

        // Shift elements to the left, leaving the None to the right most possible slot
        for (int j = i; j < this.Count - 1; ++j)
            this.Elements[j] = this.Elements[j + 1];

        // Set the last element to None
        this.Elements[this.Count] = Element.None; // Equivalent of pushing the Elements[i] being None to the front of the array, only that we skip the stupid temp var bullshit that would be needed if we did it that way...
    }

    #endregion
}
