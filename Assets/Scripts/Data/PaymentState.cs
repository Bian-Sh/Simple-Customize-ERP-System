using System.Collections.Generic;
/// <summary>
/// 支付状态
/// </summary>
public class PaymentState
{
    /// <summary>
    /// 总额
    /// </summary>
    public float total;
    /// <summary>
    /// 付款记录
    /// </summary>
    public List<Payment> payments;
}

public class Payment 
{
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
}