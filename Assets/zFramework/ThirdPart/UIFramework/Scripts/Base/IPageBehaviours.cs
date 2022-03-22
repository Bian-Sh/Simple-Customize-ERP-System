using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zFrame.UI
{
    /// <summary>
    /// 所有页面的行为基准
    /// </summary>
    public interface IPageBehaviours 
    {
        /// <summary>
        /// 界面被显示
        /// </summary>
        void OnEnter();
        /// <summary>
        /// 界面停留(禁用交互)
        /// </summary>
        void OnPause();
        /// <summary>
        /// 界面继续(可以交互)
        /// </summary>
        void OnResume();
        /// <summary>
        /// 界面关闭
        /// </summary>
        void OnExit();
    }
}
