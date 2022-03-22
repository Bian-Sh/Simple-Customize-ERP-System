namespace zFrame.FSM
{
    public interface IState
    {
        
        /// <summary>
        /// 状态进入
        /// </summary>
        void OnEnter();
        /// <summary>
        /// 状态更新
        /// </summary>
        void OnUpdate();
        /// <summary>
        /// 状态退出
        /// </summary>
        void OnExit();
        /// <summary>
        /// 获得状态
        /// </summary>
        string StateName { get; }
    }
}
