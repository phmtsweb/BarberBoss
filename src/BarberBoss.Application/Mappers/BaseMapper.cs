using AutoMapper;

namespace BarberBoss.Application.Mappers;

public abstract class BaseMapper<TEntity, TRequest, TResponse> : Profile
{
    protected BaseMapper()
    {
        RequestToEntity();
        EntityToResponse();
    }
    protected virtual void RequestToEntity()
    {
        CreateMap<TRequest, TEntity>();
    }

    protected virtual void EntityToResponse()
    {
        CreateMap<TEntity, TResponse>();
    }
    
}

public abstract class BaseMapper<TEntity, TRequest, TResponse, TShortResponse> : BaseMapper<TEntity, TRequest, TResponse>
{
    protected override void EntityToResponse()
    {
        base.EntityToResponse();
        CreateMap<TEntity, TShortResponse>();
    }
}