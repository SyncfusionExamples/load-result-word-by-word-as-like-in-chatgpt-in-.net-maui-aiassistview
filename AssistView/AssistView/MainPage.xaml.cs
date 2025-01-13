using Syncfusion.Maui.AIAssistView;
using Syncfusion.Maui.Chat;
using Syncfusion.Maui.ListView;
using System;
using System.Reflection;

namespace AssistView
{
    public partial class MainPage : ContentPage
    {
        [Obsolete]
        public MainPage()
        {

            InitializeComponent();

            var propertyInfo = sfAIAssistView.GetType().GetField("AssistViewChat", BindingFlags.Instance | BindingFlags.NonPublic);
            var chat = (propertyInfo!.GetValue(sfAIAssistView));

            var propertyInfo1 = chat!.GetType().GetProperty("ChatListView", BindingFlags.Instance | BindingFlags.NonPublic);
            var chatListView = propertyInfo1!.GetValue(chat);

            SfListView listView = (chatListView as SfListView)!;

            MessagingCenter.Subscribe<GettingStartedViewModel, string>(this, "ScrollTo", (sender, arg) =>
            {
                int index = listView!.DataSource!.DisplayItems.Count;

                listView.ItemsLayout!.ScrollToRowIndex(index, ScrollToPosition.End, false);
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<GettingStartedViewModel, string>(this, "ScrollTo");
        }
    }
}
