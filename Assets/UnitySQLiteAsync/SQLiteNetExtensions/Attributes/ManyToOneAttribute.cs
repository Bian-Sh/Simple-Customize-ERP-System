﻿namespace SQLiteNetExtensions.Attributes
{
    public class ManyToOneAttribute : RelationshipAttribute
    {
        public ManyToOneAttribute(string foreignKey = null, string inverseProperty = null)
            : base(foreignKey, null, inverseProperty)
        {
        }

    }
}