using Syncfusion.Maui.AIAssistView;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace AssistView
{
    public class GettingStartedViewModel : INotifyPropertyChanged
    {
        #region Field
        private ObservableCollection<IAssistItem>? messages;
        private AzureAIService? azureAIService;
        private string? inputText;
        private AssistItem? responseItem;
        #endregion

        #region Constructor
        public GettingStartedViewModel()
        {
            azureAIService = new AzureAIService();
            this.messages = new ObservableCollection<IAssistItem>();
            this.AssistViewRequestCommand = new Command<object>(ExecuteRequestCommand);
        }
        #endregion

        public ICommand AssistViewRequestCommand { get; set; }

        public ObservableCollection<IAssistItem>? AssistItems
        {
            get
            {
                return this.messages;
            }

            set
            {
                this.messages = value;
                RaisePropertyChanged("AssistItems");
            }
        }

        public AssistItem? ResponseItem
        {
            get
            {
                return responseItem;
            }
            set
            {
                responseItem = value; RaisePropertyChanged("ResponseItem");
            }
        }

        public string? InputText
        {
            get { return inputText; }
            set { inputText = value; RaisePropertyChanged("InputText"); }
        }

        private async void ExecuteRequestCommand(object obj)
        {
            // Set Handled to true to restrict the addition of the user-requested item to the view.
            // If the requested item is added, the response indicator will also be added.
            (obj as Syncfusion.Maui.AIAssistView.RequestEventArgs)!.Handled = true;

            // Retrieve the user-requested item using RequestEventArgs.RequestItem.
            var request = (obj as Syncfusion.Maui.AIAssistView.RequestEventArgs)!.RequestItem;

            // Manually add the user-requested item to the AssistItems collection.
            this.AssistItems!.Add(request);

            // Clear the text of the editor in the SfAIAssistView.
            this.InputText = string.Empty;

            // Call the GetResult method to retrieve the ResponseItem corresponding to the requested item.
            await this.GetResult(request).ConfigureAwait(true);
        }

        private async Task GetResult(object inputQuery)
        {
            // Initially add a dummy response with an empty string.
            this.ResponseItem = new AssistItem() { Text = string.Empty };
            this.AssistItems!.Add(this.ResponseItem);

            AssistItem request = (AssistItem)inputQuery;
            if (request != null)
            {
                // Generate a response.
                var userAIPrompt = this.GetUserAIPrompt(request.Text);
                var response = await azureAIService!.GetResultsFromAI(request.Text, userAIPrompt).ConfigureAwait(true);
                response = response.Replace("\n", "<br>");

                // Split the response into words and store them in an array or collection.
                string[] words = response.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                // Update the ResponseItem's Text property word by word with a delay to simulate continuous response generation.
                foreach (string word in words)
                {
                    await Task.Delay(50);
                    this.ResponseItem.Text += word + " ";

                }
                MessagingCenter.Send(this, "ScrollTo",string.Empty);
                this.ResponseItem.RequestItem = inputQuery;
            }
        }

        private string GetUserAIPrompt(string userPrompt)
        {
            string userQuery = $"Given User query: {userPrompt}." +
                      $"\nSome conditions need to follow:" +
                      $"\nGive heading of the topic and simplified answer in 4 points with numbered format" +
                      $"\nGive as string alone" +
                      $"\nRemove ** and remove quotes if it is there in the string.";
            return userQuery;
        }

        #region PropertyChanged

        /// <summary>
        /// Property changed handler.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Occurs when property is changed.
        /// </summary>
        /// <param name="propName">changed property name</param>
        public void RaisePropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        
        #endregion
    }
}
