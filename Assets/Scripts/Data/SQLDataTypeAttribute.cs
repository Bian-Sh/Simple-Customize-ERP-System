using System;

public class SQLDataTypeAttribute : Attribute
{
    public string type;
    public SQLDataTypeAttribute(string type)
    {
        this.type = type;
    }
}
