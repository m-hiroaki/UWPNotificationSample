using Microsoft.Toolkit.Uwp.Notifications;
using System;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace BackgroundTasks
{
    // You must use a sealed class, and make sure the output is a WINMD.
    public sealed class SampleBackgroundTask : IBackgroundTask
    {
        public void Run( IBackgroundTaskInstance taskInstance )
        {
            var deferal = taskInstance.GetDeferral( );
            // Get the background task details
            // Store the content received from the notification so it can be retrieved from the UI.
            //RawNotification notification = (RawNotification)taskInstance.TriggerDetails;
            //settings.Values[taskName] = notification.Content;
            SendToast( "Hello toast" );

            deferal.Complete( );
        }

        private async void SendToast( string message )
        {
            ToastContent content = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "Background Task Completed"
                            },

                            new AdaptiveText()
                            {
                                Text = message
                            }
                        }
                    }
                },
                Launch = "http://www.google.com"
            };

            content.ActivationType = ToastActivationType.Protocol;

            var notification = new ToastNotification( content.GetXml( ) );
            notification.Activated += Notification_Activated;
            ToastNotificationManager.CreateToastNotifier( ).Show( notification );
        }

        private void Notification_Activated( ToastNotification sender, object args )
        {
            ;
        }
    }

    public sealed class NotificationActionBackgroundTask : IBackgroundTask
    {
        public async void Run( IBackgroundTaskInstance taskInstance )
        {
            var deferral = taskInstance.GetDeferral();

            // The URI to launch
            var uriBing = new Uri(@"https://www.google.com");

            try
            {
                // Launch the URI
                System.Diagnostics.Process.Start("https://www.google.com");
            }
            catch (Exception e)
            {
                ;
            }
            deferral.Complete( );
        }
    }
}
