
namespace zFrame
{
    public static partial class Utility
    {
        public static partial class Profiler
        {
            /// <summary>
            /// 性能分析辅助器接口。
            /// </summary>
            public interface IProfilerHelper
            {
                /// <summary>
                /// 开始采样。
                /// </summary>
                /// <param name="name">采样名称。</param>
                void BeginSample(string name);

                /// <summary>
                /// 结束采样。
                /// </summary>
                void EndSample();
            }
        }
    }
}
