using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GdPicture14;
using GdPicture14.WEB;
using System.Reflection;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Drawing;
using GdPicture14.Annotations;
using Newtonsoft.Json;

/**
 *  Features Included :
 *  -------------------
 *  Custom Toolbar
 *  Open
 *  Load From URI
 *  Previous / Next Page
 *  Pagination
 *  Zoon Out
 *  Zoom In
 *  Rotate Left 90
 *  Rotate Right 90
 *  
 *  Custom Snap In
 *  ---------------
 *  Add timestamp
 *  Add Signature
 *  Add "Approved" stamp
 *  Add "Rejected" Stamp
 * 
 *  Save File
 *  ---------
 *  TXT file saves proeprly as Tiff. With PDF problem opening the file
 *  
 *  Rotation - is Supported only in Image files
 *  
 *  
 *  TO-DO
 *  ******
 *  1) Save Window - for Txt Save asTiff only -- NOT A GOOD IDEA
 *  2) DocType Not Image - Disable Rotation -- DONE
 *  3) Add - Crop black Borders, Auto Deskew, Punch hole removal & Despeckle
 */

namespace DocumentImporter
{
    internal class RotateActionParameters
    {
        public int CurrentPage { get; set; }
        public RegionOfInterest RegionOfInterest { get; set; }
        public int[] Pages { get; set; }
    }

    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            docuView.Timeout = Global.SESSION_TIMEOUT;
            docuView.CacheFolder = Global.GetCacheDirectory();
            docuView.StickySessionsEnabled = Global.STICKY_SESSION;
            if (!IsPostBack)
            {
               //this.LoadComplete += _Default_LoadComplete;
               CustomSnapInGeneration();
                //saveButtondocuView
            }
            
        }

        /// <summary>
        /// Generates and register the custom snap-in content
        /// </summary>
        private void CustomSnapInGeneration()
        {
            using (HtmlGenericControl icon = new HtmlGenericControl("svg"))
            {
                icon.Attributes["viewBox"] = "0 0 16 16";
                icon.Attributes["width"] = "100%";
                icon.Attributes["height"] = "100%";
                icon.InnerHtml = "<path d=\"M0 16c2-6 7.234-16 16-16-4.109 3.297-6 11-9 11s-3 0-3 0l-3 5h-1z\"></path>";

                using (HtmlGenericControl panel = new HtmlGenericControl("div"))
                {
                    panel.ClientIDMode = ClientIDMode.Static;
                    panel.ID = "LeaveRequestApprovalSnapInPanel" + docuView.UniqueID;

                    using (HtmlGenericControl customSnapInButtonDiv = new HtmlGenericControl("div"))
                    {
                        customSnapInButtonDiv.Style["float"] = "left";
                        customSnapInButtonDiv.Style["margin-top"] = "6px";
                        customSnapInButtonDiv.Style["margin-left"] = "6px";

                        // Time Stamp
                        using (HtmlGenericControl customSnapInButton = new HtmlGenericControl("button"))
                        {
                            customSnapInButton.ID = "LeaveRequestApprovalSnapInTimestamp" + docuView.UniqueID;
                            customSnapInButton.Attributes["class"] = "btn-valid";
                            customSnapInButton.Style["margin-bottom"] = "15px";
                            customSnapInButton.Style["margin-left"] = "15px";
                            customSnapInButton.Style["height"] = "26px";
                            customSnapInButton.Style["width"] = "80px";
                            customSnapInButton.Style["display"] = "block";
                            customSnapInButton.Style["background-color"] = ColorTranslator.ToHtml(docuView.ActiveSelectedColor);
                            customSnapInButton.InnerHtml = "Timestamp";
                            customSnapInButton.Attributes["onclick"] = "addTimestamp(); return false;";
                            customSnapInButtonDiv.Controls.Add(customSnapInButton);
                        }
                        panel.Controls.Add(customSnapInButtonDiv);

                        // Signature
                        using (HtmlGenericControl customSnapInButton = new HtmlGenericControl("button"))
                        {
                            customSnapInButton.ID = "LeaveRequestApprovalSnapInSignature" + docuView.UniqueID;
                            customSnapInButton.Attributes["class"] = "btn-valid";
                            customSnapInButton.Style["margin-bottom"] = "15px";
                            customSnapInButton.Style["margin-left"] = "15px";
                            customSnapInButton.Style["height"] = "26px";
                            customSnapInButton.Style["width"] = "80px";
                            customSnapInButton.Style["display"] = "block";
                            customSnapInButton.Style["background-color"] = ColorTranslator.ToHtml(docuView.ActiveSelectedColor);
                            customSnapInButton.InnerHtml = "Signature";
                            customSnapInButton.Attributes["onclick"] = "addSignature(); return false;";
                            customSnapInButtonDiv.Controls.Add(customSnapInButton);
                        }

                        // Approved RubberStamp
                        using (HtmlGenericControl customSnapInBtn = new HtmlGenericControl("button") )
                        {
                            customSnapInBtn.ID = "LeaveRequestApprovalSnapIn_ApprovedRubberStamp" + docuView.UniqueID;
                            /*
                            customSnapInBtn.Attributes["viewBox"] = "0 0 512 512";
                            customSnapInBtn.Attributes["width"] = "100%";
                            customSnapInBtn.Attributes["height"] = "100%";
                            customSnapInBtn.InnerHtml = "<path d=\"M 502.17 471.293 C 498.894 479.536 491.698 485 482.614 485 H 29.392 c - 9.074 0 - 16.275 - 5.452 - 19.557 - 13.681 l - 1.611 11.991 C 6.007 499.252 15.508 512 29.392 512 h 453.223 c 13.883 0 23.357 - 12.748 21.168 - 28.689 L 502.17 471.293 Z M 29.392 459 h 453.223 c 13.883 0 23.357 - 13.272 21.168 - 29.215 L 485.65 293.901 C 483.802 280.073 472.189 268 459.705 268 H 312.91 c - 9.316 - 21.166 - 19.587 - 46.501 - 19.587 - 78.786 c 0 - 65.296 33.704 - 94.909 33.704 - 124.417 c 0 - 34.522 - 30.432 - 62.552 - 71.975 - 64.558 c - 41.648 2.006 - 74.72 30.035 - 74.72 64.558 c 0 29.507 32.359 59.12 32.359 124.417 c 0 32.285 - 9.665 57.621 - 18.431 78.786 H 52.3 c - 12.484 0 - 24.07 12.073 - 25.918 25.901 L 8.224 429.785 C 6.007 445.728 15.508 459 29.392 459 Z\" ></path>";

                            */
                                    customSnapInBtn.Attributes["class"] = "btn-valid";
                        customSnapInBtn.Style["margin-bottom"] = "15px";
                        customSnapInBtn.Style["margin-left"] = "15px";
                        customSnapInBtn.Style["height"] = "26px";
                        customSnapInBtn.Style["width"] = "80px";

                        customSnapInBtn.Style["display"] = "block";
                        customSnapInBtn.Style["background-color"] = ColorTranslator.ToHtml(docuView.ActiveSelectedColor);
                        customSnapInBtn.InnerHtml = "Approved Stamp";
                       
                            customSnapInBtn.Attributes["onclick"] = "addApprovedStamp(); return false;";
                            customSnapInButtonDiv.Controls.Add(customSnapInBtn);
                        }

                        // Rejected RubberStamp
                        using (HtmlGenericControl customSnapInBtn = new HtmlGenericControl("button"))
                        {
                            customSnapInBtn.ID = "LeaveRequestApprovalSnapIn_RejectedRubberStamp" + docuView.UniqueID;
                            customSnapInBtn.Attributes["class"] = "btn-valid";
                            customSnapInBtn.Style["margin-bottom"] = "15px";
                            customSnapInBtn.Style["margin-left"] = "15px";
                            customSnapInBtn.Style["height"] = "26px";
                            customSnapInBtn.Style["width"] = "80px";
                            customSnapInBtn.Style["display"] = "block";
                            customSnapInBtn.Style["background-color"] = ColorTranslator.ToHtml(docuView.ActiveSelectedColor);
                            customSnapInBtn.InnerHtml = "Rejected Stamp";
                            customSnapInBtn.Attributes["onclick"] = "addRejectedStamp(); return false;";
                            customSnapInButtonDiv.Controls.Add(customSnapInBtn);
                        }

                        panel.Controls.Add(customSnapInButtonDiv);

                    }
                    docuView.AddCustomSnapIn("LeaveRequestApprovalSnapIn", "Leave Request Approval", icon, panel, true);
                }
            }
        }




        /// <summary>
        /// Get the DocuViewareMessage for specified text message and icon type
        /// </summary>
        /// <param name="text"></param>
        /// <param name="iconType"></param>
        /// <returns></returns>
        private static DocuViewareMessage GetMessage(string text, DocuViewareMessageIcon iconType)
        {
            DocuViewareMessage msg = new DocuViewareMessage(text, icon: iconType);
            return msg;
        }


        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public static void HandleSaveFile(CustomActionEventArgs customActionEventArgs)
        {
            
            DocumentType docType = customActionEventArgs.docuVieware.GetDocumentType();
            if (docType == DocumentType.DocumentTypeTXT)
            {
                MemoryStream memStream = null;
                GdPictureStatus status;

                status = customActionEventArgs.docuVieware.GetDocumentData(out memStream);

                if (status == GdPictureStatus.OK)
                {
                    string nm = customActionEventArgs.docuVieware.GetDocumentName();
                    int indexOf = nm.IndexOf('.');
                    if (indexOf > 0)
                        nm = nm.Substring(0, indexOf);

                    nm = nm + ".tiff";
                    string fileName = HttpRuntime.AppDomainAppPath + "\\" + nm;
                    FileStream fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite);
                    fs.Write(memStream.GetBuffer(), 0, (int)memStream.Length);
                    customActionEventArgs.docuVieware.SaveAsTIFF(memStream);

                    fs = null;
                } else
                {
                    customActionEventArgs.message = GetMessage("Save : Fail to get Stream data.", DocuViewareMessageIcon.Error);
                }

                memStream = null;
            }
        }
*/

        /// <summary>
        /// Triggers when user clicks on Rotate -90 or Rotate +90. Rotates the image file as per the request.
        /// </summary>
        /// <remarks>
        /// Only image file formats can be Rotated. Shows error message if File is not Supported, File is not of proper format, 
        /// Cannot select Page, Failed to Rotate the file contents. 
        /// </remarks>
        /// <param name="e">The CustomActionEventArgs received from the Global.asax.cs custom actions handler</param>
        public static void HandleRotationAction(CustomActionEventArgs e)
        {
            GdPictureStatus status = GdPictureStatus.Aborted;

            // For Images
            if (e.docuVieware.GetDocumentType() == DocumentType.DocumentTypeBitmap)
            {
                int imageId;
                status = e.docuVieware.GetNativeImage(out imageId);
                if (status == GdPictureStatus.OK)
                {
                    GdPictureImaging gdPictureImaging = new GdPictureImaging();
                    RotateActionParameters rotateParams = JsonConvert.DeserializeObject<RotateActionParameters>(e.args.ToString());

                    int currPage = rotateParams.CurrentPage;
                    status = gdPictureImaging.SelectPage(imageId, currPage);    //  rotateParams.CurrentPage);
                    if (status == GdPictureStatus.OK)
                    {
                        switch (e.actionName)
                        {
                            case "rotateM90":
                                status = gdPictureImaging.Rotate(imageId, RotateFlipType.Rotate270FlipNone);
                                break;
                            case "rotateP90":
                                status = gdPictureImaging.Rotate(imageId, RotateFlipType.Rotate90FlipNone);
                                break;
                        }
                        if (status != GdPictureStatus.OK)
                        {
                            e.message = new DocuViewareMessage("Error during rotating: " + status + ".", icon: DocuViewareMessageIcon.Error);
                        }
                        else
                        {
                            status = e.docuVieware.RedrawPage();
                            e.message = status == GdPictureStatus.OK ? new DocuViewareMessage("Rotation successfuly applied.", icon: DocuViewareMessageIcon.Ok) : new DocuViewareMessage("Error during redraw pages : " + status + ".", icon: DocuViewareMessageIcon.Error);
                        }
                    }
                    else
                    {
                        e.message = new DocuViewareMessage("Error during page selection: " + status + ".", icon: DocuViewareMessageIcon.Error);
                    }

                    rotateParams = null;
                    gdPictureImaging = null;
                }
                else
                {
                    e.message = new DocuViewareMessage("Error during get native image : " + status + ".", icon: DocuViewareMessageIcon.Error);
                }
            }
            else if (e.docuVieware.GetDocumentType() == DocumentType.DocumentTypePDF)
            {
                GdPicturePDF gdPdf = null;
                if (e.docuVieware.GetNativePDF(out gdPdf) == GdPictureStatus.OK)
                {
                    RotateActionParameters rotateParams = JsonConvert.DeserializeObject<RotateActionParameters>(e.args.ToString());
                    
                    int currPage = rotateParams.CurrentPage;
                    gdPdf.SelectPage(currPage);
                    switch (e.actionName)
                    {
                        case "rotateM90":
                            status = gdPdf.RotatePage(-90);
                            break;
                        case "rotateP90":
                            status = gdPdf.RotatePage(90);
                            break;
                    }
                    if (status != GdPictureStatus.OK)
                    {
                        e.message = GetMessage("Error during rotating: " + status + ".", DocuViewareMessageIcon.Error);
                    }
                    else
                    {
                        status = e.docuVieware.RedrawPage();
                        e.message = status == GdPictureStatus.OK ? GetMessage("Rotation successfuly applied.", DocuViewareMessageIcon.Ok) : GetMessage("Error during redraw pages : " + status + ".", DocuViewareMessageIcon.Error);
                    }

                    rotateParams = null;
                    gdPdf = null;
                }
                else
                {
                    e.message = GetMessage("Error during get native image : " + status + ".", DocuViewareMessageIcon.Error);
                }
            } else
            {
                e.message = GetMessage("Only Images & PDF formats Rotation supported", DocuViewareMessageIcon.Error);
            }

            

            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public static void HandleUnsupportedFileTypeAction(CustomActionEventArgs e)
        {
            e.message = GetMessage("Only PDF & DOCX file formsts supported.", DocuViewareMessageIcon.Warning);
        }

        #region "Annotations Handler & Methods"

        /// <summary>
        /// Triggered when the user clicks either the "Apply Timestamp" or "Apply signature" button
        /// It retrives the native document depending on its type (image or PDF), create and initialize
        /// the proper AnnotationManager object that will be used to draw the annotations.        
        /// </summary>
        /// <param name="customActionEventArgs">The arguments received from the Global.asax.cs custom actions handler</param>
        public static void HandleAnnotationAction(CustomActionEventArgs customActionEventArgs)
        {
            if (customActionEventArgs.docuVieware.PageCount > 0)
            {
                DocumentType docType = customActionEventArgs.docuVieware.GetDocumentType();
                switch (docType)
                {
                    case DocumentType.DocumentTypeBitmap:
                        AnnotateImageDocument(customActionEventArgs);
                        break;
                    case DocumentType.DocumentTypePDF:
                        AnnotatePdfDocument(customActionEventArgs);
                        break;
                    //case DocumentType.DocumentTypeOpenXMLWord:
                    //    break;
                    default:
                        customActionEventArgs.message = new DocuViewareMessage("Document format not supported", icon: DocuViewareMessageIcon.Error);
                        break;
                }
            }
            else
            {
                customActionEventArgs.message = GetMessage("Please open a filee first." + ".", DocuViewareMessageIcon.Error);
            }

            return;
        }

        /// <summary>
        /// Dedicated to process image documents
        /// It retrieves the currently loaded native image handle and initializes an annotation manager object from it.
        /// </summary>
        /// <param name="customActionEventArgs">The arguments received from the custom action handler</param>
        private static void AnnotateImageDocument(CustomActionEventArgs customActionEventArgs)
        {
            int imageId;
            GdPictureStatus status = customActionEventArgs.docuVieware.GetNativeImage(out imageId);
            if (status == GdPictureStatus.OK)
            {
                using (AnnotationManager annotationMngr = new AnnotationManager() )
                {                    
                    status = annotationMngr.InitFromGdPictureImage(imageId);
                    if (status == GdPictureStatus.OK)
                    {
                        if (customActionEventArgs.actionName == "addTimestamp")
                            AddTimestampAnnotation(customActionEventArgs, annotationMngr);
                        else if (customActionEventArgs.actionName == "addSignature")
                            AddSignatureAnnotation(customActionEventArgs, annotationMngr);
                        else if (customActionEventArgs.actionName == "addApprovedStamp")
                            AddApprovedStampAnnotation(customActionEventArgs, annotationMngr);
                        else if (customActionEventArgs.actionName == "addRejectedStamp")
                            AddRejecteStampAnnotation(customActionEventArgs, annotationMngr);
                    }
                    else
                        customActionEventArgs.message = new DocuViewareMessage("Failed to initialize Annotation Manager. Status is : " + status + ".",
                            icon: DocuViewareMessageIcon.Error);
                }
            } else
            {
                customActionEventArgs.message = new DocuViewareMessage("Failed to retrieve native document. Status is : " + status + ".",
                            icon: DocuViewareMessageIcon.Error);
            }

            return;
        }


        /// <summary>
        /// Dedicated to process image documents
        /// It retrieves the currently loaded native image handle and initializes an annotation manager object from it.
        /// </summary>
        /// <param name="customActionEventArgs">The arguments received from the custom action handler</param>
        private static void AnnotatePdfDocument(CustomActionEventArgs customActionEventArgs)
        {
            GdPicturePDF gdPicturePdf;
            GdPictureStatus status = customActionEventArgs.docuVieware.GetNativePDF(out gdPicturePdf);

            if (status == GdPictureStatus.OK)
            {
                using (AnnotationManager annotateMngr = new AnnotationManager() )
                {
                    status = annotateMngr.InitFromGdPicturePDF(gdPicturePdf);
                    if (status == GdPictureStatus.OK)
                    {
                        if (customActionEventArgs.actionName == "addTimestamp")
                            AddTimestampAnnotation(customActionEventArgs, annotateMngr);
                        else if (customActionEventArgs.actionName == "addSignature")
                            AddSignatureAnnotation(customActionEventArgs, annotateMngr);
                        else if (customActionEventArgs.actionName == "addApprovedStamp")
                            AddApprovedStampAnnotation(customActionEventArgs, annotateMngr);
                        else if (customActionEventArgs.actionName == "addRejectedStamp")
                            AddRejecteStampAnnotation(customActionEventArgs, annotateMngr);
                    }
                    else
                        customActionEventArgs.message = new DocuViewareMessage("Failed to initialize Annotation Manager. Status is : " + status + ".",
                            icon: DocuViewareMessageIcon.Error);
                }
            }
            else
            {
                customActionEventArgs.message = new DocuViewareMessage("Failed to retrieve native document. Status is : " + status + ".",
                            icon: DocuViewareMessageIcon.Error);
            }

            return;
        }

        /// <summary>
        /// Draw the signature at the given location with a given size from a base64 encoded PNG image
        /// </summary>
        /// <param name="customActionEventArgs">The arguments received from the custom action handler</param>
        /// <param name="annotationManager">The annotation manager object</param>
        private static void AddSignatureAnnotation(CustomActionEventArgs customActionEventArgs, AnnotationManager annotationMngr)
        {
            ClearAnnotation(annotationMngr, "signature");
            const string BASE64_SIGNATURE_IMAGE = "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";
            AnnotationEmbeddedImage signature = annotationMngr.AddEmbeddedImageAnnotFromBase64(BASE64_SIGNATURE_IMAGE,
                5.38f, 7.9f, 0.86f, 0.86f);
            signature.CanRotate = false;
            signature.Tag = "signature";
            annotationMngr.SaveAnnotationsToPage();
            customActionEventArgs.docuVieware.ReloadAnnotations();
        }


        /// <summary>
        /// Draw the annotation timestamp with custom parameters and custom content
        /// </summary>
        /// <param name="customActionEventArgs">The arguments received from the custom action handler</param>
        /// <param name="annotationManager">The annotation manager object</param>
        private static void AddTimestampAnnotation(CustomActionEventArgs customActionEventArgs, AnnotationManager annotationMngr)
        {
            // Remove timestamp if added on page
            ClearAnnotation(annotationMngr, "timestamp");
            AnnotationText timestamp = annotationMngr.AddTextAnnot(0.2f, 0.2f, 3.0f, 0.2f, DateTime.Now.ToString("G"));
            timestamp.AutoSize = true;
            timestamp.Fill = false;
            timestamp.Stroke = false;
            timestamp.FontName = "Courier New";
            timestamp.ForeColor = Color.OrangeRed;
            timestamp.FontSize = 10;
            timestamp.CanEditText = false;
            timestamp.Tag = "timestamp";
            annotationMngr.SaveAnnotationsToPage();
            customActionEventArgs.docuVieware.ReloadAnnotations();
        }

        /// <summary>
        /// Draws the "Approved" stamp  
        /// </summary>
        /// <param name="customActionEventArgs"></param>
        /// <param name="annotationMngr"></param>
        private static void AddApprovedStampAnnotation(CustomActionEventArgs customActionEventArgs, AnnotationManager annotationMngr)
        {
            ClearAnnotation(annotationMngr, "approved");

            AnnotationRubberStamp approvedStamp = getRubberStamp(annotationMngr, "Approved");

            approvedStamp.Tag = "approved";
            annotationMngr.SaveAnnotationsToPage();
            customActionEventArgs.docuVieware.ReloadAnnotations();
        }

        /// <summary>
        /// Draws the "Rejected" stamp
        /// </summary>
        /// <param name="customActionEventArgs"></param>
        /// <param name="annotationMngr"></param>
        private static void AddRejecteStampAnnotation(CustomActionEventArgs customActionEventArgs, AnnotationManager annotationMngr)
        {
            ClearAnnotation(annotationMngr, "rejected");

            AnnotationRubberStamp approvedStamp = getRubberStamp(annotationMngr, "Rejected");

            approvedStamp.Tag = "rejected";
            annotationMngr.SaveAnnotationsToPage();
            customActionEventArgs.docuVieware.ReloadAnnotations();
        }

        /// <summary>
        /// Returns a RubberStamp with all properties set and passed Text.
        /// </summary>
        /// <param name="annotationMngr"></param>
        /// <param name="stampText"></param>
        /// <returns></returns>
        private static AnnotationRubberStamp getRubberStamp(AnnotationManager annotationMngr, string stampText)
        {

            AnnotationRubberStamp stamp = annotationMngr.AddRubberStampAnnot(Color.Red, 0.2f, 0.2f, 3.0f, 1.2f, stampText);
            stamp.BorderWidth = 0.2f;

            stamp.DashCap = System.Drawing.Drawing2D.DashCap.Flat;
            stamp.Fill = true;
            if (stampText.Trim() == "Approved")
                stamp.FillColor = Color.White;
            else
                stamp.FillColor = Color.Black;

            stamp.FontName = "Arial";
            stamp.FontStyle = System.Drawing.FontStyle.Bold;
            stamp.ForeColor = Color.Red;
            stamp.Opacity = 100f;
            stamp.Printable = true;
            stamp.RadiusFactor = 0.25f;
            stamp.Stroke = true;
            stamp.StrokeColor = Color.Red;

            return stamp;
        }


        /// <summary>
        /// Makes sure annotations are not added twice on the same page
        /// </summary>
        /// <param name="annotationMngr">AnnotationManager object</param>
        /// <param name="tag">Annotation tag to clean</param>
        private static void ClearAnnotation(AnnotationManager annotationMngr, string tag)
        {
            for (int i=0; i < annotationMngr.GetAnnotationCount(); i++)
            {
                Annotation annot = annotationMngr.GetAnnotationFromIdx(i);
                if (annot.Tag == tag)
                    annotationMngr.DeleteAnnotation(i);
            }
        }

        #endregion


        /// <summary>
        /// Deletes current page of Image and/or PDF file formats
        /// </summary>
        /// <param name="e"></param>
        public static void HandleRemovePage(CustomActionEventArgs e)
        {
            RotateActionParameters rotateParams = JsonConvert.DeserializeObject<RotateActionParameters>(e.args.ToString());
            int currPage = rotateParams.CurrentPage;

            rotateParams = null;

            // For PDF FILES
            if (e.docuVieware.GetDocumentType() == DocumentType.DocumentTypePDF)
            {
                GdPicturePDF gdPdf = null;
                if (e.docuVieware.GetNativePDF(out gdPdf) == GdPictureStatus.OK)
                {
                    gdPdf.DeletePage(currPage);
                    e.message = GetMessage ("Page Deleted Successfully ", DocuViewareMessageIcon.Information); 
                }
            }

            // For Image Files, Scanned docs
            int imgId;
            if (e.docuVieware.GetNativeImage(out imgId) == GdPictureStatus.OK)
            {
                GdPictureImaging gdImg = new GdPictureImaging();
                if (gdImg.SelectPage(imgId, currPage) == GdPictureStatus.OK)
                {
                    if ( gdImg.TiffDeletePage(imgId, currPage) == GdPictureStatus.OK)
                        //gdImg.ReleaseGdPictureImage(imgId) == GdPictureStatus.OK)
                    {
                        e.message = GetMessage("Document was rejected successfully.", DocuViewareMessageIcon.Information);
                    }
                }
            } else
            {
                e.message = GetMessage("Remove Page : File Type not supported ! ", DocuViewareMessageIcon.Warning);
            }

            return;
        }

        /// <summary>
        /// Releases file from the Memory & Closes the file
        /// </summary>
        /// <param name="e"></param>
        public static void HandleCloseDocument(CustomActionEventArgs e)
        {

            // Release from memory
            int imgId;
            if (e.docuVieware.GetNativeImage(out imgId) == GdPictureStatus.OK)
            {
                GdPictureImaging gdImg = new GdPictureImaging();
                gdImg.ReleaseGdPictureImage(imgId);
            }
            
            // Close the document
            e.docuVieware.Close();
            
            
            return;
        }

        /// <summary>
        /// Handles Image Cleaup Actions 
        /// </summary>
        /// <param name="e"></param>
        public static void HandleImageCleanupAction(CustomActionEventArgs e)
        {
           if (e.docuVieware.PageCount > 0)
            {
                if (e.docuVieware.GetDocumentType() == DocumentType.DocumentTypeBitmap)
                {
                    int imageId;
                    GdPictureStatus status = e.docuVieware.GetNativeImage(out imageId);
                    if (status == GdPictureStatus.OK)
                    {
                        status = GdPictureStatus.GenericError;
                        using (GdPictureImaging gdPictImg = new GdPictureImaging())
                        {
                            RotateActionParameters parameters = JsonConvert.DeserializeObject<RotateActionParameters>(e.args.ToString());
                            if (parameters.RegionOfInterest != null && parameters.RegionOfInterest.Width > 0 && parameters.RegionOfInterest.Height > 0)
                            {
                                gdPictImg.SetROI((int)Math.Round(parameters.RegionOfInterest.Left * gdPictImg.GetHorizontalResolution(imageId), 0),
                                    (int)Math.Round(parameters.RegionOfInterest.Top * gdPictImg.GetVerticalResolution(imageId), 0),
                                    (int)Math.Round(parameters.RegionOfInterest.Width * gdPictImg.GetHorizontalResolution(imageId), 0),
                                    (int)Math.Round(parameters.RegionOfInterest.Height * gdPictImg.GetVerticalResolution(imageId), 0));
                            }
                            if (e.actionName != "punchHoleRemoval" || (e.actionName == "punchHoleRemoval" && gdPictImg.GetBitDepth(imageId) == 1) )
                            {
                                foreach (var page in parameters.Pages)
                                {
                                    status = gdPictImg.SelectPage(imageId, page);
                                    if (status == GdPictureStatus.OK)
                                    {
                                        switch (e.actionName)
                                        {
                                            case "automaticRemoveBlackBorders":
                                                status = gdPictImg.DeleteBlackBorders(imageId, 10, false);
                                                break;
                                            case "autoDeskew":
                                                status = gdPictImg.AutoDeskew(imageId);
                                                break;
                                            case "punchHoleRemoval":
                                                status = gdPictImg.RemoveHolePunch(imageId, HolePunchMargins.MarginLeft | HolePunchMargins.MarginRight | HolePunchMargins.MarginBottom | HolePunchMargins.MarginTop);
                                                break;
                                            case "despeckle":
                                                status = gdPictImg.FxDespeckle(imageId);
                                                break;
                                        }

                                        if (status != GdPictureStatus.OK)
                                        {
                                            e.message = GetMessage("Error during apply filter: " + status + " on page " + page, DocuViewareMessageIcon.Error);
                                            break;
                                        }
                                    } else
                                    {
                                        e.message = GetMessage("Error during page selectionr: " + status + "." + page, DocuViewareMessageIcon.Error);
                                        break;
                                    }
                                }   // foreach
                                if (status == GdPictureStatus.OK)
                                {
                                    status = e.docuVieware.RedrawPages(parameters.Pages);
                                    e.message = status == GdPictureStatus.OK ? new DocuViewareMessage("Filter successfuly applied.", icon: DocuViewareMessageIcon.Ok) : GetMessage("Error during redraw pages : " + status + ".", DocuViewareMessageIcon.Error);
                                }
                            }
                            else
                            {
                                e.message = GetMessage("Your image must be 1 bit-depth to apply this filter!", DocuViewareMessageIcon.Error);
                            }
                        }   // using
                    }
                    else
                    {
                        e.message = GetMessage("Error during get native image : " + status + ".", DocuViewareMessageIcon.Error);
                    }
                }
                else
                {
                    e.message = GetMessage("Only raster formats are supported!", DocuViewareMessageIcon.Error);
                }
            }
            else
            {
                e.message = GetMessage("Please open an image first." + ".", DocuViewareMessageIcon.Error);
            }

            return;
        }


        #region "docuView EVENTS"

        protected void docuView_NewDocumentLoaded(object sender, NewDocumentLoadedEventArgs e)
        {
            string str = "Doc Name = " + docuView.GetDocumentName() + " Type = " + docuView.GetDocumentType();
            Console.WriteLine("Doc Type Name : " + str);
            /*
            // Update doc Name in Label
            docNameLbl.Text = str;

            if (docuView.GetDocumentType() != DocumentType.DocumentTypeOpenXMLWord &&
                docuView.GetDocumentType() != DocumentType.DocumentTypePDF && 
                docuView.GetDocumentType() != DocumentType.DocumentTypeBitmap &&
                docuView.GetDocumentType() != DocumentType.DocumentTypeTXT )
            {              
                //docuView_CustomAction(sender, { actionName: "UnsupportedFile"} );

                //CustomActionEventArgs.({"UnsupportedFile"});

                // close the displayed doc
                docuView.Close();

            }
            */
            /*
            GdPicturePDF gdPdf = null;
            docuView.GetNativePDF(out gdPdf );
            gdPdf.DeletePage(1);
            */
        //    int imgId;
        //    docuView.GetNativeImage(out imgId);
            
            
            
            
        }

        protected void docuView_CustomAction(object sender, CustomActionEventArgs e)
        {
            // If file type/format, is not allowed
            if ( (sender.ToString() == "OnlyPDF_DOCX")) {
                // Display an ERROR msg of ONLY Opening allowed Allowed Files
                DocuViewareMessage docMsg = new DocuViewareMessage
                    ("Only PDF & DOCX file formsts supported.", "#ff5656", 2500, 300, 300, 
                    false, "130%", "normal", "#ffffff", "none", "none", "48px", 
                    DocuViewareMessageIcon.Error );
                
                // EXCEP - CustomActionEventArgs is null
                e.message = docMsg;


            }
        }

        protected void docuView_Unload(object sender, EventArgs e)
        {
            /*
             * If a file is Open
             * Make sure to Close the file, before opening a new one
             */ 
            if (docuView.GetDocumentName() != null)
                docuView.Close();
            docNameLbl.Text = "";

        }

        #endregion

    }
}