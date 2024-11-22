namespace Empresa.Proyecto.Core.Entities
{
    /// <summary>
    /// Entidad nueva con relacion a Simple entity a travez del campo Option
    /// </summary>
    public class NewEntity:BaseEntity
    {
        public string Name { get; set; } = null!;
        public SimpleEntity Option { get; set; } = null!;
    }
}
