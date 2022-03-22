using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace zFrame.FSM
{
    public class FSM : IEnumerable
    {
        private List<IState> m_list;
        private IState m_currentState;

        /// <summary>获取当前状态</summary>
        public IState CurrentState
        {
            get
            {
                return m_currentState;
            }
            set
            {
                ChangeState(value);
            }
        }

        public FSM()
        {
            m_list = new List<IState>();
        }
        /// <summary>
        /// 添加指定的状态
        /// </summary>
        /// <param name="_state">指定状态</param>
        public void AddState(IState _state)
        {
            IState _tmpState = GetState(_state.StateName);
            if (_tmpState == null)
            {
                m_list.Add(_state);
            }
            else
            {
                Debug.LogWarningFormat("FSMSystem(容错)：该状态【{0}】已经被添加！", _state.StateName);
            }
        }

        /// <summary>
        /// 移除指定的状态
        /// </summary>
        public void RemoveState(IState _state)
        {
            IState _tmpState = GetState(_state.StateName);
            if (_tmpState != null)
            {
                m_list.Remove(_tmpState);
            }
            else
            {
                Debug.LogWarningFormat("FSMSystem(容错)：该状态【{0}】已经被移除！", _state.StateName);
            }
        }


        //获取相应状态
        public IState GetState(string state)
        {
            //遍历List里面所有状态取出相应的
            foreach (IState _state in m_list)
            {
                if (_state.StateName.Trim() == state.Trim())
                {
                    return _state;
                }
            }
            return null;
        }
        /// <summary>
        /// 状态机状态翻转
        /// </summary>
        /// <param name="state">指定状态机</param>
        public void ChangeState(string state)
        {
            ChangeState(GetState(state));
        }

        /// <summary>
        /// 状态机存在
        /// </summary>
        /// <param name="name">指定状态机</param>
        public bool Exists(string name)
        {
            return m_list.Exists(v => v.StateName == name);
        }

        /// <summary>
        /// 移除当前活动状态
        /// </summary>
        public void CleanActiveState()
        {
            CurrentState?.OnExit();
            CurrentState = null;
        }


        /// <summary>
        /// 状态机状态翻转
        /// </summary>
        /// <param name="state">指定状态机</param>
        public void ChangeState(IState state)
        {
            if (m_currentState != state)
            {
                m_currentState?.OnExit();
                m_currentState = state;
                m_currentState?.OnEnter();
            }
            else if (null != state)
            {
                Debug.LogWarningFormat("FSMSystem(错误)：该状态【{0}】正在运行中！", state.StateName);
            }
        }
        /// <summary>
        /// 更新状态机状态
        /// </summary>
        public void Update()
        {
            CurrentState?.OnUpdate();
        }

        /// <summary>
        /// 移除所有状态
        /// </summary>
        public void RemoveAllState()
        {
            CurrentState?.OnExit();
            CurrentState = null;
            m_list.Clear();
        }

        /// <summary>
        /// 获得枚举器
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return m_list.GetEnumerator();
        }
    }
}
