using System;

namespace UnityFramework.Utils
{
    public class Singleton<T> where T : Singleton<T>
	{
		private static T _Instance;

		public static T Instance
		{
			get
			{
				if(_Instance == null)
				{
					_Instance = (T)Activator.CreateInstance(typeof(T),true);
					_Instance.Initializer();
				}
				return _Instance;
			}
		}

		protected virtual void Initializer()
		{

		}
    }
}
