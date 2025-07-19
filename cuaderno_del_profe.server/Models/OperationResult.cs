namespace cuaderno_del_profe.server.Models
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }

        public OperationResult(bool Success, string Message)
        {
            this.Success = Success;
            this.Message = Message;
        }
        public OperationResult(bool Success, string Message, Dictionary<string, string> Errors)
        {
            this.Success = Success;
            this.Message = Message;
            this.Errors = Errors;
        }
        public OperationResult(bool Success, string Message, object Data)
        {
            this.Success = Success;
            this.Message = Message;
            this.Data = ClearFiles(Data);
        }
        public OperationResult(bool Success, string Message, object Data, string token, Dictionary<string, string> Errors = null)
        {
            this.Success = Success;
            this.Message = Message;
            this.Data = ClearFiles(Data);
            this.Token = token;
            this.Errors = Errors;
        }
        public OperationResult(Dictionary<string, string> Errors)
        {
            this.Success = false;
            this.Message = "Los datos ingresados no son válidos";
            this.Errors = Errors;
        }
        public OperationResult(string Field, string Error)
        {
            var errors = new Dictionary<string, string>();
            errors.Add(Field, Error);

            this.Success = false;
            this.Message = "Los datos ingresados no son válidos";
            this.Errors = errors;
        }
        private object ClearFiles(object data)
        {
            foreach (var prop in data.GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(System.Web.HttpUtility))
                {
                    prop.SetValue(data, null);
                }
            }
            return data;
        }
    }
}
