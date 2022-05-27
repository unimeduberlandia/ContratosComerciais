namespace Painel.Core
{
    public class AjaxReturn
    {
        public bool Json = true;
        public bool Success { get; set; }
        public int Status { get; set; }
        public object Result { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public string Target { get; set; }
        public bool Swal { get; set; }
        public string SwalType { get; set; }
        public bool CloseAnyModal { get; set; }
        public string JsFunction { get; set; }
        public string JsParameters { get; set; }
        public bool JsAlert { get; set; }
        public bool JsReloadPage { get; set; }
    }
}
