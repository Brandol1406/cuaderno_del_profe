namespace cuaderno_del_profe.server.Models
{
    public interface IUserAccessor
    {
        int idUsuario { get; }
        public string? username { get; }
    }
    public class UserAccessor : IUserAccessor
    {
        public int idUsuario { get; set; }
        public string? username { get; set; }
    }
}
