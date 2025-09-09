# load-result-word-by-word-as-like-in-chatgpt-in-.net-maui-aiassistview
This demo shows how to load result word by word as like in ChatGpt in .NET MAUI AIAssistView.

## Sample

```xaml

        public GettingStartedViewModel()
        {
            azureAIService = new AzureAIService();
            this.messages = new ObservableCollection<IAssistItem>();
            this.AssistViewRequestCommand = new Command<object>(ExecuteRequestCommand);
        }

        public ICommand AssistViewRequestCommand { get; set; }


        public string? InputText
        {
            get { return inputText; }
            set { inputText = value; RaisePropertyChanged("InputText"); }
        }

        private async void ExecuteRequestCommand(object obj)
        {
            // Set Handled to true to prevent the addition of the user-requested item to the view. If the requested item is added to the ViewModel's AssistItems collection, the response indicator will also be included immediately. 
            // To restrict the response generator, we are adding the request item manually to collection.
            var eventArgs = obj as Syncfusion.Maui.AIAssistView.RequestEventArgs;
            eventArgs.Handled = true;

            // Retrieve the user-requested item using RequestEventArgs.RequestItem.
            var request = eventArgs.RequestItem;

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

```

## Requirements to run the demo

To run the demo, refer to [System Requirements for .NET MAUI](https://help.syncfusion.com/maui/system-requirements)

## Troubleshooting:
### Path too long exception

If you are facing path too long exception when building this example project, close Visual Studio and rename the repository to short and build the project.

## License

Syncfusion® has no liability for any damage or consequence that may arise from using or viewing the samples. The samples are for demonstrative purposes. If you choose to use or access the samples, you agree to not hold Syncfusion® liable, in any form, for any damage related to use, for accessing, or viewing the samples. By accessing, viewing, or seeing the samples, you acknowledge and agree Syncfusion®'s samples will not allow you seek injunctive relief in any form for any claim related to the sample. If you do not agree to this, do not view, access, utilize, or otherwise do anything with Syncfusion®'s samples.
