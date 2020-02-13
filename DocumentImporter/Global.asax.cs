using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using GdPicture14.WEB;
using System.Reflection;

namespace DocumentImporter
{
    public class Global : HttpApplication
    {
        public static readonly int SESSION_TIMEOUT = 20; //Set to 20 minutes. use -1 to handle DocuVieware session timeout through asp.net session mechanism.
        public static readonly bool STICKY_SESSION = true; //Set false to use DocuVieware on Servers Farm witn non sticky sessions.

        void Application_Start(object sender, EventArgs e)
        {
            try
            {
                Assembly.Load("GdPicture.NET.14.WEB.DocuVieware");
            }
            catch (System.IO.FileNotFoundException)
            {
                throw new System.IO.FileNotFoundException(" The system cannot find the DocuVieware assembly. Please set the Copy Local Property of the GdPicture.NET.14.WEB.DocuVieware reference to true. More information: https://msdn.microsoft.com/en-us/library/t1zz5y8c(v=vs.100).aspx");
            }

            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            try
            {
                //Unlocking DocuVieware     KEY of Ver 14
                DocuViewareManager.SetupConfiguration();
                DocuViewareLicensing.RegisterKEY("21187597895849788112611168965395491828");
                // Expired Key - 0419207224848486358422032 
                // Active - 21187597895849788112611168965395491828
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("expired"))
                {
                    throw new Exception("GdPicture.NET.14 Key is Expired. Please provide an active Key and try again.");
                }
                else
                {
                    throw new Exception("Error occured while Configuring and/or Registering GdPicture.NET.14 : \n " + ex.Message);
                }
            }

            DocuViewareEventsHandler.CustomAction += CustomActionsHandler;
            
        }

        public static string GetCacheDirectory()
        {
            return HttpRuntime.AppDomainAppPath;    // + "\\cache";
        }

        public static string GetDocumentsDirectory()
        {
            return HttpRuntime.AppDomainAppPath + "\\documents";
        }

        /// <summary>
        /// CustomActionEvent Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomActionsHandler(object sender, CustomActionEventArgs e)
        {
            switch (e.actionName)
            {
                case "addTimestamp":
                case "addSignature":
                case "addApprovedStamp":
                case "addRejectedStamp":
                    _Default.HandleAnnotationAction(e);
                    break;
                case "rotateM90":
                case "rotateP90":
                    _Default.HandleRotationAction(e);
                    break;
                case "UnsupportedFile":
                    _Default.HandleUnsupportedFileTypeAction(e);
                    break;
                case "RemovePage":
                    _Default.HandleRemovePage(e);
                    break;
                case "CloseDoc":
                    _Default.HandleCloseDocument(e);
                    break;
                case "automaticRemoveBlackBorders":
                case "autoDeskew":
                case "punchHoleRemoval":
                case "despeckle":
                    _Default.HandleImageCleanupAction(e);
                    break;
            }
        }
    }
}