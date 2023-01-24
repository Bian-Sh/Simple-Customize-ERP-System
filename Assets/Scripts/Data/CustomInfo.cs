using System;
/// <summary>
/// 客户信息
/// </summary>
[Serializable]
public class CustomInfo:Data
{
    /// <summary>
    /// 所属订单索引
    /// </summary>
    public int order_idx;
    /// <summary>
    /// 用户名
    /// </summary>
    public string name;
    /// <summary>
    /// 电话
    /// </summary>
    public string phone;
    /// <summary>
    /// 地址
    /// </summary>
    public string address;

    /// <summary>
    /// 客户公司
    /// </summary>
    public string company;
    /// <summary>
    /// 公司主页
    /// </summary>
    public string url;
    /// <summary>
    /// 备注
    /// </summary>
    public string summary;
}