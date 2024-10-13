using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    // Small wrapper class because Unity can't properly serialize a List<T[]> or a T[][] to be displayed on the inspector.
    // Basically, the tinitest hack in the entire code, and it works pretty nicely.
    [System.Serializable]
    public struct InputEelementCombinationLayer
    {
        public InputElementTuple[] inputElements;
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

    [Header("Element Configuration")]
    // Basically, rather than hard coding a list of combinations for opposites and another for combinations and then going through them in order, we instead
    // have a list of layers, which could potentially allow us to have multiple combination types with different precedences, which makes it far more flexible
    // for future extension, even tho we probably won't be using this shit to that extent lol.
    // In this case:
    // - Layer[0] corresponds to the opposites, because they have to be evaluated first.
    // - Layer[1] corresponds to the elemental combinations, because they have to be evaluated afterwards to get the correct result.
    [SerializeField] private InputEelementCombinationLayer[] elementCombinationLayers;
    [SerializeField] private Sprite defaultElementImage;
    [SerializeField] private InputElementImage[] elementImages;


    // This actually contains the real list of dicts that will allow us to access the elemental combinations easily.
    // The other variables are there for easy inputting of data from Unity's inspector.
    private Dictionary<ElementPair, Element>[] combinations;
    private Sprite[] images;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        GenerateCombinationData();
        GenerateImageData();
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void GenerateCombinationData()
    {
        int len = this.elementCombinationLayers.Length;
        combinations = new Dictionary<ElementPair, Element>[len];
        
        // Process each layer (eg: opposite or combinations layer)
        for(int i = 0; i < len; ++i)
        {
            var layer = this.elementCombinationLayers[i];
            // Process each elemental tuple within the current layer (eg: Q + A = None)
            foreach (var elementTuple in layer.inputElements)
            {
                var inputPair = new ElementPair(elementTuple.elementA, elementTuple.elementB);
                var outputElement = elementTuple.elementC;
                this.combinations[i].Add(inputPair, outputElement);
            }
        }
    }

    private void GenerateImageData()
    {
        int len = this.elementImages.Length;
        this.images = new Sprite[(int)Element.COUNT];
        for (int i = 0; i < len; ++i)
            this.images[i] = this.defaultElementImage;
        foreach(var img in this.elementImages)
            this.images[(int)img.element] = img.image;
    }

    #endregion
}
