using Syncfusion.Maui.ListView;
using System.Reflection;

namespace AssistView
{
    public class ScrollToBehavior : Behavior<ContentPage>
    {
        private ContentPage? _associatedPage;
        private SfListView? _listView;

        [Obsolete]
        protected override void OnAttachedTo(ContentPage bindable)
        {
            base.OnAttachedTo(bindable);

            _associatedPage = bindable;

            // Access the SfListView from sfAIAssistView
            var sfAIAssistView = _associatedPage.FindByName<object>("sfAIAssistView");
            if (sfAIAssistView != null)
            {
                var propertyInfo = sfAIAssistView.GetType().GetField("AssistViewChat", BindingFlags.Instance | BindingFlags.NonPublic);
                var chat = propertyInfo?.GetValue(sfAIAssistView);

                if (chat != null)
                {
                    var propertyInfo1 = chat.GetType().GetProperty("ChatListView", BindingFlags.Instance | BindingFlags.NonPublic);
                    var chatListView = propertyInfo1?.GetValue(chat);

                    _listView = chatListView as SfListView;
                }
            }

            if (_listView != null)
            {
                // Subscribe to MessagingCenter
                MessagingCenter.Subscribe<GettingStartedViewModel, string>(_associatedPage, "ScrollTo", (sender, arg) =>
                {
                    int index = _listView!.DataSource!.DisplayItems.Count;
                    _listView.ItemsLayout!.ScrollToRowIndex(index, ScrollToPosition.End, false);
                });
            }
        }

        [Obsolete]
        protected override void OnDetachingFrom(ContentPage bindable)
        {
            base.OnDetachingFrom(bindable);

            // Unsubscribe from MessagingCenter
            MessagingCenter.Unsubscribe<GettingStartedViewModel, string>(_associatedPage, "ScrollTo");
            _associatedPage = null;
            _listView = null;
        }
    }
}
