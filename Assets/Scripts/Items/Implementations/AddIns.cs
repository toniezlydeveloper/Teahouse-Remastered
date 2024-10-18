using System;
using System.Collections.Generic;
using UnityEngine;

namespace Items.Implementations
{
    // ReSharper disable once StaticMemberInGenericType
    public class AddInProcessor<TAddIn> : IAddInsHolder, IAddInProcessor, IAddInProgress, IAddInType where TAddIn : Enum
    {
        private float _processingProgress;

        public float NormalizedProgress => ProcessingProgress / MaxProcessing;
        public string Name => NamesByAddInType[typeof(TAddIn)];
        public Type AddInType => typeof(TAddIn);

        public List<Enum> HeldAddIns { get; } = new();
        public float ProcessingProgress
        {
            get => _processingProgress;
            set => _processingProgress = Mathf.Clamp(value, 0, MaxProcessing);
        }

        private const float MaxProcessing = 100f;

        private static readonly Dictionary<Type, string> NamesByAddInType = new()
        {
            { typeof(HerbType), "Mortar" },
            { typeof(FlowerType), "Dehydrator" },
        };

        public bool IsReady() => ProcessingProgress >= MaxProcessing;
        
        public void Reset() => ProcessingProgress = 0f;

        public IItem Get() => new AddIn<TAddIn> { Type = (TAddIn)HeldAddIns[0] };
    }
    
    // ReSharper disable once StaticMemberInGenericType
    public class AddInStorage<TAddIn> : IAddInType where TAddIn : Enum
    {
        public string Name => NamesByAddInType[typeof(TAddIn)];
        public Type AddInType => typeof(TAddIn);

        private static readonly Dictionary<Type, string> NamesByAddInType = new()
        {
            { typeof(TeabagType), "Teabag" },
            { typeof(HerbType), "Herb" },
            { typeof(FlowerType), "Flower" },
        };
    }
    
    // ReSharper disable once StaticMemberInGenericType
    public class AddIn<TAddIn> : IAddInsHolder, IAddInGenericType, IAddInType where TAddIn : Enum
    {
        public List<Enum> HeldAddIns => new List<Enum> { Type };
        public string Name => NamesByAddInType[typeof(TAddIn)];
        public Type AddInType => typeof(TAddIn);
        public Enum GenericType => Type;
        
        public TAddIn Type { get; set; }

        private static readonly Dictionary<Type, string> NamesByAddInType = new()
        {
            { typeof(TeabagType), "Teabag" },
            { typeof(HerbType), "Herb" },
            { typeof(FlowerType), "Flower" }
        };
    }
    
    public interface IAddInType : IItem
    {
        Type AddInType { get; }
    }

    public interface IAddInGenericType
    {
        Enum GenericType { get; }
    }

    public interface IAddInProcessor : IItem
    {
        bool IsReady();
        void Reset();
        IItem Get();
    }

    public interface IAddInProgress
    {
        float NormalizedProgress { get; }
    }

    public interface IAddInType<out TAddIn> where TAddIn : Enum
    {
        TAddIn Type { get; }
    }

    public interface IAddInsHolder
    {
        List<Enum> HeldAddIns { get; }
    }

    public enum WaterType
    {
        None = 0,
        Low = 1,
        Medium = 2,
        Boiling = 3
    }

    public enum TeabagType
    {
        None = 0,
        Lavender = 1,
        Rose = 2
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