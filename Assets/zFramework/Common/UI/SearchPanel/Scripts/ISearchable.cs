namespace zFrame.UI
{
    // 以下为计划支持，看情况
    //如果有多个关键字请使用英文 ","符号分隔开， 建议是使用多个关键字,把搜索的粒度细化，
    //比如设备名称是 "1F-北侧通道"  可以设置成"1F,北侧，通道 "

    public interface ISearchable
    {
        /// <summary>
        /// 配置的可供搜索的内容上下文
        /// </summary>
        string SearchContext { get; }
    }
}
