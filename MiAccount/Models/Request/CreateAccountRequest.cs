namespace MiAccount.Models.Request
{
    public class CreateAccountRequest
    {
        public string Name { get; set; }
        public int TokenTimeout { get; set; }
    }
}
