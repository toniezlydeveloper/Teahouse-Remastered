using System;
using UnityEngine;

namespace Items.Implementations
{
    public class AddInStorage<TAddIn> : IAddInStorage where TAddIn : Enum
    {
        private float _processingProgress;
        
        public bool RequiresProcessing { get; set; }
        public TAddIn Type { get; set; }
        
        public float ProcessingProgress
        {
            get => _processingProgress;
            set => _processingProgress = Mathf.Clamp(value, 0, MaxProcessing);
        }

        private const float MaxProcessing = 100f;
        
        public bool CanGet()
        {
            if (!RequiresProcessing)
            {
                return true;
            }

            return ProcessingProgress >= MaxProcessing;
        }

        public object Get() => new AddIn<TAddIn> { Type = Type };
        
        public void Reset() => ProcessingProgress = 0f;
    }
    
    public class AddIn<TAddIn> where TAddIn : Enum
    {
        public TAddIn Type { get; set; }
    }
    
    public interface IAddInStorage
    {
        bool CanGet();
        object Get();
        void Reset();
    }
    
    public enum HerbType
    {
        None = 0,
        Basil = 1,
        Thyme = 2,
        Peppermint = 3,
        Spearmint = 4,
        Lemongrass = 5,
        Sage = 6,
        Eucalyptus = 7
    }

    public enum FlowerType
    {
        None = 0,
        Jasmine = 1,
        Chrysanthemum = 2,
        ButterflyPeaFlower = 3,
        Calendula = 4,
        Marigold = 5
    }

    public enum FruitType
    {
        None,
        Pineapple,
        Mango,
        Pomegranate,
        Pear,
        Fig
    }

    public enum RootType
    {
        None,
        Licorice,
        Ginger,
        Turmeric
    }

    public enum SpiceType
    {
        None,
        VanillaBean,
        Saffron,
        Nutmeg,
        Coriander
    }

    public enum WoodsyType
    {
        None,
        CinnamonBark,
        CedarwoodChips,
        SandalwoodChips,
        PineNeedles
    }

    public enum DairyType
    {
        None = 0,
        CoconutMilk = 1,
        CondensedMilk = 2,
        Butter = 3,
        CashewMilk = 4
    }

    public enum SweetenerType
    {
        None = 0,
        DateSyrup = 1,
        Molasses = 2,
        CitrusSyrup = 3,
        BerrySyrup = 4,
        Honeycomb = 5,
        LavenderSyrup = 6,
        RoseSyrup = 7,
        MapleSyrup = 8,
        ElderFlowerSyrup = 9
    }

    public enum TexturedType
    {
        None = 0,
        ChiaSeeds = 1,
        AloeVeraGel = 2,
        CoconutShreds = 3
    }
}