using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : SingletonPersistent<ElementManager>
{
    #region Structs

    // This struct will be used to determine the elemental combinations
    // A + B = C
    // Opposite element cancellations are basically combination operations where C is Element.None
    [System.Serializable]
    public struct InputElementTuple
    {
        public Element elementA;
        public Element elementB;
        public Element elementC;
    }

    // This struct will be used to assign an image to every single element when used in game during element queuing.
    // NOTE : The buttons in the player UI have hardcoded images, maybe I should use a slightly different image for those?
    [System.Serializable]
    public struct InputElementImage
    {
        public Element element;
        public Sprite image;
    }

    #endregion

    #region Variables
    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods
    #endregion
}
