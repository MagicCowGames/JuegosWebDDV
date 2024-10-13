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

    // This struct will be used to actually store the real elemental combination data
    // There will be a list of these, each being a layer of possible combinations
    public struct ElementCombinationData
    {
        public Dictionary<ElementPair, Element> combinations;
        public List<List<Element>> combinableElements;
    }

    #endregion

    #region Variables

    [Header("Element Combination Settings")]
    // Basically, rather than hard coding a list of combinations for opposites and another for combinations and then going through them in order, we instead
    // have a list of layers, which could potentially allow us to have multiple combination types with different precedences, which makes it far more flexible
    // for future extension, even tho we probably won't be using this shit to that extent lol.
    // In this case:
    // - Layer[0] corresponds to the opposites, because they have to be evaluated first.
    // - Layer[1] corresponds to the elemental combinations, because they have to be evaluated afterwards to get the correct result.
    [SerializeField] private InputEelementCombinationLayer[] elementCombinationLayers;

    [Header("Element Visual Settings")]
    [SerializeField] private Sprite defaultElementImage;
    [SerializeField] private InputElementImage[] elementImages;

    // NOTE : Discarded idea, the element queue length is configured on the constructor, so that each wizard-like / caster class can use their own config
    // (eg: player has len 5, some npcs len 3 or whatever, etc...)
    // [Header("Element Queue Settings")]
    // [SerializeField] private int elementQueueLength = 5; // number of elements configured to be in all element queues for the game.
    // TODO : Add a section here that controls special combinations for "Magicks", or maybe have a new manager for that.


    // This actually contains the real list of dicts that will allow us to access the elemental combinations easily.
    // The other variables are there for easy inputting of data from Unity's inspector.
    private ElementCombinationData[] combinationData;
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

    // NOTE : Layer 0 corresponds to opposites, layer 1 corresponds to combinations

    public Element[] GetCombinableElements(Element element, int layer)
    {
        if (layer < 0 || layer >= this.combinationData.Length)
            return new Element[0];
        return this.combinationData[layer].combinableElements[(int)element].ToArray();
    }

    public Element GetCombination(Element elementA, Element elementB, int layer)
    {
        var pair = new ElementPair(elementA, elementB);
        if (this.combinationData[layer].combinations.ContainsKey(pair))
            return this.combinationData[layer].combinations[pair];
        return Element.None;
    }

    public Sprite GetSprite(Element element)
    {
        int idx = (int)element;
        if (idx < 0 || idx >= this.images.Length)
            return this.defaultElementImage;
        return this.images[(int)element];
    }

    #endregion

    #region PrivateMethods

    private void GenerateCombinationData()
    {
        int len = this.elementCombinationLayers.Length;
        this.combinationData = new ElementCombinationData[len];
        
        // Process each layer (eg: opposite or combinations layer)
        for(int i = 0; i < len; ++i)
        {
            this.combinationData[i].combinableElements = new List<List<Element>>();
            for (int j = 0; j < (int)Element.COUNT; ++j)
            {
                this.combinationData[i].combinableElements.Add(new List<Element>());
            }

            var layer = this.elementCombinationLayers[i];
            // Process each elemental tuple within the current layer (eg: Q + A = None)
            foreach (var elementTuple in layer.inputElements)
            {
                var elementA = elementTuple.elementA;
                var elementB = elementTuple.elementB;
                var elementC = elementTuple.elementC;

                var pairAB = new ElementPair(elementA, elementB);
                
                this.combinationData[i].combinations.Add(pairAB, elementC);
                this.combinationData[i].combinableElements[(int)elementA].Add(elementB);
                this.combinationData[i].combinableElements[(int)elementB].Add(elementA);
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
