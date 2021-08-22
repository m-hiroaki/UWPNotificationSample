// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SDKTemplateCS;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
using System.Net.Http;

namespace RawNotificationsSampleCS
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            this.Suspending += new SuspendingEventHandler(OnSuspending);
        }

        async protected void OnSuspending(object sender, SuspendingEventArgs args)
        {
            SuspendingDeferral deferral = args.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        async protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //     Do an asynchronous restore
                await SuspensionManager.RestoreAsync();

            }
            var rootFrame = new Frame();
            rootFrame.Navigate(typeof(MainPage));
            Window.Current.Content = rootFrame;
            MainPage p = rootFrame.Content as MainPage;
            p.RootNamespace = this.GetType().Namespace;
            Window.Current.Activate();
        }

        async protected override void OnBackgroundActivated( BackgroundActivatedEventArgs args )
        {
            var deferral = args.TaskInstance.GetDeferral();

            // The URI to launch

            await SendToast( "hello" );
        
            deferral.Complete( );
        }


        private async Task SendToast( string message )
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

        private async void Notification_Activated( ToastNotification sender, object args )
        {
            using ( var client = new HttpClient( ) )
            {
                var result1 = await client.GetAsync(@"https://www.google.com"); // GET
            }
        }
    }
}
