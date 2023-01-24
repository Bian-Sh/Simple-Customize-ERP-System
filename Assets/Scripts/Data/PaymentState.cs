using System.Collections.Generic;
/// <summary>
/// 支付状态
/// </summary>
public class PaymentState:Data
{
    /// <summary>
    /// 所属订单索引
    /// </summary>
    public int order_idx;

    /// <summary>
    /// 总额
    /// </summary>
    public float total;
    /// <summary>
    /// 付款记录
    /// </summary>
    public List<Payment> payments;
}

public class Payment :Data
{
    /// <summary>
    /// 所属支付状态索引
    /// </summary>
    public int payment_idx;

    /// <summary>
    /// 款项类目
    /// </summary>
    public string name;
    /// <summary>
    /// 数额
    /// </summary>
    public float value;
    /// <summary>
    /// 数据录入时间
    /// </summary>
    public string data;
    /// <summary>
    /// 备注
    /// </summary>
    public string summary;
}