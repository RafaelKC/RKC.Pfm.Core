
namespace RKC.Pfm.Core.Domain.Communs;

public interface IEntityDto
{
    public Guid Id { get; set; }
}

public abstract class EntityDto: IEntityDto
{
    public Guid Id { get; set; }

    public EntityDto()
    {
    }
    
    public EntityDto(IEntity entity)
    {
        Id = entity.Id;
    }
}