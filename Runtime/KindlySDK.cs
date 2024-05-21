using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Kindly
{
    public class KindlySDK : MonoBehaviour
    {
        string botKey = "";
        string languageCode = "en";


#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void KindlySDK_start(string botKey);

        [DllImport("__Internal")]
        private static extern void KindlySDK_displayChat(string languageCode);

        [DllImport("__Internal")]
        private static extern void KindlySDK_setNewContext(Dictionary<string, string> context);

        [DllImport("__Internal")]
        private static extern void KindlySDK_clearNewContext(Dictionary<string, string> context);

        [DllImport("__Internal")]
        private static extern void KindlySDK_setAPNSDeviceTokenData(byte[] deviceToken);

        [DllImport("__Internal")]
        private static extern void KindlySDK_setAPNSDeviceTokenStr(string deviceToken);

        [DllImport("__Internal")]
        private static extern void KindlySDK_saveAuthToken(string token);

        [DllImport("__Internal")]
        private static extern void KindlySDK_closeChat();

        [DllImport("__Internal")]
        private static extern void KindlySDK_endChat();

        [DllImport("__Internal")]
        private static extern void KindlySDK_notificationReceived(Dictionary<object, object> userInfo);



        // Test

        [DllImport("__Internal")]
        private static extern void KindlySDK_log();

        [DllImport("__Internal")]
        private static extern void KindlySDK_displayTestScreen();

        [DllImport("__Internal")]
        private static extern void KindlySDK_displayTestChat();

        [DllImport("__Internal")]
        private static extern void KindlySDK_testStart();

        [DllImport("__Internal")]
        private static extern void KindlySDK_testStartAndDisplayChat();
#endif

#if UNITY_ANDROID
        private static AndroidJavaObject kindlyTestClassObject;
        private static AndroidJavaClass kindlyClass;
        private static AndroidJavaObject kindlyInstance;
        private static AndroidJavaObject kindlyClassObject;
        private static AndroidJavaObject application;
        private static AndroidJavaObject currentActivity;
#endif


        // Start is called before the first frame update
        void Start()
        {
#if UNITY_IOS || UNITY_ANDROID
            Debug.Log("Kindly SDK is supported on this platform");
#else
            Debug.Log("Kindly SDK is not supported on this platform");
#endif

#if UNITY_ANDROID
            using (AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                currentActivity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
                Debug.Log("currentActivity name is: " + currentActivity.Call<string>("getLocalClassName"));

                application = currentActivity.Call<AndroidJavaObject>("getApplication");

                kindlyTestClassObject = new AndroidJavaObject("no.kindly.chatsdk.chat.KindlyTest");
                kindlyInstance = new AndroidJavaObject("no.kindly.chatsdk.chat.ChatKindlySDK");

                using (AndroidJavaClass pluginClass = new AndroidJavaClass("no.kindly.chatsdk.chat.ChatKindlySDK"))
                {
                    Debug.Log("pluginClass is: " + pluginClass);
                    if (pluginClass != null)
                    {
                        // log this point
                        Debug.Log("pluginClass is not null");
                        kindlyClass = pluginClass;
                        // kindlyTestClass = pluginClass.CallStatic<AndroidJavaObject>("getInstance", activityObject);
                    }
                    else
                    {
                        Debug.Log("pluginClass is null");
                    }
                }

                // using (AndroidJavaObject pluginObject = new AndroidJavaObject("no.kindly.chatsdk.chat.ChatKindlySDK"))
                // {
                //     Debug.Log("pluginObject is: " + pluginObject);
                //     if (pluginObject != null)
                //     {
                //         // log this point
                //         Debug.Log("pluginObject is not null");
                //         kindlyInstance = pluginObject;
                //     } else {
                //         Debug.Log("pluginClass is null");
                //     }
                // }
            }
#endif
        }

        // Update is called once per frame
        void Update()
        {


        }


        public void OnDisplayKindlyButtonClicked()
        {
            Debug.Log("Kindly Button Clicked");

#if UNITY_IOS
            KindlySDK_log();

            try
            {
                Debug.Log("KindlySDK_start");
                KindlySDK_start(botKey);
            }
            catch (Exception e)
            {
                Debug.Log("KindlySDK_start Error: " + e.Message);
            }

            try
            {
                Debug.Log("KindlySDK_displayChat");
                KindlySDK_displayChat(languageCode);
            }
            catch (Exception e)
            {
                Debug.Log("KindlySDK_displayChat Error: " + e.Message);
            }
#elif UNITY_ANDROID
            if (kindlyInstance != null)
            {
                Debug.Log("kindlyInstance is not null");


                // kindlyInstance.Call("start", botKey, languageCode);
                // var application = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
                if (currentActivity != null)
                {
                    Debug.Log("currentActivity is not null");

                    if (application != null)
                    {
                        Debug.Log("application is not null");
                        kindlyInstance.Call("start", application, botKey, languageCode);

                        // kindlyInstance.Call("launchChat", currentActivity);
                    }
                    else
                    {
                        Debug.Log("application is null");
                    }
                }
                else
                {
                    Debug.Log("currentActivity is null");
                }
            }
            else
            {
                Debug.Log("kindlyClass is null");
            }
#endif
        }

        public void OnDisplayTestKindlyButtonClicked()
        {
            Debug.Log("Kindly Test Button Clicked");

#if UNITY_IOS
            try
            {
                Debug.Log("KindlySDK_testStart");
                Dictionary<string, string> context = new Dictionary<string, string> {
                    { "name", "John Doe" },
                    { "email", "test@test.com"}
                };
                Debug.Log("context: " + context);
                KindlySDK_setNewContext(context);
            }
            catch (Exception e)
            {
                Debug.Log("KindlySDK_testStart Error: " + e.Message);
            }

            // try
            // {
            //     Debug.Log("KindlySDK_testStartAndDisplayChat");
            //     KindlySDK_testStartAndDisplayChat();
            // }
            // catch (Exception e)
            // {
            //     Debug.Log("KindlySDK_testStartAndDisplayChat Error: " + e.Message);
            // }
#elif UNITY_ANDROID
            if (kindlyTestClassObject != null)
            {
                try
                {
                    // kotlin declaration: public fun test() {
                    // kindlyTestClass.Call("test");
                    // kindlyTestClassObject.Call("retrofitTest");

                    // kotlin declaration: public fun testSDK(application: Application, context: Context) {
                    // kindlyTestClassObject.Call("testSDK", application, currentActivity);

                    kindlyInstance.Call("test");
                }
                catch (Exception e)
                {
                    Debug.Log("kindlyTestClassObject test Error: " + e.Message);
                }
            }
            else
            {
                Debug.Log("kindlyTestClassObject is null");
            }
#endif
        }
    }

}
