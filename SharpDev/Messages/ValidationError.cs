namespace SharpDev.Messages
{
    public class ValidationError
    {
        public string EntityId { get; set; }
        public string PropertyName { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public string RuleName { get; set; }
    }
}