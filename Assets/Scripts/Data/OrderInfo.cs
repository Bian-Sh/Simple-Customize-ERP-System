using System;
using System.Collections.Generic;

/// <summary>
/// 订单信息
/// </summary>
[Serializable]
public class OrderInfo 
{
    /// <summary>
    /// 订单编号
    /// </summary>
    public string id;

    /// <summary>
    /// 订单配送地址
    /// </summary>
    public string address;

    /// <summary>
    /// 客户信息
    /// </summary>
    public CustomInfo custom;

    /// <summary>
    /// 产品
    /// </summary>
    public List<Product> products;

    /// <summary>
    /// 备注
    /// </summary>
    public string summary;

    /// <summary>
    /// 订单生产时间
    /// </summary>
    public string data;

    /// <summary>
    /// 付费状态
    /// </summary>
    public PaymentState state;
}
