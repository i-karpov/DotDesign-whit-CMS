using DataAnnotationsExtensions.ClientValidation;

[assembly: WebActivator.PreApplicationStartMethod(typeof(DotDesign.WebUI.App_Start.RegisterClientValidationExtensions), "Start")]

namespace DotDesign.WebUI.App_Start
{
    public static class RegisterClientValidationExtensions {
        public static void Start() {
            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();            
        }
    }
}