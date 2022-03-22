namespace UnityEngine.UI
{
    public static class ToggleHandlerExtension 
    {
        public static void SetWithoutNotify(this Toggle toggle, bool value) 
        {
            toggle.SetIsOnWithoutNotify(value);
            var handlers = toggle.GetComponents<IToggleActionHandler>();
            foreach (var item in handlers)
            {
                item.UpdateComponents();
            }
        }
    }
}
