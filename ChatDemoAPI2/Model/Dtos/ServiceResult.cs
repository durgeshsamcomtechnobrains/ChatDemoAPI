namespace ChatDemoAPI2.Model.Dtos
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}
