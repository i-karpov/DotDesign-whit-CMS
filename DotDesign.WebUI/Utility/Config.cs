using System;
using System.Configuration;

namespace DotDesign.WebUI.Utility
{
    /// <summary>
    /// Provides global constants and configuration settings.
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Provides paging settings for CMS.
        /// </summary>
        public static PagingSettings CmsPagingSettings
        {
            get
            {
                return new PagingSettings(Convert.
                    ToInt32(ConfigurationManager.AppSettings["CmsItemsPerPageCount"]));
            }
        }

        /// <summary>
        /// Provides paging settings for client part of application
        /// </summary>
        public static PagingSettings ClientPagingSettings
        {
            get
            {
                return new PagingSettings(Convert.
                    ToInt32(ConfigurationManager.AppSettings["ClientItemsPerPageCount"]));
            }
        }

        /// <summary>
        /// Provides path to default image returned when requested resource wasn't found. 
        /// </summary>
        public static String ResourceNotFoundImageRelativePath
        {
            get
            {
                return ConfigurationManager.AppSettings["ResourceNotFoundImageRelativePath"];
            }
        }

        /// <summary>
        /// Provides default page name.
        /// </summary>
        public static String DefaultPageName
        {
            get
            {
                return ConfigurationManager.AppSettings["DefaultPageName"];
            }
        }

        /// <summary>
        /// Message, displayed when uploading data is incorrect.
        /// </summary>
        public static String ImageDataErrorMessage
        {
            get
            {
                return "Your upload did not seem valid. Please try again using only correct images!";
            }
        }

        /// <summary>
        /// Message, displayed when trying to create item with non-unique Url.
        /// </summary>
        // TODO: specify error type!
        public static String UrlErrorMessage
        {
            get
            {
                return "Sorry, item with such Url allready exists. Try another one please.";
            }
        }

        /// <summary>
        /// Message, displayed when login/password pair is incorrect.
        /// </summary>
        public static String LoginErrorMessage
        {
            get
            {
                return "The user name or password provided is incorrect.";
            }
        }

        /// <summary>
        /// Message, displayed when trying to create item with non-unique name.
        /// </summary>
        public static String TextErrorMessage
        {
            get
            {
                return "Sorry, item with such text allready exists. Try another one please.";
            }
        }

        /// <summary>
        /// Message, displayed when trying to change password with invalid old password provided.
        /// </summary>
        public static String WrongOldPasswordError
        {
            get
            {
                return "Old pasword you have provided seems to be invalid. Try again, please.";
            }
        }

        /// <summary>
        /// Message, displayed when provided value is too long.
        /// </summary>
        public static String TooLongValueMessage
        {
            get
            {
                return "Sorry, provided value is too long. Try another one, please.";
            }
        }
    }
}