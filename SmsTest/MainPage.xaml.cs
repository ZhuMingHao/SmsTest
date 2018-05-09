using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Devices.Sms;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmsTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private const string BackgroundTaskEntryPoint = "SmsBackgroundTask.SampleSmsBackgroundTask";
        private const string BackgroundTaskName = "SampleSmsBackgroundTask";

        private async void Register_BackgroundTask()
        {
            var settings = ApplicationData.Current.LocalSettings;
            try
            {
                var access = await BackgroundExecutionManager.RequestAccessAsync();
                switch (access)
                {
                    case BackgroundAccessStatus.Unspecified:
                        break;
                    case BackgroundAccessStatus.AlwaysAllowed:
                        break;
                    case BackgroundAccessStatus.AllowedSubjectToSystemPolicy:
                        break;
                    case BackgroundAccessStatus.DeniedBySystemPolicy:
                        break;
                    case BackgroundAccessStatus.DeniedByUser:
                        break;
                    default:
                        break;
                }

                SmsMessageType _messageType = SmsMessageType.Text; // set as Text as default
                _messageType = SmsMessageType.Text;

                SmsFilterRule _filterRule = new SmsFilterRule(_messageType);


                SmsFilterActionType _type = SmsFilterActionType.Accept;
                _type = SmsFilterActionType.Accept;


                SmsFilterRules _Rules = new SmsFilterRules(_type);

                IList<SmsFilterRule> rules = _Rules.Rules;

                rules.Add(_filterRule);

                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
                SmsMessageReceivedTrigger trigger = new SmsMessageReceivedTrigger(_Rules);

                taskBuilder.SetTrigger(trigger);
                taskBuilder.TaskEntryPoint = BackgroundTaskEntryPoint;
                taskBuilder.Name = BackgroundTaskName;

                foreach (var cur in BackgroundTaskRegistration.AllTasks)
                {
                    if (cur.Value.Name == BackgroundTaskName)
                    {
                        return;
                    }
                }

                BackgroundTaskRegistration taskRegistration = taskBuilder.Register();

                taskRegistration.Completed += new BackgroundTaskCompletedEventHandler(OnCompleted);

                //LOG              
            }
            catch (Exception ex)
            {
                //LOG               
            }
        }

        private void OnCompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Register_BackgroundTask();
        }
    }
}
