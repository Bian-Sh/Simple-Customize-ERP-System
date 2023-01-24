using System;
using System.Collections.Generic;
/// <summary>
/// 产品
/// </summary>
[Serializable]
public class Product
{
    /// <summary>
    /// 产品名称
    /// </summary>
    public string name;

    /// <summary>
    /// 产品类型
    /// </summary>
    public string type;
    /// <summary>
    /// 拉手
    /// </summary>
    public string handle;
    /// <summary>
    ///  材料
    /// </summary>
    public string material;
    /// <summary>
    /// 颜色
    /// </summary>
    public string color;

    /// <summary>
    /// 柜体
    /// </summary>
    public string Cabinet;

    /// <summary>
    /// 铰链
    /// </summary>
    public List<Joints> joints;
    /// <summary>
    /// 产品高
    /// </summary>
    public int heigh;
   /// <summary>
   /// 产品宽
   /// </summary>
    public int width;

    /// <summary>
    /// 开向
    /// </summary>
    public string dirction;

    /// <summary>
    /// 数量
    /// </summary>
    public int quantity;
}