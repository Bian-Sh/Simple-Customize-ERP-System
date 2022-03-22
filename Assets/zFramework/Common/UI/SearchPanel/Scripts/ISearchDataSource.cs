using System.Collections.Generic;
namespace zFrame.UI
{
    /// <summary>
    /// 约定搜索的数据交换
    /// </summary>
    public interface ISearchDataSource
    {
        /// <summary>
        /// 为 SearchPanel提供源数据
        /// </summary>
        List<ISearchable> SearchableSourceData { get; }
        /// <summary>
        /// 更新 cell item
        /// </summary>
        /// <param name="cell"></param>
        void UpdateCell(BaseCell cell,ISearchable data);
    }
}
