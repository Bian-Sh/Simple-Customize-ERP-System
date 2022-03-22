using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zFrame.Events;

namespace zFrame.UI
{
    public class ContextMenuEventArgs : BaseEventArgs
    {
        /// <summary>
        /// 光标下的游戏对象
        /// </summary>
        public GameObject Selected { private set; get; }
        /// <summary>
        /// 传递的协商指令
        /// </summary>
        public string command { private set; get; }

        /// <summary>
        /// 鼠标或者触笔事件
        /// </summary>
        /// <param name="_t">事件类型</param>
        /// <param name="_sender">事件发送者</param>
        /// <param name="_selected">被选中的游戏对象</param>
        /// <param name="_buttonID">按键编号</param>
        /// <param name="_hit">碰撞点信息</param>
        public ContextMenuEventArgs Config(ContextMenuEvent _t, GameObject _sender, GameObject _selected, string cmd)
        {
            Config(_t, _sender);
            Selected = _selected;
            command = cmd;
            return this;
        }

        public override void Dispose()
        {
            base.Dispose();
            Selected = null;
        }
    }
    public enum ContextMenuEvent
    {
        PointEnter, //光标移入菜单
        PointExit, //光标移出菜单
        Click, //点击了菜单
    }
}
