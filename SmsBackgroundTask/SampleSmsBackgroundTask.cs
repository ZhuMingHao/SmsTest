using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Devices.Sms;
using Windows.UI.Notifications;

namespace SmsBackgroundTask
{
    public sealed class SampleSmsBackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferal = taskInstance.GetDeferral();
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);
            DisplayToast(taskInstance);
            deferal.Complete();
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            settings.Values[sender.Task.TaskId.ToString()] = "Canceled";
            Debug.WriteLine("Background " + sender.Task.Name + " Cancel Requested...");
        }

        private void DisplayToast(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                SmsMessageReceivedTriggerDetails smsDetails = taskInstance.TriggerDetails as SmsMessageReceivedTriggerDetails;
                SmsTextMessage2 smsTextMessage;

                if (smsDetails.MessageType == SmsMessageType.Text)
                {
                    smsTextMessage = smsDetails.TextMessage;

                    XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
                    XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
                    stringElements.Item(0).AppendChild(toastXml.CreateTextNode(smsTextMessage.From));
                    stringElements.Item(1).AppendChild(toastXml.CreateTextNode(smsTextMessage.Body));

                    ToastNotification notification = new ToastNotification(toastXml);
                    ToastNotificationManager.CreateToastNotifier().Show(notification);
                }
                // Message ACK to operator
                smsDetails.Accept();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error displaying toast: " + ex.Message);
            }
        }
    }
}
