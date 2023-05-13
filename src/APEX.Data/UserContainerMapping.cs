namespace APEX.Data
{
    public record UserContainerMapping(long Id, 
                                       ApexUser User, 
                                       long UserId, 
                                       FileContainer Container,
                                       long ContainerId);
}