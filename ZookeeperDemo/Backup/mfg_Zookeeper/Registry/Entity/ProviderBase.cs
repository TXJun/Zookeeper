namespace mfg_Zookeeper.Registry.Entity
{
    using System;
    using System.Runtime.CompilerServices;

    public class ProviderBase : EntityBase
    {
        [CompilerGenerated]
        private string <Address>k__BackingField;
        [CompilerGenerated]
        private int <Weight>k__BackingField;

        public string Address
        {
            [CompilerGenerated]
            get
            {
                return this.<Address>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Address>k__BackingField = value;
            }
        }

        public int Weight
        {
            [CompilerGenerated]
            get
            {
                return this.<Weight>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Weight>k__BackingField = value;
            }
        }
    }
}

