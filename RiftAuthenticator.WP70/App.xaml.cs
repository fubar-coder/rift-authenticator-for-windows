/*
 * This file is part of RIFT™ Authenticator for Windows.
 *
 * RIFT™ Authenticator for Windows is free software: you can redistribute 
 * it and/or modify it under the terms of the GNU General Public License 
 * as published by the Free Software Foundation, either version 3 of the 
 * License, or (at your option) any later version.
 *
 * RIFT™ Authenticator for Windows is distributed in the hope that it will 
 * be useful, but WITHOUT ANY WARRANTY; without even the implied warranty 
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with RIFT™ Authenticator for Windows.  If not, see 
 * <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace RiftAuthenticator.WP7
{
    public partial class App : Application
    {
        private class QuitException : Exception
        {
        }

        internal static Library.IAccountManager AccountManager { get; set; }
        internal static Library.IAccount Account { get; set; }
        internal static string AuthCreateUsername { get; set; }
        internal static string AuthCreatePassword { get; set; }
        internal static string AuthCreateDeviceId { get; set; }
        internal static string AuthCreateDescription { get; set; }
        internal static string[] SecurityQuestions { get; set; }

        internal static bool ExitApp { get; set; }
        internal static bool BackToMainPage { get; set; }

        internal static string CreateDefaultAccountDescription()
        {
            var activeAccounts = App.AccountManager.Where(x => !x.IsEmpty).Count();
            if (activeAccounts == 0)
            {
                return WP7.Resources.AppResource.AuthNameDefault;
            }
            else
            {
                var accountIndex = activeAccounts + 1;
                string accountDescription;
                while (App.AccountManager.FindAccount((accountDescription = string.Format(WP7.Resources.AppResource.AuthNameNumber, accountIndex))) != null)
                {
                    ++accountIndex;
                }
                return accountDescription;
            }
        }

        internal static Library.IAccount CreateNewAccountObject()
        {
            Library.IAccount newAccount;
            if (Account.IsEmpty)
            {
                newAccount = Account;
            }
            else
            {
                newAccount = AccountManager.CreateAccount();
            }
            return newAccount;
        }

        internal static void AddNewAccountObject(Library.IAccount newAccount)
        {
            if (newAccount != Account)
            {
                AccountManager.Add(newAccount);
                Account = newAccount;
            }
        }

        internal static void SaveNewAccountObject(Library.IAccount newAccount)
        {
            AddNewAccountObject(newAccount);
            AccountManager.SaveAccounts();
        }

        internal static void ExecuteTimeSync(System.Windows.Threading.Dispatcher dispatcher)
        {
            ExecuteTimeSync(dispatcher, null);
        }

        internal static void ExecuteTimeSync(System.Windows.Threading.Dispatcher dispatcher, Action afterSyncFunc)
        {
            Library.TrionServer.BeginGetTimeOffset((ar) =>
            {
                dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        var timeOffset = Library.TrionServer.EndGetTimeOffset(ar);
                        foreach (var account in AccountManager)
                            account.TimeOffset = timeOffset;
                        AccountManager.SaveAccounts();
                        if (afterSyncFunc != null)
                            afterSyncFunc();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, WP7.Resources.AppResource.MessageBoxTitleError, MessageBoxButton.OK);
                    }
                });
            }, null);
        }

        internal static void Quit()
        {
            throw new QuitException();
        }


        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is QuitException)
                return;

            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}