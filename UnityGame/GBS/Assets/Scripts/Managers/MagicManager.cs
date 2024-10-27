using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagicManager : SingletonPersistent<MagicManager>
{
    #region Structs - Elements

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
    public struct InputElementVisualData
    {
        public Element element;
        public Sprite image;
        public Color color;
    }

    // This struct will be used to actually store the real elemental combination data
    // There will be a list of these, each being a layer of possible combinations
    public struct ElementCombinationData
    {
        public Dictionary<ElementPair, Element> combinations;
        public List<List<Element>> combinableElements;
    }

    // This struct contains how much damage 1 single unit of a given element does when applied to an entity.
    [System.Serializable]
    public struct InputElementDamageData
    {
        public Element element;
        public float damage;
    }

    #endregion

    #region Structs - Forms

    [System.Serializable]
    public struct InputFormVisualData
    {
        public Form form;
        public Sprite image;
    }

    #endregion

    #region Variables - Elements

    // Basically, rather than hard coding a list of combinations for opposites and another for combinations and then going through them in order, we instead
    // have a list of layers, which could potentially allow us to have multiple combination types with different precedences, which makes it far more flexible
    // for future extension, even tho we probably won't be using this shit to that extent lol.
    // In this case:
    // - Layer[0] corresponds to the opposites, because they have to be evaluated first.
    // - Layer[1] corresponds to the elemental combinations, because they have to be evaluated afterwards to get the correct result.
    [Header("Element Combination Settings")]
    [SerializeField] private InputEelementCombinationLayer[] elementCombinationLayers;

    [Header("Element Visual Settings")]
    [SerializeField] private InputElementVisualData defaultElementVisualData;
    [SerializeField] private InputElementVisualData[] elementVisualData;

    // NOTE : Discarded idea, the element queue length is configured on the constructor, so that each wizard-like / caster class can use their own config
    // (eg: player has len 5, some npcs len 3 or whatever, etc...)
    // [Header("Element Queue Settings")]
    // [SerializeField] private int elementQueueLength = 5; // number of elements configured to be in all element queues for the game.
    // TODO : Add a section here that controls special combinations for "Magicks", or maybe have a new manager for that.


    // This actually contains the real list of dicts that will allow us to access the elemental combinations easily.
    // The other variables are there for easy inputting of data from Unity's inspector.
    private ElementCombinationData[] combinationData;
    private Sprite[] images;
    private Color[] colors;

    #endregion

    #region Variables - Forms

    [Header("Form Visual Settings")]
    [SerializeField] private InputFormVisualData defaultFormVisualData;
    [SerializeField] private InputFormVisualData[] formVisualData;

    private Sprite[] formSprites;

    #endregion

    // NOTE : In the future, this might have to be modified to also consider element status data such as wet, burn, poison, etc...
    // Also you need to think about how to handle electricity doing more damage if the target is wet, or water having knockback, etc...
    #region Variables - Elements - Damage Values

    [Header("Element Damage Settings")]
    [SerializeField] private InputElementDamageData defaultElementDamageData;
    [SerializeField] private InputElementDamageData[] elementDamageData;

    private float[] elementDamageValues;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        GenerateElementData();
        GenerateFormData();
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods - Elements

    // NOTE : Layer 0 corresponds to opposites, layer 1 corresponds to combinations

    public Element[] GetCombinableElements(Element element, int layer)
    {
        if (layer < 0 || layer >= this.combinationData.Length)
            return new Element[0];
        return this.combinationData[layer].combinableElements[(int)element].ToArray();
    }

    public Element GetElementCombination(Element elementA, Element elementB, int layer)
    {
        var pair = new ElementPair(elementA, elementB);
        if (this.combinationData[layer].combinations.ContainsKey(pair))
            return this.combinationData[layer].combinations[pair];
        return Element.None;
    }

    public Sprite GetElementSprite(Element element)
    {
        int idx = (int)element;
        if (idx < 0 || idx >= this.images.Length)
            return this.defaultElementVisualData.image;
        return this.images[(int)element];
    }

    public Color GetElementColor(Element element)
    {
        int idx = (int)element;
        if (idx < 0 || idx >= this.images.Length)
            return this.defaultElementVisualData.color;
        return this.colors[(int)element];
    }

    public int GetCombinationLayers()
    {
        return this.combinationData.Length;
    }

    public float GetElementDamage(Element element)
    {
        int idx = (int)element;
        if (idx < 0 || idx >= this.elementDamageValues.Length)
            return this.defaultElementDamageData.damage;
        return this.elementDamageValues[idx];
    }

    #endregion

    #region PublicMethods - Forms

    public Sprite GetFormSprite(Form form)
    {
        return this.formSprites[(int)form];
    }

    #endregion

    #region PrivateMethods - Elements

    private void GenerateElementData()
    {
        GenerateElementCombinationData();
        GenerateElementVisualData();
        GenerateElementDamageData();
    }

    private void GenerateElementCombinationData()
    {
        int len = this.elementCombinationLayers.Length;
        this.combinationData = new ElementCombinationData[len];
        
        // Process each layer (eg: opposite or combinations layer)
        for(int i = 0; i < len; ++i)
        {
            this.combinationData[i].combinations = new Dictionary<ElementPair, Element>();
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

    private void GenerateElementVisualData()
    {
        int len = this.elementVisualData.Length;
        this.images = new Sprite[(int)Element.COUNT];
        this.colors = new Color[(int)Element.COUNT];
        for (int i = 0; i < len; ++i)
        {
            this.images[i] = this.defaultElementVisualData.image;
            this.colors[i] = this.defaultElementVisualData.color;
        }
        foreach (var img in this.elementVisualData)
        {
            this.images[(int)img.element] = img.image;
            this.colors[(int)img.element] = img.color;
        }
    }

    private void GenerateElementDamageData()
    {
        int inputLen = (int)this.elementDamageData.Length;
        int elementsLen = (int)Element.COUNT;
        
        this.elementDamageValues = new float[elementsLen];
        for (int i = 0; i < elementsLen; ++i)
            this.elementDamageValues[i] = this.defaultElementDamageData.damage;

        for(int i = 0; i < inputLen; ++i)
            this.elementDamageValues[(int)this.elementDamageData[i].element] = this.elementDamageData[i].damage;
    }

    #endregion

    #region PrivateMethods - Forms

    private void GenerateFormData()
    {
        GenerateFormVisualData();
    }

    private void GenerateFormVisualData()
    {
        int len = this.formVisualData.Length;
        this.formSprites = new Sprite[(int)Form.COUNT];
        
        for (int i = 0; i < len; ++i)
            this.formSprites[i] = this.defaultFormVisualData.image;

        foreach (var form in this.formVisualData)
            this.formSprites[(int)form.form] = form.image;
    }

    #endregion
}
