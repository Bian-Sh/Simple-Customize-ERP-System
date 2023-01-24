using System;
/// <summary>
/// 铰链
/// </summary>
[Serializable]
public class Joints:Data
{
    /// <summary>
    /// 所属的产品的索引
    /// </summary>
    public int product_idx;
    /// <summary>
    /// 铰链名称
    /// </summary>
    public string name;
    /// <summary>
    /// 数量
    /// </summary>
    public int quantity;
}